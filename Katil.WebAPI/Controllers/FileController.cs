using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.Files;
using Katil.Business.Services.Files;
using Katil.Business.Services.TokenServices;
using Katil.Common.Utilities;
using Katil.WebAPI.Filters;
using Katil.WebAPI.WebApiHelpers.FileHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Katil.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/file")]
    public class FileController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly ITokenService _tokenService;

        public FileController(IFileService fileService, ITokenService tokenService)
        {
            _fileService = fileService;
            _tokenService = tokenService;
        }

        [HttpPost("{disputeGuid:Guid}")]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(900_000_000)]
        [AuthorizationRequired(roles: new[] { "Staff User", "External User", "Extended External User", "Access Code User" })]
        public async Task<IActionResult> Post(Guid disputeGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tempFileFolder = _fileService.GetFilePath("Temp");

            FileUtils.CheckIfNotExistsCreate(tempFileFolder);

            var fileModel = new FileResponse();
            var files = await FileStreamingHelper.ParseRequestForm(this, tempFileFolder, fileModel);

            List<FileResponse> fileList = new List<FileResponse>();

            foreach (var file in files.file)
            {
                var newFileGuid = Guid.NewGuid();
                var createdDateTime = DateTime.UtcNow.GetCmDateTime();

                var fileRelativePath = Path.Combine(
                    createdDateTime.Year.ToString(),
                    createdDateTime.Month.ToString(),
                    createdDateTime.Day.ToString(),
                    fileModel.FileType.ToString(),
                    newFileGuid.ToString());

                var fileRequest = new FileRequest
                {
                    FileGuid = newFileGuid,
                    FileSize = (int)file.Length,
                    FileMimeType = file.ContentType,
                    FileName = fileModel.FileName,
                    OriginalFileName = file.Name,
                    FileType = fileModel.FileType,
                    FilePath = fileRelativePath,
                    AddedBy = files.model.AddedBy,
                    FilePackageId = files.model.FilePackageId
                };

                var fileResponse = await _fileService.CreateAsync(fileRequest, createdDateTime);
                fileList.Add(fileResponse);

                var absolutePath = _fileService.GetFilePath(fileRelativePath);
                FileUtils.CheckIfNotExistsCreate(Path.GetDirectoryName(absolutePath));
                System.IO.File.Move(file.TemporaryLocation, absolutePath);
            }

            return Ok(fileList);
        }

        [HttpDelete("{fileId:int}")]
        [AuthorizationRequired(roles: new[] { "Staff User", "External User", "Extended External User", "Access Code User" })]
        public async Task<IActionResult> Delete(int fileId)
        {
            var fileInfo = await _fileService.GetAsync(fileId);
            if (fileInfo != null)
            {
                var filePath = _fileService.GetFilePath(fileInfo.FilePath);
                System.IO.File.Delete(filePath);

                var result = await _fileService.DeleteAsync(fileId);
                if (result)
                {
                    return Ok(ApiReturnMessages.Deleted);
                }
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/file/{year:int:minlength(4):maxlength(4)}/{month:int:min(1):max(12)}/{day:int:min(1):max(31)}/{filename:maxlength(255)}")]
        public async Task<IActionResult> GetByUrl(int year, int month, int day, string filename, [FromQuery]string token, [FromQuery]bool? isInline = null)
        {
            if (token.IsBase64String() == false)
            {
                return BadRequest(string.Format(ApiReturnMessages.TokenIsNotSpecified));
            }

            var tokenWithFileId = token.Base64Decode().Split(":");
            var isValidToken = await _tokenService.ValidateToken(tokenWithFileId[0]);

            if (isValidToken == false)
            {
                return Unauthorized();
            }

            var file = await _fileService.GetAsync(year, month, day, filename);
            if (file != null)
            {
                if (file.FileId.ToString() != tokenWithFileId[1])
                {
                    return Unauthorized();
                }

                var absoluteFilePath = _fileService.GetFilePath(file.FilePath);

                ////var contentDispositionType = isInline != null && (bool)isInline ? "inline" : "attachment";
                var contentDispositionType = isInline ?? false ? "inline" : "attachment";

                // Set up the content-disposition header with proper encoding of the filename
                var contentDisposition = new ContentDispositionHeaderValue(contentDispositionType);
                Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();

                return await FileStreamingHelper.GetFile(absoluteFilePath, file.FileMimeType);
            }

            return NotFound();
        }
    }
}

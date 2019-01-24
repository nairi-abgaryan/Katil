using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using EasyNetQ;
using Katil.Business.Entities.Models.Files;
using Katil.Business.Services.SystemSettingsService;
using Katil.Business.Services.TokenServices;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.Data.Repositories.UnitOfWork;
using CmFile = Katil.Data.Model.File;

namespace Katil.Business.Services.Files
{
    public class FileService : KatilServiceBase, IFileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;

        public FileService(ITokenService tokenService, ISystemSettingsService settingsService, IUnitOfWork unitOfWork, IBus bus)
            : base(tokenService, settingsService)
        {
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task<FileResponse> CreateAsync(FileRequest fileRequest, DateTime createdDateTime)
        {
            var newFile = Mapper.Map<FileRequest, CmFile>(fileRequest);
            newFile.FileDate = DateTime.UtcNow;
            newFile.IsDeleted = false;

            var result = await _unitOfWork.FileRepository.InsertAsync(newFile);
            await _unitOfWork.Complete();

            if (result.FileType == Constants.FileUploadType)
            {
                result.FileName = string.Format(
                    "{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(result.FileName),
                    result.FileId,
                    Path.GetExtension(result.FileName));
            }
            else
            {
                result.FileName = string.Format(
                    "{0}{1}",
                    Path.GetFileNameWithoutExtension(result.FileName),
                    Path.GetExtension(result.FileName));
            }

            _unitOfWork.FileRepository.Update(result);
            await _unitOfWork.Complete();

            var fileResponse = Mapper.Map<FileResponse>(result);

            fileResponse.FileUrl = GetFileUrl(result);

            return fileResponse;
        }

        public async Task<bool> DeleteAsync(int fileId)
        {
            var file = await _unitOfWork.FileRepository.GetByIdAsync(fileId);
            if (file != null)
            {
                file.IsDeleted = true;
                _unitOfWork.FileRepository.Attach(file);
                var result = await _unitOfWork.Complete();

                if (result == 1)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        public async Task<FileResponse> GetAsync(int year, int month, int day, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) && Path.HasExtension(filename) == false)
            {
                throw new InvalidDataException("Invalid file name specified");
            }

            DateTime theDate = new DateTime(year, month, day);

            var file = await _unitOfWork.FileRepository.GetNoTrackingByIdAsync(
                r => r.CreatedDate.Value.Date.Equals(theDate.Date) &&
                string.Equals(r.FileName, filename, StringComparison.OrdinalIgnoreCase));

            if (file != null)
            {
                return Mapper.Map<CmFile, FileResponse>(file);
            }

            return null;
        }

        public async Task<FileResponse> GetAsync(int fileId)
        {
            var file = await _unitOfWork.FileRepository.GetByIdAsync(fileId);
            if (file != null)
            {
                return Mapper.Map<CmFile, FileResponse>(file);
            }

            return null;
        }

        public string GetFilePath(string filePath)
        {
            var rootFileFolder = SystemSettingsService.GetSetting(SettingKeys.FileStorageRoot).Result.Value;
            return Path.Combine(rootFileFolder, filePath);
        }

        public string GetCommonFilePath(string filePath)
        {
            var rootFileFolder = SystemSettingsService.GetSetting(SettingKeys.CommonFileStorageRoot).Result.Value;
            return Path.Combine(rootFileFolder, filePath);
        }

        public async Task<DateTime?> GetLastModifiedDateAsync(object fileId)
        {
            var lastModifiedDate = await _unitOfWork.FileRepository.GetLastModifiedDateAsync((int)fileId);
            if (lastModifiedDate != null)
            {
                return lastModifiedDate;
            }

            return null;
        }

        public async Task<CmFile> GetNoTrackingFileAsync(int id)
        {
            var file = await _unitOfWork.FileRepository.GetNoTrackingByIdAsync(p => p.FileId == id);
            return file;
        }

        public async Task<bool> CheckAddedBy(int fileId, int addedBy)
        {
            return await _unitOfWork.FileRepository.CheckAddedByExistance(fileId, addedBy);
        }

        public async Task<bool> FileExists(int fileId)
        {
            var file = await _unitOfWork.FileRepository.GetByIdAsync(fileId);
            if (file != null)
            {
                return true;
            }

            return false;
        }

        private string GetFileUrl(CmFile file)
        {
            var rootPath = SystemSettingsService.GetSetting(SettingKeys.FileRepositoryBaseUrl).Result.Value;

            return string.Format(
                "{0}/{1}/{2}/{3}/{4}",
                rootPath,
                file.CreatedDate?.Year,
                file.CreatedDate?.Month,
                file.CreatedDate?.Day,
                file.FileName);
        }
    }
}

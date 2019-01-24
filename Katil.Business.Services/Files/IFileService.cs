using System;
using System.Threading.Tasks;
using Katil.Business.Entities.Models.Files;
using Katil.Data.Model;

namespace Katil.Business.Services.Files
{
    public interface IFileService : IServiceBase
    {
        Task<FileResponse> CreateAsync(FileRequest fileRequest, DateTime createdDateTime);

        Task<bool> DeleteAsync(int fileId);

        Task<FileResponse> GetAsync(int year, int month, int day, string filename);

        Task<FileResponse> GetAsync(int fileId);

        string GetFilePath(string filePath);

        string GetCommonFilePath(string filePath);

        Task<File> GetNoTrackingFileAsync(int id);

        Task<bool> CheckAddedBy(int fileId, int addedBy);

        Task<bool> FileExists(int fileId);
    }
}
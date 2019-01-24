using System;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;

namespace Katil.Data.Repositories.Files
{
    public interface IFileRepository : IRepository<File>
    {
        Task<DateTime?> GetLastModifiedDateAsync(int fileId);

        Task<bool> CheckAddedByExistance(int fileId, int addedBy);
    }
}

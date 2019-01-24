using System;
using System.Linq;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.Files
{
    public class FileRepository : BaseRepository<File>, IFileRepository
    {
        public FileRepository(KatilContext context)
            : base(context)
        {
        }

        public async Task<bool> CheckAddedByExistance(int fileId, int addedBy)
        {
            return await System.Threading.Tasks.Task.FromResult<bool>(true);
        }

        public async Task<DateTime?> GetLastModifiedDateAsync(int fileId)
        {
            var dates = await Context.Files
                .Where(p => p.FileId == fileId)
                .Select(d => d.ModifiedDate)
                .ToListAsync();

            if (dates != null)
            {
                return dates.FirstOrDefault();
            }

            return null;
        }
    }
}

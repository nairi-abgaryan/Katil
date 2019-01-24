using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.Files;
using Katil.Data.Repositories.SystemSettings;
using Katil.Data.Repositories.Token;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KatilContext _context;
        private ITokenRepository tokenRepository = null;
        private IUserRepository userRepository = null;
        private ISystemSettingsRepository systemSettingsRepository = null;
        private IFileRepository fileRepository = null;

        public UnitOfWork(KatilContext context)
        {
            _context = context;
        }

        public ITokenRepository TokenRepository => tokenRepository ?? new TokenRepository(_context);

        public IUserRepository UserRepository => userRepository ?? new UserRepository(_context);

        public ISystemSettingsRepository SystemSettingsRepository => systemSettingsRepository ?? new SystemSettingsRepository(_context);

        public IFileRepository FileRepository => fileRepository ?? new FileRepository(_context);

        public async Task<int> Complete()
        {
            using (var scope = _context.Database.BeginTransaction())
            {
                try
                {
                    var res = await _context.SaveChangesAsync();
                    scope.Commit();
                    return res;
                }
                catch (DbException ex)
                {
                    scope.Rollback();
                    Debug.WriteLine(ex.Message);
                    return await System.Threading.Tasks.Task.FromResult(ex.ErrorCode);
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}

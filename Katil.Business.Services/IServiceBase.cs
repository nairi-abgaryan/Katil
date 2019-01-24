using System;
using System.Threading.Tasks;

namespace Katil.Business.Services
{
    public interface IServiceBase
    {
        Task<DateTime?> GetLastModifiedDateAsync(object id);
    }
}

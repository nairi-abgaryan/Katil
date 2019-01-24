using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.Base
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        DbSet<TEntity> DbSet { get; set; }

        void Attach(TEntity obj);

        Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where);

        Task<ICollection<TEntity>> GetAllAsync();

        TEntity GetById(int id);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetNoTrackingByIdAsync(Expression<Func<TEntity, bool>> where);

        Task<TEntity> InsertAsync(TEntity obj);

        void Update(TEntity obj);

        Task<bool> Delete(int id);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Katil.Data.Model;
using Katil.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Repositories.Base
{
    /// <summary>
    /// Base repo for repositories
    /// </summary>
    /// <typeparam name="T">Class type</typeparam>
    public class BaseRepository<T> : IRepository<T>
        where T : class
    {
        private readonly KatilContext context;

        protected BaseRepository(KatilContext context)
        {
            this.context = context;
            DbSet = context.Set<T>();
        }

        public DbSet<T> DbSet { get; set; }

        protected List<string> TrackedProperties { get; set; }

        protected KatilContext Context => context;

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.Where(where).ToListAsync();
        }

        public async Task<T> InsertAsync(T obj)
        {
            var inserted = await DbSet.AddAsync(obj);
            return inserted.Entity;
        }

        public void Update(T obj)
        {
            DbSet.Update(obj);
        }

        public void Attach(T obj)
        {
            try
            {
                DbSet.Attach(obj);
                Context.Entry(obj).State = EntityState.Modified;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var obj = await DbSet.FindAsync(id);
            var prop = obj.GetType().GetProperty("IsDeleted");

            if (prop != null)
            {
                prop.SetValue(obj, true);
                DbSet.Attach(obj);
                Context.Entry(obj).State = EntityState.Modified;

                return true;
            }

            return false;
        }

        public async Task<T> GetNoTrackingByIdAsync(Expression<Func<T, bool>> where)
        {
            return await DbSet.AsNoTracking().SingleOrDefaultAsync(where);
        }
    }
}

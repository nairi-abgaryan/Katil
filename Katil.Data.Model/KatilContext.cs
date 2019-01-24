using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Katil.UserResolverService;
using Microsoft.EntityFrameworkCore;

namespace Katil.Data.Model
{
    public class KatilContext : DbContext
    {
        private readonly IUserResolver _userResolver;

        #region Constructor

        public KatilContext(DbContextOptions<KatilContext> options, IUserResolver userResolver)
                : base(options)
        {
            _userResolver = userResolver;
        }

        #endregion

        #region DBSets

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<SystemUserRole> SystemUserRoles { get; set; }

        public virtual DbSet<File> Files { get; set; }

        public virtual DbSet<SystemSettings> SystemSettings { get; set; }

        public virtual DbSet<UserToken> UserTokens { get; set; }

        #endregion

        #region Fluent API

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyParticipantRelations(modelBuilder);
            ApplyFileRelations(modelBuilder);
            ApplyCommonFileRelations(modelBuilder);
            ApplyIndexes(modelBuilder);
            ApplyIsDeletedFilter(modelBuilder);
            ApplyColumnsCustomTypes(modelBuilder);
            AddDefaultValues(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        #endregion

        #region Private member methods

        private void AddDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>().Property(r => r.FileType).HasDefaultValue(0);
            modelBuilder.Entity<File>().Property(r => r.FileStatus).HasDefaultValue((byte)0);
            modelBuilder.Entity<File>().Property(r => r.FileConsidered).HasDefaultValue(true);
            modelBuilder.Entity<File>().Property(r => r.FileReferenced).HasDefaultValue(false);

            modelBuilder.Entity<SystemUserRole>().Property(r => r.SessionDuration).HasDefaultValue(900);
        }

        private void ApplyIsDeletedFilter(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>().HasQueryFilter(r => r.IsDeleted == false);
        }

        private void ApplyColumnsCustomTypes(ModelBuilder modelBuilder)
        {
        }

        private void ApplyParticipantRelations(ModelBuilder modelBuilder)
        {
        }

        private void ApplyFileRelations(ModelBuilder modelBuilder)
        {
        }

        private void ApplyCommonFileRelations(ModelBuilder modelBuilder)
        {
        }

        private void ApplyIndexes(ModelBuilder modelBuilder)
        {
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            {
                var userId = _userResolver.GetUserId();

                foreach (var entity in entities)
                {
                    ////TODO: Talk with Tigran about date and times
                    if (entity.State == EntityState.Added)
                    {
                        if (entity.Entity.GetType().Equals(((BaseEntity)entity.Entity).CreatedBy != null))
                        {
                            continue;
                        }

                        ((BaseEntity)entity.Entity).CreatedDate = DateTime.UtcNow;
                        ((BaseEntity)entity.Entity).CreatedBy = userId;
                    }

                    ((BaseEntity)entity.Entity).ModifiedDate = DateTime.UtcNow;
                    ((BaseEntity)entity.Entity).ModifiedBy = userId;
                }
            }
        }

        #endregion
    }
}

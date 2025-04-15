
using JobSyncProvider.DataAccess.Data;
using JobSyncProvider.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;

namespace JobSyncProvider.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<string, object> _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<T>);
                _repositories[type] = Activator.CreateInstance(repositoryType, _context);
            }

            return (IRepository<T>) _repositories[type];
        }

        public async Task RollBackAsync()
        {
            foreach(var entry in _context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                }
                else if(entry.State == EntityState.Modified)
                {
                    entry.State = EntityState.Unchanged;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Unchanged;
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

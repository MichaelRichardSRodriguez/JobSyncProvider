using JobSyncProvider.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace JobSyncProvider.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

		public IQueryable<T> GetQueryable()
		{
			return _dbSet.AsQueryable();
		}
		public async Task AddRecordAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
		{
			return await _dbSet.AnyAsync(expression);
		}

		public void DeleteRecord(T entity)
        {

            _dbSet.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllRecordsAsync(Expression<Func<T, bool>>? expression = null,
                                                      Expression<Func<T, object>>[]? includedProperties = null,
                                                      bool tracked = false)
                                                      //int pageNumber = 1,
                                                      //int pageSize = 10)
        {

            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }

            //query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();


			//USAGE
			//    var product = repository.Get(
			//                                    p => p.Id == 1,                // filter (get the product with Id = 1)
			//                                    includeProperties: new Expression<Func<Product, object>>[]
			//                                    {
			//                                        p => p.Category,            // include Category
			//                                        p => p.Supplier             // include Supplier
			//
			//
			//   }


		}


        public async Task<T> GetRecordAsync(Expression<Func<T, bool>>? expression = null,
                                              Expression<Func<T, object>>[]? includedProperties = null,
                                              bool tracked = false)
        {

            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includedProperties != null)
            {
                foreach (var includedProperty in includedProperties)
                {
                    query = query.Include(includedProperty);
                }
            }

            return await query.FirstOrDefaultAsync();

        }

        public async Task<T> GetRecordByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void UpdateRecord(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}

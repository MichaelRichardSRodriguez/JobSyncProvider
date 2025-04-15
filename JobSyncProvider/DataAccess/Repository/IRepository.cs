using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JobSyncProvider.DataAccess.Repository
{
    public interface IRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAllRecordsAsync(Expression<Func<T, bool>>? expression = null,
                                        Expression<Func<T, object>>[]? includedProperties = null,
                                        bool tracked = false); 
          //                              int pageNumber = 1,
										//int pageSize = 10);
        public IQueryable<T> GetQueryable();
		Task<T> GetRecordAsync(Expression<Func<T, bool>>? expression = null, 
                        Expression<Func<T, object>>[]? includedProperties = null, 
                        bool tracked = false);
        Task<T> GetRecordByIdAsync(int id);

        Task AddRecordAsync(T entity);

        void UpdateRecord(T entity);
   
        void DeleteRecord(T Entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> expression);

    }
}

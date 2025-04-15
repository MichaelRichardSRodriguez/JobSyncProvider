using JobSyncProvider.DataAccess.Repository;

namespace JobSyncProvider.DataAccess.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        Task CommitAsync();
        Task RollBackAsync();
    }
}

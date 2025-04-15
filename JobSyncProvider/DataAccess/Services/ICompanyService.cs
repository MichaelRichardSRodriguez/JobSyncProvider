using JobSyncProvider.Models;
using System.Linq.Expressions;

namespace JobSyncProvider.DataAccess.Services
{
	public interface ICompanyService
	{
		Task AddCompanyAsync(Company company);
		Task UpdateCompanyAsync(Company company);
		Task DeleteCompanyAsync(int id);
		Task<Company> GetCompanyByIdAsync(int id);
		Task<Company> GetCompanyByEmployerIdAsync(string id);
		Task<IEnumerable<Company>> GetAllCompaniesAsync(); //int currentPageNumber = 1, int currentPageSize = 10);
		Task<bool> IsExistingCompany(Expression<Func<Company, bool>> expression);
	}
}

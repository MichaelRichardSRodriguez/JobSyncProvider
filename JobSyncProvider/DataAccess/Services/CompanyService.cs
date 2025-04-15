using JobSyncProvider.DataAccess.Repository;
using JobSyncProvider.DataAccess.UnitOfWork;
using JobSyncProvider.Models;
using System.Linq.Expressions;

namespace JobSyncProvider.DataAccess.Services
{
	public class CompanyService : ICompanyService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Company> _companyRepository;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
			_companyRepository = _unitOfWork.GetRepository<Company>();
        }

		public async Task AddCompanyAsync(Company company)
		{
			await _companyRepository.AddRecordAsync(company);
			await _unitOfWork.CommitAsync();
		}

		public async Task DeleteCompanyAsync(int id)
		{
			var company = await _companyRepository.GetRecordByIdAsync(id);

			if (company == null)
			{
				throw new KeyNotFoundException("Company not found.");
			}

			 _companyRepository.DeleteRecord(company);
			await _unitOfWork.CommitAsync();
		}

		public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
		{
			var companies = await _companyRepository.GetAllRecordsAsync();
			companies = companies.OrderBy(c => c.Name);

			return companies;
		}

		public async Task<Company> GetCompanyByIdAsync(int id)
		{
			return await _companyRepository.GetRecordByIdAsync(id);
		}

		public async Task<Company> GetCompanyByEmployerIdAsync(string id)
		{

			var company = await _companyRepository.GetRecordAsync(
					expression: c => c.EmployerId == id);

			return company;

		}

		public async Task<bool> IsExistingCompany(Expression<Func<Company, bool>> expression)
		{
			return await _companyRepository.AnyAsync(expression);
		}

		public async Task UpdateCompanyAsync(Company company)
		{
			_companyRepository.UpdateRecord(company);
			await _unitOfWork.CommitAsync();
		}
	}
}

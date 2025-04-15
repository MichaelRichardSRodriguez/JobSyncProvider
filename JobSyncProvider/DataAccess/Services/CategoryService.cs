using JobSyncProvider.DataAccess.Repository;
using JobSyncProvider.DataAccess.UnitOfWork;
using JobSyncProvider.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace JobSyncProvider.DataAccess.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
			_categoryRepository = _unitOfWork.GetRepository<Category>();
        }
        public async Task AddCategoryAsync(Category category)
		{
			await _categoryRepository.AddRecordAsync(category);
			await _unitOfWork.CommitAsync();
		}

		public async Task DeleteCategoryAsync(int id)
		{
			var category = await _categoryRepository.GetRecordByIdAsync(id);

			if (category == null)
			{
				throw new KeyNotFoundException("Category not found.");
			}

			_categoryRepository.DeleteRecord(category);
			await _unitOfWork.CommitAsync();
		}

		public async Task<IEnumerable<Category>> GetAllCategoriesAsync() //int currentPageNumber = 1, int currentPageSize = 10)
		{

			return await _categoryRepository.GetAllRecordsAsync(); //pageNumber: currentPageNumber, pageSize: currentPageSize);

		}

		public async Task<Category> GetCategoryByIdAsync(int id)
		{

			var category =  await _categoryRepository.GetRecordAsync(
					expression: c => c.CategoryId == id,
					includedProperties: new Expression<Func<Category, object>>[]
					{
						c => c.CreatedByUser,
						c => c.ModifiedByUser
					});

			return category;

		}

		public async Task<bool> IsExistingCategory(Expression<Func<Category, bool>> expression)
		{
			return await _categoryRepository.AnyAsync(expression);
		}

		public async Task UpdateCategoryAsync(Category category)
		{
			_categoryRepository.UpdateRecord(category);
			await _unitOfWork.CommitAsync();
		}
	}
}

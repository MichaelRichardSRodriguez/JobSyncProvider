using JobSyncProvider.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Linq.Expressions;

namespace JobSyncProvider.DataAccess.Services
{
	public interface ICategoryService
	{
		Task AddCategoryAsync(Category category);
		Task UpdateCategoryAsync(Category category);
		Task DeleteCategoryAsync(int id);
		Task<Category> GetCategoryByIdAsync(int id);
		Task<IEnumerable<Category>> GetAllCategoriesAsync(); //int currentPageNumber = 1, int currentPageSize = 10);
		Task<bool> IsExistingCategory(Expression<Func<Category,bool>> expression);

	}
}

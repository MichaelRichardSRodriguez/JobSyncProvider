using JobSyncProvider.Models;

namespace JobSyncProvider.ViewModels
{
	public class CategoriesVM
	{

		public IEnumerable<Category> Categories { get; set; }
		public int TotalItems { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }

	}
}

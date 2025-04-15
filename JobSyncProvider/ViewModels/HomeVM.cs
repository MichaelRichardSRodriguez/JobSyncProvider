using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobSyncProvider.ViewModels
{
	public class HomeVM
	{

		public IEnumerable<SelectListItem> Categories { get; set; }
	}
}

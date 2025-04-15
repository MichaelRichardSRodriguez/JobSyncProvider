namespace JobSyncProvider.DataAccess.Services
{
	public interface ILogoService
	{

		Task<string> UploadLogoAsync(IFormFile file, string? existingLogoUrl);
		void DeleteLogo(string existingLogoUrl);

	}
}

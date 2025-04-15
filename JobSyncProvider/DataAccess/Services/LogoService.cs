
using JobSyncProvider.Models;

namespace JobSyncProvider.DataAccess.Services
{
	public class LogoService : ILogoService
	{

		private readonly IWebHostEnvironment _webHostEnvironment;

		public LogoService(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<string> UploadLogoAsync(IFormFile? file, string? existingLogoUrl)
		{
			if (file == null)
				return existingLogoUrl; // No new file uploaded, return the existing logo URL

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string logoPath = @"images\Logos";
			string finalPath = Path.Combine(wwwRootPath, logoPath);

			// If directory doesn't exist, create it
			if (!Directory.Exists(finalPath))
				Directory.CreateDirectory(finalPath);

			// If an existing logo exists, delete it
			if (!string.IsNullOrEmpty(existingLogoUrl))
			{
				string existingFilePath = Path.Combine(wwwRootPath, existingLogoUrl.TrimStart('\\'));

				if (System.IO.File.Exists(existingFilePath))
				{
					// Delete the existing file
					System.IO.File.Delete(existingFilePath); 
				}
			}

			// Generate a new file name and save the new file
			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file?.FileName);
			string newFilePath = Path.Combine(finalPath, fileName);

			using (var fileStream = new FileStream(newFilePath, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			// Return the new logo URL
			return @"\" + logoPath + @"\" + fileName; 

		}

		public void DeleteLogo(string existingLogoUrl)
		{

			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string logoPath = @"images\Logos";
			string finalPath = Path.Combine(wwwRootPath, logoPath);

			// If an existing logo exists, delete it
			if (!string.IsNullOrEmpty(existingLogoUrl))
			{
				string existingFilePath = Path.Combine(wwwRootPath, existingLogoUrl.TrimStart('\\'));

				if (System.IO.File.Exists(existingFilePath))
				{
					// Delete the existing file
					System.IO.File.Delete(existingFilePath);
				}
			}

		}
	}
}

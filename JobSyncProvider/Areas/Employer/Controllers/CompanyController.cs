using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobSyncProvider.Models;
using JobSyncProvider.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using JobSyncProvider.Utilities;
using JobSyncProvider.DataAccess.Services;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JobSyncProvider.Areas.Employer.Controllers
{
    [Area("Employer")]
	[Authorize(Roles = StaticDetails.ROLE_EMPLOYER + "," + StaticDetails.ROLE_ADMIN)]
	public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly ILogoService _logoService;

        public CompanyController(ICompanyService companyService,
								ILogoService logoService,
								IWebHostEnvironment webHostEnvironment)
        {
            _companyService = companyService;
			_logoService = logoService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Employer/Company
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
		public async Task<IActionResult> GetAllCompanies([FromQuery] string selectedStatus)
		{
            var companies = await _companyService.GetAllCompaniesAsync();

            if (selectedStatus?.ToUpper() != "ALL")
            {
                bool isActive = selectedStatus.ToUpper() == "ACTIVE"? true : false;

                companies = companies.Where(c => c.IsActive == isActive);
            }

			return Json(new {data = companies});

		}

		// GET: Employer/Company/Details/5
		public async Task<IActionResult> Details(int id)
        {

            var company = await _companyService.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Employer/Company/Create
        public IActionResult Create()
        {

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			var existingCompany = _companyService.GetCompanyByEmployerIdAsync(userId);

            if (existingCompany.Result != null)
            {
                return RedirectToAction(nameof(Details),new { id = existingCompany.Result.CompanyId });
            }

			return View();
        }

        // POST: Employer/Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                company.DateCreated = DateTime.Now;
                company.IsActive = StaticDetails.STATUS_ACTIVE;

				// Check and upload the logo, ensuring the old file is deleted if necessary
				company.LogoUrl = await _logoService.UploadLogoAsync(file, null);

				if (User.IsInRole(StaticDetails.ROLE_EMPLOYER))
                {
					// Get currently logged in User Id from Identity
					var claimsIdentity = (ClaimsIdentity)User.Identity;
					var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

					company.EmployerId = userId;
					await _companyService.AddCompanyAsync(company);

					TempData["success"] = "Company Profile successfully added.";

					return RedirectToAction(nameof(Index), "Home", new { area = "Guest" });

                }

                // if Admin user, Employer Id is NULL
				await _companyService.AddCompanyAsync(company);

				TempData["success"] = "Company Profile successfully added.";

				return RedirectToAction(nameof(Index));

			}

            return View(company);
        }

        // GET: Employer/Company/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            var company = await _companyService.GetCompanyByIdAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Employer/Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Company company,IFormFile? file)
        {
            if (id != company.CompanyId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    //Get Existing Values
                    var companyFromDb = await _companyService.GetCompanyByIdAsync(id);
					string? existingLogoUrl = companyFromDb.LogoUrl; // Get the existing logo URL from the company object

					if (companyFromDb == null)
                    {
                        ModelState.AddModelError("", "Company is not existing.");
                        return View(company);
                    }

                    //Pass the current values to existing entity
					companyFromDb.Description = company.Description;    
                    companyFromDb.Location = company.Location;
                    companyFromDb.Name = company.Name;
					companyFromDb.FoundedYear = company.FoundedYear;
					companyFromDb.DateModified = DateTime.Now;
                    companyFromDb.Website = company.Website;

					//Update Logo
					companyFromDb.LogoUrl = await _logoService.UploadLogoAsync(file, existingLogoUrl);

					//Update Record
					await _companyService.UpdateCompanyAsync(companyFromDb);

					TempData["success"] = "Company Profile successfully updated.";
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _companyService.IsExistingCompany(c => c.CompanyId == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _companyService.GetCompanyByIdAsync(id);

			if (category == null)
                return Json(new { success = false, message = "Company Profile does not exist." });

			await _companyService.DeleteCompanyAsync(id);
			_logoService.DeleteLogo(category.LogoUrl);

			return Json(new { success = true, message = "Company Profile successfully deleted." });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateStatus([FromBody] int id)
		{
			if (id == 0)
			{
				return NotFound();
			}

			var category = await _companyService.GetCompanyByIdAsync(id);

			if (category == null)
			{
				return Json(new { success = false, message = "Company not found." });
			}

			try
			{

                if (category.IsActive == StaticDetails.STATUS_ACTIVE)
                {
                    category.IsActive = StaticDetails.STATUS_INACTIVE;
                }
                else
                {
                    category.IsActive = StaticDetails.STATUS_ACTIVE;
                }

                await _companyService.UpdateCompanyAsync(category);
            }
			catch (DbUpdateConcurrencyException ex)
			{

				return Json(new { success = false, message = $"Concurrency issue occured, : {ex.Message}." });

				// Consider logging the exception for more details
				//throw;
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}." });
				// Log the exception for further investigation
			}

			return Json(new { success = true, message = "Category Status successfully updated." });
		}

        private string Generate_LogoURL(IFormFile file)
        {

			string wwwRootPath = _webHostEnvironment.WebRootPath;

			// Generate FileName and Get File Path
			string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
			string logoPath = @"images\Logos";
			string finalPath = Path.Combine(wwwRootPath, logoPath);

			// Create Directory if not Existing
			if (!Directory.Exists(finalPath))
				Directory.CreateDirectory(finalPath);

			// Copy File to the Directory
			using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
			{
				file.CopyTo(fileStream);
			}

			return  (@"\" + logoPath + @"\" + fileName);

        }

	}
}

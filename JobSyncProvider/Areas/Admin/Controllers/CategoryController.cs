using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobSyncProvider.DataAccess.Data;
using JobSyncProvider.Models;
using JobSyncProvider.Utilities;
using System.Security.Claims;
using JobSyncProvider.DataAccess.UnitOfWork;
using JobSyncProvider.DataAccess.Services;
using JobSyncProvider.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace JobSyncProvider.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = StaticDetails.ROLE_ADMIN)]
	public class CategoryController : Controller

	{
		private readonly ICategoryService _categoryService;

		public CategoryController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public IActionResult Index()
		{

			return View();
        }

		// GET: Admin/Category/Details/5
		public async Task<IActionResult> Details(int id)
		{

			var category = await _categoryService.GetCategoryByIdAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// GET: Admin/Category/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Admin/Category/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("CategoryId,Name,Status")] Category category)
		{
			if (ModelState.IsValid)
			{
				bool existingCategoryName = await _categoryService.IsExistingCategory(c => EF.Functions.Like(c.Name,category.Name)
											&& c.CategoryId != category.CategoryId);

				if (!existingCategoryName)
				{ 
					var claimsIdentity = (ClaimsIdentity)User.Identity;
					var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

					category.DateCreated = DateTime.Now;
					category.Status = StaticDetails.STATUS_VALID;
					category.CreatedBy = userId;

					await _categoryService.AddCategoryAsync(category);
					TempData["success"] = "Category successfully created.";

					return RedirectToAction(nameof(Index));
				}
				else
				{
					ModelState.AddModelError("Name", "Existing Category Name");
					TempData["error"] = "Unable to save record, please check message.";
				}
			}
			else
			{
				TempData["error"] = "Please check all details before saving.";
			}

			return View(category);
		}

		// GET: Admin/Category/Edit/5
		public async Task<IActionResult> Edit(int id)
		{

			var category = await _categoryService.GetCategoryByIdAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Admin/Category/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Status")] Category category)
		{
			if (id != category.CategoryId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				bool existingCategoryName = await _categoryService.IsExistingCategory(c => EF.Functions.Like(c.Name, category.Name)
											&& c.CategoryId != category.CategoryId);

				if (existingCategoryName)
				{
					ModelState.AddModelError("Name", "Existing Category Name");
					TempData["error"] = "Unable to save record, please check message.";
					return View(category);
				}

				try
				{

					var claimsIdentity = (ClaimsIdentity)User.Identity;
					var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

					var categoryFromDb = await _categoryService.GetCategoryByIdAsync(id);
					category.DateCreated = categoryFromDb.DateCreated;
					category.Status = categoryFromDb.Status;
					category.CreatedBy = categoryFromDb.CreatedBy;
					category.DateUpdated = DateTime.Now;
					category.ModifiedBy = userId;

					await _categoryService.UpdateCategoryAsync(category);
					TempData["success"] = "Category successfully updated.";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await _categoryService.IsExistingCategory(c => c.CategoryId == category.CategoryId))
					{
						return NotFound();
					}
					else
					{
						TempData["error"] = "Update failed, due to concurrency issue. Please try again.";
						throw;
					}
				}

				return RedirectToAction(nameof(Index));
			}

			return View(category);
		}

		#region API CALL

		[HttpGet]
		public async Task<IActionResult> GetAllCategories([FromQuery] string selectedStatus) //int currentPage = 1, int pageSize = 10)
		{

			var categories = await _categoryService.GetAllCategoriesAsync(); /* (currentPage,pageSize);*/

			//         var totalCount = categories.Count();

			//         CategoriesVM categoriesVM = new()
			//         {
			//             Categories = categories,
			//             TotalItems = totalCount,
			//             CurrentPage = currentPage,
			//             TotalPages = totalCount / pageSize
			//         };

			//return View(categoriesVM);

			//return View(categories);

			if (selectedStatus?.ToUpper() != "ALL")
			{
				categories = categories.Where(c => c.Status == selectedStatus);
			}

			return Json(new { data = categories });
		}

		// POST: Admin/Category/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]

		[HttpDelete]
		public async Task<IActionResult> Delete(int id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);

			if (category != null)
			{
				await _categoryService.DeleteCategoryAsync(id);
			}

			return Json(new { success = true , message = "Category successfully deleted." });
		}

		[HttpPost]
		public async Task<IActionResult> UpdateStatus([FromBody] int id)
		{
			if (id == 0)
			{
				return NotFound();
			}

			var category = await _categoryService.GetCategoryByIdAsync(id);

			if (category == null)
			{
				return Json(new { success = false, message = "Category not found." });
			}

			try
			{
				if (category.Status == StaticDetails.STATUS_VALID)
				{
					category.Status = StaticDetails.STATUS_INVALID;
				}
				else
				{
					category.Status = StaticDetails.STATUS_VALID;
				}

				await _categoryService.UpdateCategoryAsync(category);
			}
			catch (DbUpdateConcurrencyException)
			{

				TempData["error"] = "Update failed, due to concurrency issue. Please try again.";
				// Consider logging the exception for more details
				throw;
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = $"An unexpected error occurred: {ex.Message}." });
				// Log the exception for further investigation
			}

			return Json(new { success = true, message = "Category Status successfully updated." });
		}

		#endregion

	}
}

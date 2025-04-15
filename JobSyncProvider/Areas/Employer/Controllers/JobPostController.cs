using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobSyncProvider.DataAccess.Data;
using JobSyncProvider.Models;

namespace JobSyncProvider.Areas.Employer.Controllers
{
    [Area("Employer")]
    public class JobPostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employer/JobPost
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.JobPosts.Include(j => j.Category).Include(j => j.Company).Include(j => j.Employer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Employer/JobPost/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts
                .Include(j => j.Category)
                .Include(j => j.Company)
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(m => m.JobPostId == id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return View(jobPost);
        }

        // GET: Employer/JobPost/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId");
            ViewData["EmployerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Employer/JobPost/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobPostId,Title,Description,Location,Salary,EmployerId,CategoryId,CompanyId,JobStatus")] JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", jobPost.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", jobPost.CompanyId);
            ViewData["EmployerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", jobPost.EmployerId);
            return View(jobPost);
        }

        // GET: Employer/JobPost/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", jobPost.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", jobPost.CompanyId);
            ViewData["EmployerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", jobPost.EmployerId);
            return View(jobPost);
        }

        // POST: Employer/JobPost/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobPostId,Title,Description,Location,Salary,EmployerId,CategoryId,CompanyId,JobStatus")] JobPost jobPost)
        {
            if (id != jobPost.JobPostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobPostExists(jobPost.JobPostId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", jobPost.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Companies, "CompanyId", "CompanyId", jobPost.CompanyId);
            ViewData["EmployerId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", jobPost.EmployerId);
            return View(jobPost);
        }

        // GET: Employer/JobPost/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPost = await _context.JobPosts
                .Include(j => j.Category)
                .Include(j => j.Company)
                .Include(j => j.Employer)
                .FirstOrDefaultAsync(m => m.JobPostId == id);
            if (jobPost == null)
            {
                return NotFound();
            }

            return View(jobPost);
        }

        // POST: Employer/JobPost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobPost = await _context.JobPosts.FindAsync(id);
            if (jobPost != null)
            {
                _context.JobPosts.Remove(jobPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobPostExists(int id)
        {
            return _context.JobPosts.Any(e => e.JobPostId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Data;
using PeninsulaPhysiotherapy.Models;

namespace PeninsulaPhysiotherapy.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
              return _context.JobVM != null ? 
                          View(await _context.JobVM.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.JobVM'  is null.");
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.JobVM == null)
            {
                return NotFound();
            }

            var jobVM = await _context.JobVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobVM == null)
            {
                return NotFound();
            }

            return View(jobVM);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustName,CustPhone,Gender,DateAndTime,Therapist,JobType,JobStatus")] JobVM jobVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobVM);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.JobVM == null)
            {
                return NotFound();
            }

            var jobVM = await _context.JobVM.FindAsync(id);
            if (jobVM == null)
            {
                return NotFound();
            }
            return View(jobVM);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,CustName,CustPhone,Gender,DateAndTime,Therapist,JobType,JobStatus")] JobVM jobVM)
        {
            if (id != jobVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobVMExists(jobVM.Id))
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
            return View(jobVM);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.JobVM == null)
            {
                return NotFound();
            }

            var jobVM = await _context.JobVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobVM == null)
            {
                return NotFound();
            }

            return View(jobVM);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.JobVM == null)
            {
                return Problem("Entity set 'ApplicationDbContext.JobVM'  is null.");
            }
            var jobVM = await _context.JobVM.FindAsync(id);
            if (jobVM != null)
            {
                _context.JobVM.Remove(jobVM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobVMExists(int? id)
        {
          return (_context.JobVM?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

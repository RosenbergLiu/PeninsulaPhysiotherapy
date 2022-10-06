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
    public class FeedbackVMsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackVMsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FeedbackVMs
        public async Task<IActionResult> Index()
        {
              return View(await _context.FeedbackVM.ToListAsync());
        }

        // GET: FeedbackVMs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FeedbackVM == null)
            {
                return NotFound();
            }

            var feedbackVM = await _context.FeedbackVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackVM == null)
            {
                return NotFound();
            }

            return View(feedbackVM);
        }

        // GET: FeedbackVMs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FeedbackVMs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Rating,CommentText,CommentDate,CommentBy")] FeedbackVM feedbackVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedbackVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedbackVM);
        }

        // GET: FeedbackVMs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FeedbackVM == null)
            {
                return NotFound();
            }

            var feedbackVM = await _context.FeedbackVM.FindAsync(id);
            if (feedbackVM == null)
            {
                return NotFound();
            }
            return View(feedbackVM);
        }

        // POST: FeedbackVMs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rating,CommentText,CommentDate,CommentBy")] FeedbackVM feedbackVM)
        {
            if (id != feedbackVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedbackVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedbackVMExists(feedbackVM.Id))
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
            return View(feedbackVM);
        }

        // GET: FeedbackVMs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FeedbackVM == null)
            {
                return NotFound();
            }

            var feedbackVM = await _context.FeedbackVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedbackVM == null)
            {
                return NotFound();
            }

            return View(feedbackVM);
        }

        // POST: FeedbackVMs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FeedbackVM == null)
            {
                return Problem("Entity set 'ApplicationDbContext.FeedbackVM'  is null.");
            }
            var feedbackVM = await _context.FeedbackVM.FindAsync(id);
            if (feedbackVM != null)
            {
                _context.FeedbackVM.Remove(feedbackVM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedbackVMExists(int id)
        {
          return _context.FeedbackVM.Any(e => e.Id == id);
        }
    }
}

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
    public class TherapistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TherapistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Therapists
        public async Task<IActionResult> Index()
        {
              return _context.TherapistVM != null ? 
                          View(await _context.TherapistVM.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TherapistVM'  is null.");
        }

        // GET: Therapists/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.TherapistVM == null)
            {
                return NotFound();
            }

            var therapistVM = await _context.TherapistVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (therapistVM == null)
            {
                return NotFound();
            }

            return View(therapistVM);
        }

        // GET: Therapists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Therapists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Level,Email,Phone")] TherapistVM therapistVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(therapistVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(therapistVM);
        }

        // GET: Therapists/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TherapistVM == null)
            {
                return NotFound();
            }

            var therapistVM = await _context.TherapistVM.FindAsync(id);
            if (therapistVM == null)
            {
                return NotFound();
            }
            return View(therapistVM);
        }

        // POST: Therapists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Level,Email,Phone")] TherapistVM therapistVM)
        {
            if (id != therapistVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(therapistVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TherapistVMExists(therapistVM.Id))
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
            return View(therapistVM);
        }

        // GET: Therapists/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.TherapistVM == null)
            {
                return NotFound();
            }

            var therapistVM = await _context.TherapistVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (therapistVM == null)
            {
                return NotFound();
            }

            return View(therapistVM);
        }

        // POST: Therapists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TherapistVM == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TherapistVM'  is null.");
            }
            var therapistVM = await _context.TherapistVM.FindAsync(id);
            if (therapistVM != null)
            {
                _context.TherapistVM.Remove(therapistVM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TherapistVMExists(string id)
        {
          return (_context.TherapistVM?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

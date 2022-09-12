using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> userManager;

        public TherapistsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
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
                .FirstOrDefaultAsync(m => m.Email == id);
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
        public async Task<IActionResult> Create([Bind("Email,FullName,Level,Phone")] TherapistVM therapistVM)
        {
            
            var user = await userManager.FindByEmailAsync(therapistVM.Email);
            
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "email not regiested");
                return View();
            }
            if (_context.TherapistVM != null)
            {
                var therapist = await _context.TherapistVM.FindAsync(therapistVM.Email);
                if (therapist != null)
                {
                    ModelState.AddModelError(string.Empty, "email already regiested as a therapist");
                    return View();
                }
            }
            
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
        public async Task<IActionResult> Edit(string id, [Bind("Email,FullName,Level,Phone")] TherapistVM therapistVM)
        {
            if (id != therapistVM.Email)
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
                    if (!TherapistVMExists(therapistVM.Email))
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
                .FirstOrDefaultAsync(m => m.Email == id);
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
          return (_context.TherapistVM?.Any(e => e.Email == id)).GetValueOrDefault();
        }
    }
}

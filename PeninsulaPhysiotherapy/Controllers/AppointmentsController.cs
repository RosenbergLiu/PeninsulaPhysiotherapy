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
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
              return _context.AppointmentVM != null ? 
                          View(await _context.AppointmentVM.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.AppointmentVM'  is null.");
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AppointmentVM == null)
            {
                return NotFound();
            }

            var appointmentVM = await _context.AppointmentVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentVM == null)
            {
                return NotFound();
            }

            return View(appointmentVM);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Gender,Phone,AppDate,Therapist,JobType")] AppointmentVM appointmentVM)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentVM);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AppointmentVM == null)
            {
                return NotFound();
            }

            var appointmentVM = await _context.AppointmentVM.FindAsync(id);
            if (appointmentVM == null)
            {
                return NotFound();
            }
            return View(appointmentVM);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Gender,Phone,AppDate,Therapist,JobType")] AppointmentVM appointmentVM)
        {
            if (id != appointmentVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentVM);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentVMExists(appointmentVM.Id))
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
            return View(appointmentVM);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AppointmentVM == null)
            {
                return NotFound();
            }

            var appointmentVM = await _context.AppointmentVM
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentVM == null)
            {
                return NotFound();
            }

            return View(appointmentVM);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AppointmentVM == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AppointmentVM'  is null.");
            }
            var appointmentVM = await _context.AppointmentVM.FindAsync(id);
            if (appointmentVM != null)
            {
                _context.AppointmentVM.Remove(appointmentVM);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentVMExists(int id)
        {
          return (_context.AppointmentVM?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

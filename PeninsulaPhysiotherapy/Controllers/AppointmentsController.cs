using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Data;
using PeninsulaPhysiotherapy.Models;
using PeninsulaPhysiotherapy.Services;
using SendGrid.Helpers.Mail;

namespace PeninsulaPhysiotherapy.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailSender _emailSender;

        public AppointmentsController(
            ApplicationDbContext context, UserManager<AppUser> userManager,IEmailSender emailSender)
        {
            _context = context;
            this.userManager = userManager;
            this._emailSender = emailSender;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var loggedUserName = User.FindFirstValue(ClaimTypes.Name);
            ViewBag.CreateBy = loggedUserName;
            ViewBag.Therapists = new List<AppUser>();
            ViewBag.TherapistName = new Dictionary<string, string>();
            foreach (var user in userManager.Users)
            {
                (ViewBag.TherapistName)[user.Email] = user.UserName;

                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);
                }
            }
            





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
        public async Task<IActionResult> Create()
        {
            ViewBag.Therapists = new List<AppUser>();
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);
                }
            }

            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Gender,Phone,AppDate,Therapist,JobType")] AppointmentVM appointmentVM)
        {
            ViewBag.Therapists = new List<AppUser>();
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);
                }
            }
            if (ModelState.IsValid)
            {
                appointmentVM.JobStatus = "Submited";
                appointmentVM.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                _context.Add(appointmentVM);
                await _context.SaveChangesAsync();
                await _emailSender.SendEmailAsync(User.FindFirstValue(ClaimTypes.Email).ToString(), "Appointment Submited", appointmentVM.AppDate.ToString());
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
        [RequireHttps]
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
                    appointmentVM.JobStatus = "Submited";
                    appointmentVM.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
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

        public async Task<IActionResult> Approve(int id)
        {
            
            var appointmentVM = await _context.AppointmentVM.FindAsync(id);
            if (appointmentVM == null)
            {
                return NotFound();
            }
            appointmentVM.JobStatus = "Approved";
            _context.Update(appointmentVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Reject(int id)
        {
            var appointmentVM = await _context.AppointmentVM.FindAsync(id);
            if (appointmentVM == null)
            {
                return NotFound();
            }
            appointmentVM.JobStatus = "Reject";
            _context.Update(appointmentVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Finish(int id)
        {
            var appointmentVM = await _context.AppointmentVM.FindAsync(id);
            if (appointmentVM == null)
            {
                return NotFound();
            }
            appointmentVM.JobStatus = "Finished";
            _context.Update(appointmentVM);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}

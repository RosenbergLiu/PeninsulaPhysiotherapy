using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Data;
using PeninsulaPhysiotherapy.Models;
using System.Security.Claims;

namespace PeninsulaPhysiotherapy.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailSender _emailSender;

        public AppointmentsController(
            ApplicationDbContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
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
            string resources = "";

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);

                    resources = resources + "," + user.UserName.ToString();
                }
            }
            string events = "";
            if (_context.AppointmentVM != null)
            {
                var appointments = await _context.AppointmentVM.ToListAsync();

                foreach (var appointment in appointments)
                {
                    string title = "Booked";
                    string start = appointment.AppDate.Date.ToString("yyyy-MM-dd") + "T" + appointment.AppDate.TimeOfDay;
                    string end = appointment.AppDate.AddHours(1).Date.ToString("yyyy-MM-dd") + "T" + appointment.AppDate.AddHours(1).TimeOfDay;
                    string resourceId = "";

                    var therapistEmail = appointment.Therapist;
                    var TherapistUser = await userManager.FindByEmailAsync(therapistEmail);
                    if (TherapistUser != null)
                    {
                        resourceId = TherapistUser.UserName;
                    }
                    string record = title + "," + start + "," + end + "," + resourceId;
                    events = events + ";" + record;
                };
            }
            if (events.Length > 0)
            {
                ViewBag.Events = events.Substring(1, events.Length - 1);
            }


            if (resources.Length > 0)
            {
                ViewBag.Resources = resources.Substring(1, resources.Length - 1);
            }


            return View();
        }


        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Gender,Phone,SelectedDate,Therapist,JobType")] AppointmentVM appointmentVM)
        {
            ViewBag.Therapists = new List<AppUser>();
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);
                }
            }
            //Date not null
            if (appointmentVM.SelectedDate == "")
            {
                
                RedirectToAction(nameof(Create));
                ModelState.AddModelError(string.Empty, "Please select a time");
            }
            
            //Post
            if (ModelState.IsValid)
            {
                appointmentVM.JobStatus = "Submited";
                appointmentVM.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                appointmentVM.AppDate = DateTimeConverter(appointmentVM.SelectedDate);
                var SelectedTherapist = await userManager.FindByNameAsync(appointmentVM.Therapist);
                appointmentVM.Therapist = SelectedTherapist.Email;
                _context.Add(appointmentVM);
                await _context.SaveChangesAsync();
                await _emailSender.SendEmailAsync(User.FindFirstValue(ClaimTypes.Email).ToString(), "Appointment Submited", appointmentVM.AppDate.ToString());
                return RedirectToAction(nameof(Index));
            }
            return View(appointmentVM);
        }

        public DateTime DateTimeConverter(string dateFromCal)
        {
            var dateOnly = dateFromCal.Split('T')[0].Split('-');
            int year = int.Parse(dateOnly[0]);
            int month = int.Parse(dateOnly[1]);
            int day = int.Parse(dateOnly[2]);
            var timeOnly = dateFromCal.Split('T')[1].Split('+')[0].Split(':');
            Console.WriteLine(dateOnly);
            Console.WriteLine(timeOnly);
            int hour = int.Parse(timeOnly[0]);
            int minute = int.Parse(timeOnly[1]);
            int second = int.Parse(timeOnly[2]);
            var outDate = new DateTime(year, month, day, hour, minute, second);

            return outDate;
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
            else
            {
                var TherapistUser = await userManager.FindByEmailAsync(appointmentVM.Therapist);
                appointmentVM.Therapist = TherapistUser.UserName;
            }
            ViewBag.Therapists = new List<AppUser>();
            string resources = "";

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Therapist"))
                {
                    ViewBag.Therapists.Add(user);
                    resources = resources + "," + user.UserName.ToString();
                }
            }
            string events = "";
            if (_context.AppointmentVM != null)
            {
                var appointments = await _context.AppointmentVM.ToListAsync();

                foreach (var appointment in appointments)
                {
                    string title = "Booked";
                    string start = appointment.AppDate.Date.ToString("yyyy-MM-dd") + "T" + appointment.AppDate.TimeOfDay;
                    string end = appointment.AppDate.AddHours(1).Date.ToString("yyyy-MM-dd") + "T" + appointment.AppDate.AddHours(1).TimeOfDay;
                    string resourceId = "";
                    var therapistEmail = appointment.Therapist;
                    var TherapistUser = await userManager.FindByEmailAsync(therapistEmail);
                    if (TherapistUser != null)
                    {
                        resourceId = TherapistUser.UserName;
                    }
                    else
                    {
                        resourceId = therapistEmail;
                    }
                    string record = title + "," + start + "," + end + "," + resourceId;
                    events = events + ";" + record;
                };
            }

            if (events.Length > 0)
            {
                ViewBag.Events = events.Substring(1, events.Length - 1);
            }
            if (resources.Length > 0)
            {
                ViewBag.Resources = resources.Substring(1, resources.Length - 1);
            }


            return View(appointmentVM);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttps]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Gender,Phone,SelectedDate,Therapist,JobType")] AppointmentVM appointmentVM)
        {
            if (id != appointmentVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appointmentVM.AppDate = DateTimeConverter(appointmentVM.SelectedDate);
                    appointmentVM.JobStatus = "Submited";
                    appointmentVM.CreatedBy = User.FindFirstValue(ClaimTypes.Name);
                    var SelectedTherapist = await userManager.FindByNameAsync(appointmentVM.Therapist);
                    appointmentVM.Therapist = SelectedTherapist.Email;
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

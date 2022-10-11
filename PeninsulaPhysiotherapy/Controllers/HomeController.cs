using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeninsulaPhysiotherapy.Data;
using PeninsulaPhysiotherapy.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PeninsulaPhysiotherapy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.CommentList = null;
            if (_context.FeedbackVM != null)
            {
                var CommentList = await _context.FeedbackVM.ToListAsync();
                ViewBag.CommentList = CommentList;
            }
            return View();
        }


        [Authorize]
        public IActionResult Feedback()
        {

            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Feedback([Bind("Id,Rating,CommentText,CommentDate,CommentBy")] FeedbackVM feedbackVM)
        {
            if (feedbackVM.Rating == 0)
            {
                ModelState.AddModelError(string.Empty, "Please make a rating");
            }
            if (ModelState.IsValid)
            {
                feedbackVM.CommentBy = User.FindFirstValue(ClaimTypes.Name);
                feedbackVM.CommentDate = DateTime.Now;
                _context.Add(feedbackVM);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedbackVM);
        }






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
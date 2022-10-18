using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PeninsulaPhysiotherapy.Data;
using PeninsulaPhysiotherapy.Models;
using PeninsulaPhysiotherapy.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using System.Security.Claims;

namespace PeninsulaPhysiotherapy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> userManager;
        private readonly IFileUploadService _uploadService;
        private readonly IEmailSender _emailSender;


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager, IFileUploadService uploadService, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            this.userManager = userManager;
            _uploadService = uploadService;
            _emailSender = emailSender;
        }


        public IActionResult Files()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.CommentList = null;
            if (_context.FeedbackVM != null)
            {
                var CommentList = await _context.FeedbackVM.ToListAsync();
                ViewBag.CommentList = CommentList;
            }

            List<DataPoint> dataPoints = new List<DataPoint>();
            var commentList = await _context.FeedbackVM.ToListAsync();
            ViewBag.commentCount = commentList.Count;
            int ratingSum = 0;
            int ratingSum1 = 0;
            int ratingSum2 = 0;
            int ratingSum3 = 0;
            int ratingSum4 = 0;
            int ratingSum5 = 0;
            foreach (var comment in commentList)
            {
                ratingSum += comment.Rating;
                if (comment.Rating == 1) { ratingSum1 += 1; }
                if (comment.Rating == 2) { ratingSum2 += 1; }
                if (comment.Rating == 3) { ratingSum3 += 1; }
                if (comment.Rating == 4) { ratingSum4 += 1; }
                if (comment.Rating == 5) { ratingSum5 += 1; }
            }

            int ratingAvg = ratingSum / (commentList.Count);

            string ratingState = $"{commentList.Count} people have reviewed us as average of {ratingAvg} stars";
            ViewBag.ratingState = ratingState;
            dataPoints.Add(new DataPoint("1 Star", ratingSum1));
            dataPoints.Add(new DataPoint("2 Stars", ratingSum2));
            dataPoints.Add(new DataPoint("3 Stars", ratingSum3));
            dataPoints.Add(new DataPoint("4 Stars", ratingSum4));
            dataPoints.Add(new DataPoint("5 Stars", ratingSum5));
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

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
            var loggedUser = await userManager.FindByNameAsync(User.FindFirstValue(ClaimTypes.Name));

            if (ModelState.IsValid)
            {
                if (loggedUser.rated)
                {
                    var feedbackList = await _context.FeedbackVM.ToListAsync();
                    foreach (var f in feedbackList)
                    {
                        if (f.CommentBy == loggedUser.Id)
                        {
                            _context.FeedbackVM.Remove(f);
                        }
                    }
                }
                feedbackVM.CommentBy = loggedUser.Id;
                feedbackVM.CommentDate = DateTime.Now;
                _context.Add(feedbackVM);
                await _context.SaveChangesAsync();
                loggedUser.rated = true;
                await userManager.UpdateAsync(loggedUser);




                return RedirectToAction(nameof(Index));
            }
            return View(feedbackVM);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [RequireHttps]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Files(IFormFile file)
        {
            if (file != null)
            {
                await _uploadService.UploadFileAsync(file);
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SendBulkEmail()
        {
            var users = await userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                await _emailSender.SendEmailAsync(
                    user.Email.ToString(),
                    "25% discount from Peninsula Physiotherapy",
                    "@We are celebrating 2-year anniversary. This is a 25% off voucher from us to thank you for supporting us."
                    
                    );
                
            }
            return RedirectToAction(nameof(Files));
        }
        



    }
}
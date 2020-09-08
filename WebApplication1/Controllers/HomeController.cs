using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Data;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, 
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;



        }

        public async Task<IActionResult> Index()
        {
            //Conference c = new Conference();
            //List<Conference> lst = new List<Conference>();
            //lst.Add(c);
            //return View(lst); ;
            return View(await db.Conferences.ToListAsync());
        }

        public async Task<IActionResult> GoToConf(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
             
                
                db.UserGoConfs.Add(new UserGoConf { ConferenceId = id, 
                    ApplicationUser = await _userManager.FindByEmailAsync(User.Identity.Name) });
                db.SaveChanges();
                return Content("Добавляем ");
            }


            //Identity / Account / Login
            return Redirect("~/Identity/Account/Login?ReturnUrl=~/Home/Details/"+id);
        }

        // GET: Home/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await db.Conferences.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p=>p.Id==id));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

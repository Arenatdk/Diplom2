using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Data;
using WebApplication1.Models;


namespace WebApplication1.Controllers
{
    public class ConferenceSettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ILogger<ConferenceSettingsController> _logger;
        private ApplicationDbContext db;
        private IWebHostEnvironment _appEnvironment;

        public ConferenceSettingsController(ILogger<ConferenceSettingsController> 
            logger, ApplicationDbContext context , 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment appEnvironment)
        {
            _logger = logger;
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
        }

        public IActionResult CreateConf() // создать конфу
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateConf(Conference Conference)
        {
            Conference.ApplicationUser = await _userManager.GetUserAsync(HttpContext.User);
            db.Conferences.Add(Conference);
            await db.SaveChangesAsync();
            return RedirectToAction("MyConfList");
        }



        // GET: ConferenceSettings
        public async Task<ActionResult> MyConfList()
        {
            string UserF =  _userManager.GetUserId(User);
            return View(await db.Conferences.Where(p => p.ApplicationUserId == UserF).ToListAsync());
            //return View();
        }

        public async Task<ActionResult> Myfiles(int id)
        {
            ViewData["id"] = id;

            return View(db.MyFiles.Where(p=>p.ConferenceId==id).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(FileViewModel uploadedform)
        {
            
            if (uploadedform.File != null)
            {
                long ina = uploadedform.File.Length;
                ina = ina / 1024;
                //ina = ina / 1024; //mb
                string fsize = "";
                if( ina / 1024!=0 )
                {
                    fsize = (ina / 1024).ToString() + " mb"; // mb
                    
                }
                else
                {
                    fsize = ina.ToString() + " kb";

                }


                string unique = Guid.NewGuid().ToString();
                // путь к папке Files
                string path = "/Files/" + unique; 
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedform.File.CopyToAsync(fileStream);
                }
                Conference cf = db.Conferences.Find(uploadedform.ConferenceId);
                MyFile file = new MyFile {Size=fsize, Name = uploadedform.File.FileName, Path = unique, Conference = cf,  ConferenceId = uploadedform.ConferenceId };
                db.MyFiles.Add(file);
                db.SaveChanges();
            }

            return Redirect("~/ConferenceSettings/Myfiles/" + uploadedform.ConferenceId); 
        }


        public VirtualFileResult GetVirtualFile(string key)
        {
            MyFile mf =  db.MyFiles.First(p=>p.Path==key);
            var filepath = Path.Combine("~/Files", key);
            return File(filepath, "application/octet-stream", mf.Name);
        }

        public IActionResult DeleteFile(string key,int id)
        {
            MyFile mf = db.MyFiles.First(p => p.Path == key);
            System.IO.File.Delete(Path.Combine("wwwroot/Files", key));

            db.MyFiles.Remove(mf);
            db.SaveChanges();
            return Redirect("~/ConferenceSettings/Myfiles/" + id);
        }

        public async Task<ActionResult> Members(int id) //Учасники
        {
            ViewData["id"] = id;
            var uuu = db.UserGoConfs.Include(p=>p.ApplicationUser).Where(p => p.ConferenceId == id).Select(p=>p.ApplicationUser).ToList();
            return View( uuu);
            //return View();
        }

        public async Task<ActionResult> TimeTable(int id) 
        {
            ViewData["id"] = id;
            
            return View(await db.CalendarEvents.Where(p => p.ConferenceId == id).ToListAsync());
            //return View();
        }

        public async Task<JsonResult> GetCalendarEvent(int id)
        {
            
            return new JsonResult(await db.CalendarEvents.Where(p => p.ConferenceId == id).ToListAsync());
            //return View();
        }
        public async Task<JsonResult> DeleteCalendarEvent(int id) 
        {
            CalendarEvent ce = db.CalendarEvents.Find(id);
            db.CalendarEvents.Remove(ce);
            await db.SaveChangesAsync();
            return new JsonResult("ok");
            //return View();
        }


        public async Task<JsonResult> CreateCalendarEvent(CalendarEvent ce)
        {



            ce.Conference = db.Conferences.Find(ce.ConferenceId);
            db.CalendarEvents.Add(ce);
            await db.SaveChangesAsync();
            return new JsonResult(ce.Id);
            //return View();
        }


        // GET: ConferenceSettings/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ViewData["id"] = id;
            return View(await db.Conferences.FirstOrDefaultAsync(p => p.Id==id));
        }

        // GET: ConferenceSettings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConferenceSettings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ConferenceSettings/Edit/5
        public async Task<ActionResult> EditConf(int id)
        {
            ViewBag.id = id;
            return View(await db.Conferences.Include(x => x.ApplicationUser).FirstOrDefaultAsync(p => p.Id == id));
        }

        // POST: ConferenceSettings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConf(Conference cf)
        {
            try
            {
                Conference temp1 =  db.Conferences.Find(cf.Id);
                cf.ApplicationUser = temp1.ApplicationUser;
                cf.ApplicationUserId = temp1.ApplicationUserId;
                // TODO: Add update logic here
                db.Conferences.Update(cf);
                db.SaveChanges();
                return Redirect("~/ConferenceSettings/Details/" +cf.Id);
            }
            catch
            {
                return View();
            }
        }

        // GET: ConferenceSettings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConferenceSettings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            if (id != null)
            {
                Conference conf = await db.Conferences.FirstOrDefaultAsync(p => p.Id == id);
                if (conf != null)
                {
                    db.Conferences.Remove(conf);
                    await db.SaveChangesAsync();
                    return RedirectToAction("MyConfList");
                }
            }
            return NotFound();
        }
    }
}
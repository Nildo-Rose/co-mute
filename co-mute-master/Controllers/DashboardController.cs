using co_mute_master.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace co_mute_master.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : Controller
    {
        private DbEntities db = new DbEntities();

        public DashboardController(DbEntities _db) { db = _db; }


        [Authorize(Roles = "User", AuthenticationSchemes = "User_Schema")]
        [HttpGet]
        [Route("index")]
        public IActionResult Index()
        {
            return View("index");
        }

        [Authorize(Roles = "User", AuthenticationSchemes = "User_Schema")]
        [Route("")]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var profileName = db.Registers.SingleOrDefault(a => a.Email.Equals(user.Value));
            ViewBag.PersonalDetail = db.Registers.Where(a => a.Id == profileName.Id).ToList();
            return View();
        }

        [HttpGet]
        [Route("editprofile/{id}")]
        public IActionResult EditProfile(int id)
        {
           
            var profileFile = db.Registers.Find(id);
            return View("EditProfile", profileFile);
        }

        [HttpPost]
        [Route("editprofile/{id}")]
        public IActionResult EditProfile(int id, Register user)
        {
            try
            {
                var currentUser = db.Registers.Find(id);
                currentUser.Name = user.Name;
                currentUser.Surname = user.Surname;
                currentUser.Phone = user.Phone;
                db.SaveChanges();
                return RedirectToAction("profile", "Dashboard");

            }
            catch (Exception e)
            { TempData["error"] = e.Message; }
       

            
            return View("EditProfile");
           
        }

        [HttpGet]
        [Route("deleteprofile/{id}")]
        public IActionResult DeleteProfile(int id)
        {
            try
            {
                var userRole = db.UserRoles.Find(id);
                db.UserRoles.Remove(userRole);
                db.SaveChanges();

                var profile = db.Registers.Find(id);
                db.Registers.Remove(profile);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }
            return RedirectToAction("profile", "Dashboard"); 
        }

    }
}

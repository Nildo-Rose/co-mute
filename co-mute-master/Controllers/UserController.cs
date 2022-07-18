using co_mute_master.Models;
using co_mute_master.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace co_mute_master.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private DbEntities db = new DbEntities();
        private SecurityManager securityManager = new SecurityManager();

        public UserController(DbEntities _db) { db = _db; }

        [HttpGet]
        [Route("register")]
        public ActionResult Register()
        {
            var userAccount = new Register();
            return View("Register", userAccount);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Register user)
        {
            if (!ModelState.IsValid) return View("register");

            try
            {
                var exists = db.Registers.Count(a => a.Email.Equals(user.Email)) > 0;
                var userId = db.Registers.Find(user.Id);
                if (!exists)
                {
                    user.Password = BCrypt.Net.BCrypt.HashString(user.Password.Trim());
                    user.Status = true;
                    db.Registers.Add(user);
                    db.SaveChanges();

                    //Defining the role that the user bellongs to.
                    var userRole = new UserRole() { RoleId = 1, UserId = user.Id };
                    db.UserRoles.Add(userRole);
                    db.SaveChanges();
                    return RedirectToAction("login");
                }
                else
                {
                    ViewBag.Error = "this account already exist please register if you don't have any account or click on forgot password";
                    return View("register");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message.ToString();
            }

            return View("register");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            var userAccount = processLogin(email, password);
            if (userAccount != null)
            {
                securityManager.SignIn(this.HttpContext, userAccount, "User_Schema");
                return RedirectToAction("index", "Dashboard");
            }
            else
            {
                ViewBag.Error = "Invalid user account please register if you don't have an account!!!";
                return View("Login");
            }
        }
        private Register processLogin(string email, string password)
        {
            var userAccount = db.Registers.SingleOrDefault(a => a.Email.Equals(email) && a.Status == true);
            if (userAccount != null)
            {
                var roleAccount = userAccount.UserRoles.FirstOrDefault();
                if (roleAccount.RoleId == 1 && BCrypt.Net.BCrypt.Verify(password, userAccount.Password))
                {
                    return userAccount;
                }
            }
            return null;
        }
        [Authorize(Roles = "User", AuthenticationSchemes = "User_Schema")]
        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext, "User_Schema");
            return RedirectToAction("login", "user");

        }
        [HttpGet]
        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}

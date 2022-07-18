using co_mute_master.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace co_mute_master.Controllers
{
    [Route("carpool")]
    public class CarpoolController : Controller
    {
        private DbEntities db = new DbEntities();
        public CarpoolController(DbEntities _db) { db = _db; }
        private readonly Random _random = new Random();

        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.CarpoolOpportunity = db.CarPools.ToList();
            ViewBag.Msg = TempData["error"];
            ViewBag.Success = TempData["success"];
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var carpool = new CarPool();
            return View("Add", carpool);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(CarPool carPool)
        {
           

            try
            {
                
                db.CarPools.Add(carPool);
                db.SaveChanges();
                return RedirectToAction("index", "carpool");
            }
            catch (Exception e)
            { TempData["error"] = e.Message; }

            return View("Add");
        }

        [HttpGet]
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            ViewBag.CarpoolDetails = db.CarPools.Find(id);
            return View("Details");
        }

        //Processing to join the car-pool opportunity
        [HttpPost]
        [Route("processjoin")]
        public IActionResult ProcessJoin(int id, JoinLeaveOpp joinOpportunity)
        {


          
            var user = User.FindFirst(ClaimTypes.Name);
            var joinUser = db.Registers.SingleOrDefault(a => a.Email.Equals(user.Value));
            var joinLeave = db.CarPools.Find(id);
            var j = db.JoinLeaveOpps.Find(id);


            joinOpportunity.Id = _random.Next(1, 10000);
            joinOpportunity.RegId = joinUser.Id;
            joinOpportunity.CarOppId = joinLeave.Id;
            joinOpportunity.DateJoined = DateTime.Now;
            joinOpportunity.LeaveJoine = true;
            if (joinOpportunity.LeaveJoine == true)
            {
                db.JoinLeaveOpps.Add(joinOpportunity);
                db.SaveChanges();
                TempData["success"] = "You've successfully joined the car-pool opportunity!!!!";
                return RedirectToAction("index", "carpool");
            }
            else if (joinOpportunity.LeaveJoine == false) 
            {
                joinOpportunity.LeaveJoine = true;
                db.SaveChanges();
                TempData["success"] = "You've successfully joined the car-pool opportunity!!!!";
                return RedirectToAction("index", "carpool");
            }
            else
            {
                TempData["error"] = "You've already joined the car-pool opportunity you can't join this twice!!!!";
                return RedirectToAction("index", "carpool"); 
            }  

        }

        //Processing to Leave the car-pool Opportunity
        [HttpPost]
        [Route("processleave")]
        public IActionResult ProcessLeave(int id, JoinLeaveOpp joinOpportunity)
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var joinUser = db.Registers.SingleOrDefault(a => a.Email.Equals(user.Value));
            var joinLeave = db.CarPools.Find(id);

            if (joinLeave.Id != joinOpportunity.CarOppId || joinOpportunity.LeaveJoine == null || joinOpportunity.LeaveJoine == true)
            {
                //joinOpportunity.RegId = joinUser.Id;
                //joinOpportunity.CarId = joinLeave.Id;
                //joinOpportunity.DateJoined = DateTime.Now;
                joinOpportunity.LeaveJoine = false;
                //db.JoinOpportunities.Add(joinOpportunity);
                db.SaveChanges();
                return RedirectToAction("index", "carpool");
            }
            else
            {
                TempData["error"] = "Your already joined the car-pool opportunity you can't join this twice";
                return View("Details");
            }
            


        }

        //All car-pool joined opportunity
        [Route("jonedopportunity")]
        public IActionResult JoinOpportunity()
        {

            ViewBag.CarpoolOpportunity = db.CarPools.ToList();
            return View();
        }


    }
}

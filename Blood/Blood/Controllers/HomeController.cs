using Blood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace Blood.Controllers
{
    public class HomeController : Controller
    {
        private bloodinformation db = new bloodinformation();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Include = "D_Id,Doner_Name,Password,Address,Email,Phone, Date,Blood")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
            ModelState.Clear();
            ViewBag.Message = user.Doner_Name + " Registration successfully";

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {          
                var Details = db.Users.FirstOrDefault(x => x.Doner_Name == user.Doner_Name && x.Password == user.Password);
                if (Details != null)
                {
                    Session["D_Id"] = Details.D_Id.ToString();

                    return RedirectToAction("Dashbord");
                }
                else
                {
                    ViewBag.Message = "Doner_Name && Password is invalid .";
                }
            
            return View();
        }
        public ActionResult Dashbord()
        {
            if(Session["D_Id"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }


        public ActionResult UserInformation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserInformation(User info)
        {
            List<User> details = db.Users.ToList().
                Where(x => x.Doner_Name == info.Doner_Name && x.Password == info.Password).Select(s => new User
                {
                    Doner_Name = s.Doner_Name,
                    Address = s.Address,
                    Email = s.Email,
                    Phone = s.Phone,
                    Date = s.Date,
                    Blood = s.Blood

                }).ToList();

            TempData["details"] = details;

            if (details != null)
            {
                return RedirectToAction("UserGetdonerInfo");
            }
            else
            {
                ViewBag.Message = "Not Found";
            }

            return View();
        }

        public ActionResult UserGetdonerInfo()
        {
            var IModel = (List<User>)TempData["details"];
            return View(IModel);
        }


        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Doner_NamePassword,,Address,Email,Phone,Date,Blood")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UserGetdonerInfo");
            }
            return View(user);
        }


    }
}
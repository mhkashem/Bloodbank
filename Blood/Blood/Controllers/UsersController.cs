using Blood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blood.Controllers
{
    public class UsersController : Controller
    {
        private bloodinformation db = new bloodinformation();


        public ActionResult Show()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Information()
        {
            return View();
        }
 

        [HttpPost]
        public ActionResult Information(User info)
        {
            List<User> details = db.Users.ToList().
                Where(x => x.Address == info.Address && x.Blood == info.Blood).Select(s => new User
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
                return RedirectToAction("DonerInfo");
            }
            else
            {
                ViewBag.Message = "Not Found";
            }

            return View();
        }

        public ActionResult DonerInfo()
        {
            var IModel = (List<User>)TempData["details"];
            return View(IModel);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
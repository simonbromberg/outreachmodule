using OutreachModule.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OutreachModule.Controllers
{
    public class HomeController : Controller
    {
        private OutreachDBEntities _entities = new OutreachDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.CampList = new SelectList(_entities.Camps, "Id", "selectRow");
            return View();
        }

        public ActionResult Go(int selectedId)
        {
            return RedirectToAction("Details", "Camps",new {id = selectedId});
        }
    }


}

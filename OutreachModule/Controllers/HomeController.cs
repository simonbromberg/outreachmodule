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
    [Authorize]

    public class HomeController : Controller
    {
        static private string CookieDefaultCamp = "DefaultCamp";
        private ModelManager manager = new ModelManager();
        // GET: Home
        public ActionResult Index()
        {
            var campId = getDefaultCamp();
            if (campId != null) {
                ViewBag.SavedCamp = manager.getCampWithId((int)campId);            
                ViewBag.ToScreenCount = manager.getListOfUnscreenedPatientsForCamp((int)campId).Count();
                ViewBag.ToExamineCount = manager.getListOfUnfinishedExaminationsForCamp((int)campId).Count();
            }
            else
            {
                ViewBag.ToScreenCount = "N/A";
                ViewBag.ToExamineCount = "N/A";
            }
            ViewBag.CampList = new SelectList(manager.campList, "Id", "selectRow", getDefaultCamp());
            return View();
        }

        private Nullable<int> getDefaultCamp()
        {
            var campCookie = Request.Cookies[CookieDefaultCamp];
            Nullable<int> campId = null;
            if (campCookie != null)
            {
                campId = Convert.ToInt32(campCookie.Value);
            }
            return campId;
        }
        public ActionResult Go(int selectedId)
        {
            HttpCookie cookie = new HttpCookie(CookieDefaultCamp);
            cookie.Value = selectedId.ToString();
            HttpContext.Response.Cookies.Remove(CookieDefaultCamp);
            HttpContext.Response.SetCookie(cookie);

            return RedirectToAction("Index");
        }

        public ActionResult GoToRegistration()
        {
            var c = getDefaultCamp();
            if (c == null)
            {
                TempData["message"] = "No camp selected";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Camp", new { campId = c});
        }

        public ActionResult GoToScreening()
        {
            var c = getDefaultCamp();
            if (c == null)
            {
                TempData["message"] = "No camp selected";
                return RedirectToAction("Index");
            }
            return RedirectToAction("PatientScreeningQueue", "Camp", new { campId = c });
        }

        public ActionResult GoToExamination()
        {
            var c = getDefaultCamp();
            if (c == null)
            {
                TempData["message"] = "No camp selected";
                return RedirectToAction("Index");
            }
            return RedirectToAction("ExaminationQueue", "Camp", new { campId = c });
        }
    }


}

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
using CustomExtensions;

namespace OutreachModule.Controllers
{
    [Authorize]
    public class PatientsController : Controller
    {
        private ModelManager manager = new ModelManager();
        // GET: Home
        public ActionResult Index(string sortOrder, string searchString)
        {
            var list = manager.patientList;
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.search(searchString)).ToList();
            }
            ViewBag.searchString = searchString;

            ViewBag.CampSortParm = String.IsNullOrEmpty(sortOrder) ? "code_desc" : "";
            ViewBag.CampSortIcon = String.IsNullOrEmpty(sortOrder) ? "&#9650;" : "&#9660;";
            switch (sortOrder)
            {
                case "code_desc":
                    list = list.OrderByDescending(s => s.camp_code).ToList();
                    break;
                default:
                    list = list.OrderBy(s => s.camp_code).ToList();
                    break;
            }

            return View(list);
        }
        
        // GET: Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var model = manager.getPatientViewModel((int)id);
            return View(model);
        }

        // GET: Home/Create
        public ActionResult Create(int? campId)
        {
            //SetGenderViewBag();
            //SetCampViewBag(campId);
            //return View();
            return RedirectToAction("Index", "Home");
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(Patient patientToCreate, HttpPostedFileBase file)
        {
            var success = manager.addPatient(patientToCreate,file);
            if (!success)
            {
                SetCampViewBag(patientToCreate.campId);
                SetGenderViewBag(patientToCreate.gender);
                ViewBag.Message = "Saving was unsuccessful";
                return View();
            }
            return RedirectToAction("Index");
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var patient = manager.getPatientWithId((int)id);
            SetGenderViewBag(patient.gender);
            SetCampViewBag(patient.campId);
            return View(patient);
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Exclude ="photoPath")] int id, Patient patient,HttpPostedFileBase file)
        {
            var successs = manager.editPatient(patient, file);
            if (successs) {
                return RedirectToAction("Details");
            }
            SetCampViewBag(patient.campId);
            SetGenderViewBag(patient.gender);
            ViewBag.Message = "Edit was unsuccessful";
            return View(patient);          
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Patient patient = manager.getPatientWithId((int)id);
            if (patient == null)
            {
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (manager.removePatient(id))
            {
                return RedirectToAction("Index");
            }
            return View(id);
        }
        private void SetGenderViewBag(string gender = null) {
            ViewBag.GenderSelect = manager.GenderSelect(gender);
        }
        private void SetCampViewBag(int? id = null)
        {
            ViewBag.CampSelect = manager.CampSelect(id);
        }
    }


}

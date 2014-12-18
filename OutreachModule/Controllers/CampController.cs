using OutreachModule.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutreachModule.Controllers
{
    [Authorize]
    public class CampController : Controller
    {
        private ModelManager manager = new ModelManager();
        // GET: Camp
        public ActionResult Index(int? campId)
        {
            if (campId == null)
            {
                return RedirectToAction("Index", "Camps");
            }
            var camp = manager.getCampWithId((int)campId);
            persistCamp(camp);
            if (camp == null)
            {
                return RedirectToAction("Index", "Camps");
            }
            var model = manager.getCampViewModel((int)campId);
            return View(model);
        }

        [Authorize(Roles = OutreachRoles.RoleAdmin)]
        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var camp = manager.getCampWithId((int)id);
            if (camp == null)
            {
                return RedirectToAction("Index");
            }
            SetCampTypeViewBag(camp.type);
            return View(camp);
        }

        // POST: Camp/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = OutreachRoles.RoleAdmin)]
        public ActionResult Edit(Camp camp)
        {
            if (ModelState.IsValid)
            {
                var success = manager.editCamp(camp);
                if (success) { 
                    return RedirectToAction("Index", new { campId = camp.Id });
                }
            }
            ViewBag.Message = "Saving was unsuccessful";
            return View(camp);
        }

        public ActionResult AddPatient()
        {
            var campId = Convert.ToInt32(Url.RequestContext.RouteData.Values["campId"]);
            var camp = manager.getCampWithId(campId);
            ViewBag.camp = camp;         
            SetGenderViewBag();
            string campCode;
            if (camp == null)
            {
                ViewBag.CampSelect = new SelectList(manager.Camps, "Id", "code");
                campCode = "9999";
            }
            else
            {
                campCode = camp.code;
            }
            var mrn = manager.newMrn(campCode);
            var model = new Patient();
            model.mrnRef = mrn;
            model.mrnId = mrn.Id;
            model.fromCamp = camp;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddPatient(Patient patientToAdd, HttpPostedFileBase file)
        {
            var camp = getCamp();
            ViewBag.camp = camp;
            SetGenderViewBag(patientToAdd.gender);

            if (patientToAdd == null)
            {
                return RedirectToAction("Index");
            }
            patientToAdd.campId = camp.Id;
            var success  = manager.addPatient(patientToAdd, file);
            if (!success)
            {
                ViewBag.Message = "Saving unsuccessful";
                return View(patientToAdd);
            }
            return RedirectToAction("Patient", new {patientId = patientToAdd.Id});
        }
        public ActionResult Patient(int? patientId)
        {
            var camp = getCamp();
            ViewBag.camp = camp;

            if (camp == null)
            {
                return RedirectToAction("Index");
            }
            if (patientId == null)
            {
                return RedirectToAction("Index", new {campId = camp.Id} );
            }
            var patient = manager.getPatientViewModel((int)patientId);
            return View(patient);
        }
        [Authorize(Roles = OutreachRoles.RoleAdmin)]
        public ActionResult DeletePatient(int? patientId)
        {
            var camp = getCamp();
            if (patientId == null)
            {
                return RedirectToAction("Index", new { campId = camp.Id });
            }
            else
            {
                if (!manager.removePatient((int)patientId))
                {
                    ViewBag.Message = "Patient failed to be removed";
                    return RedirectToAction("Patient", new { id = patientId });
                }
                return RedirectToAction("Index", new { campId = camp.Id });
                
            }
        }
        [Authorize(Roles=OutreachRoles.RoleAdmin)]
        public ActionResult DeleteAllPatientExaminations(int? patientId)
        {
            var camp = getCamp();
            if (patientId == null)
            {
                return RedirectToAction("Index",new {campId = camp.Id});
            }
            else
            {
                var patient = manager.getPatientWithId((int)patientId);
                var examinations = patient.Examinations.ToList();
                foreach (var e in examinations) { 
                    manager.removeExamination(e);
                }
                return RedirectToAction("Patient", new { patientId = patientId });
            }
        }
        public ActionResult EditPatient(int? patientId)
        {
            ViewBag.camp = getCamp();

            if (patientId == null)
            {
                return RedirectToAction("Index");
            }
            var p = manager.getPatientWithId((int)patientId);
            if (p == null)
            {
                return RedirectToAction("Index");
            }
            SetGenderViewBag(p.gender);
            return View(p);
        }

        [HttpPost]
        public ActionResult EditPatient(Patient p, HttpPostedFileBase file)
        {
            var camp = getCamp();
            ViewBag.camp = camp;
            SetGenderViewBag(p.gender);

            if (p == null)
            {
                return RedirectToAction("Index");
            }
            p.campId = camp.Id;
            var success = manager.editPatient(p, file);
            if (!success)
            {
                ViewBag.Message = "Saving unsuccessful";
                return View(p);
            }
            return RedirectToAction("Patient", new { id = p.Id });
        }

        public ActionResult PatientScreeningQueue(int campId)
        {
            var list = manager.getListOfUnscreenedPatientsForCamp(campId);
            return View(list);
        }

        public ActionResult ExaminationQueue(int campId)
        {
            var list = manager.getListOfUnfinishedExaminationsForCamp(campId);
            return View(list);
        }

        public ActionResult GoToExamination(int examId)
        {
            var exam = manager.getExaminationWithId(examId);
            return RedirectToAction("Detail", "Examination", new { examId = examId,campId = exam.campId,patientId = exam.patientId});
        }
        private void SetGenderViewBag(string Gender = null)
        {
            ViewBag.GenderSelect = manager.GenderSelect(Gender);
        }
        private void persistCamp(Camp camp)
        {
            TempData["Camp"] = camp;
        }
        private Camp getCamp()
        {
            var camp = (Camp)TempData["Camp"];
            persistCamp(camp);
            return camp;
        }
        private void SetCampTypeViewBag(string type = null)
        {
            ViewBag.TypeSelect = manager.CampTypeSelect(type);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using OutreachModule.Models;
using System.Diagnostics;
namespace OutreachModule.Controllers
{
    [Authorize]
    public class ExaminationController : Controller
    {
        private ModelManager manager = new ModelManager();
        // GET: Examination
        public ActionResult Index(int? patientId, int? campId)
        {
            Debug.Print("index get action result");
            if (patientId == null || campId == null)
            {
                return RedirectToAction("Index","Home");
            }

            var camp = manager.getCampWithId((int)campId);
            var patient = manager.getPatientWithId((int)patientId);
            ExaminationCreateModel model = GetExaminationInitialModel();
            model.dateStarted = DateTime.Now;
            model.patient = patient;
            model.camp = camp;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExaminationCreateModel m,string Command)
        {
            Examination examination;
            bool shouldUpdate = false;
            if (m.examinationID != null)
            {
                shouldUpdate = true;
                examination = manager.getExaminationWithId((int)m.examinationID);
            }
            else
            {
                examination = new Examination(m);
            }
            examination.dateComplete = DateTime.Now;
            examination.user = User.Identity.GetUserName();
            examination.spectacles = m.spectacles;
            examination.status = Command == "Save" ? (int)Examination.ExamStatus.ScreeningInProgress : (int)Examination.ExamStatus.ScreeningDone;

            m = FillExaminationModel(m);

            if (shouldUpdate)
            {
                manager.editExamination(examination);
                manager.editComplaints(m, examination.Id);
            }
            else {
                
                manager.addExamination(examination);
                manager.addComplaintsFrom(m,examination.Id);

            }
            ViewBag.camp = manager.getCampWithId((int)m.campId);

            if (Command == "Save") // save without discharge
            {
                m.examinationID = examination.Id;
                m.patient = manager.getPatientWithId(examination.patientId);
                m.camp = manager.getCampWithId(examination.campId);
                ModelState.Clear();
                return View(m);
            }

            return RedirectToAction("Patient", "Camp", new { patientId = m.patientId });
        }
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult AddOther(string name)
        {
            var model = new OtherListModel(name);
            model.list.Add("");
            return View(model);
        }
        public ActionResult Detail(int? examId)
        {
            if (examId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var examination = manager.getExaminationDetailWithId((int) examId);
            return View(examination);
        }

        [Authorize(Roles = OutreachRoles.RoleAdmin)]
        public ActionResult Delete(int examId)
        {
            var exam = manager.getExaminationWithId(examId);
            manager.removeExamination(exam);

            return RedirectToAction("Patient", "Camp", new { patientId = exam.patientId });
        }
        [Authorize(Roles = OutreachRoles.RoleAdmin)]
        public ActionResult Delete2(int examId)
        {
            manager.deleteE2WithId(examId);
            return RedirectToAction("Detail", new { examId = examId });
        }
        public ActionResult ContinueExamination(int examId)
        {
            ExaminationSection2CreateModel model = new ExaminationSection2CreateModel();
            var exam = manager.getExaminationWithId(examId);
            model.exam = exam;
            model.dateStarted = DateTime.Now;
            return View(model);
        }
        [HttpPost]
        public ActionResult ContinueExamination(ExaminationSection2CreateModel model)
        {
            model.diagnosis.SelectedDiagnosisLeft = GetSelected(model.diagnosis.PostedDiagnosisLeft, ListType.MedicalHistory);
            model.diagnosis.SelectedDiagnosisRight = GetSelected(model.diagnosis.PostedDiagnosisRight, ListType.MedicalHistory);
            
            manager.addE2WithModel(model,User.Identity.GetUserName());
            return RedirectToAction("Detail", new { examId = model.examId });
        }
        public ActionResult PrintReferral(int? examId)
        {
            if (examId == null)
            {
                return null;
            }
            var model = manager.getE2WithId((int)examId);
            return View(model);
        }
        public ActionResult PrintPrescription(int? examId)
        {
            if (examId == null)
            {
                return null;
            }
            var model = manager.getE2WithId((int)examId);
            return View(model);
        }
        private List<CheckboxItem> GetSelected(PostedComplaints posted,ListType type)
        {
            if (posted != null && posted.ComplaintIds.Any()) {
                return ExaminationCheckboxRepository.GetList(type)
                 .Where(x => posted.ComplaintIds.Any(s => x.Id.ToString().Equals(s)))
                 .ToList();
            }
            return new List<CheckboxItem>();
        }
        private ExaminationCreateModel FillExaminationModel(ExaminationCreateModel model)
        {
            // setup properties
            var postedLeft = model.PostedLeftComplaints;
            var postedRight = model.PostedRightComplaints;
            var selectedLeft = GetSelected(postedLeft, ListType.Complaint);
            var selectedRight = GetSelected(postedRight, ListType.Complaint);
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            //setup a view model
            model.AvailableComplaints = ExaminationCheckboxRepository.GetList(ListType.Complaint);
            model.SelectedLeftComplaints = selectedLeft;
            model.SelectedRightComplaints = selectedRight;
            model.PostedLeftComplaints = postedLeft;
            model.PostedRightComplaints = postedRight;

            postedLeft = model.PostedLeftOcularHistory;
            postedRight = model.PostedRightOcularHistory;
            selectedLeft = GetSelected(postedLeft, ListType.OcularHistory);
            selectedRight = GetSelected(postedRight, ListType.OcularHistory);
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            model.AvailableOcularHistory = ExaminationCheckboxRepository.GetList(ListType.OcularHistory);
            model.SelectedLeftOcularHistory = selectedLeft;
            model.SelectedRightOcularHistory = selectedRight;
            model.PostedLeftOcularHistory = postedLeft;
            model.PostedRightOcularHistory = postedRight;

            postedLeft = model.PostedMedicalHistory;
            postedRight = model.PostedFamilyHistory;
            selectedLeft = GetSelected(postedLeft, ListType.MedicalHistory);
            selectedRight = GetSelected(postedRight, ListType.MedicalHistory);
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            model.AvailableMedicalHistory = ExaminationCheckboxRepository.GetList(ListType.MedicalHistory);
            model.SelectedMedicalHistory = selectedLeft;
            model.SelectedFamilyHistory = selectedRight;
            model.PostedMedicalHistory = postedLeft;
            model.PostedFamilyHistory = postedRight;

            return model;
        }

        /// <summary>
        /// for setup initial view model
        /// </summary>
        private ExaminationCreateModel GetExaminationInitialModel()
        {
            //setup properties
            var model = new ExaminationCreateModel();
            var selected = new List<CheckboxItem>();

            //setup a view model
            model.AvailableComplaints = ExaminationCheckboxRepository.GetList(ListType.Complaint);
            model.SelectedLeftComplaints = new List<CheckboxItem>(); ;
            model.SelectedRightComplaints = new List<CheckboxItem>(); ;

            model.AvailableOcularHistory = ExaminationCheckboxRepository.GetList(ListType.OcularHistory);
            model.SelectedLeftOcularHistory = new List<CheckboxItem>(); ;
            model.SelectedRightOcularHistory = new List<CheckboxItem>(); ;

            model.AvailableMedicalHistory = ExaminationCheckboxRepository.GetList(ListType.MedicalHistory);
            model.SelectedMedicalHistory = new List<CheckboxItem>(); ;
            model.SelectedFamilyHistory = new List<CheckboxItem>(); ;
            return model;
        }
    }
}
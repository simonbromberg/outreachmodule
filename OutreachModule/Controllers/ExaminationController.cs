using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using OutreachModule.Models;
namespace OutreachModule.Controllers
{
    [Authorize]
    public class ExaminationController : Controller
    {
        private ModelManager manager = new ModelManager();
        // GET: Examination
        public ActionResult Index(int? patientId, int? campId)
        {
            if (patientId == null || campId == null)
            {
                return RedirectToAction("Index","Home");
            }
            ViewBag.camp = manager.getCampWithId((int)campId);
            ViewBag.patient = manager.getPatientWithId((int)patientId);
            ExaminationCreateModel model = GetExaminationInitialModel();
            model.campId = (int)campId;
            model.patientId = (int)patientId;
            model.dateStarted = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExaminationCreateModel m)
        {
            var examination = new Examination(m);
            examination.dateComplete = DateTime.Now;
            examination.user = User.Identity.GetUserName();
            m = FillExaminationModel(m);
            if (m == null)
            {
                ViewBag.message = "Error saving";
                return View(m);
            }
            manager.addExamination(examination);
            manager.addComplaintsFrom(m,examination.Id);
            ViewBag.camp = manager.getCampWithId(m.campId);
            //ViewBag.patient = manager.getPatientWithId(m.patientId);
            return RedirectToAction("Patient", "Camp", new { id = m.patientId });
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

            return RedirectToAction("Patient", "Camp", new { id = exam.patientId });
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
            var selectedLeft = GetSelected(model.PostedLeftComplaints,ListType.Complaint);
            var selectedRight = GetSelected(model.PostedRightComplaints,ListType.Complaint);
            var postedLeft = model.PostedLeftComplaints;
            var postedRight = model.PostedRightComplaints;
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            //setup a view model
            model.AvailableComplaints = ExaminationCheckboxRepository.GetList(ListType.Complaint);
            model.SelectedLeftComplaints = selectedLeft;
            model.SelectedRightComplaints = selectedRight;
            model.PostedLeftComplaints = postedLeft;
            model.PostedRightComplaints = postedRight;

            selectedLeft = GetSelected(model.PostedLeftOcularHistory,ListType.OcularHistory);
            selectedRight = GetSelected(model.PostedRightOcularHistory,ListType.OcularHistory);
            postedLeft = model.PostedLeftOcularHistory;
            postedRight = model.PostedRightOcularHistory;
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            model.AvailableOcularHistory = ExaminationCheckboxRepository.GetList(ListType.OcularHistory);
            model.SelectedLeftOcularHistory = selectedLeft;
            model.SelectedRightOcularHistory = selectedRight;
            model.PostedLeftOcularHistory = postedLeft;
            model.PostedRightOcularHistory = postedRight;

            return model;
        }

        /// <summary>
        /// for setup initial view model for all fruits
        /// </summary>
        private ExaminationCreateModel GetExaminationInitialModel()
        {
            //setup properties
            var model = new ExaminationCreateModel();
            var selected = new List<CheckboxItem>();

            //setup a view model
            model.AvailableComplaints = ExaminationCheckboxRepository.GetList(ListType.Complaint);
            model.SelectedLeftComplaints = selected;
            model.SelectedRightComplaints = selected;

            model.AvailableOcularHistory = ExaminationCheckboxRepository.GetList(ListType.OcularHistory);
            model.SelectedLeftOcularHistory = selected;
            model.SelectedRightOcularHistory = selected;
            return model;
        }
    }
}
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
            ExaminationCreateModel model = GetExaminationInitialModel();//new ExaminationCreateModel();
            model.campId = (int)campId;
            model.patientId = (int)patientId;
            model.dateStarted = DateTime.Now;
            //var list = manager.listOfComplaintChoices;
            //model.leftComplaints = list;
            //model.rightComplaints = list;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ExaminationCreateModel m)
        {
            var examination = new Examination(m);
            examination.dateComplete = DateTime.Now;
            examination.user = User.Identity.GetUserName();
            if (m == null)
            {
                ViewBag.message = "Error saving";
                return View(m);
            }
            manager.addExamination(examination);
            ViewBag.camp = manager.getCampWithId(m.campId);
            ViewBag.patient = manager.getPatientWithId(m.patientId);
            return View("Patient", "Camp", examination.patientId);
        }
       
        private ExaminationCreateModel GetExaminationModel(PostedComplaints postedLeft, PostedComplaints postedRight)
        {
            // setup properties
            var model = new ExaminationCreateModel();
            var selectedLeft = new List<CheckboxItem>();
            var selectedRight = new List<CheckboxItem>();
            var postedLeftIds = new string[0];
            var postedRightIds = new string[0];
            if (postedLeft == null) postedLeft = new PostedComplaints();
            if (postedRight == null) postedRight = new PostedComplaints();
            // if a view model array of posted fruits ids exists
            // and is not empty,save selected ids
            if (postedLeft.ComplaintIds != null && postedLeft.ComplaintIds.Any())
            {
                postedLeftIds = postedLeft.ComplaintIds;
            }
            if (postedRight.ComplaintIds != null && postedRight.ComplaintIds.Any())
            {
                postedRightIds = postedRight.ComplaintIds;
            }
            // if there are any selected ids saved, create a list of fruits
            if (postedLeftIds.Any())
            {
                selectedLeft = ComplaintRepository.GetAll()
                 .Where(x => postedLeftIds.Any(s => x.Id.ToString().Equals(s)))
                 .ToList();
            }
            if (postedRightIds.Any())
            {
                selectedRight = ComplaintRepository.GetAll()
                 .Where(x => postedRightIds.Any(s => x.Id.ToString().Equals(s)))
                 .ToList();
            }
            //setup a view model
            model.AvailableComplaints = ComplaintRepository.GetAll().ToList();
            model.SelectedLeftComplaints = selectedLeft;
            model.SelectedRightComplaints = selectedRight;
            model.PostedLeftComplaints = postedLeft;
            model.PostedRightComplaints = postedRight;
            return model;
        }

        /// <summary>
        /// for setup initial view model for all fruits
        /// </summary>
        private ExaminationCreateModel GetExaminationInitialModel()
        {
            //setup properties
            var model = new ExaminationCreateModel();
            var selectedFruits = new List<CheckboxItem>();

            //setup a view model
            model.AvailableComplaints = ComplaintRepository.GetAll().ToList();
            model.SelectedLeftComplaints = selectedFruits;
            model.SelectedRightComplaints = selectedFruits;

            return model;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OutreachModule.Models
{
    public class CampViewModel
    {
        public CampViewModel(Camp cmp, List<Patient> cmpPatients)
        {
            camp = cmp;
            campPatients = cmpPatients;
        }
        public Camp camp { get; set; }
        public List<Patient> campPatients { get; set; }
    }
    public class PatientDetailViewModel
    {
        public PatientDetailViewModel(Patient pt, List<Camp> ptCamps, List<Examination> ptExams)
        {
            patient = pt;
            patientCamps = ptCamps;
            examinations = ptExams;
        }
        public Patient patient { get; set; }
        public List<Camp> patientCamps { get; set; }
        public List<Examination> examinations { get; set; }
    }
    public class ExaminationDetailModel
    {
        public ExaminationDetailModel(Examination ex,List<ExamComplaint> exComplaints) 
        {
            examination = ex;
            complaints = exComplaints;
        }
        public Examination examination { get; private set; }

        public List<ExamComplaint> leftEyeComplaints
        {
            get
            {
                return complaintsForEye("L");
            }
        }
        public List<ExamComplaint> rightEyeComplaints
        {
            get
            {
                return complaintsForEye("R");
            }
        }

        public List<ExamComplaint> leftEyeOcularHistory
        {
            get
            {
                return ocularHistoryForEye("L");
            }
        }
        public List<ExamComplaint> rightEyeOcularHistory
        {
            get
            {
                return ocularHistoryForEye("R");
            }
        }
        private List<ExamComplaint> complaints;
        private List<ExamComplaint> complaintsForEye(string eye)
        {
            return complaints.Where(x => (x.eye == eye && x.group == ExamComplaint.GroupComplaint)).ToList();
        }
        private List<ExamComplaint> ocularHistoryForEye(string eye)
        {
            return complaints.Where(x => (x.eye == eye && x.group == ExamComplaint.GroupOcularHistory)).ToList();
        }
    }
    public class ModelManager
    {
        private OutreachDBEntities db = new OutreachDBEntities();
        private bool saveChanges()
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Debug.Print("Failed to save database " + e.Message);
                return false;
            }
            return true;
        }

        //CAMP
        public CampViewModel getCampViewModel(int id)
        {
            var camp = getCampWithId(id);
            var campPatients = db.camp_patient.Where(s => s.campId == id).ToList().ConvertAll(x => x.Patient);
            return new CampViewModel(camp, campPatients);
        }
        public Camp getCampWithId(int c)
        {
            return db.Camps.Find(c);
        }

        public List<Camp> campList
        {
            get
            {
                return Camps.ToList();
            }
        }
        public DbSet<Camp> Camps
        {
            get
            {
                return db.Camps;
            }
        }

        public SelectList CampSelect(int? id = null)
        {
            if (id == null)

                return new SelectList(Camps, "Id", "code");

            else

                return new SelectList(Camps, "Id", "code", id);
        }
        public SelectList CampTypeSelect(string type = null)
        {
            var options = new[] { "Type 1", "Type 2", "Type 3", "Type 4" };
            if (type == null)
            {
                return new SelectList(options.Select(x => new { value = x, text = x }), "value", "text");
            }
            else
            {
                return new SelectList(options.Select(x => new { value = x, text = x }), "value", "text", type);
            }
        }

        public bool removeCamp(int id)
        {
            //Note: removing a camp causes all patients that were created with that camp to be deleted as well
            var camp = db.Camps.Find(id);
            if (camp == null)
            {
                return false;
            }
            db.Camps.Remove(camp);
            var campPatients = db.camp_patient.Where(s => s.campId == camp.Id); //patients added to this camp
            var patients = db.Patients.Where(s => s.campId == camp.Id); //patients created at this camp

            foreach (var row in campPatients)
            {
                db.camp_patient.Remove(row);
            }
            foreach (var row in patients)
            {
                removePatient(row.Id);
            }
            return saveChanges();
        }

        public bool addCamp(Camp newCamp)
        {
            if (db.Camps.Where(x => x.code == newCamp.code).Count() > 0)
            {
                Debug.Print("Camp already exists");
                return false;
            }
            db.Camps.Add(newCamp);
            return saveChanges();
        }

        public bool editCamp(Camp camp)
        {
            db.Entry(camp).State = EntityState.Modified;
            return saveChanges();
        }

        public bool addPatientToCamp(int patient, int camp)
        {
            if (db.camp_patient.Where(s => s.patientId == patient && s.campId == camp).Count() > 0)
            {
                Debug.Print("Patient " + patient + " already in camp " + camp);
                return true;
            }
            var campPatient = camp_patient.NewWithIDs(patient, camp);
            db.camp_patient.Add(campPatient);
            return saveChanges();
        }
        //MRN 
        public mrn newMrn(string campCode)
        {
            var mrn = new mrn();
            db.mrns.Add(mrn);
            db.SaveChanges();
            mrn.recordnumber = campCode + "-" + mrn.Id.ToString();
            db.SaveChanges();
            return mrn;
        }
        //PATIENT
        public PatientDetailViewModel getPatientViewModel(int pt)
        {
            var patient = getPatientWithId(pt);
            if (patient == null)
            {
                return null;
            }
            var patientCamps = db.camp_patient.Where(s => s.patientId == pt).ToList().ConvertAll(x => x.Camp);
            var examinations = db.Examinations.Where(s => s.patientId == pt).ToList();
            return new PatientDetailViewModel(patient, patientCamps, examinations);
        }
        public Patient getPatientWithId(int p)
        {
            return db.Patients.Find(p);
        }
        public List<Patient> patientList
        {
            get
            {
                return Patients.ToList();
            }
        }
        public DbSet<Patient> Patients
        {
            get
            {
                return db.Patients;
            }
        }

        public SelectList GenderSelect(string Gender = null)
        {
            if (Gender == null)
            {
                return new SelectList(new[] { "Male", "Female" }.Select(x => new { type = x, text = x }), "type", "text");
            }

            else
            {
                return new SelectList(new[] { "Male", "Female" }.Select(x => new { type = x, text = x }), "type", "text", Gender);
            }
        }

        public bool addPatient(Patient newPt)
        {
            return addPatient(newPt, null);
        }
        public bool addPatient(Patient newPt, HttpPostedFileBase imageFile)
        {
            if (newPt == null)
            {
                return false;
            }
            newPt.dateadded = DateTime.Now;
            newPt.lastupdated = DateTime.Now;
            db.Patients.Add(newPt);
            var success = addPatientToCamp(newPt.Id, newPt.campId);

            if (imageFile != null && imageFile.ContentLength > 0)
            {
                var path = "patient" + newPt.Id.ToString() + Path.GetExtension(imageFile.FileName);
                imageFile.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ptimages"), path));
                newPt.photoPath = path;
                success = saveChanges();
            }
            return success;
        }

        private bool addImageToPatient(Patient pt, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                removePatientImage(pt);
                var path = "patient" + pt.Id.ToString() + Path.GetExtension(image.FileName);
                image.SaveAs(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ptimages"), path));
                pt.photoPath = path;
                return saveChanges();
            }
            return false;
        }
        public bool editPatient(Patient editedPt, HttpPostedFileBase imageFile)
        {
            if (editedPt.dateadded == null)
            {
                editedPt.dateadded = DateTime.Now;
            }
            editedPt.lastupdated = DateTime.Now;
            db.Entry(editedPt).State = EntityState.Modified;
            addImageToPatient(editedPt, imageFile);
            return saveChanges();
        }

        public bool removePatient(int id)
        {
            var patient = db.Patients.Find(id);
            if (patient == null)
            {
                Debug.Print("Trying to remove invalid patient " + id + ". No patient found");
                return false;
            }

            var patientCamps = db.camp_patient.Where(s => s.patientId == id); //camps this patient has been added to 
            foreach (var row in patientCamps)
            {
                db.camp_patient.Remove(row);
            }
            removePatientImage(patient);
            foreach (var exam in patient.Examinations)
            {
                removeExamination(exam);
            }
            db.Patients.Remove(patient);
            return saveChanges();
        }
        private bool removePatientImage(Patient pt)
        {
            if (pt.photoPath != null) //existing file may have different extension
            {
                string fullPath = System.Web.HttpContext.Current.Request.MapPath("~/ptimages/" + pt.photoPath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                return true;
            }
            return false;
        }

        // Examination
        public Examination getExaminationWithId(int e)
        {
            return db.Examinations.Find(e);
        }
        public ExaminationDetailModel getExaminationDetailWithId(int e)
        {
            Examination ex = getExaminationWithId(e);
            List<ExamComplaint> complaints = getComplaintsForExam(e);
            return new ExaminationDetailModel(ex, complaints);
        }
        public Examination addExamination(Examination newEx)
        {
            try
            {
                db.Examinations.Add(newEx);
            }
            catch (Exception e)
            {
                Debug.Print("Error adding examination " + e.Message);
            }
            saveChanges();
            return newEx;
        }
        public bool removeExamination(int e)
        {
            var exam = db.Examinations.Find(e);
            if (exam != null)
            {
                return removeExamination(exam);
            }
            return false;
        }
        public bool removeExamination(Examination e)
        {
            removeComplaintsForExamination(e.Id);
            db.Examinations.Remove(e);
            return saveChanges();
        }

        public List<ExamComplaint> getComplaintsForExam(int examId)
        {
            return db.ExamComplaints.Where(x => x.examinationId == examId).ToList();  
        }

        public bool addComplaintsFrom(ExaminationCreateModel m, int examId)
        {
            addComplaints("L",m.SelectedLeftComplaints,m.otherLeft,examId,ExamComplaint.GroupComplaint);
            addComplaints("R", m.SelectedRightComplaints, m.otherRight, examId, ExamComplaint.GroupComplaint);
            addComplaints("L", m.SelectedLeftOcularHistory, m.otherOcularHistoryLeft, examId, ExamComplaint.GroupOcularHistory);
            addComplaints("R", m.SelectedRightOcularHistory, m.otherOcularHistoryRight, examId, ExamComplaint.GroupOcularHistory);
            return saveChanges();
        }

        private void removeComplaintsForExamination(int id)
        {
            var list = db.ExamComplaints.Where(x => x.examinationId == id);
            foreach (var ec in list) {
                db.ExamComplaints.Remove(ec);
            } 
        }

        public bool addComplaints(string eye, IEnumerable<CheckboxItem> list, string other, int examId, string group) 
        {
            var s = false;
            string saveOther = null;
            foreach (var c in list)
            {
                if (c.Name == "Other")
                {
                    saveOther = other;
                }
                else
                {
                    saveOther = null;
                }
                db.ExamComplaints.Add(ExamComplaint.newComplaintWith(eye, c.Name, saveOther, examId,group));
            }
            return s;
        }

    }
}
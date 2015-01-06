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
        public List<ExamComplaint> medicalHistory
        {
            get
            {
                return medicalHistoryForGroup("M");
            }
        }
        public List<ExamComplaint> familyHistory
        {
            get
            {
                return medicalHistoryForGroup("F");
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
        private List<ExamComplaint> medicalHistoryForGroup(string MorF)
        {
            return complaints.Where(x => (x.eye == MorF && x.group == ExamComplaint.GroupMedicalHistory)).ToList();
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
            var examinations = patient.Examinations.ToList();
            foreach (var exam in examinations)
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
            List<ExamComplaint> complaints = ex.ExamComplaints.ToList(); //getComplaintsForExam(e);
            return new ExaminationDetailModel(ex, complaints);
        }

        public List<Patient> getListOfUnscreenedPatientsForCamp(int campId)
        {
            return Patients.Where(x => (x.campId == campId && x.Examinations.Count == 0)).ToList();
        }
        public List<Examination> getListOfUnfinishedExaminationsForCamp(int campId)
        {
            return db.Examinations.Where(x => (x.campId == campId && x.status == (int)Examination.ExamStatus.ScreeningDone)).ToList();
        }
        public List<Examination> getListOfDoneExaminationsForCamp (int campId)
        {
            return db.Examinations.Where(x => (x.campId == campId && x.status == (int)Examination.ExamStatus.ExaminationDone)).ToList();
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
        public bool editExamination(Examination e)
        {            
            db.Entry(e).State = EntityState.Modified;
            return saveChanges();
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
            addComplaints("L",m.SelectedLeftComplaints,m.OtherComplaintsLeftList,examId,ExamComplaint.GroupComplaint);
            addComplaints("R", m.SelectedRightComplaints,m.OtherComplaintsRightList, examId, ExamComplaint.GroupComplaint);
            addComplaints("L", m.SelectedLeftOcularHistory, m.OtherLeftOcularHistoryList, examId, ExamComplaint.GroupOcularHistory);
            addComplaints("R", m.SelectedRightOcularHistory, m.OtherRightOcularHistoryList, examId, ExamComplaint.GroupOcularHistory);
            addComplaints("M", m.SelectedMedicalHistory,m.OtherMedicalHistoryList, examId, ExamComplaint.GroupMedicalHistory);
            addComplaints("F", m.SelectedFamilyHistory, m.OtherFamilyHistoryList, examId, ExamComplaint.GroupMedicalHistory);
            
            //addComplaints("L", m.SelectedLeftComplaints, m.hasOtherComplaintsLeft ? m.otherLeft : null, examId, ExamComplaint.GroupComplaint);
            //addComplaints("R", m.SelectedRightComplaints,m.hasOtherComplaintsRight ? m.otherRight : null, examId, ExamComplaint.GroupComplaint);
            //addComplaints("L", m.SelectedLeftOcularHistory, m.hasOtherOcularHistoryLeft ? m.otherOcularHistoryLeft : null, examId, ExamComplaint.GroupOcularHistory);
            //addComplaints("R", m.SelectedRightOcularHistory, m.hasOtherOcularHistoryRight ? m.otherOcularHistoryRight : null, examId, ExamComplaint.GroupOcularHistory);
            //addComplaints("M", m.SelectedMedicalHistory, m.hasOtherMedicalHistory ? m.otherMedicalHistory : null, examId, ExamComplaint.GroupMedicalHistory);
            //addComplaints("F", m.SelectedFamilyHistory, m.hasOtherFamilyHistory ? m.otherFamilyHistory : null, examId, ExamComplaint.GroupMedicalHistory);
            return saveChanges();
        }

        public bool editComplaints(ExaminationCreateModel m, int examId)
        {
            // easier just to remove and re-add, although that's pretty wasteful...s
            removeComplaintsForExamination(examId);
            addComplaintsFrom(m, examId);
            return saveChanges();
        }
        private void removeComplaintsForExamination(int id)
        {
            var list = db.ExamComplaints.Where(x => x.examinationId == id);
            foreach (var ec in list) {
                db.ExamComplaints.Remove(ec);
            } 
        }

        //public void addComplaints(string eye, IEnumerable<CheckboxItem> list, string other, int examId, string group)
        //{
        //    foreach (var c in list)
        //    {
        //        db.ExamComplaints.Add(ExamComplaint.newComplaintWith(eye, c.Name, null, examId, group));
        //    }
        //    if (other != null)
        //    {
        //        db.ExamComplaints.Add(ExamComplaint.newComplaintWith(eye, "Other", other, examId, group));
        //    }
        //}
        public void addComplaints(string eye, IEnumerable<CheckboxItem> list, List<string> other, int examId, string group) 
        {
            foreach (var c in list)
            {
                db.ExamComplaints.Add(ExamComplaint.newComplaintWith(eye, c.Name,null, examId,group));
            }
            foreach (var c in other)
            {
                if (c.Length != 0) { 
                    db.ExamComplaints.Add(ExamComplaint.newComplaintWith(eye, "Other", c, examId, group));
                }
            }
        }

        //Examination Part 2
        public E2 getE2WithId(int id)
        {
            return db.E2.Find(id);
        }

        public E2 addE2WithModel(ExaminationSection2CreateModel model, string username)
        {
            var e2 = new E2();
            e2.Id = (int)model.examId;
            e2.user = username;
            e2.timestamp = DateTime.Now;
            // Acuity
            e2.acuity_method = model.visualAcuity.method;
            e2.acuity_left = model.visualAcuity.leftAcuity;
            e2.acuity_right = model.visualAcuity.rightAcuity;

            // Refraction
            var leftRefraction = Refraction.refractionFrom(model.refraction.left);
            leftRefraction.e2Id = e2.Id;
            db.Refractions.Add(leftRefraction);
            var rightRefraction = Refraction.refractionFrom(model.refraction.right);
            rightRefraction.e2Id = e2.Id;
            db.Refractions.Add(rightRefraction);

            // Alignment
            foreach (var align in model.alignment)
            {
                var a = new Alignment();
                a.horizontal = align.horizontalSelection;
                a.vertical = align.verticalSelection;
                a.index = align.index;
                a.e2Id = e2.Id;
                db.Alignments.Add(a);
            }

            // Anterior Segment
            if (!model.anteriorSegment.isNormal) {
                e2.abnormal_anterior = model.anteriorSegment.abnormalEye.ToString();
                e2.abnormal_anterior_descr = model.anteriorSegment.description;
            }

            // Spectacles
            if (model.spectacles.dispensed)
            {
                e2.spectacles_dispensed = model.spectacles.dispensed;
                e2.spectacles_comment = model.spectacles.comment;
                e2.spectacles_cost = model.spectacles.cost;
            }

            // Referral
            if (model.PatientReferral.referred)
            {
                e2.referral = model.PatientReferral.referred;
                e2.referral_comment = model.PatientReferral.comment;
                e2.referral_reason = model.PatientReferral.reason;
                e2.referral_other = model.PatientReferral.reasonOther;
                e2.referral_location = model.PatientReferral.hospital;
            }

            // Diagnosis
            addDiagnosis("L", model.diagnosis.SelectedDiagnosisLeft, model.diagnosis.OtherDiagnosisLeftList,e2.Id);
            addDiagnosis("R", model.diagnosis.SelectedDiagnosisRight, model.diagnosis.OtherDiagnosisRightList, e2.Id);
            
            // Comments + Counseling
            e2.comments = model.Comments.comment;
            e2.counseling = model.PatientCounseling.comment;

            db.E2.Add(e2);
            if (!saveChanges())
            {
                Debug.Print("Error saving E2");
            }
            db.Examinations.Find(e2.Id).status = (int)Examination.ExamStatus.ExaminationDone;
            saveChanges();
            return e2;
        }
        public void addDiagnosis(string eye, IEnumerable<CheckboxItem> selected,List<string>other,int id) {
            foreach (var d in selected)
            {
                db.Diagnosis.Add(Diagnosis.newDiagnosisWith(eye, id, d.Name, null));
            }

            foreach (var d in other)
            {
                db.Diagnosis.Add(Diagnosis.newDiagnosisWith(eye, id, "Other", d));
            }

        }
        public bool deleteE2WithId(int id)
        {
            var e2 = db.E2.Find(id);
            var list = e2.Refractions.ToList();
            foreach (var r in list)
            {
                db.Refractions.Remove(r);
            }
            var list2 = e2.Diagnosis.ToList();
            foreach (var d in list2)
            {
                db.Diagnosis.Remove(d);
            }
            var list3 = e2.Alignments.ToList();
            foreach (var a in list3)
            {
                db.Alignments.Remove(a);
            }
            db.E2.Remove(e2);
            return saveChanges();
        }
    }
}
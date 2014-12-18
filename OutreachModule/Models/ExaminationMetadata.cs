using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Diagnostics;

namespace OutreachModule.Models

{
    public partial class ExamComplaint
    {
        public static string GroupComplaint = "Complaint";
        public static string GroupOcularHistory = "Ocular History";
        public static string GroupMedicalHistory = "Medical History";
        public static ExamComplaint newComplaintWith(string eye, string complaint, string other, int examId, string group)
        {
            var c = new ExamComplaint();
            c.eye = eye;
            c.complaint = complaint;
            c.otherComplaint = other;
            c.examinationId = examId;
            c.group = group;
            return c;
        }
    }
    [MetadataType(typeof(ExaminationMetadata))]
    public partial class Examination
    {
        public enum ExamStatus : int
        {
            None,
            ScreeningInProgress,
            ScreeningDone,
            ExaminationDone
        }
        public Examination(ExaminationCreateModel model)
        {
            campId = (int)model.campId;
            patientId = (int)model.patientId;
            dateStarted = model.dateStarted;
        }
        public string niceDate
        {
            get
            {
                return dateStarted.ToShortDateString() + " " + dateStarted.ToShortTimeString();
            }
        }
    }
    public class OtherListModel
    {
        public OtherListModel(string n)
        {
            name = n;
            list = new List<string>();
        }
        public string name { get; set; }
        public List<string> list { get; set; }
    }
    public class ExaminationMetadata
    {
        [Display(Name="Date Started")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm:ss}")]
        public System.DateTime dateStarted;
        [Display(Name = "Date Complete")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm:ss}")]
        public System.DateTime dateComplete;
    }
    public class CheckboxItem
    {
        //Integer value of a checkbox
        public int Id { get; set; }

        //String name of a checkbox
        public string Name { get; set; }

        //Boolean value to select a checkbox
        //on the list
        public bool IsSelected { get; set; }

        //Object of html tags to be applied
        //to checkbox, e.g.:'new{tagName = "tagValue"}'
        public object Tags { get; set; }
    }

    public class PostedComplaints
    {
        //this array will be used to POST values from the form to the controller
        public string[] ComplaintIds { get; set; }
    }

    public class ExaminationCreateModel
    {
        public ExaminationCreateModel()
        {
            OtherComplaintsRight = new OtherListModel("OtherComplaintsRightList");
            OtherComplaintsLeft = new OtherListModel("OtherComplaintsLeftList");

            OtherRightOcularHistory = new OtherListModel("OtherRightOcularHistoryList");
            OtherLeftOcularHistory = new OtherListModel("OtherLeftOcularHistoryList");

            OtherMedicalHistory = new OtherListModel("OtherMedicalHistoryList");
            OtherFamilyHistory = new OtherListModel("OtherFamilyHistoryList");
        }
        public string description
        {
            get
            {
                return "Camp: " + campId.ToString() + ", Patient:" + patientId.ToString() + ", Examination" + examinationID.ToString() + "\nDate Started: " + dateStarted.ToShortDateString() + ", Completed: " + dateComplete.ToShortDateString() + ", Spectacles " + spectacles.ToString();
            }
        }
        public Nullable<int> examinationID {get; set;} //if based on existing examination
        public IEnumerable<CheckboxItem> AvailableComplaints { get; set; }
        public IEnumerable<CheckboxItem> SelectedLeftComplaints { get; set; }
        public IEnumerable<CheckboxItem> SelectedRightComplaints { get; set; }
        public PostedComplaints PostedLeftComplaints { get; set; }
        public PostedComplaints PostedRightComplaints { get; set; }

        public OtherListModel OtherComplaintsRight { get; set;}
        public List<string> OtherComplaintsRightList{get{return OtherComplaintsRight.list;}}
        public OtherListModel OtherComplaintsLeft { get; set; }
        public List<string> OtherComplaintsLeftList{get{return OtherComplaintsLeft.list;}}

        public IEnumerable<CheckboxItem> AvailableOcularHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedLeftOcularHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedRightOcularHistory { get; set; }
        public PostedComplaints PostedLeftOcularHistory { get; set; }
        public PostedComplaints PostedRightOcularHistory { get; set; }

        public OtherListModel OtherRightOcularHistory { get; set; }
        public List<string> OtherRightOcularHistoryList{get{return OtherRightOcularHistory.list;}}
        public OtherListModel OtherLeftOcularHistory { get; set; }
        public List<string> OtherLeftOcularHistoryList{get{return OtherLeftOcularHistory.list;}}

        public IEnumerable<CheckboxItem> AvailableMedicalHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedMedicalHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedFamilyHistory { get; set; }
        public PostedComplaints PostedMedicalHistory { get; set; }
        public PostedComplaints PostedFamilyHistory { get; set; }

        public OtherListModel OtherMedicalHistory { get; set; }
        public List<string> OtherMedicalHistoryList { get { return OtherMedicalHistory.list; } }
        public OtherListModel OtherFamilyHistory { get; set; }
        public List<string> OtherFamilyHistoryList { get { return OtherFamilyHistory.list; } }

        public bool spectacles { get; set; }

        private Camp _camp;
        public Camp camp
        {
            get { return _camp; }
            set 
            {
                _camp = value;
                campId = value.Id;
            }
        }
        private Patient _patient;
        public Patient patient {
            get { return _patient; }
            set {
                _patient = value;
                patientId = value.Id;
            }
        }

        public Nullable<int> campId { get; set; }
        public Nullable<int> patientId { get; set; }
        public string photoPath { get; set; }
        public System.DateTime dateStarted { get; set; }
        public System.DateTime dateComplete { get; set; }
    }

    public enum ListType
    {
        Complaint,
        OcularHistory,
        MedicalHistory
    };
    public static class ExaminationCheckboxRepository
    {
        private static string[] ComplaintOptions = new string[] { "Blurry Vision", "Dryness", "Redness", "Swollen Lid"};
        private static string[] OcularHistoryOptions = new string[] { "Refractive Error", "Squint", "Corneal Opacity", "Cataract" };
        private static string[] MedicalHistoryOptions = new string[] { "Diabetes", "High Blood Pressure", "Cancer", "High Cholesterol", "Allergies" };
        public static CheckboxItem GetComplaints(int id)
        {
            return GetList(ListType.Complaint).FirstOrDefault(x => x.Id.Equals(id));
        }

        public static CheckboxItem GetOcularHistory(int id)
        {
            return GetList(ListType.OcularHistory).FirstOrDefault(x => x.Id.Equals(id));
        }

        public static CheckboxItem GetMedicalHistory(int id)
        {
            return GetList(ListType.MedicalHistory).FirstOrDefault(x => x.Id.Equals(id));
        }

        public static IEnumerable<CheckboxItem> GetList(ListType type)
        {
            var rawList = optionsListForType(type);
            
            var list = new List<CheckboxItem>();
            int i;
            for (i = 0; i < rawList.Count(); i++) {
                list.Add(new CheckboxItem { Name = rawList[i], Id = i + 1 });
            }
            //list.Add(new CheckboxItem {Name = "Other", Id = i + 1});
            return list;
        }

        private static string[] optionsListForType(ListType type)
        {
            switch (type)
            {
                case ListType.Complaint:
                    return ComplaintOptions;
                case ListType.OcularHistory:
                    return OcularHistoryOptions;
                case ListType.MedicalHistory:
                    return MedicalHistoryOptions;
                default:
                    return null;
            }
        }
    }
    //second examination section
    public class ExaminationSection2CreateModel
    {
        public ExaminationSection2CreateModel()
        {
            OtherDiagnosisRight = new OtherListModel("OtherDiagnosisRightList");
            OtherDiagnosisLeft = new OtherListModel("OtherDiagnosisLeftList");

            visualAcuity = new VisualAcuityModel();
            refraction = new Dictionary<Eye,RefractionModel>() {{Eye.Right,new RefractionModel("RE")},{Eye.Left,new RefractionModel("LE")}};
            alignment = new List<AlignmentModel>();
            anteriorSegment = new AnteriorSegmentModel();
            externalAdnexa = new ExternalAdnexaModel();
            fundusFindings = new FundusFindingsModel();
            pupillaryReaction = new PupillaryReactionModel();

        }
        private Camp _camp;
        public Camp camp
        {
            get { return _camp; }
            set
            {
                _camp = value;
                campId = value.Id;
            }
        }
        private Patient _patient;
        public Patient patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                patientId = value.Id;
            }
        }

        public Nullable<int> campId { get; set; }
        public Nullable<int> patientId { get; set; }
        public string photoPath { get; set; }
        public System.DateTime dateStarted { get; set; }
        public System.DateTime dateComplete { get; set; }

        public VisualAcuityModel visualAcuity { get; set; }
        public Dictionary<Eye,RefractionModel> refraction { get; set; }
        public RefractionModel rightRefraction { get; set; }
        public List<AlignmentModel> alignment { get; set; } //list of 9, left to right, top to bottom
        public AnteriorSegmentModel anteriorSegment { get; set; }
        public ExternalAdnexaModel externalAdnexa { get; set; }
        public FundusFindingsModel fundusFindings {get; set;}
        public PupillaryReactionModel pupillaryReaction { get; set; }
        public int leftTonometry { get; set; }
        public int rightTonometry { get; set; }

        public IEnumerable<CheckboxItem> AvailableDiagnosisOptions { get; set; }
        public IEnumerable<CheckboxItem> SelectedDiagnosisLeft { get; set; }
        public IEnumerable<CheckboxItem> SelectedDiagnosisRight { get; set; }
        public PostedComplaints PostedDiagnosisLeft { get; set; }
        public PostedComplaints PostedDiagnosisRight { get; set; }

        public OtherListModel OtherDiagnosisLeft { get; set; }
        public List<string> OtherDiagnosisLeftList{get{return OtherDiagnosisLeft.list;}}
        public OtherListModel OtherDiagnosisRight { get; set; }
        public List<string> OtherDiagnosisRightList{get{return OtherDiagnosisRight.list;}}

        public string Comments { get; set;}

        public bool SpectaclesDispensed { get; set; }
        public string SpectaclesComment { get; set; }
        public string SpectaclesCost { get; set; }

        public string PatientCounseling { get; set; }
        
        public bool PatientReferral { get; set; }
        public string PatientReferralComment { get; set; }
        public string PatientReferralReason { get; set; }
        public string PatientReferralReasonOther { get; set; }
        public string PatientReferralHospital { get; set; }
    }
    public class RefractionModel
    {
        public RefractionModel(string i)
        {
            eye = i;
        }
        public string eye { get; set; }
        public int sph { get; set; }
        public int cyl { get; set; }
        public int axis { get; set; }
    }

    public class AcuityMeasurement
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class VisualAcuityModel
    {
        public IEnumerable<SelectListItem> AcuityOptions
        {
            get
            {
                return _acuityOptions.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString()});
            }
        }
        private readonly List<string> _acuityOptions = new List<string> { "20/630", "20/500", "20/400", "20/320", "20/250", "20/200", "20/160", "20/125", "20/100", "20/80", "20/63", "20/50", "20/40", "20/32", "20/25", "20/20" };
        public static List<string> options = new List<string> { "Unaided", "Best Corrected", "Pinhole", "Presenting" };
        public string method { get; set; }
        public string leftAcuity { get; set; }
        public string rightAcuity { get; set; }
    }
    public class AlignmentModel
    {
        public int horizontal { get; set; }
        public int vertical { get; set; }
    }
    public enum Eye: int {
        Left,
        Right,
        Both
    }
    public class AnteriorSegmentModel
    {
        public bool isNormal { get; set; }
        public Eye abnormalEye { get; set; }
        public string description { get; set; }
    }
    public class ExternalAdnexaModel
    {
        public static string[] options = new string[] { "Option 1", "Option 2", "Option 3" };
        public bool isNormal { get; set; }
        public Eye abnormalEye { get; set;}
        public string description { get; set; }
        public string comorbidities { get; set; }
    }
    public class PupillaryReactionModel
    {
        public bool isPresent { get; set;}
        public string comment { get; set; }
    }
    public class FundusFindingsModel
    {
        public bool isPresent { get; set; }
        public string comment { get; set; }
    }

}
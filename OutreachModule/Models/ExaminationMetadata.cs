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
    public class ExaminationMetadata
    {
        [Display(Name = "Date Started")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm:ss}")]
        public System.DateTime dateStarted;
        [Display(Name = "Date Complete")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm:ss}")]
        public System.DateTime dateComplete;
        [UIHint("spectacles")]
        public bool spectacles;
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
    [MetadataType(typeof(E2Metadata))]
    public partial class E2
    {
        public List<Diagnosis> DiagnosisLeft
        {
            get
            {
                var list = Diagnosis;
                foreach (var m in list)
                {
                    bool b =  m.group=="L";
                    Debug.Print(m.value+" <"+m.group + "> " + b.ToString());
                }
                return Diagnosis.Where(m => m.group == "L").ToList();
            }
        }
        public List<Diagnosis> DiagnosisRight
        {
            get
            {
                return Diagnosis.Where(m => m.group == "R").ToList();
            }
        }
        [DisplayName("Referral")]
        public string displayReferral
        {
            get
            {
                string display = "";

                if (referral != null && (bool)referral)
                {
                    var reason = referral_other != null && referral_other.Length > 0 ? referral_other : referral_reason;
                    display += "Referred to: " + referral_location + " for " + reason;
                }
                else
                {
                    display += "Not referred";
                }
                if (referral_comment != null && referral_comment.Length > 0) {
                    display += " — Comment: " + referral_comment;
                }
                return display;
            }
        }
        [DisplayName("Spectacles")]
        public string displaySpectacles
        {
            get
            {
                string display = "";
                if (spectacles_dispensed != null && (bool)spectacles_dispensed)
                {
                    display += "Dispensed (Cost: " + spectacles_cost + ")";
                }
                else
                {
                    display += "Not Dispensed";
                }

                if (spectacles_comment != null && spectacles_comment.Length > 0)
                {
                    display += " — Comment: " + spectacles_comment;
                }
                
                return display;
            }
        }
    }
    public class E2Metadata
    {
        //Acuity
        [Display(Name = "Method")]
        public string acuity_method;
        [Display(Name = "Left")]
        public string acuity_left;
        [Display(Name = "Right")]
        public string acuity_right;

        [Display(Name = "Special Questions or Comments")]
        public string comments;
        [Display(Name = "Patient Counseling")]
        public string counseling;

        [Display(Name = "Anterior Segment")]
        public string abnormal_anterior;
        [Display(Name = "Description")]
        public string abnormal_anterior_descr { get; set; }

        [Display(Name = "Spectacles")]
        [UIHint("spectacles")]
        public Nullable<bool> spectacles_dispensed;
        [Display(Name = "Comment")]
        public string spectacles_comment;
        [Display(Name = "Cost")]
        public string spectacles_cost;
        [Display(Name = "Referral")]
        public Nullable<bool> referral;
        [Display(Name = "Comment")]
        public string referral_comment;
        [Display(Name = "Reason")]
        public string referral_reason;
        [Display(Name = "Other")]
        public string referral_other;
        [Display(Name = "Referral Location")]
        public string referral_location { get; set; }

        [Display(Name = "Timestamp")]
        public Nullable<System.DateTime> timestamp { get; set; }
        [Display(Name = "User")]
        public string user { get; set; }
    }

    public partial class Diagnosis
    {
        public static Diagnosis newDiagnosisWith(string eye, int id, string value,string other)
        {
            var diag = new Diagnosis();
            diag.e2Id = id;
            diag.value = value;
            diag.group = eye;
            diag.other = other;
            return diag;
        }
        public string displayValue {
            get
            {
                if (other != null)
                {
                    return value + ": " + other;
                }
                else return value;
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
            visualAcuity = new VisualAcuityModel();
            refraction = new RefractionModel();
            alignment = new List<AlignmentModel>();
            for (var i = 0; i < 9; i++)
            {
                alignment.Add(new AlignmentModel(i));
            }
            anteriorSegment = new AnteriorSegmentModel();
            externalAdnexa = new ExternalAdnexaModel();
            fundusFindings = new FundusFindingsModel();
            diagnosis = new DiagnosisModel();
            Comments = new CommentsModel("Special Questions or Comments");

            spectacles = new SpectaclesDispensedModel();
            PatientCounseling = new CommentsModel("Patient Counseling");
            PatientReferral = new ReferralModel();
        }
        private Examination _exam;
        public Examination exam
        {
            get { return _exam; }
            set
            {
                _exam = value;
                examId = value.Id;
            }
        }
        public Nullable<int> examId { get; set; }

        public string photoPath { get; set; }
        public System.DateTime dateStarted { get; set; }
        public System.DateTime dateComplete { get; set; }

        public VisualAcuityModel visualAcuity { get; set; }
        public RefractionModel refraction { get; set; }
        public List<AlignmentModel> alignment { get; set; } //list of 9, left to right, top to bottom
        public AnteriorSegmentModel anteriorSegment { get; set; }
        public ExternalAdnexaModel externalAdnexa { get; set; }
        public FundusFindingsModel fundusFindings {get; set;}
        public PupillaryReactionModel pupillaryReaction { get; set; }
        public int leftTonometry { get; set; }
        public int rightTonometry { get; set; }

        public DiagnosisModel diagnosis { get; set; }

        public CommentsModel Comments { get; set;}

        public SpectaclesDispensedModel spectacles {get; set;}

        public CommentsModel PatientCounseling { get; set; }
        
        public ReferralModel PatientReferral { get; set; }
    }
    public class RefractionModel
    {
        public RefractionModel()
        {
            left = new RefractionSubModel("LE");
            right = new RefractionSubModel("RE");
        }
        public RefractionSubModel left {get; set;}
        public RefractionSubModel right {get; set;}
    }

    public class RefractionSubModel {
        public RefractionSubModel(string i)
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
        private readonly List<string> _acuityOptions = new List<string> { "Enter acuity","20/630", "20/500", "20/400", "20/320", "20/250", "20/200", "20/160", "20/125", "20/100", "20/80", "20/63", "20/50", "20/40", "20/32", "20/25", "20/20" };
        public static List<string> options = new List<string> { "Unaided", "Best Corrected", "Pinhole", "Presenting" };
        public string method { get; set; }
        public int l { get; set; }
        public int r { get; set; }
        public string leftAcuity
        {
            get
            {
                return _acuityOptions[l];
            }
        }
        public string rightAcuity
        {
            get
            {
                return _acuityOptions[r];
            }
        }
    }
    public class AlignmentModel
    {
        public AlignmentModel()
        {
            index = 0;
            horizontal = 29;
            vertical = 21;
        }
        public AlignmentModel(int i)
        {
            index = i;
            horizontal = 29;
            vertical = 21;
        }
        public AlignmentModel(int i, string h, string v) //initialize with previous selection
        {
            index = i;
            horizontal = _horizontalAlignmentOptions.IndexOf(h);
            vertical = _verticalAlignmentOptions.IndexOf(v);
        }
        public int index { get; set; }
        public int horizontal { get; set; }
        public int vertical { get; set; }
        public string horizontalSelection
        {
            get
            {
                return _horizontalAlignmentOptions[horizontal];
            }
        }
        public string verticalSelection
        {
            get
            {
                return _verticalAlignmentOptions[vertical];
            }
        }
        public IEnumerable<SelectListItem> HorizontalOptions
        {
            get
            {
                return _horizontalAlignmentOptions.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString() });
            }
        }

        public IEnumerable<SelectListItem> VerticalOptions
        {
            get
            {
                return _verticalAlignmentOptions.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString() });
            }
        }
        public readonly List<string> _horizontalAlignmentOptions = new List<string> {"90XT","85XT","80XT","75XT","70XT","65XT","60XT","55XT","50XT","45XT","40XT","35XT","30XT","25XT","20XT","18XT","16XT","14XT","12XT","10XT","9XT","8XT","7XT","6XT","5XT","4XT","3XT","2XT","1XT","O","1ET","2ET","3ET","4ET","5ET","6ET","7ET","8ET","9ET","10ET","12ET","14ET","16ET","18ET","20ET","25ET","30ET","35ET","40ET","45ET","50ET","55ET","60ET","65ET","70ET","75ET","80ET","85ET","90ET"};
        public readonly List<string> _verticalAlignmentOptions = new List<string> { "50LHT", "45LHT", "40LHT", "35LHT", "30LHT", "25LHT", "20LHT", "18LHT", "16LHT", "14LHT", "12LHT", "10LHT", "9LHT", "8LHT", "7LHT", "6LHT", "5LHT", "4LHT", "3LHT", "2LHT", "1LHT", "O", "1RHT", "2RHT", "3RHT", "4RHT", "5RHT", "6RHT", "7RHT", "8RHT", "9RHT", "10RHT", "12RHT", "14RHT", "16RHT", "18RHT", "20RHT", "25RHT", "30RHT", "35RHT", "40RHT", "45RHT", "50RHT" };
    }
    public enum Eye: int {
        Neither,
        Left,
        Right,
        Both
    }
    public class AnteriorSegmentModel
    {
        public AnteriorSegmentModel()
        {
            isNormal = true;
        }
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
    public class DiagnosisModel
    {
        public DiagnosisModel()
        {
            OtherDiagnosisRight = new OtherListModel("diagnosis.OtherDiagnosisRightList");
            OtherDiagnosisLeft = new OtherListModel("diagnosis.OtherDiagnosisLeftList");
            SelectedDiagnosisLeft = new List<CheckboxItem>();
            SelectedDiagnosisRight = new List<CheckboxItem>();
            AvailableDiagnosisOptions = ExaminationCheckboxRepository.GetList(ListType.MedicalHistory);
        }
        public IEnumerable<CheckboxItem> AvailableDiagnosisOptions { get; set; }
        public IEnumerable<CheckboxItem> SelectedDiagnosisLeft { get; set; }
        public IEnumerable<CheckboxItem> SelectedDiagnosisRight { get; set; }
        public PostedComplaints PostedDiagnosisLeft { get; set; }
        public PostedComplaints PostedDiagnosisRight { get; set; }

        public OtherListModel OtherDiagnosisLeft { get; set; }
        public List<string> OtherDiagnosisLeftList { get { return OtherDiagnosisLeft.list; } }
        public OtherListModel OtherDiagnosisRight { get; set; }
        public List<string> OtherDiagnosisRightList { get { return OtherDiagnosisRight.list; } }
    }
    public class CommentsModel
    {
        public CommentsModel(string title)
        {
            name = title;
            comment = "";
        }
        public string name { get; set; }
        public string comment { get; set; }
    }
    public class SpectaclesDispensedModel
    {
        //public SpectaclesDispensedModel()
        //{

        //}
        public bool dispensed { get; set; }
        public string comment { get; set; }
        public string cost { get; set; }
    }
    public class ReferralModel
    {
        public IEnumerable<SelectListItem> ReferralReasons
        {
            get
            {
                return _referralReasons.Select((r, index) => new SelectListItem { Text = r, Value = index.ToString() });
            }
        }
        private readonly List<string> _referralReasons = new List<string> {"Cataract","Cornea","Glaucoma","Other"};
        public int otherIndex 
        {
            get
            {
                return _referralReasons.Count - 1;
            }
       }
        public bool referred { get; set; }
        public string comment { get; set; }
        public string reason { get; set; }
        public string reasonOther { get; set; }
        public string hospital { get; set; }
    }

    public partial class Refraction
    {
        public static Refraction refractionFrom(RefractionSubModel model)
        {
            var refraction = new Refraction();
            refraction.eye = model.eye;
            refraction.sph = model.sph;
            refraction.cyl = model.cyl;
            refraction.axis = model.axis;
            return refraction;
        }
    }
}
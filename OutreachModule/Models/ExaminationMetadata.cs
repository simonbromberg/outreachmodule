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
            list = new List<string>(){""};
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
            OtherComplaintsRight = new OtherListModel("OtherComplaintsRight");
            OtherComplaintsLeft = new OtherListModel("OtherComplaintsLeft");
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

        public string otherLeft { get; set; }
        public string otherRight { get; set; }
        public OtherListModel OtherComplaintsRight { get; set;}
        public OtherListModel OtherComplaintsLeft { get; set; }
        public bool hasOtherComplaintsRight { get; set; }
        public bool hasOtherComplaintsLeft {get;set;}

        public IEnumerable<CheckboxItem> AvailableOcularHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedLeftOcularHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedRightOcularHistory { get; set; }
        public PostedComplaints PostedLeftOcularHistory { get; set; }
        public PostedComplaints PostedRightOcularHistory { get; set; }

        public string otherOcularHistoryLeft { get; set; }
        public string otherOcularHistoryRight { get; set; }
        public bool hasOtherOcularHistoryRight { get; set; }
        public bool hasOtherOcularHistoryLeft { get; set; }

        public IEnumerable<CheckboxItem> AvailableMedicalHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedMedicalHistory { get; set; }
        public IEnumerable<CheckboxItem> SelectedFamilyHistory { get; set; }
        public PostedComplaints PostedMedicalHistory { get; set; }
        public PostedComplaints PostedFamilyHistory { get; set; }

        public string otherMedicalHistory { get; set; }
        public string otherFamilyHistory { get; set; }
        public bool hasOtherMedicalHistory { get; set; }
        public bool hasOtherFamilyHistory { get; set; }

        public bool spectacles { get; set; }

        private Camp _camp;
        public Camp camp
        {
            get { return _camp; }
            set 
            {
                Debug.Print("Setting camp");
                _camp = value;
                campId = value.Id;
            }
        }
        private Patient _patient;
        public Patient patient {
            get { return _patient; }
            set {
                Debug.Print("Setting patient");
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
}
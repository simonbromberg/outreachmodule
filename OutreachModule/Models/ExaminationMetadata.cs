using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace OutreachModule.Models

{
    public partial class ExamComplaint
    {
        public ExamComplaint(string eye, string complaint, string other, int examId)
        {
            this.eye = eye;
            this.complaint = complaint;
            this.otherComplaint = other;
            this.examinationId = examId;
        }
    }
    [MetadataType(typeof(ExaminationMetadata))]
    public partial class Examination
    {
        public Examination(ExaminationCreateModel model)
        {
            campId = model.campId;
            patientId = model.patientId;
            campId = model.campId;
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

    public class ExaminationMetadata
    {
        [Display(Name="Date Started")]
        public System.DateTime dateStarted { get; set; }
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
        public IEnumerable<CheckboxItem> AvailableComplaints { get; set; }
        public IEnumerable<CheckboxItem> SelectedLeftComplaints { get; set; }
        public IEnumerable<CheckboxItem> SelectedRightComplaints { get; set; }
        public PostedComplaints PostedLeftComplaints { get; set; }
        public PostedComplaints PostedRightComplaints { get; set; }

        public string otherLeft { get; set; }
        public string otherRight { get; set; }

        public int campId { get; set; }
        public int patientId { get; set; }
        public System.DateTime dateStarted { get; set; }
        public System.DateTime dateComplete { get; set; }
    }
    public static class ComplaintRepository
    {
        /// <summary>
        /// for get fruit for specific id
        /// </summary>
        public static CheckboxItem Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        /// <summary>
        /// for get all fruits
        /// </summary>
        public static IEnumerable<CheckboxItem> GetAll()
        {                
            return new List<CheckboxItem> {
                              new CheckboxItem {Name = "Blurry Vision", Id = 1 },
                              new CheckboxItem {Name = "Dryness", Id = 2},
                              new CheckboxItem {Name = "Redness", Id = 3},
                              new CheckboxItem {Name = "Swollen Lid", Id = 4},
                              new CheckboxItem {Name = "Other", Id = 5}
                            };
        }
    }
}
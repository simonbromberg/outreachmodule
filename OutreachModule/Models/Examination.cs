//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OutreachModule.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Examination
    {
        public Examination()
        {
            this.ExamComplaints = new HashSet<ExamComplaint>();
        }
    
        public int Id { get; set; }
        public int patientId { get; set; }
        public System.DateTime dateStarted { get; set; }
        public System.DateTime dateComplete { get; set; }
        public string user { get; set; }
        public int campId { get; set; }
        public bool spectacles { get; set; }
        public Nullable<int> status { get; set; }
    
        public virtual Camp Camp { get; set; }
        public virtual ICollection<ExamComplaint> ExamComplaints { get; set; }
        public virtual Patient Patient { get; set; }
    }
}

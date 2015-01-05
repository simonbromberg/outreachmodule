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
    
    public partial class E2
    {
        public E2()
        {
            this.Alignments = new HashSet<Alignment>();
            this.Diagnosis = new HashSet<Diagnosis>();
            this.Refractions = new HashSet<Refraction>();
        }
    
        public int Id { get; set; }
        public string acuity_method { get; set; }
        public string comments { get; set; }
        public string counseling { get; set; }
        public string abnormal_anterior { get; set; }
        public string abnormal_anterior_descr { get; set; }
        public Nullable<bool> spectacles_dispensed { get; set; }
        public string spectacles_comment { get; set; }
        public string spectacles_cost { get; set; }
        public Nullable<bool> referral { get; set; }
        public string referral_comment { get; set; }
        public string referral_reason { get; set; }
        public string referral_other { get; set; }
        public string referral_location { get; set; }
        public string acuity_left { get; set; }
        public string acuity_right { get; set; }
        public Nullable<System.DateTime> timestamp { get; set; }
        public string user { get; set; }
    
        public virtual ICollection<Alignment> Alignments { get; set; }
        public virtual ICollection<Diagnosis> Diagnosis { get; set; }
        public virtual Examination Examination { get; set; }
        public virtual ICollection<Refraction> Refractions { get; set; }
    }
}

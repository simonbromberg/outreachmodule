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
    
    public partial class Patient
    {
        public Patient()
        {
            this.camp_patient = new HashSet<camp_patient>();
            this.Examinations = new HashSet<Examination>();
        }
    
        public int Id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public Nullable<int> age { get; set; }
        public string family_member_name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string block { get; set; }
        public string gram_panchayat { get; set; }
        public string contact_1 { get; set; }
        public string contact_2 { get; set; }
        public string family_contact_number { get; set; }
        public int campId { get; set; }
        public string photoPath { get; set; }
        public Nullable<int> mrnId { get; set; }
        public Nullable<System.DateTime> dateadded { get; set; }
        public Nullable<System.DateTime> lastupdated { get; set; }
        public bool hasSMS_contactFam { get; set; }
        public bool hasSMS_contact1 { get; set; }
        public bool hasSMS_contact2 { get; set; }
    
        public virtual Camp fromCamp { get; set; }
        public virtual ICollection<camp_patient> camp_patient { get; set; }
        public virtual mrn mrnRef { get; set; }
        public virtual ICollection<Examination> Examinations { get; set; }
    }
}

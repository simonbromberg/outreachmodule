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
    
    public partial class camp_patient
    {
        public int Id { get; set; }
        public int campId { get; set; }
        public int patientId { get; set; }
    
        public virtual Camp Camp { get; set; }
        public virtual Patient Patient { get; set; }
    }
}

﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OutreachDBEntities : DbContext
    {
        public OutreachDBEntities()
            : base("name=OutreachDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Camp> Camps { get; set; }
        public virtual DbSet<camp_patient> camp_patient { get; set; }
        public virtual DbSet<mrn> mrns { get; set; }
        public virtual DbSet<ExamComplaint> ExamComplaints { get; set; }
        public virtual DbSet<Examination> Examinations { get; set; }
        public virtual DbSet<Alignment> Alignments { get; set; }
        public virtual DbSet<Diagnosi> Diagnosis { get; set; }
        public virtual DbSet<E2> E2 { get; set; }
        public virtual DbSet<Refraction> Refractions { get; set; }
    }
}

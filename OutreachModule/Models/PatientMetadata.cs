using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CustomExtensions;

namespace OutreachModule.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {

        public string birthdateFormatted
        {
            get
            {
                if (birthdate != null)
                {
                    return ((DateTime)birthdate).ToString("d MMMM yyyy");
                }
                    return null;
            }
        }
        [Display(Name = "Camp Code")]
        public string camp_code
        {
            get
            {
                if (fromCamp != null) {
                    return fromCamp.code;
                }
                return null;
            }
        }
        public bool search(string query)
        {
            var ignoreCase = StringComparison.OrdinalIgnoreCase;

            if (query.Contains("male", ignoreCase))
            {
                return gender.EqualsIgnoreCase(query);
            }
            return mrn.Contains(query, ignoreCase) || name.Contains(query,ignoreCase) || camp_code.Contains(query,ignoreCase) || address_1.Contains(query,ignoreCase) || address_2.Contains(query,ignoreCase);
        }
        [Display(Name = "Medical Record Number")]
        public string mrn
        {
            get
            {
                if (mrnRef != null)
                {
                    return mrnRef.recordnumber;
                }
                return null;
            }
        }
    }

    public class PatientMetadata
    {
        [Display(Name = "Name")]
        [Required]
        public string name;
        [Required]
        [Display(Name = "Sex")]
        public string gender;
        [Display(Name = "Birthday")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}")]
        public Nullable<System.DateTime> birthdate;
        [Required]
        [Display(Name = "Age")]
        public Nullable<int> age;
        [Display(Name = "Family Member Name")]
        public string family_member_name;
        [Display(Name = "Contact #")]
        public string family_contact_number;
        [Display(Name = "Street Address")]
        public string address_1;
        [Display(Name = "Apt #")]
        public string address_2;
        [Display(Name = "State")]
        public string state;
        [Display(Name = "District")]
        public string district;
        [Display(Name = "Block")]
        public string block;
        [Display(Name = "Gram Panchayat")]
        public string gram_panchayat;
        [Display(Name = "Phone #")]
        public string contact_1;

        [UIHint("SMSCompatible")]
        public bool hasSMS_contactFam { get; set; }
        [UIHint("SMSCompatible")]
        public bool hasSMS_contact1 { get; set; }
        [UIHint("SMSCompatible")]
        public bool hasSMS_contact2 { get; set; }
        [Display(Name = "Alternative Phone #")]
        public string contact_2;
        [Display(Name = "Date Added")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm}")]
        public Nullable<System.DateTime> dateadded;
        [Display(Name = "Last Updated")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy HH:mm}")]
        public Nullable<System.DateTime> lastupdated;
    }

    public class CreatePatientViewModel
    {
        public Patient patient;
        public mrn mrnRef;
    }
}
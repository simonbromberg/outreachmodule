using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OutreachModule.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {
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
        [Display(Name = "Gender")]
        public string gender;
        [Display(Name = "Birthday")]
        [DisplayFormat(DataFormatString = "{0:MMMM d, yyyy}")]
        public Nullable<System.DateTime> birthdate;
        [Required]
        [Display(Name = "Age")]
        public Nullable<int> age;
        [Display(Name = "Family Member Name")]
        public string family_member_name;
        [Display(Name = "Family Member Contact #")]
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
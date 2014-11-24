using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OutreachModule.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    [MetadataType(typeof(CampMetadata))]
    public partial class Camp
    {
        public string selectRow
        {
            get
            {
                var dateString = date != null ? " - " + ((DateTime)date).ToString("yyyy-MM-dd") : "";
                return code + " " + location_1 + dateString;
            }
        }
    }

    public class CampMetadata
    {
        [Display(Name = "Camp Code")]
        [Required]
        public string code;
        [Display(Name = "Description")]
        public string description;
        [Display(Name = "Location")]
        public string location_1;
        [Display(Name = " ")]
        public string location_2;
        [Display(Name = "Organizing Authority")]
        public string organizer;
        [Display(Name = "Contact Person")]
        public string contact_person;
        [Display(Name = "Contact Person's Mobile Number")]
        public string contact_number;
        [Display(Name = "Camp Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public Nullable<System.DateTime> date;
        [Display(Name = "Camp Type")]
        public string type;
    }
}
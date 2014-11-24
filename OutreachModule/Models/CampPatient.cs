namespace OutreachModule.Models
{
    using System;
    using System.Collections.Generic;

    public partial class camp_patient
    {
        public static camp_patient NewWithIDs(int ptId, int cmpId)
        {
            var cp = new camp_patient();
            cp.patientId = ptId;
            cp.campId = cmpId;
            return cp;
        }

    }
}
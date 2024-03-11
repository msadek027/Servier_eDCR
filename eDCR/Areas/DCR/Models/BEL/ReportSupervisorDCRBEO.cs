using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportSupervisorDCRBEO
    {
     
        public string MarketName { get; set; }

        public string Date { get; set; }

        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }

        public string ShiftName { get; set; }

        public string CallType { get; set; }
        public string Accompany { get; set; }
        public string LocationFollowed { get; set; }
       
     
        public string DCRType { get; set; }
        public string DCRSubType { get; set; }
        public string AddressWord { get; set; }
        public string NoOfInterns { get; set; }

        public string Remarks { get; set; }
    }
}
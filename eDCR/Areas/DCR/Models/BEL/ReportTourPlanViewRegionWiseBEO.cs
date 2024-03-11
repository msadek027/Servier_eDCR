using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportTourPlanViewRegionWiseBEO
    {
        
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }   
        public string LocName { get; set; }
        public string LocCode { get; set; }
        public string DayNumber { get; set; }

        public string MInstName { get; set; }
        public string MMeetingPlace { get; set; }
        public string MSetTime { get; set; }
        public string MAccompany { get; set; }
        public string MAllowence { get; set; }


        public string EInstName { get; set; }
        public string EMeetingPlace { get; set; }
        public string ESetTime { get; set; }
        public string EAccompany { get; set; }
        public string EAllowence { get; set; }

        public string MReview { get; set; }
        public string EReview { get; set; }
    }
}
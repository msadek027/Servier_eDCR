using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDCRPlanExecutionBEL
    {
        public string SL { get; set; }
        public string MarketName { get; set; }
        public string Date { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string ShiftName { get; set; }
        public string PlanSelected { get; set; }
        public string PlanSample { get; set; }
        public string PlanGift { get; set; }
        public string DCRSelected { get; set; }
        public string DCRSample { get; set; }
        public string DCRGift { get; set; } 
        public string Accompany { get; set; }
        public string Remarks { get; set; }
        public string DcrType { get;  set; }
    }


}
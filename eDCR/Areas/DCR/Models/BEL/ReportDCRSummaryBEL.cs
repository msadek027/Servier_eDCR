using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDCRSummaryBEL
    {
        public string SL { get; set; }
        public string Date { get; set; }
        public string MarketName { get; set; }       
        public string Accompany { get; set; }
        public string SelectedRegularQty { get; set; }
        public string SampleRegularQty { get; set; }
        public string SampleInternQty { get; set; }
        public string GiftRegularQty { get; set; }
        public string GiftInternQty { get; set; }
        public string TotalDOT { get; set; }
        public string RegularDOT { get; set; }
        public string InternDOT { get; set; }
        public string MorningWP { get; set; }
        public string EveningWP { get; set; }
        public string TotalDCR { get; set; }
        public string MorningDCR { get; set; }
        public string EveningDCR { get; set; }
        public string InternDCR { get; set; }
        public string OtherDCR { get; set; }
        public string MorningAbsent { get; set; }
        public string EveningAbsent { get; set; }


        public string MPOCode { get; set; }
        public string MPOName { get; set; }
   

    }

    public class ReportDCRDetail
    {
      
        public string MPGroup { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
    

        public string ItemType { get; set; }

        public string Quantity { get; set; }

        public string SetDate { get; set; }

        public string DcrType { get; set; }
    }


    public class ReportDcrSummaryTmRsm
    {
        public string SL { get; set; }
        public string Date { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string MarketName { get; set; }
 
        public string TotalDCR { get; set; }
        public string DoubleCall { get; set; }
        public string SingleCall { get; set; }
        public string MorningDCR { get; set; }
        public string EveningDCR { get; set; }
        public string NewDoctorDCR { get; set; }
        public string InternDoctorDCR { get; set; }
        public string NoOfInternDoctorVisited { get; set; }
        public string OtherDCR { get; set; }
        public string NoOfChemist { get; set; }
        public string Accompany { get; set; }
    }


}
    
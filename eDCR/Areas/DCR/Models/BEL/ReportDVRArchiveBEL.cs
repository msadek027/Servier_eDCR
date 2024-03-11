using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDVRArchiveBEL
    {
        public string SL { get; set; }
        public string Date { get; set; }
        public string DayName { get; set; }      
        public string MarketName { get; set; }     
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string MPGroup { get; set; }
        
        public string MorningDOT { get; set; }
        public string EveningDOT { get; set; }
        public string TotalDOT { get; set; }
        public string InternDOT { get; set; }


      
    }
}
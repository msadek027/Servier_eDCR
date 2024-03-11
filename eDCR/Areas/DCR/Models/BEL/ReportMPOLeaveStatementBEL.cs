using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportMPOLeaveStatementBEL
    {

        public string LocCode { get; set; }

        public string RegionName { get; set; }
        public string TerritoryName { get; set; }
        public string MarketName { get; set; }     
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string FullLeave { get; set; }
        public string MorningLeave { get; set; }
        public string EveningLeave { get; set; }
        public string SetDate { get; set; }

        public string ShiftName { get; set; }

    }
}
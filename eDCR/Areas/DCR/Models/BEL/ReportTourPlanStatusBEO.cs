using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportTourPlanStatusBEO
    {
        public string Designation { get; set; }
        public string TPCount { get; set; }
    }

    public class ReportTourPlanStatus
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string MarketName { get; set; }
        public string Designation { get; set; }
        public string TPStatus { get; set; }
        public string DVRStatus { get; set; }
        public string PWDSStatus { get; set; }
        public string GWDSStatus { get; set; }
    }

    public class ReportTourPlanChangeFortnightStatus
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string MarketName { get; set; }
        public string Designation { get; set; }
        public string ChangeTPStatus { get; set; }
        public string Fortnight1Status { get; set; }
        public string Fortnight2Status { get; set; }
        public string BillStatus { get; set; }
        public string StockCheckStatus { get; set; }
    }
}
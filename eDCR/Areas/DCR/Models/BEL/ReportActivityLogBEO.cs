using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportActivityLogBEO
    {
        public string SL { get;  set; }
        public string EmployeeCode { get;  set; }
        public string EmployeeName { get;  set; }
        public string Designation { get;  set; }
        public string MarketName { get;  set; }
        public string LocCode { get;  set; }
        public string SetDate { get;  set; }
        public string Operation { get;  set; }
        public string Message { get;  set; }
        public string Terminal { get; set; }
        public string SetLocCode { get;  set; } 
        public string SetByID { get; set; }
        public string SetByName { get; set; }
    }
}
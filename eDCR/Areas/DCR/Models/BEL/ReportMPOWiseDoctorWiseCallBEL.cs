using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportMPOWiseDoctorWiseCallBEL
    {
      
        public string MPOName { get; set; }        
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }    
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackSize { get; set; }
        public string FromDate { get; set; }
        public string Quantity { get; set; }
        public string DcrType { get; set; }
    }


    public class ReportDoctorWiseNoOfCallBEO
    {
        public string MarketName { get; set; }
        public string MPGroup { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public string TotalCall { get; set; }
    }

    }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDCRDoctorAbsentBEL
    {
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryName { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string MPGroup { get; set; }
        public string MarketName { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Degrees { get; set; } 
        public string Speciality { get; set; }
        public string ShiftName { get; set; }
        public string Absent { get; set; }
        public string NotAllowed { get; set; }
        public string Missed { get; set; }
 
    }

    public class ReportDCRDoctorAbsentDetail
    {
        public string MPGroup { get; set; }
        public string Date { get; set; }
        public string NotAllowed { get; set; }
        public string Absent { get; set; }
        public string Missed { get; set; }
    }
}
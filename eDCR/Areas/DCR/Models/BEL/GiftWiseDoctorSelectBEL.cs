using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class GiftWiseDoctorSelectBEL
    {
  
        public string MasterSL { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }
        public string MasterStatus { get; set; }   
        public string MPGroup { get; set; }
        public string MonthNumber { get; set; }
        public virtual ICollection<GiftWiseDoctorSelectDetail> DetailList { get; set; }
    }
    public class GiftWiseDoctorSelectDetail
    {
        public string MasterSL { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string TotalQty { get; set; }
        public string Specialization { get; set; }
    }
     
}
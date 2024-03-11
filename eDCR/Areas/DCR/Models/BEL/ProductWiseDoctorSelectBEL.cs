using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ProductWiseDoctorSelectBEL 
    {
        public string PWDSMstSL { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string MasterStatus { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string DivisionCode { get; set; }
        public string MonthNumber { get; set; }
        public string DivisionName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TMId { get; set; }
        public string TMName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public virtual ICollection<ProductWiseDoctorSelectMaster> ProductWiseDoctorSelectMaster { get; set; }
        public virtual ICollection<ProductWiseDoctorSelectDetail> ProductWiseDoctorSelectDetail { get; set; }


        public string MPGroup { get; set; }

        public string MonthName { get; set; }
    }

    public class ProductWiseDoctorSelectMaster
    {
        public string MasterSL { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string MonthNumber { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }
        public string MPGroup { get; set; }
        public string MasterStatus { get; set; }
    }

    public class ProductWiseDoctorSelectDetail
    {
      
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }


        public string TotalQty { get; set; }

        public string Specialization { get; set; }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class HRDataUploadBEL
    {
            public virtual ICollection<HRDataUploadMPO> hRDataUploadMPOList { get; set; }

    }


        public class HRDataUploadMPO
        {
         
            public string EmployeeID { get; set; }
            public string EmployeeName { get; set; }
            public string Phone { get; set; }
            public string Designation { get; set; }   
            public string ProductGroup { get; set; }
            public string MarketGroup { get; set; }
            public string MarketCode { get; set; }
            public string MarketName { get; set; }
            public string TerritoryCode { get; set; }
            public string TerritoryName { get; set; }
            public string RegionCode { get; set; }
            public string RegionName { get; set; }
            public string DivisionCode { get; set; }
            public string DivisionName { get; set; }
            public string DepotCode { get; set; }
            public string DepotName { get; set; }    
            public string CompanyCode { get; set; }
        }

}
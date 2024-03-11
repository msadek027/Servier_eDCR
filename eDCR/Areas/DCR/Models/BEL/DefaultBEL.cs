using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class DefaultBEL
    {
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string Designation { get; set; }
       
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

        public string TerritoryCode { get; set; }
       public string TerritoryManagerID { get; set; }
        public string TerritoryManagerName { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }  
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
     
        public string MPGroup { get; set; }
        public string IsPartialAllow { get; set; }
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeName { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public string TerritoryName { get; internal set; }
    }


    public class GenMonth
    {
        public string MonthNumber { get; set; }
        public string MonthName { get; set; }
    }

    public class ProductInfo
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }

     

        public string ItemType { get; set; }
    }


}
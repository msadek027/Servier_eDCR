using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDCRItemExecutionBEL
    {
       


        public string ItemType { get; set; }
        public string ProductCode { get; set; }      
        public string Year { get; set; }
        public   string MonthNumber{ get; set; }
        public string TerritoryManagerID { get; set; }
        public string MPGroup{ get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string TerritoryManagerName { get; set; }
        public string RestQty { get; set; }
        public string ExecuteQty { get; set; }
        public string BalanceQty { get; set; }
        public string VariableQty { get; set; }
        public string CentralQty { get; set; }
        public string TotalQty { get; set; }
     
    }

     public class ReportDCRItemExecutionDetail
     {
         public string ProductCode { get; set; }
         public string ProductName { get; set; }      
         public string RestQty { get; set; }
         public string ExecuteQty { get; set; }
         public string BalanceQty { get; set; }
         public string SetDate { get; set; }
         public string CentralQty { get; set; }
         public string TotalQty { get; set; }
         public string VariableQty { get; set; }
     }

}
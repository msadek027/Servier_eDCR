using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class PromotionalItemViewBEL
    {    
        public virtual ICollection<PromotionalItemViewDetail> ItemList { get; set; }
    }
    public class PromotionalItemViewDetail
    {
       
        public string ProductCode { get; set; }
        public string ProductName { get; set; }       
        public string MPGroup { get; set; }
       public string MonthNumber { get; set; }
       public string Year { get; set; }
       public string Remark { get; set; }
       public string ItemType { get; set; }
       public string TotalQty { get; set; }
       public string ItemFor { get; set; }
       public string RestQty { get; set; }
       public string GivenQty { get; set; }
       public string ExecuteQty { get; set; }
       public string BalanceQty { get; set; }
       public string LossQty { get; set; }
       public string GainQty { get; set; }
       public string AdjustmentType { get; set; }
       public string CentralQty { get; set; }
       public string VariableQty { get; set; }
       public string PhysicalQty { get; set; }
    }


    public class ReportPromotionalItemExecuion
    {
        public string SL { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string MpoCode { get; set; }
        public string MpoName { get; set; }
        public string MarketName { get; set; }
        public string SBU { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ItemType { get; set; }


        public string TotalQty { get; set; }
        public string ExecuteQty { get; set; }
        public string BalanceQty { get; set; }
        public string PromotionalCost { get; set; }
       
    }


}
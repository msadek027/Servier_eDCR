using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class PromotionalItemUploadBEL
    {
        public virtual ICollection<PromotionalItemUploadDetail> ItemList { get; set; }
    }
    public class PromotionalItemUploadDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }     
        public string Quantity { get; set; }
        public string MPGroup { get; set; }
        public string MonthNumber { get; set; }
        public string Year { get; set; }     
        public string SetDate { get; set; }
        public string ItemFor { get; set; }
        public string ItemType { get; set; }

    
    }

    public class PromotionalItemFormatDataUploadBEO
    {
        public string FileName { get; set; }       
        public virtual ICollection<PromotionalItemFormatDataUploadDetail> ItemList { get; set; }
    }
    public class PromotionalItemFormatDataUploadDetail
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string GiftName { get; set; } 
        public string Quantity { get; set; }
        public string MPGroup { get; set; }
        public string MonthNumber { get; set; }
        public string Year { get; set; } 
        public string ItemFor { get; set; }
        public string FileName { get; set; }
        public string SBU { get; set; }
        public string MarketGroup { get; set; }
    }

}
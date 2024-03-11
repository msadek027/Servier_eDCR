using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ItemDataUploadBEL
    {
        public virtual ICollection<ItemDataUploadDetail> ItemList { get; set; }
    }

    public class ItemDataUploadDetail
    {
    
        public string ProductCode { get; set; }
        public string ProductName { get; set; }  
        public string GenericName { get; set; }
        public string PackSize { get; set; }
        public string BrandName { get; set; }
        public string MarketGroup { get; set; }
        public string SBU { get; set; }
        public string CostPerUnit { get; set; }
        public string HilightingProduct { get;  set; }
    }
}
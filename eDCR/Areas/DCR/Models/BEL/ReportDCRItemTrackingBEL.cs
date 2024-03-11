using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDCRItemTrackingBEL
    {
        public string ItemType { get; set; }

        public string TerritoryManagerID { get; set; }

        public string MPGroup { get; set; }

        public string ProductCode { get; set; }

        public string GiftCode { get; set; }

        public string MPOCode { get; set; }

        public string MPOName { get; set; }

        public string TerritoryManagerName { get; set; }

        public string GivenQty { get; set; }

        public string ExecuteQty { get; set; }

        public string ToDate { get; set; }

        public string FromDate { get; set; }
    }
}
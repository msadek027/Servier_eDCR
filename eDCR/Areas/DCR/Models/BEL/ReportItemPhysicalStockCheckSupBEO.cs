using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportItemPhysicalStockCheckSupBEO
    {
        public string SetDate { get; set; }
        public string MarketName { get; set; }
        public string MPOName { get; set; }

        public string ProductName { get; set; }
        public string PackSize { get; set; }

        public string ItemType { get; set; }
        public string eDCRQty { get; set; }
        public string PhysicalQty { get; set; }
        public string Remarks { get; set; }


        public string CheckedByID { get; set; }
        public string CheckedByName { get; set; }
        public string CheckedByDesignation { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportAdminBEO
    {
    }
    public class ReportAdminPromotionalItemExecuion
    {
        public string SL { get; set; }
        public string MPGroup { get; set; }
        public string MarketName { get; set; }
        public string CarryQty { get; set; }       
        public string CentralQty { get; set; }
        public string AddDefiQty { get; set; }
        public string TotalQty { get; set; }
        public string ExecuteQty { get; set; }
        public string BalanceQty { get; set; }
       
    }
}
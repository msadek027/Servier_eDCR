using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportItemBalanceBEL
    {
        public string ItemType { get; set; }

        public string MPGroup { get; set; }

        public string ProductCode { get; set; }

        public string GiftCode { get; set; }

        public string ProductName { get; set; }

        public string RestQty { get; set; }

        public string ExecuteQty { get; set; }

        public string BalanceQty { get; set; }

        public string CurrentMonthQty { get; set; }

        public string CentralQty { get; set; }

        public string TotalQty { get; set; }

        public string VariableQty { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportSampleStatementSummaryBEO
    {
        public string MarketName { get; set; }
        public string EmployeeName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ReceiveQty { get; set; }
        public string ExecuteQty { get; set; }
        public string BalanceQty { get; set; }
    }
}
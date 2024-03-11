using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Universal.Gateway
{
    public class ReportData
    {
        public string ReportName { get; set; }
        public string ReportFormatType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string BuyerCode { get; set; }
        public string BuyerName { get; set; }
    }
}
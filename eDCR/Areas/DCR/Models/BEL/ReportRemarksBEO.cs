using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportRemarksBEO
    {
        public string SL { get; set; }   
        public string FromDate { get; set; }    
        public string MPGroup { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ItemType { get; set; }
        public string PackSize { get; set; }
        public string Quantity { get; set; }     
        public string Remark { get; set; }
    }
}
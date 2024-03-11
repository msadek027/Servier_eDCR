using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportItemStatementBEL
    {

        public string SL { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
     
        public string ItemType { get; set; }
        public string MPGroup { get; set; }
        public string MarketName { get; set; }

        public string RestQty { get; set; }
        public string CentralQty { get; set; }
        public string TotalQtyAddDefi0130 { get; set; }
        public string TotalQty { get; set; }
        public string GivenQty0108 { get; set; }
        public string TotalStock0108 { get; set; }



        public string d01 { get; set; }
        public string d02 { get; set; }
        public string d03 { get; set; }
        public string d04 { get; set; }
        public string d05 { get; set; }
        public string d06 { get; set; }
        public string d07 { get; set; }
        public string d08 { get; set; }

        public string TotalDCR0108 { get; set; }
        public string GivenQty0915 { get; set; }
        public string TotalStock0915 { get; set; }
        public string d09 { get; set; }
        public string d10 { get; set; }
        public string d11 { get; set; }
        public string d12 { get; set; }
        public string d13 { get; set; }
        public string d14 { get; set; }
        public string d15 { get; set; }



        public string TotalDCR0915 { get; set; }
        public string GivenQty1623 { get; set; }
        public string TotalStock1623 { get; set; }

    
        public string d16 { get; set; }
        public string d17 { get; set; }
        public string d18 { get; set; }
        public string d19 { get; set; }
        public string d20 { get; set; }
        public string d21 { get; set; }
        public string d22 { get; set; }
        public string d23 { get; set; }

        public string TotalDCR1623 { get; set; }
        public string GivenQty2431 { get; set; }
        public string TotalStock2431 { get; set; }
        public string d24 { get; set; }
        public string d25 { get; set; }
        public string d26 { get; set; }
        public string d27 { get; set; }
        public string d28 { get; set; }
        public string d29 { get; set; }
        public string d30 { get; set; }
        public string d31 { get; set; }

        public string TotalDCR2431 { get; set; }

        public string TotalDCR0131 { get; set; }
    public string ClosingStock { get; set; }


      
    }

    public class ReportItemStatementDetailLink
    {



        public string SetDate { get; set; }
        public string MPGroup { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string Remarks { get; set; }

       
    }
}
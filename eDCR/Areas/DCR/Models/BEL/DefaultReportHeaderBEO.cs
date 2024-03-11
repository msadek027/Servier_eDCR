using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class DefaultReportHeaderBEO
    {
        public string vCntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        public string vPrintDate = "Print Date: " + DateTime.Now.ToString("MMMM dd, yyyy") + ".";
        public string vCompanyName = HttpContext.Current.Session["OwnerName"].ToString();
        //public string vCompanyName = "Square Pharmaceuticals Ltd.";
        // public string vCompanyName = "Square Pharmaceuticals Ltd. - Kenya";

       
    }
}
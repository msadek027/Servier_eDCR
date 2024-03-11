using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportSelectedItemPlanBEL 
    {
     
        public string MarketName { get; set; }    
        public string MPOName { get; set; }     
        public string ProductName { get; set; }
        public string CentralQty { get; set; }
        public string VariableQty { get; set; }
        public string RestQty { get; set; }       
        public string TotalQty { get; set; }
        public string BalanceQty { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }

    }
}
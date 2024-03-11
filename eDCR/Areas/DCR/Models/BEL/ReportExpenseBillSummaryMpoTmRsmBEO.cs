using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportExpenseBillSummaryMpoTmRsmBEO
    {        
       
    }
  
    public class ReportExpenseBillMpoTmRsmDaTaOthersBEO
    {
        public string MasterSL { get; set; }
        public string TA { get; set; }
        public string Distance { get; set; }  
        public string RegionCode { get; set; }
        public string RegionType { get; set; }
        public string cmbRegionType { get; set; }
        public string RegionRate { get; set; }
        public string MileageRate { get; set; }
        public string FuelRate { get; set; }      
        public string Allowence { get; internal set; }
        public string AllowenceRate { get; internal set; }
    }
public class AllowanceNatuare
    { 
    public string Allowence { get; internal set; }
    public string AllowenceRate { get; internal set; }
}

public class ReportExpenseBillSummaryMpoTmRsmDepotBEO
    {
        public string SL { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string MarketName { get; set; }
        public string TerritoryName { get; set; }        
        public string DA { get; set; }
        public string TA { get; set; }
        public string WaitingTotal { get; set; }
        public string ApproveTotal { get; set; }
        public string MiscManualTADABill { get; set; }
        public string GrandTotal { get; set; }

     
       
    }

   
}
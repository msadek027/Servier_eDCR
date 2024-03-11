using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ExpenseBillMpoTmRsmBEO
    {
        public string SL { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string LocCode { get; set; }
        public string MarketName { get; set; }
        public string TerritoryName { get; set; }
        public string DA { get; set; }
        public string TA { get; set; }
        public string WaitingTotal { get;  set; }
        public string ApproveTotal { get;  set; }
        public string TotalAmount { get; set; }
        public string MiscManualTADABill { get;  set; }
        public string GrandTotal { get;  set; }


        //For Depot Report
        public string DepotName { get; set; }
        public string RegionName { get; set; }
   
    }
    public class ExpenseBillMpoTmRsmDetail
    {
        public string SL { get; set; }      
        public string DayNumber { get; set; }
        public string MorningPlace { get; set; }
        public string EveningPlace { get; set; }
        public string AllowanceNature { get; set; }
        public string Distance { get; set; }
        public string DA { get; set; }
        public string TA { get; set; }
        public string TotalAmount { get; set; }
        public string UserRemarks { get; set; }
        public string IsHoliday { get; set; }
        public string ReviewStatus { get; set; }     
        public string ApproveStatus { get; set; }
        public string Recommend { get; set; }
        public string SupervisorID { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorDesignation { get; set; }
        


    }


    public class ExpenseBillMpoTmRsmInsertSummaryDetail
    {
        public virtual ICollection<ExpenseBillMpoTmRsmInsertSummaryDetailList> ItemList { get; set; }
    }
    public class ExpenseBillMpoTmRsmInsertSummaryDetailList
    {

        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string DayNumber { get; set; }
        public string LocCode { get; set; }
        public string EmployeeCode { get; set; }     
        public string Recommend { get; set; }
        public string ReviewStatus { get; set; }
        public string Designation { get; set; }
        public string MiscManualTADABill { get; set; }

    }
    public class ExpenseBillMpoTmRsmDaTa
    {
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string LocCode { get; set; }
        public string Designation { get; set; }
        public string Distance { get; set; }
        public string TA { get; set; }
        public string RegionCode { get; set; }
        public string MasterSL { get; set; }
        public string MAllowence { get; set; }
        public string EAllowence { get; set; }
        public string DA { get; set; }
        public string IsHoliday { get; set; }
        public string Remarks { get; set; }

    }

}
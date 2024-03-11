using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ExpenseBillFormBEL
    {
       
        public string MPGroup { get; set; }       
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string Date { get; set; }     


    }
  
    public class ImgModel
    {
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
     
    }


    public class ExpenseBillMpoTmRsmInsertBEO
    {
        public virtual ICollection<ExpenseBillMpoTmRsmInsertSummaryBEO> ItemList { get; set; }
    }

    public class ExpenseBillMpoTmRsmInsertSummaryBEO
    {
        
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string LocCode { get; set; }
        public string EmployeeCode { get; set; }
        public string Date { get; set; }

        public string Recommend { get; set; }
        public string ReviewStatus { get; set; }
        public string Designation { get; set; }
        public string MiscManualTADABill { get; set; }
       
}


    public class ExpenseBillMpoTmRsmTaBEO
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportFortnightMonitoringRemarksSupBEO
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }    
 
        public string SetDate { get; set; }
        public string InstName { get; set; }
        public string AllownceNature { get; set; }      

        public string ColleagueName { get; set; }

        public string MDouble { get; set; }

        public string EDouble { get; set; }

        public string MSingle { get; set; }

        public string ESingle { get; set; }

        public string ChemistCount { get; set; }

        public string ShiftName { get; set; }

        public string Review { get; set; }

        public string Remarks { get; set; }

        public string FortNightType { get; set; }
        public string Year { get; set; }
        public string MonthName { get; set; }
        public string MonthNumber { get; set; }
    }
    public class ReportFortnightMonitoring
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }        
        public string SetDate { get; set; }
        public string PaperAudit { get; set; }
        public string ProblemsIdentifed { get; set; }
        public string Remarks { get; set; }
       

    }
    public class ReportFortnightOverallPer
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }

        public string SetDate { get; set; }

        public string MarketShare { get; set; }
        public string SalesAchi { get; set; }
        public string Leadership { get; set; }
        public string Remarks { get; set; }


    }
    public class ReportFortnightHistory
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }
        public string Designation { get; set; }     
        public string SetDate { get; set; }

        public string ParaType { get; set; }

        public string Remarks { get; set; }


    }
    public class ReportFortnightSupervisorComments
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string SetDate { get; set; }  
        public string Remarks { get; set; }
        public string ManagerRemarks { get; set; }


    }
}
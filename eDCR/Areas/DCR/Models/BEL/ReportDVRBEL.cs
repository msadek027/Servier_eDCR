using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDVRBEL
    {
        public string SL { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string ShiftName { get; set; }
        public string Potential { get; set; }
        public string Adoption { get;  set; }
        public string Frequency { get;  set; }
        public string MPOCode { get; set; }
        public string MPGroup { get; set; }
        public string MED { get; set; }       
        public string MEP { get; set; }
        public string MEE { get; set; }
        public string MEA { get; set; }

        public string MD { get; set; }

        public string md01 { get; set; }
        public string md02 { get; set; }
        public string md03 { get; set; }
        public string md04 { get; set; }
        public string md05 { get; set; }
        public string md06 { get; set; }
        public string md07 { get; set; }
        public string md08 { get; set; }
        public string md09 { get; set; }
        public string md10 { get; set; }
        public string md11 { get; set; }
        public string md12 { get; set; }
        public string md13 { get; set; }
        public string md14 { get; set; }
        public string md15 { get; set; }
        public string md16 { get; set; }
        public string md17 { get; set; }
        public string md18 { get; set; }
        public string md19 { get; set; }
        public string md20 { get; set; }
        public string md21 { get; set; }
        public string md22 { get; set; }
        public string md23 { get; set; }
        public string md24 { get; set; }
        public string md25 { get; set; }
        public string md26 { get; set; }
        public string md27 { get; set; }
        public string md28 { get; set; }
        public string md29 { get; set; }
        public string md30 { get; set; }
        public string md31 { get; set; }


        public string ED { get; set; }
        public string ed01 { get; set; }
        public string ed02 { get; set; }
        public string ed03 { get; set; }
        public string ed04 { get; set; }
        public string ed05 { get; set; }
        public string ed06 { get; set; }
        public string ed07 { get; set; }
        public string ed08 { get; set; }
        public string ed09 { get; set; }
        public string ed10 { get; set; }
        public string ed11 { get; set; }
        public string ed12 { get; set; }
        public string ed13 { get; set; }
        public string ed14 { get; set; }
        public string ed15 { get; set; }
        public string ed16 { get; set; }
        public string ed17 { get; set; }
        public string ed18 { get; set; }
        public string ed19 { get; set; }
        public string ed20 { get; set; }
        public string ed21 { get; set; }
        public string ed22 { get; set; }
        public string ed23 { get; set; }
        public string ed24 { get; set; }
        public string ed25 { get; set; }
        public string ed26 { get; set; }
        public string ed27 { get; set; }
        public string ed28 { get; set; }
        public string ed29 { get; set; }
        public string ed30 { get; set; }
        public string ed31 { get; set; }
     
    }

    public class WorkPlan
    {



        public string SetDate { get; set; }
        public string MPGroup { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }
        public string Quantity { get; set; }
      

        public string DoctorID { get; set; }

        public string DoctorName { get; set; }

        public string ShiftName { get; set; }

        public string Type { get; set; }
    }
    public class ArchiveReportDVRBEL
    {
        public string SL { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string ShiftName { get; set; }
      
        public string MarketName { get; set; }
        public string MPGroup { get; set; }
        public string MED { get; set; }
        public string MEP { get; set; }
        public string MEE { get; set; }
        public string MEA { get; set; }
        public string MD { get; set; }     
        public string ED { get; set; }
      
    }
    public class ReportDvrWpDcrBEL
    {
        public string SL { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string ShiftName { get; set; }
   
       
        public string MD { get; set; }
        public string ED { get; set; }
        public string MED { get; set; }
        public string MP { get; set; }
        public string EP { get; set; }
        public string MEP { get; set; }
        public string ME { get; set; }
        public string EE { get; set; }
        public string MEE { get; set; }
        public string MA { get; set; }
        public string EA { get; set; }     
        public string MEA { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
    }
    }
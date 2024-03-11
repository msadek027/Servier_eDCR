using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class ReportDoctorCoverageBEL
    {
        public string SL { get; set; }
        public string MarketName { get; set; }
        public string MPOID { get; set; }
        public string MPOName { get; set; }
        public string TotalNoOfDoctor { get; set; }
        public string TotalNoOfDOTPlanDoctor { get; set; }
        public string DoctorPlannedPer { get; set; }
        public string TotalNoOfVisitedDoctor { get; set; }
        public string DoctorCoveragePer { get; set; }

        public string TotalNoOfDOTPlan { get; set; }
        public string TotalExecutionOfDOTPlan { get; set; }
        public string TotalExecutionOfDOTDoctorWithoutDOTPlan { get; set; }
        public string PlanVsExecutionPer { get; set; }


        public string TotalExecutionOfNonDOTDoctor { get; set; }
        public string TotalNoOfNonDOTDoctorVisited { get; set; }



        public string TotalExecutionOfNewDoctor { get; set; }
        public string TotalNoOfNewDoctorVisited { get; set; }       
        public string TotalExecutionOfInternDoctor { get; set; }
     
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class WorkPlanBEL
    {
        public string MasterSL { get; set; }
        public string MPOCode { get; set; }
        public string MPOName { get; set; }
        public string SetDate { get; set; }
        public string MPGroup { get; set; }
        public virtual ICollection<WorkPlanDetail> WorkPlanDetailList { get; set; }
        public virtual ICollection<WorkPlanSubDetail> WorkPlanSubDetailList { get; set; }
        
    }

    public class WorkPlanDetail
    {
        public string MasterSL { get; set; }
        public string DetailSL { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }

        public string InstCode { get; set; }

        public string ShiftName { get; set; }

        public string AnSL { get; set; }

        public string InstName { get; set; }
    }
    public class WorkPlanSubDetail
    {
        public string DetailSL { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Quantity { get; set; }

        public string InstCode { get; set; }

        public string ShiftName { get; set; }

        public string AnSL { get; set; }
    }
}
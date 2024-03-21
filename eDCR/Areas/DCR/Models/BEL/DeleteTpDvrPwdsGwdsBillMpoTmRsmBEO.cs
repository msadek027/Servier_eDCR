using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO
    {
        public string MonthNumber { get; set; }

        public string LocCode { get; set; }

        public string Year { get; set; }

        public string OperationMode { get; set; }
        public string DayNumber { get; set; }

        public string Status { get; set; }
    }
    public class ScheduleJobListBEO
    {
        public string SL { get; set; }
        public string JobName { get; set; }

        public string LogDate { get; set; }

        public string Status { get; set; }
    }
        public class ViewTpBEO
    {
        public string SL { get; set; }
        public string DayNumber { get; set; }
        public string MInstName { get; set; }
        public string MMeetingPlace { get; set; }
        public string MSetTime { get; set; }
        public string MAllowence { get; set; }
        public string MReview { get; set; }
        public string EInstName { get; set; }
        public string EMeetingPlace { get; set; }
        public string ESetTime { get; set; }
        public string EAllowence { get; set; }
        public string EReview { get; set; }

        public string Status { get; set; }

    }
    public class ViewDvrBEO
    {
        public string SL { get; set; }
        public string DayNumber { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Shift { get; set; }
        public string Degree { get; set; }
        public string Specialization { get; set; }
        public string Institute { get; set; }
        public string MpoCode { get; set; }
        public string MpoName { get; set; }
        public string Status { get; set; }
        

    }
    public class ViewGwdsBEO
    {
        public string SL { get; set; }
        public string GiftCode { get; set; }
        public string GiftName { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string TotalQty { get; set; }
        public string Status { get; set; }

    }
    public class ViewPwdsBEO
    {
        public string SL { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public string TotalQty { get; set; }
        public string Status { get; set; }


    }
}
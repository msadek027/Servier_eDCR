using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.BEL
{
    public class TourPlanBEO
    {
    }
    public class TourPlanningMaster
    {
        public string MPOCode { get; set; }
        public string MasterSL { get; set; }
        public string MonthNumber { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }
        public string MasterStatus { get; set; }
        public string MPOName { get; set; }
        public string MPGroup { get; set; }
        public string ViewReview { get; set; }
        public string VW { get; set; }
        public string DayNumber { get; set; }
        public string RMasterStatus { get; set; }
    }



    public class TourPlanningReport
    {
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
    }
    public class TourPlanCreationMpoModelBEO
    {
        public string MonthNumber { get; set; }
        public string Year { get; set; }
        public string MasterStatus { get; set; }
        public string MPGroup { get; set; }
        public virtual ICollection<TourPlanCreationMpoBEO> DetailList { get; set; }
    }
    public class TourPlanCreationMpoBEO
    {
        public string DayNumber { get; set; }

        public string mShiftType { get; set; }
        public string mLocation { get; set; }
        public string mLocationMultiple { get; set; }
        
        public string mAddress { get; set; }
        public string mReportTime { get; set; }
        public string mDailyAllowence { get; set; }    
        public string mReview { get; set; }

        public string eShiftType { get; set; }
        public string eLocation { get; set; }
        public string eLocationMultiple { get; set; }
        public string eAddress { get; set; }
        public string eReportTime { get; set; }
        public string eDailyAllowence { get; set; }
        public string eReview { get; set; }

    }
    public class MorningEveningLocation
    {
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
    }
    public class TPShiftWiseLocation
    {
        public string MShiftType { get; set; }
        public string EShiftType { get; set; }
        public string MInstCode { get; set; }
        public string MInstName { get; set; }
        public string EInstCode { get; set; }
        public string EInstName { get; set; }
        public string YetAssigned { get; set; }
        public string MAllowence { get; set; }
        public string EAllowence { get; set; }

    }
    public class TourPlanSupMaster
    {
        public string MasterSL { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string LocCode { get; set; }
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string MonthName { get; set; }
        public string DayNumber { get; set; }
        public string MasterStatus { get; set; }
        public string ViewReview { get; set; }
        public string VW { get; set; }
        public string Designation { get; set; }

    }

    public class TourPlanSupReport
    {
        public string Year { get; set; }
        public string MonthNumber { get; set; }
        public string DayNumber { get; set; }
        public string MInstName { get; set; }
        public string Allowence { get; set; }
        public string MMeetingPlace { get; set; }
        public string MSetTime { get; set; }
        public string EInstName { get; set; }
        public string EMeetingPlace { get; set; }
        public string ESetTime { get; set; }
        public string EInstCode { get; set; }
        public string MInstCode { get; set; }
        public string MAllowence { get; set; }
        public string EAllowence { get; set; }
        public string YetAssigned { get; set; }
        public string LocCode { get; set; }
        public string EmployeeCode { get; set; }
        public string MAccompany { get; set; }
        public string EAccompany { get; set; }
        public string MReview { get; set; }
        public string EReview { get; set; }
    }
}
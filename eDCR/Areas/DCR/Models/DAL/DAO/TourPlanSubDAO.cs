using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.DAO;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class TourPlanSubDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();
        public List<TourPlanSupMaster> GetPopupView(DefaultParameterBEO model)
        {
            string Qry = @"SELECT distinct  MST_SL,LOC_CODE,EMPID,EMPNAME,DESIGNATION,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_SUP_TOUR  Where VW='View'
                         AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";
            string WhereClause = defaultDAO.GetImmediateSubordinate_WhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }

            if (EmpTypeSession == "Manager" || EmpTypeSession == "Sr. Manager")
            {
                string Qry2 = @"MINUS SELECT distinct  MST_SL,LOC_CODE,EMPID,EMPNAME,DESIGNATION,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_SUP_TOUR  Where VW='View'
                         AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' AND DESIGNATION = 'RSM' AND DIVISION_CODE IN(Select DIVISION_CODE from HR_DSM Where M_ID_MAPPING = '"+ LocCodeSession + "' AND DSM_ID IS NOT NULL)";
                Qry = Qry + Qry2;
            }
           


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupMaster> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupMaster
                    {
                        MasterSL = row["MST_SL"].ToString(),
                        EmployeeCode = row["EMPID"].ToString(),
                        EmployeeName = row["EMPNAME"].ToString(),
                        Designation = row["Designation"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        MasterStatus = row["MST_STATUS"].ToString(),
                        VW = row["VW"].ToString()

                    }).ToList();
            return item;
        }

       

        public List<TourPlanSupMaster> GetPopupReview(DefaultParameterBEO model)
        {
            string Qry = "SELECT distinct  MST_SL,LOC_CODE,EMPID,EMPNAME,DESIGNATION,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_SUP_TOUR Where Review='Yes' and MST_STATUS='Approved' ";
            Qry = Qry + " AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";
            string WhereClause = defaultDAO.GetImmediateSubordinate_WhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }
            if (EmpTypeSession == "Manager" || EmpTypeSession == "Sr. Manager")
            {
                string Qry2 = @" MINUS SELECT distinct  MST_SL,LOC_CODE,EMPID,EMPNAME,DESIGNATION,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_SUP_TOUR Where Review='Yes' and MST_STATUS='Approved'
                         AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' AND DESIGNATION = 'RSM' AND DIVISION_CODE IN(Select DIVISION_CODE from HR_DSM Where M_ID_MAPPING = '"+ LocCodeSession + "' AND DSM_ID IS NOT NULL)";
                Qry = Qry + Qry2;
            }


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupMaster> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupMaster
                    {
                        MasterSL = row["MST_SL"].ToString(),
                        EmployeeCode = row["EMPID"].ToString(),
                        EmployeeName = row["EMPNAME"].ToString(),
                        Designation = row["Designation"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        MasterStatus = row["MST_STATUS"].ToString(),
                        VW = "ReView"
                    }).ToList();
            return item;
        }

        public List<TPShiftWiseLocation> DetailValueTmRsm(TourPlanSupMaster model)
        {
            string Qry = "Select distinct a.SHIFT_TYPE M_SHIFT_TYPE,a.ALLOWANCE_NATURE M_ALLOWANCE_NATURE,b.SHIFT_TYPE E_SHIFT_TYPE,b.ALLOWANCE_NATURE E_ALLOWANCE_NATURE" +
                " from" +
                " (Select Day_Number,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE from VW_SUP_TOUR Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and  LOC_CODE='" + model.LocCode + "' and SHIFT_NAME='m') a, " +
                " (Select Day_Number,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE from VW_SUP_TOUR Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and  LOC_CODE='" + model.LocCode + "' and SHIFT_NAME='e') b " +
                " Where a.Day_Number=b.Day_Number(+)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TPShiftWiseLocation> item;

            item = (from DataRow row in dt.Rows
                    select new TPShiftWiseLocation
                    {
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),

                        MShiftType = row["M_SHIFT_TYPE"].ToString(),
                        EShiftType = row["E_SHIFT_TYPE"].ToString(),

                    }).ToList();
            return item;
        }

        public List<TourPlanSupReport> GetViewData(TourPlanSupReport model)
        {
            string Qry = " SELECT LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
                        " FROM VW_SUP_TP_RPT Where LOC_CODE = '" + model.LocCode + "'";     

            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " And MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " And YEAR=" + model.Year + "";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupReport
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        Allowence = row["ALLOWANCE_NATURE"].ToString(),

                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),


                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }

        public List<TourPlanSupReport> GetReviewData(TourPlanSupReport model)
        {
            string Qry = "SELECT  LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
            "  FROM VW_SUP_TP_RPT Where YEAR||MONTH_NUMBER||DAY_NUMBER IN" +
            " (Select distinct YEAR||MONTH_NUMBER||DAY_NUMBER from SUP_TOUR_MST A JOIN SUP_TOUR_DTL B ON A.MST_SL= B.MST_SL Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'  and Review='Yes')";

            // " (Select distinct YEAR||MONTH_NUMBER||DAY_NUMBER from VW_SUP_TOUR  Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'  and Review='Yes') ";

            Qry = Qry + " and LOC_CODE = '" + model.LocCode + "'";
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " And MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " And YEAR=" + model.Year + "";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupReport
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        Allowence = row["ALLOWANCE_NATURE"].ToString(),

                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),

                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }
        public bool PushNotification(TourPlanSupMaster model)
        {

            if (pushNotification.IsConnectedToInternet())
            {
                string MonthName = DateTime.Now.ToString("MMMM");
                pushNotification.SaveToDatabase(model.LocCode, "TP", "TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.", model.Year, model.MonthNumber);
                pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.LocCode), "TP", "TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.");



            }
            return true;
        }
        public bool SaveUpdateSup(TourPlanSupMaster model)
        {
            bool isTrue = false;
            int month = ((Convert.ToInt16(model.Year) - Convert.ToInt16(DateTime.Now.Year)) * 12) + Convert.ToInt16(model.MonthNumber) - Convert.ToInt16(DateTime.Now.Month);

            //if (month >= 0)
            //{
                if (model.ViewReview == "View")
                {
                    if (SaveUpdateView(model))
                    {
                        isTrue = true;
                    }
                }
                if (model.ViewReview == "Review")
                {
                  if (SaveUpdateReview(model))
                  {
                 
                    isTrue = true;
                   }
                  }


            //string QryTourPlanPartialAllow = "Update Tour_Partial_Allow set Is_Partial_Allow='No' Where MP_GROUP='" + model.MPGroup + "' ";
            //dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTourPlanPartialAllow);
      //  }
            return isTrue;
        }

        private bool SaveUpdateView(TourPlanSupMaster model)
        {

            bool isTrue = false;
            string QryIsExistsMST = "Select MST_SL from SUP_TOUR_MST Where Loc_Code='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and MST_STATUS='Approved'";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);

            string QryTempDel = "Delete from  OP_SUP_TP_VW Where Loc_Code='" + model.LocCode + "'";
            string QryTempIns = "Insert Into OP_SUP_TP_VW (MST_SL, LOC_CODE, YEAR, MONTH_NUMBER,  MST_STATUS, DAY_NUMBER, SHIFT_NAME, SHIFT_TYPE, DTL_STATUS, REVIEW, DTL_SL,  INST_NAME, SUB_DTL_STATUS)" +
                " (Select MST_SL, LOC_CODE, YEAR, MONTH_NUMBER,  MST_STATUS, DAY_NUMBER, SHIFT_NAME, SHIFT_TYPE, DTL_STATUS, REVIEW, DTL_SL,  INST_NAME, SUB_DTL_STATUS from VW_Sup_TOUR Where Loc_Code='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";

            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempDel);
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempIns);

            //String QryTemp = "Select * from OP_SUP_TP_VW";
            //DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryTemp);

           // int row = dt.Rows.Count;
            if (tuple1.Item1)
            {
                IUMode = "U";
                isTrue = true;
            }
            else
            {
                string QryMaster = "Update  SUP_TOUR_MST Set MST_STATUS='Approved'  Where MST_SL in (Select distinct  MST_SL from OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
                string QryDetail = "Update SUP_TOUR_DTL Set DTL_STATUS ='Approved' Where SHIFT_NAME||DAY_NUMBER||MST_SL in  (Select distinct SHIFT_NAME||DAY_NUMBER||MST_SL from OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
                string QrySubDetail = "Update SUP_TOUR_SUB_DTL Set Sub_DTL_STATUS='Approved' Where DTL_SL in (Select distinct DTL_SL from OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
               
                
                
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryMaster))
                {
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail))
                    {
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail))
                        {
                            IUMode = "I";
                            isTrue = true;
                        }
                    }
                }

            }
         //if (pushNotification.IsConnectedToInternet())
            //{
            

            string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            //    pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.LocCode), "TP", "TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.");
            pushNotification.SaveToDatabase(model.LocCode, "TP", "TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.", model.Year, model.MonthNumber);


            //}
            return isTrue;
        }
        private bool SaveUpdateReview(TourPlanSupMaster model)
        {
            bool isTrue = false;
            string QryIsExistsMST = "Select MST_SL from SUP_TOUR_MST Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and MST_STATUS='Approved' ";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);

            string QryTempDel = "Delete from  OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "'";

            string QryTempIns = "Insert Into OP_SUP_TP_VW (MST_SL, LOC_CODE, YEAR, MONTH_NUMBER,  MST_STATUS, DAY_NUMBER, SHIFT_NAME, SHIFT_TYPE, DTL_STATUS, REVIEW, DTL_SL,  INST_NAME, SUB_DTL_STATUS)" +
             " (Select MST_SL, LOC_CODE, YEAR, MONTH_NUMBER,  MST_STATUS, DAY_NUMBER, SHIFT_NAME, SHIFT_TYPE, DTL_STATUS, REVIEW, DTL_SL,  INST_NAME, SUB_DTL_STATUS from VW_Sup_TOUR Where Loc_Code='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempDel);
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempIns);
            
            //DVRApprovedReview(model);

            string QryDetail = "Update SUP_TOUR_DTL Set DTL_STATUS ='Approved',Review='Approved' Where SHIFT_NAME||DAY_NUMBER||MST_SL in  (Select distinct SHIFT_NAME||DAY_NUMBER||MST_SL from OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and Review='Yes')";
            string QrySubDetail = "Update SUP_TOUR_SUB_DTL Set Sub_DTL_STATUS='Approved' Where DTL_SL in (Select distinct DTL_SL from OP_SUP_TP_VW Where LOC_CODE='" + model.LocCode + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and Review='Yes')";
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail);
             if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail))
                {
                    IUMode = "I";
                    isTrue = true;
                }



            //if (pushNotification.IsConnectedToInternet())
            //{
            string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            //    pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.LocCode), "Change TP", "Change TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.");

            pushNotification.SaveToDatabase(model.LocCode, "TP", "Change TP Approved," + MonthName, "You requested monthly TP for " + MonthName + " is Approved. Please sync it.", model.Year, model.MonthNumber);

            //}
            return isTrue;
        }

        public List<TourPlanSupReport> MorningInst(TourPlanSupMaster model)
        {
            string Qry = "Select a.INST_NAME,a.YetAssigned from " +
                " ((Select distinct INST_NAME,'true' YetAssigned from VW_SUP_TOUR  Where LOC_CODE='" + model.LocCode + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and   SHIFT_NAME='m')" +
               " UNION " +
               "(Select distinct INST_NAME,'false' YetAssigned from VW_SUP_DOC_INST_MPO_MAPPING  Where LOC_CODE='" + model.LocCode + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and SHIFT_NAME='Morning')) a" +
               " Order by  CASE WHEN a.YetAssigned = 'true' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupReport
                    {
                        MInstCode = row["INST_NAME"].ToString(),
                        MInstName = row["INST_NAME"].ToString(),
                        YetAssigned = row["YetAssigned"].ToString()

                    }).ToList();
            return item;
        }

        public List<TourPlanSupReport> EveningInst(TourPlanSupMaster model)
        {
            string Qry = "Select a.INST_NAME,a.YetAssigned from " +
                     " ((Select distinct INST_NAME,'true' YetAssigned from VW_SUP_TOUR  Where  LOC_CODE='" + model.LocCode + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and  SHIFT_NAME='e')" +
                    " UNION " +
                    "(Select distinct INST_NAME,'false' YetAssigned from VW_SUP_DOC_INST_MPO_MAPPING  Where LOC_CODE='" + model.LocCode + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and SHIFT_NAME='Evening')) a" +
                    " Order by  CASE WHEN a.YetAssigned = 'true' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanSupReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanSupReport
                    {
                        EInstCode = row["INST_NAME"].ToString(),
                        EInstName = row["INST_NAME"].ToString(),
                        YetAssigned = row["YetAssigned"].ToString()

                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportTourPlanSupBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";

            string Qry = "SELECT LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                   "  FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " and  MONTH_NUMBER='" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            if (model.LocCode != "" && model.LocCode != null)
            {
                if (model.LocName != null && model.LocName != "")
                {
                    string lastItem = model.LocName;                
                    vHeader = vHeader + ", Employee Name : " + lastItem;
                }
                Qry = Qry + " and LOC_CODE = '" + model.LocCode + "'";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanSupBEO
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmpCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),                    

                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
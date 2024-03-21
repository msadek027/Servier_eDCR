using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Common;
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
    public class DeleteTpDvrPwdsGwdsBillMpoTmRsmDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        DateFormat dateFormat = new DateFormat();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);


        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();

        public bool DeleteTpDvrPwdsGwdsBillMpoTmRsm(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            model.DayNumber = model.DayNumber == ""|| model.DayNumber == null ? "" : model.DayNumber.Trim();
            bool isTrue = false;
            string Qry = "SELECT FN_EDCR_USER_SUPPORT('" + model.LocCode + "','" + model.Year + "','" + model.MonthNumber + "','" + model.DayNumber + "','" + model.OperationMode + "') FROM DUAL";
            IUMode = dbHelper.GetValue(Qry);

            pushNotification.SaveToDatabase(model.LocCode, "Delete", model.OperationMode + " Deleted," + model.MonthNumber, model.OperationMode + " of " + model.MonthNumber + " Deleted.", model.Year, model.MonthNumber);

            isTrue = true;
            return isTrue;
        }
        public bool ApproveWaitingTpDvrPwdsGwdsBillMpoTmRsm(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {

            bool isTrue = false;
            string Qry = "SELECT FN_TP_DVR_MPO_TM_STATUS_CHANGE('" + model.LocCode + "','" + model.Year + "','" + model.MonthNumber + "','" + model.DayNumber + "','" + model.OperationMode + "','"+ model.Status+ "') FROM DUAL";
            IUMode = dbHelper.GetValue(Qry);

            pushNotification.SaveToDatabase(model.LocCode, "Update", model.OperationMode + " Updated," + model.MonthNumber, model.OperationMode + " of " + model.MonthNumber + " Updated.", model.Year, model.MonthNumber);

            isTrue = true;
            return isTrue;
        }
        public List<ViewTpBEO> GetMpoTP(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {

            string Qry = @"SELECT MPO_CODE,MP_GROUP,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW,MST_STATUS  
                    FROM VW_TP_RPT  Where  YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' AND MP_GROUP='" + model.LocCode + "'  Order by To_Number(DAY_NUMBER)";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ViewTpBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ViewTpBEO
                    {
                        SL = row["Col1"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                        Status = row["MST_STATUS"].ToString(),
                    }).ToList();
            return item;

        }



        public List<ViewDvrBEO> GetMpoDVR(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            string Qry = @" Select MST_SL, MP_GROUP, MONTH_NUMBER, MONTH_NAME, YEAR, MST_STATUS, DAY_NUMBER, SHIFT_NAME, SET_TIME, DTL_STATUS, REVIEW, DTL_SL, SUB_DTL_STATUS, ADDITION, DOCTOR_ID, DOCTOR_NAME, MPO_CODE, MPO_NAME, POTENTIAL, DEGREES, SPECIALIZATION, PHONE, EMAIL, INST_NAME, MARKET_CODE, MARKET_NAME, ADDRESS
                            from VW_DVR  Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and MP_GROUP='" + model.LocCode + "' Order by DAY_NUMBER";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ViewDvrBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ViewDvrBEO
                    {
                        SL = row["Col1"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),
                        Institute = row["INST_NAME"].ToString(),
                        Shift = row["SHIFT_NAME"].ToString(),
                        MpoCode = row["MPO_Code"].ToString(),
                        MpoName = row["MPO_Name"].ToString(),
                        Status = row["MST_STATUS"].ToString(),


                    }).ToList();

            return item;
        }
        public List<ExpenseBillMpoTmRsmDetail> GetMpoTmRsmBILL(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            string Qry = @" SELECT   EMP_CODE,EMP_NAME,LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS,REMARKS, DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT, DA_AMOUNT+TA_AMOUNT Total,SUP_NAME,SUP_DESIGNATION
                         FROM VW_EXPENSE_BILL_MPO_TM_RSM 
                         Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and LOC_CODE='" + model.LocCode + "' Order by DAY_NUMBER";



            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ExpenseBillMpoTmRsmDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ExpenseBillMpoTmRsmDetail
                    {
                        SL = row["Col1"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        MorningPlace = row["MWP"].ToString(),
                        EveningPlace = row["EWP"].ToString(),
                        AllowanceNature = row["ALLOWANCE_NATURE"].ToString(),
                        Distance = row["DISTANCE"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),
                        TotalAmount = row["Total"].ToString(),
                        IsHoliday = row["IS_HOLIDAY"].ToString(),
                        UserRemarks = row["REMARKS"].ToString(),
                        ReviewStatus = row["REVIEW_STATUS"].ToString(),
                        Recommend = row["RECOMMEND"].ToString(),
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",
                        SupervisorID = row["ID_BOSS1"].ToString(),
                        SupervisorName = row["SUP_NAME"].ToString(),
                        SupervisorDesignation = row["SUP_DESIGNATION"].ToString()

                    }).ToList();

            return item;

        }

        public List<ViewGwdsBEO> GetMpoGWDS(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            string Qry = @" SELECT distinct MST_SL,PRODUCT_CODE,Gift_Name ||' ('||GENERIC_NAME||')' Gift_Name,REST_QTY+CENTRAL_QTY Total_Qty,DOCTOR_ID,DOCTOR_NAME,SPECIALIZATION,MST_STATUS 
                 from VW_GWDS Where  Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and MP_Group = '" + model.LocCode + "' Order by MST_SL,PRODUCT_CODE";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ViewGwdsBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ViewGwdsBEO
                    {
                        SL = row["Col1"].ToString(),
                        GiftCode = row["PRODUCT_CODE"].ToString(),
                        GiftName = row["Gift_Name"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),
                        Status = row["MST_STATUS"].ToString(),

                    }).ToList();
            return item;
        }

        public List<ViewPwdsBEO> GetMpoPWDS(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            string Qry = @"SELECT distinct MST_SL,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,DOCTOR_ID,DOCTOR_NAME,SPECIALIZATION,REST_QTY+CENTRAL_QTY Total_Qty,MST_STATUS
                   from VW_PWDS Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and MP_Group = '" + model.LocCode + "' Order by MST_SL,PRODUCT_CODE";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ViewPwdsBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ViewPwdsBEO
                    {
                        SL = row["Col1"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        DoctorId = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),
                        Status = row["MST_STATUS"].ToString(),
                    }).ToList();
            return item;
        }
        public List<ViewTpBEO> GetTmRsmTP(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            string Qry = @" SELECT LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW,MST_STATUS  
                         FROM VW_SUP_TP_RPT 
                         Where  LOC_CODE = '" + model.LocCode + "' And YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' Order by To_Number(DAY_NUMBER)";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ViewTpBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ViewTpBEO
                    {
                        SL = row["Col1"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString(),
                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                        Status = row["MST_STATUS"].ToString(),
                    }).ToList();
            return item;          
          
        }
        public List<ScheduleJobListBEO> GetScheduleJobList(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {

            string Qry = @"SELECT A.job_name,B.log_date, B.status
                            FROM user_scheduler_jobs A,
                            (SELECT job_name, MAX(log_date) log_date, status
                            FROM user_scheduler_job_log Group By job_name,status) B Where A.job_name=B.job_name";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ScheduleJobListBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ScheduleJobListBEO
                    {
                        SL = row["Col1"].ToString(),
                        JobName = row["job_name"].ToString(),
                        LogDate = row["log_date"].ToString(),
                        Status = row["status"].ToString(),
                       
                    }).ToList();
            return item;

        }
        public bool JobTrigger(ScheduleJobListBEO model)
        {
            bool isTrue = false;          
            string Qry = "BEGIN  DBMS_SCHEDULER.RUN_JOB('JOB_DAILY_DCR_SHIFT_WISE');END;";
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                {
                    IUMode = "U";
                    isTrue = true;
                }         
            return isTrue;
        }
    }
}
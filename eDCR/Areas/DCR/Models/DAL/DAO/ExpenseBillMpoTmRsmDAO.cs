using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ExpenseBillMpoTmRsmDAO : ReturnData
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
        public List<DefaultParameterBEO> GetPopupView(DefaultParameterBEO model)
        {

            string Qry = " Select EMP_CODE, EMP_NAME, DESIGNATION, LOC_CODE, MARKET_NAME, REGION_CODE, MONTH_NUMBER, MONTH_NAME, YEAR," +
            " LISTAGG(STATUS_BOSS1, ', ') WITHIN GROUP(ORDER BY STATUS_BOSS1 DESC) STATUS_BOSS1  " +
            " from(" +
            " SELECT DISTINCT EMP_CODE, EMP_NAME, DESIGNATION, LOC_CODE, MARKET_NAME, REGION_CODE, MONTH_NUMBER, MONTH_NAME, YEAR, STATUS_BOSS1" +
            " from VW_EXPENSE_BILL_MPO_TM_RSM" +
            " Where YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' AND REGION_CODE='" + model.RegionCode + "' ";
            string WhereClause = defaultDAO.GetImmediateSubordinate_WhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }
            Qry = Qry + ")";
            Qry = Qry + " GROUP BY EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,REGION_CODE,MONTH_NUMBER,MONTH_NAME,YEAR";



            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultParameterBEO> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultParameterBEO
                    {

                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        RegionCode = row["REGION_CODE"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        BillStatus = row["STATUS_BOSS1"].ToString().Length > 8 ? "Waiting" : row["STATUS_BOSS1"].ToString(),

                    }).ToList();
            return item;
        }


        public List<ExpenseBillMpoTmRsmBEO> GetSummaryExpenseBillMpoTmRsm(DefaultParameterBEO model)
        {
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME," +
                        " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                        " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL," +
                        " NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                        " from VW_EXPENSE_BILL_MPO_TM_RSM " +
                        " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }

            if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
            {
                Qry = Qry + " and LOC_CODE='" + model.LocCode + "' ";
                if (model.Designation == "MPO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MPO','SMPO')";
                }
                else
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                }
            }
            else
            {
                if (model.Designation == "MPO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MPO','SMPO')  ";
                    if(model.RegionCode!=null && model.RegionCode!="")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
               else if (model.Designation == "DSM")
                {
                    Qry = Qry + " and DESIGNATION ='DSM' ";                   
                }
               else
                {
                    Qry = Qry + " and DESIGNATION='"+ model.Designation + "'";
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
                /*
                if (HttpContext.Current.Session["Designation"].ToString() == "TM")
                {
                    Qry = Qry + " and TERRITORY_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }

                if (model.Designation == "MPO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MPO','SMPO') and LOC_CODE IN (SELECT MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "' )";

                }
                if (model.Designation == "TM")
                {
                    Qry = Qry + " AND DESIGNATION ='" + model.Designation + "' and TERRITORY_CODE IN (SELECT TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                }
                if (model.Designation == "RSM")
                {
                    Qry = Qry + " AND DESIGNATION ='" + model.Designation + "' and REGION_CODE='" + model.RegionCode + "'";
                }
                if ((model.Designation == "" || model.Designation == "null" || model.Designation == null) && model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
                }
                */
            }

            Qry = Qry + " Group BY EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME ";
            Qry = Qry + " ORDER BY MARKET_NAME";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ExpenseBillMpoTmRsmBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ExpenseBillMpoTmRsmBEO
                    {
                        SL = row["Col1"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),
                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),
                        TotalAmount = row["Total"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),

                    }).ToList();
            return item;
        }

        public List<ExpenseBillMpoTmRsmDetail> GetDetailExpenseBillMpoTmRsm(DefaultParameterBEO model)
        {

            string EmpID = HttpContext.Current.Session["EmpID"].ToString();
            string Qry = " SELECT   EMP_CODE,EMP_NAME,LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,REMARKS,IS_HOLIDAY,REVIEW_STATUS, DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT, DA_AMOUNT+TA_AMOUNT Total  " +
                         " FROM VW_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";
            if (model.EmployeeCode != "" && model.EmployeeCode != null)
            {
                Qry = Qry + " and EMP_CODE='" + model.EmployeeCode + "'";
            }
           
            Qry = Qry + " Order by DAY_NUMBER";
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
                        ApproveStatus = row["STATUS_BOSS1"].ToString().ToString() == "Approved" ? "A" : "W",
                      SupervisorID = row["ID_BOSS1"].ToString(),

                    }).ToList();
            return item;
        }



        public bool SaveUpdateSummary(ExpenseBillMpoTmRsmInsertSummaryDetail model)
        {
            string EmpID = HttpContext.Current.Session["EmpID"].ToString();
            //string LocCode = HttpContext.Current.Session["LocCode"].ToString();
            bool isTrue = false;

            foreach (ExpenseBillMpoTmRsmInsertSummaryDetailList detailModel in model.ItemList)
            {
                    if (detailModel.Designation == "MPO" || detailModel.Designation == "SMPO")
                    {
                        string QryIsExists = "Select MST_SL From ACC_DA_BILL Where  Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.LocCode + "' ";
                        var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                    if (tuple1.Item1)
                    {
                        string Qry = "Update ACC_DA_BILL set TSM_STATUS='Approved',TSM_ID='" + EmpID + "' Where  Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.LocCode + "' ";
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        IUMode = "U";
                        isTrue = true;
                    }
                    }
                if (detailModel.Designation == "TM" || detailModel.Designation == "RSM" || detailModel.Designation == "DSM" || detailModel.Designation == "Manager" || detailModel.Designation == "Sr. Manager")
                {
                    string QryIsExists = "Select MST_SL From SUP_ACC_DA_BILL Where  Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and LOC_CODE='" + detailModel.LocCode + "' ";
                    var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                    if (tuple1.Item1)
                    {
                        string Qry = "Update SUP_ACC_DA_BILL set SUP_EMP_STATUS='Approved',SUP_EMP_ID='" + EmpID + "' Where   Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and LOC_CODE='" + detailModel.LocCode + "' ";
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        IUMode = "U";
                        isTrue = true;
                    }
                }
                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(detailModel.MonthNumber));
                pushNotification.SaveToDatabase(detailModel.LocCode, "BILL", "BILL Approved," + MonthName, "Bill Summary Approved of " + MonthName + " Approved.", detailModel.Year, detailModel.MonthNumber);

                }

            

            return isTrue;
        }
        public bool SaveUpdateDetail(ExpenseBillMpoTmRsmInsertSummaryDetail model)
        {
           string EmpID = HttpContext.Current.Session["EmpID"].ToString();
           // string LocCode = HttpContext.Current.Session["LocCode"].ToString();
                
            bool isTrue = false;

            foreach (ExpenseBillMpoTmRsmInsertSummaryDetailList detailModel in model.ItemList)
            {
                 if (detailModel.Designation == "MPO" || detailModel.Designation == "SMPO")
                {
                    string QryIsExists = "Select MST_SL From ACC_DA_BILL Where Day_Number='" + detailModel.DayNumber + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.LocCode + "' ";
                    var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                    if (tuple1.Item1)
                    {
                        string Qry   = "Update ACC_DA_BILL Set TSM_STATUS='Approved',Recommend='" + detailModel.Recommend + "', TSM_ID='" + EmpID + "', REVIEW_STATUS='" + detailModel.ReviewStatus + "' Where Day_Number='" + detailModel.DayNumber + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.LocCode + "' ";

                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        IUMode = "U";
                        isTrue = true;
                    }
                    }
                    if (detailModel.Designation == "TM" || detailModel.Designation == "RSM" || detailModel.Designation == "DSM" || detailModel.Designation == "Manager" || detailModel.Designation == "Sr. Manager")
                   {
                    string QryIsExists = "Select MST_SL From SUP_ACC_DA_BILL Where Day_Number='" + detailModel.DayNumber + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and LOC_CODE='" + detailModel.LocCode + "' ";
                    var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                    if (tuple1.Item1)
                    {
                        string Qry  = "Update SUP_ACC_DA_BILL Set SUP_EMP_STATUS='Approved',Recommend='" + detailModel.Recommend + "',SUP_EMP_ID='" + EmpID + "', REVIEW_STATUS='" + detailModel.ReviewStatus + "' Where Day_Number='" + detailModel.DayNumber + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and LOC_CODE='" + detailModel.LocCode + "' ";

                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        IUMode = "U";
                        isTrue = true;
                    }
                    }

                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(detailModel.MonthNumber));
                pushNotification.SaveToDatabase(detailModel.LocCode, "BILL", "BILL Approved," + MonthName, "Bill Single Approved of " + detailModel.DayNumber + "-" + detailModel.MonthNumber + "-" + detailModel.Year + " Approved.", detailModel.Year, detailModel.MonthNumber);

                }
            

            return isTrue;
        }

        public bool SaveUpdateDaTa(ExpenseBillMpoTmRsmDaTa model)
        {
            string EmpID = HttpContext.Current.Session["EmpID"].ToString();
            //string LocCode = HttpContext.Current.Session["LocCode"].ToString();
            bool isTrue = false;
            IUMode = "";
            if (model.Designation == "MPO" || model.Designation == "SMPO")
            {
                string NDA = model.MAllowence + "," + model.EAllowence;
                string QryDA = "Update ACC_DA_BILL set REVIEW_STATUS='No', IS_HOLIDAY='" + model.IsHoliday + "' , ALLOWANCE_NATURE='" + NDA + "', AMOUNT=" + model.DA + ", Recommend='" + model.Remarks + "',TSM_ID='" + EmpID + "' Where MST_SL='" + model.MasterSL + "'  ";

                model.TA = Math.Ceiling(Convert.ToDecimal(model.TA)).ToString();
                string Qry = "Update ACC_TA_BILL set REGION_TYPE='" + model.RegionCode + "', DISTANCE=" + model.Distance + ",AMOUNT=" + model.TA + " Where MST_SL='" + model.MasterSL + "'  ";

                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDA);
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                IUMode = "U";
                isTrue = true;
            }
            if (model.Designation == "TM" || model.Designation == "RSM" || model.Designation == "DSM" ||  model.Designation == "Manager" || model.Designation == "Sr. Manager")
            {
                string NDA = model.MAllowence + "," + model.EAllowence;
                string QryDA = "Update SUP_ACC_DA_BILL set REVIEW_STATUS='No', IS_HOLIDAY='" + model.IsHoliday + "' , ALLOWANCE_NATURE='" + NDA + "', AMOUNT=" + model.DA + ", Recommend= '" + model.Remarks + "' ,SUP_EMP_ID='" + EmpID + "' Where MST_SL='" + model.MasterSL + "'  ";
                model.TA = Math.Ceiling(Convert.ToDecimal(model.TA)).ToString();
                string Qry = "Update SUP_ACC_TA_BILL set REGION_TYPE='" + model.RegionCode + "', DISTANCE=" + model.Distance + ",AMOUNT=" + model.TA + " Where MST_SL='" + model.MasterSL + "'  ";

                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDA);
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                IUMode = "U";
                isTrue = true;
            }
            string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            pushNotification.SaveToDatabase(model.LocCode, "BILL", "BILL Changed," + MonthName, "Bill Single Changed  of " + MonthName + " Changed.", model.Year, model.MonthNumber);

            return isTrue;
        }

    }
}
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
    public class DefaultExpenseBillMpoTmRsmDAO : ReturnData
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
        public List<ExpenseBillMpoTmRsmDetail> GetViewExpenseBillDepotDetailMpoTmRsm(DefaultParameterBEO model)
        {
            string EmpID = HttpContext.Current.Session["EmpID"].ToString();        
            string Qry = " SELECT   EMP_CODE,EMP_NAME,LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS, REMARKS,DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT, DA_AMOUNT+TA_AMOUNT Total " +
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
                        UserRemarks = row["REMARKS"].ToString(),
                        IsHoliday = row["IS_HOLIDAY"].ToString(),
                        ReviewStatus = row["REVIEW_STATUS"].ToString(),
                        Recommend = row["RECOMMEND"].ToString(),
                        SupervisorID = row["ID_BOSS1"].ToString(),
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",

                    }).ToList();
            return item;
        }


      
        public List<ExpenseBillMpoTmRsmBEO> GetViewExpenseBillSummaryMpoTmRsm(DefaultParameterBEO model)
        {
            string Qry =" Select EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME," +
                        " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                        " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL," +
                        " NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal"+
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
                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')";
                }
                else
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                }
            }
            else
            {
                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')  ";
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
                else if (model.Designation == "PM")
                {
                    Qry = Qry + " and DESIGNATION ='PM' ";
                }
                else
                {
                    if  (model.Designation != "" && model.Designation != null)
                    {
                        Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    }                   
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
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
                       // LocCode = row["LOC_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),

                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),

                    }).ToList();
            return item;
        }


        public Tuple<string, DataTable, List<ReportExpenseBillSummaryMpoTmRsmDepotBEO>> GetExportExpenseBillSummaryMpoTmRsm(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME," +
                         " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                         " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL," +
                         " NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                         " from VW_EXPENSE_BILL_MPO_TM_RSM " +
                         " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }

            if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
            {
                if (model.LocName != null && model.LocName != "")
                {
                    string lastItem = model.LocName;
                    vHeader = vHeader + ", Market: " + lastItem;
                }
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry = Qry + " and LOC_CODE='" + model.LocCode + "' ";
                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')";
                }
                else
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                }
            }
            else
            {
                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')  ";
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
                else if (model.Designation == "PM")
                {
                    Qry = Qry + " and DESIGNATION ='PM' ";
                }
                else
                {
                    if (model.Designation != "" && model.Designation != null)
                    {
                        Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    }
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                vHeader = vHeader + ", Designation : " + model.Designation;
            }


            Qry = Qry + " Group by EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME ";
            Qry = Qry + " ORDER BY MARKET_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportExpenseBillSummaryMpoTmRsmDepotBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportExpenseBillSummaryMpoTmRsmDepotBEO
                    {
                        SL = row["Col1"].ToString(),

                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),

                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),                  
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),

                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
        }
        public List<ExpenseBillMpoTmRsmDetail> GetViewExpenseBillDetailMpoTmRsm(DefaultParameterBEO model)
        {

            string EmpID = HttpContext.Current.Session["EmpID"].ToString();
            string Qry = " SELECT   EMP_CODE,EMP_NAME,LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS,REMARKS, DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT, DA_AMOUNT+TA_AMOUNT Total  " +
                         ", SUP_NAME,SUP_DESIGNATION " +
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
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",
                        SupervisorID = row["ID_BOSS1"].ToString(),
                        SupervisorName = row["SUP_NAME"].ToString(),
                        SupervisorDesignation = row["SUP_DESIGNATION"].ToString()

                    }).ToList();







            return item;
        }

        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmDetail>> GetExportExpenseBillDetailMpoTmRsm(DefaultParameterBEO model)
        {
            string Qry = " SELECT  LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER,REMARKS, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS, DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT,  DA_AMOUNT+TA_AMOUNT Total  " +
                            ", SUP_NAME,SUP_DESIGNATION " +
                            " FROM VW_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            string vHeader = "Month: " + month + " " + model.Year;
            vHeader = vHeader + ", Employee: " + model.EmployeeCode + ", " + model.EmployeeName + ", " + model.Designation + ", " + model.LocName;

            if (model.EmployeeCode != "" && model.EmployeeCode != null)
            {
                Qry = Qry + " and EMP_CODE='" + model.EmployeeCode + "'";
            }
            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                Qry = Qry + " and STATUS_BOSS1='" + model.MasterStatus + "'";
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
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",
                        SupervisorID = row["ID_BOSS1"].ToString(),
                        SupervisorName = row["SUP_NAME"].ToString(),
                        SupervisorDesignation = row["SUP_DESIGNATION"].ToString()
                    

                    }).ToList();

            return Tuple.Create(vHeader, dt, item);
        }




        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmDetail>> GetExportExpenseBillDepotDetailMpoTmRsm(DefaultParameterBEO model)
        {

   

            string Qry = " SELECT  LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS, REMARKS,DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT,DA_AMOUNT+TA_AMOUNT Total  " +
                            " FROM VW_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            string vHeader = "Month: " + month + " " + model.Year;
            if (model.LocCode != "" && model.LocCode != null)
            {
                // Qry = Qry + "and LOC_CODE='" + model.LocCode + "'";               
            }
            if (model.EmployeeCode != "" && model.EmployeeCode != null)
            {
                Qry = Qry + " and EMP_CODE='" + model.EmployeeCode + "'";
                vHeader = vHeader + ", Employee: " + model.EmployeeCode + ", " + model.EmployeeName + ", " + model.Designation + ", " + model.LocName;

            }         

            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                Qry = Qry + " and STATUS_BOSS1='" + model.MasterStatus + "'";
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
                        SupervisorID = row["ID_BOSS1"].ToString(),                   
                        
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",

                    }).ToList();

            return Tuple.Create(vHeader, dt, item);
        }





        public bool SaveUpdateSummary(ExpenseBillMpoTmRsmInsertBEO model)
        {

            bool isTrue = false;
            int i = 0;
            foreach (ExpenseBillMpoTmRsmInsertSummaryBEO detailModel in model.ItemList)
            {
                 i = i+1;
                string Designation = "Select DESIGNATION from VW_ARC_HR_LOC_MAPPING_ALL Where MPO_CODE='" + detailModel.EmployeeCode.Trim() + "' AND Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " ";
                detailModel.Designation= dbHelper.GetValue(Designation);
               
                if (detailModel.EmployeeCode!=null && detailModel.EmployeeCode!="")
                { 
                 detailModel.MiscManualTADABill = (detailModel.MiscManualTADABill == null || detailModel.MiscManualTADABill == "") ? "0" : detailModel.MiscManualTADABill;

                string Qry = "";
                string QryIsExists = "";
             

                    if (detailModel.Designation.Trim() == "MIO" || detailModel.Designation.Trim() == "SMIO")
                   {
                    QryIsExists = "Select NVL(MAX(MST_SL),0)  From ACC_DA_BILL Where MPO_CODE='" + detailModel.EmployeeCode.Trim() + "' AND Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + "  ";
                     
                    }
                    if (detailModel.Designation.Trim() == "RM" || detailModel.Designation.Trim() == "ZM" || detailModel.Designation.Trim() == "PM" || detailModel.Designation.Trim() == "CM" || detailModel.Designation.Trim() == "Sr. Manager")
                   {
                    QryIsExists = "Select NVL(MAX(MST_SL),0) MST_SL From SUP_ACC_DA_BILL Where EMP_CODE='" + detailModel.EmployeeCode.Trim() + "' AND Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + "  ";


                    }
                    if (QryIsExists != null && QryIsExists != "")
                    {
                        //Add New 
                        string QryAll = "Select MST_SL,TYPE from " +
                                        "(Select NVL(MAX(MST_SL), 0) MST_SL, 'MPO' TYPE  From ACC_DA_BILL Where MPO_CODE='" + detailModel.EmployeeCode.Trim() + "' AND Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " " +
                                        "UNION ALL " +
                                        "Select NVL(MAX(MST_SL), 0) MST_SL, 'TM' TYPE  From SUP_ACC_DA_BILL Where EMP_CODE='" + detailModel.EmployeeCode.Trim() + "' AND Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " " +
                                       " ) " +
                                       " Where MST_SL> 0";

                        var tuple1 = dbHelper.IsExistsWithGetSLnID(dbConn.SAConnStrReader(), QryAll);
                        detailModel.Designation = tuple1.Item3;
                        //----End New

                        // var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);

                        if (tuple1.Item1 && Convert.ToInt64(tuple1.Item2) > 0)
                        {
                            if (detailModel.Designation.Trim() == "MIO" || detailModel.Designation.Trim() == "SMIO")
                            {
                                Qry = "Update ACC_DA_BILL set MISCELLANEOUS=" + detailModel.MiscManualTADABill + " Where  MST_SL=" + tuple1.Item2 + " ";

                                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                                IUMode = "U";
                                isTrue = true;
                            }
                            if (detailModel.Designation.Trim() == "RM" || detailModel.Designation.Trim() == "ZM" || detailModel.Designation.Trim() == "DSM" || detailModel.Designation.Trim() == "CM" || detailModel.Designation.Trim() == "Sr. Manager")
                            {
                                Qry = "Update SUP_ACC_DA_BILL set MISCELLANEOUS=" + detailModel.MiscManualTADABill + " Where  MST_SL=" + tuple1.Item2 + " ";

                                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                                IUMode = "U";
                                isTrue = true;
                            }

                            pushNotification.SaveToBillDatabase(detailModel.EmployeeCode, "BILL", "BILL Changed," + detailModel.MonthNumber, "Bill Summary Changed of " + detailModel.MonthNumber + " Approved.", detailModel.Year, detailModel.MonthNumber, detailModel.MiscManualTADABill);
                        }
                    }
                }

            }
        
            return isTrue;
        }
        public List<ExpenseBillMpoTmRsmBEO> GetViewExpenseBillSummaryMpoTmRsmDepotCurrentMpoWise2(DefaultParameterBEO model)
        {
            string Str = model.EmployeeCode.Replace("[", "").Replace("]", "").Replace("\"", "'");
            model.EmployeeCode = Str;

            string TableName = model.ChkEmployeeCode == "True" ? "VW_EXPENSE_BILL_MPO_TM_RSM" : "VW_EXPENSE_BILL_MPO_TM_RSM_EMP";
            TableName = model.ChkSpecificEmployeeCode == "True" ? "VW_EXPENSE_BILL_MPO_TM_RSM_EMP" : "VW_EXPENSE_BILL_MPO_TM_RSM";
            if (model.ChkEmployeeCode == "False" && model.ChkSpecificEmployeeCode == "False")
            {
                TableName = "VW_EXPENSE_BILL_MPO_TM_RSM_EMP";
            }

            string Qry =" Select EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME," +
                        " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                        " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL,NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                        " from " + TableName + " " +
                        " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            if (model.ChkSpecificEmployeeCode == "True")
            {
                Qry = Qry + " AND EMP_CODE='" + model.SpecificEmployeeCode + "'";
            }
            else
            {
                if (model.Designation == "MIO" && (model.DepotCode != "" && model.DepotCode != null))
                {

                    string dtCurrentMpoStr = "";
                    string QryCurrentMpo = "";
                    if (model.ChkEmployeeCode == "True")
                    {

                        QryCurrentMpo = "Select DISTINCT EMP_CODE from " + TableName + " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION IN ('MPO','SMPO') AND EMP_CODE NOT IN(Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)   ";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        if (model.EmployeeCode != "" && model.EmployeeCode != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and EMP_CODE IN (" + model.EmployeeCode + ")";
                        }


                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";

                    }
                    else
                    {
                        QryCurrentMpo = "Select MPO_CODE  FROM VW_HR_LOC_MAPPING_ALL Where DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION IN ('MIO','SMIO')";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";
                    }
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";


                }
                if ((model.Designation == "RM" || model.Designation == "ZM"|| model.Designation == "PM" || model.Designation == "CM" || model.Designation == "Sr. Manager") && (model.DepotCode != "" && model.DepotCode != null))
                {

                    string dtCurrentMpoStr = "";
                    string QryCurrentMpo = "";
                    if (model.ChkEmployeeCode == "True")
                    {

                        QryCurrentMpo = "Select DISTINCT EMP_CODE from " + TableName + " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION ='" + model.Designation + "' AND EMP_CODE NOT IN(Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        if (model.EmployeeCode != "" && model.EmployeeCode != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and EMP_CODE='" + model.EmployeeCode + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";
                    }
                    else
                    {
                        QryCurrentMpo = "Select MPO_CODE  FROM VW_HR_LOC_MAPPING_ALL Where DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION ='" + model.Designation + "'";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";


                    }
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";

                }

                if ((model.Designation == "" || model.Designation == "null" || model.Designation == null) && model.DepotCode != "" && model.DepotCode != null)
                {

                    string QryCurrentMpo = "Select DISTINCT EMP_CODE from " + TableName + " Where  YEAR = " + model.Year + " AND MONTH_NUMBER = '" + model.MonthNumber + "' AND DEPOT_CODE = '" + model.DepotCode + "' ";

                    if (model.ChkEmployeeCode == "True")
                    {
                        QryCurrentMpo = QryCurrentMpo + " AND  EMP_CODE NOT IN (Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)";
                        if (model.EmployeeCode != "" && model.EmployeeCode != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and EMP_CODE IN (" + model.EmployeeCode + ")";
                        }
                    }
                    else
                    {
                        QryCurrentMpo = QryCurrentMpo + " AND  EMP_CODE  IN (Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)";
                    }

                    DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                    string dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                    dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";

                }
            }
            Qry = Qry + " Group by EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME ";
            Qry = Qry + " ORDER BY MARKET_NAME";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ExpenseBillMpoTmRsmBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ExpenseBillMpoTmRsmBEO
                    {
                        SL = row["Col1"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),                 
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),

                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),

                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),

                      
                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmBEO>> GetExportExpenseBillSummaryMpoTmRsmDepotCurrentMpoWise2(DefaultParameterBEO model)
        {
            string Str = "";
            if (model.EmployeeCode != null)
            {
                model.EmployeeCode.Replace("[", "").Replace("]", "").Replace("\"", "'");
            }
            model.EmployeeCode = Str;

            model.ChkEmployeeCode = model.ChkEmployeeCode == null? "False" : model.ChkEmployeeCode;
            model.ChkSpecificEmployeeCode = model.ChkSpecificEmployeeCode == null ? "False" : model.ChkSpecificEmployeeCode;
         
            string TableName = model.ChkEmployeeCode == "True" ? "VW_EXPENSE_BILL_MPO_TM_RSM" : "VW_EXPENSE_BILL_MPO_TM_RSM_EMP";
            TableName = model.ChkSpecificEmployeeCode == "True" ? "VW_EXPENSE_BILL_MPO_TM_RSM_EMP" : "VW_EXPENSE_BILL_MPO_TM_RSM";
            string vHeader = "";
           
            if (model.ChkEmployeeCode == "False" && model.ChkSpecificEmployeeCode == "False")
            {
                TableName = "VW_EXPENSE_BILL_MPO_TM_RSM_EMP";
            }
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME," +
                         " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                         " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL,NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                         " from " + TableName + " " +
                         " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";



            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + " " + model.Year;

            if (model.ChkSpecificEmployeeCode == "True")
            {
                Qry = Qry + " AND EMP_CODE='" + model.SpecificEmployeeCode + "'";
            }
            else
            {
                if (model.Designation == "MIO" && (model.DepotCode != "" && model.DepotCode != null))
                {

                    string dtCurrentMpoStr = "";
                    string QryCurrentMpo = "";
                    if (model.ChkEmployeeCode == "True")
                    {

                        QryCurrentMpo = "Select DISTINCT EMP_CODE from  " + TableName + "  Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION IN ('MPO','SMPO') AND EMP_CODE NOT IN(Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)   ";


                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";
                    }
                    else
                    {
                        QryCurrentMpo = "Select MPO_CODE  FROM VW_HR_LOC_MAPPING_ALL Where DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION IN ('MPO','SMPO')";

                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";

                    }
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";


                }
                if ((model.Designation == "RM" || model.Designation == "ZM" || model.Designation == "PM" || model.Designation == "CM" || model.Designation == "Sr. Manager") && (model.DepotCode != "" && model.DepotCode != null))
                {

                    string dtCurrentMpoStr = "";
                    string QryCurrentMpo = "";
                    if (model.ChkEmployeeCode == "True")
                    {

                        QryCurrentMpo = "Select DISTINCT EMP_CODE from  " + TableName + "  Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION ='" + model.Designation + "' AND EMP_CODE NOT IN(Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)   ";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";

                    }
                    else
                    {
                        QryCurrentMpo = "Select MPO_CODE  FROM VW_HR_LOC_MAPPING_ALL Where DEPOT_CODE='" + model.DepotCode + "' and DESIGNATION ='" + model.Designation + "'";
                        if (model.SBU != "" && model.SBU != null)
                        {
                            QryCurrentMpo = QryCurrentMpo + " and PRODUCT_GROUP='" + model.SBU + "'";
                        }
                        DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                        dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                        dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";

                    }
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";

                }

                //string DepotQry = "Select Distinct DEPOT_CODE from VW_EXPENSE_BILL_MPO_TM_RSM Where  YEAR = " + model.Year + " AND MONTH_NUMBER = '" + model.MonthNumber + "'";
                //DataTable dtDepot = dbHelper.GetDataTable(dbConn.SAConnStrReader(), DepotQry);
                //DataTable dtAll = new DataTable();
                //for (int i = 0; i < dtDepot.Rows.Count; i++)
                //{
                //    Qry = Qry2 + " AND DESIGNATION IN ('MPO','SMPO')";
                //    model.DepotCode = dtDepot.Rows[i][0].ToString();


                if ((model.Designation == "" || model.Designation == "null" || model.Designation == null) && model.DepotCode != "" && model.DepotCode != null)
                {

                    string QryCurrentMpo = "Select DISTINCT EMP_CODE from  " + TableName + "  Where  YEAR = " + model.Year + " AND MONTH_NUMBER = '" + model.MonthNumber + "' AND DEPOT_CODE = '" + model.DepotCode + "' ";


                    if (model.ChkEmployeeCode == "True")
                    {
                        QryCurrentMpo = QryCurrentMpo + " AND  EMP_CODE NOT IN (Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)";
                    }
                    else
                    {
                        QryCurrentMpo = QryCurrentMpo + " AND  EMP_CODE  IN (Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL)";
                    }

                    DataTable dtCurrentMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryCurrentMpo);
                    string dtCurrentMpoStr = string.Join("','", dbHelper.GetListString(dtCurrentMpo));
                    dtCurrentMpoStr = "'" + dtCurrentMpoStr + "'";
                    Qry = Qry + " and EMP_CODE  IN (" + dtCurrentMpoStr + ")";
                }
            }
            if (model.DepotName != "" && model.DepotName != null)
            {
                vHeader = vHeader + ", Depot : " + model.DepotName;
            }
            model.Designation = (model.Designation == "" || model.Designation == null) ? vHeader : vHeader + ", Designation : " + model.Designation;

            if (model.SBU != "" && model.SBU != null)
            {
                vHeader = vHeader + ", SBU : " + model.SBU;
            }

            Qry = Qry + " Group by EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME ";
            Qry = Qry + " ORDER BY MARKET_NAME";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);



            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ExpenseBillMpoTmRsmBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ExpenseBillMpoTmRsmBEO
                    {
                        SL = row["Col1"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),            
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),
                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),

                        GrandTotal = row["GrandTotal"].ToString(),
              
                     
                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
        }




        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmBEO>> GetAllDepotDataToExcelExport(DefaultParameterBEO model)
        {
           
            string vHeader = "";

          
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME," +
                         " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                         " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL,NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                         " from VW_ARC_EXPENSE_BILL_MPO_TM_RSM " +
                         " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";



            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + " " + model.Year;           

            Qry = Qry + " Group by EMP_CODE,EMP_NAME, DESIGNATION,MARKET_NAME,TERRITORY_NAME,REGION_NAME,DEPOT_NAME ";
            Qry = Qry + " ORDER BY MARKET_NAME";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);



            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ExpenseBillMpoTmRsmBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ExpenseBillMpoTmRsmBEO
                    {
                        SL = row["Col1"].ToString(),
                        EmployeeCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EMP_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),                    
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),
                        DA = row["DA_AMOUNT"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),
                        WaitingTotal = row["WAITING_TOTAL"].ToString(),
                        ApproveTotal = row["APPROVE_TOTAL"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),                     

                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
        }




        public List<ExpenseBillMpoTmRsmBEO> GetViewExpenseBillSummaryMpoTmRsmArchive(DefaultParameterBEO model)
        {
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME," +
                        " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                        " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL,"+
                        " NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                        " from VW_ARC_EXPENSE_BILL_MPO_TM_RSM " +
                        " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";


        
            if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
            {
                Qry = Qry + " and LOC_CODE='" + model.LocCode + "' ";

                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')";
                }
                else
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                }
            }
            else
            {

                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MPO','SMPO') and LOC_CODE IN (SELECT MP_GROUP from VW_ARC_HR_LOC_MAPPING Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND REGION_CODE = '" + model.RegionCode + "' )";

                }
                if (model.Designation == "RM")
                {
                    Qry = Qry + " AND DESIGNATION ='" + model.Designation + "' and TERRITORY_CODE IN (SELECT TERRITORY_CODE from VW_ARC_HR_LOC_MAPPING Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND REGION_CODE = '" + model.RegionCode + "')";
                }
                if (model.Designation == "ZM")
                {
                    Qry = Qry + " AND DESIGNATION ='" + model.Designation + "' and REGION_CODE='" + model.RegionCode + "'";
                }
                if (model.Designation == "PM")
                {
                    Qry = Qry + " AND DESIGNATION IN ('PM','CM' ,'Sr. Manager')  AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                }
                if ((model.Designation == "" || model.Designation == "null" || model.Designation == null) && model.RegionCode != "" && model.RegionCode != null)
                {
                        Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";                   
                  
                }
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
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        TotalAmount = row["Total"].ToString(),                  
                        GrandTotal = row["GrandTotal"].ToString(),

                    }).ToList();
            return item;
        }
        public List<ExpenseBillMpoTmRsmDetail> GetViewExpenseBillDetailMpoTmRsmArchive(DefaultParameterBEO model)
        {
            string Qry = " SELECT   EMP_CODE,EMP_NAME,LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REMARKS,REVIEW_STATUS, DISTANCE, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT,  DA_AMOUNT+TA_AMOUNT Total  " +
                            " FROM VW_ARC_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            if (model.LocCode != "" && model.LocCode != null)
            {
                Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";
            }
            if (model.EmployeeCode != "" && model.EmployeeCode != null)
            {
                Qry = Qry + " and EMP_CODE='" + model.EmployeeCode + "'";
            }


            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                Qry = Qry + " and STATUS_BOSS1='" + model.MasterStatus + "'";
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

                        SupervisorID = row["ID_BOSS1"].ToString(),               
         
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",

                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmBEO>> GetExportExpenseBillSummaryMpoTmRsmArchive(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME," +
                         " Sum(DA_AMOUNT) DA_AMOUNT,Sum(TA_AMOUNT) TA_AMOUNT,Sum(DA_AMOUNT)+Sum(TA_AMOUNT) Total,  " +
                         " NVL(SUM(WAITING_TOTAL),0) WAITING_TOTAL,NVL(SUM(APPROVE_TOTAL),0) APPROVE_TOTAL," +
                         " NVL(SUM(MISCELLANEOUS),0) MISCELLANEOUS,NVL(SUM(APPROVE_TOTAL),0)+NVL(SUM(MISCELLANEOUS),0) GrandTotal" +
                         " from VW_ARC_EXPENSE_BILL_MPO_TM_RSM " +
                         " Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";       

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
            {
                if (model.LocName != null && model.LocName != "")
                {
                    string lastItem = model.LocName;
                    vHeader = vHeader + ", Employee: " + lastItem;
                }
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry = Qry + " and LOC_CODE='" + model.LocCode + "' ";


                if (model.Designation == "MIO")
                {
                    Qry = Qry + " and DESIGNATION IN ('MIO','SMIO')";
                }

                else
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                }
            }
            else
            {
                if (model.Designation == "MIO")
                {
                    Qry = Qry + "  and DESIGNATION IN ('MPO','SMPO')  and LOC_CODE IN ( SELECT MP_GROUP from VW_ARC_HR_LOC_MAPPING Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND REGION_CODE = '" + model.RegionCode + "')";
                }
                if (model.Designation == "RM")
                {
                    Qry = Qry + " AND DESIGNATION ='" + model.Designation + "' and TERRITORY_CODE IN ( SELECT TERRITORY_CODE from VW_ARC_HR_LOC_MAPPING Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' AND REGION_CODE = '" + model.RegionCode + "')";

                }
                if (model.Designation == "ZM")
                {
                    Qry = Qry + "AND DESIGNATION ='" + model.Designation + "' and REGION_CODE='" + model.RegionCode + "'";
                }
                if (model.Designation == "PM")
                {
                    Qry = Qry + " AND DESIGNATION IN ('PM','CM' ,'Sr. Manager')  AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                }
                if ((model.Designation == "" || model.Designation == "null" || model.Designation == null) && model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
                }
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                vHeader = vHeader + ", Designation : " + model.Designation;
            }


            Qry = Qry + " Group by EMP_CODE,EMP_NAME, DESIGNATION,LOC_CODE,MARKET_NAME,TERRITORY_NAME ";
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
                      //  TotalAmount = row["Total"].ToString(),
                        MiscManualTADABill = row["MISCELLANEOUS"].ToString(),
                        GrandTotal = row["GrandTotal"].ToString(),
                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
        }
        public Tuple<string, DataTable, List<ExpenseBillMpoTmRsmDetail>> GetExportExpenseBillDetailMpoTmRsmAchive(DefaultParameterBEO model)
        {
            string Qry = " SELECT  LOC_CODE,STATUS_BOSS1,ID_BOSS1,DESIGNATION, DAY_NUMBER, MONTH_NUMBER, YEAR, MWP, EWP,IS_HOLIDAY,REVIEW_STATUS, DISTANCE,REMARKS, ALLOWANCE_NATURE,RECOMMEND, DA_AMOUNT, TA_AMOUNT, DA_AMOUNT+TA_AMOUNT Total  " +
                            " FROM VW_ARC_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            string vHeader = "Month: " + month + " " + model.Year;
            vHeader = vHeader + ", Employee: " + model.EmployeeCode + ", " + model.EmployeeName + ", " + model.Designation + ", " + model.LocName;

            if (model.LocCode != "" && model.LocCode != null)
            {
                Qry = Qry + "and LOC_CODE='" + model.LocCode + "'";
            }
            if (model.EmployeeCode != "" && model.EmployeeCode != null)
            {
                Qry = Qry + " and EMP_CODE='" + model.EmployeeCode + "'";
            }
            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                Qry = Qry + " and STATUS_BOSS1='" + model.MasterStatus + "'";
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
                        ReviewStatus = row["REVIEW_STATUS"].ToString(),
                        Recommend = row["RECOMMEND"].ToString(),
                        SupervisorID = row["ID_BOSS1"].ToString(),                 
                        UserRemarks = row["REMARKS"].ToString(),        
                        ApproveStatus = row["STATUS_BOSS1"].ToString() == "Approved" ? "A" : "W",

                    }).ToList();

            return Tuple.Create(vHeader, dt, item);
        }


    }
}
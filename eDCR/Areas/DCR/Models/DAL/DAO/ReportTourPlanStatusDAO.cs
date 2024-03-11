using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportTourPlanStatusDAO : ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();

        public List<ReportTourPlanStatusBEO> GetViewData(DefaultParameterBEO model)
        {
            string Qry = "SELECT DESIGNATION,COUNT(DESIGNATION) TO_COUNT " +
               "  FROM VW_TP_STATUS Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";


            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }

            Qry = Qry + " GROUP BY DESIGNATION";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanStatusBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanStatusBEO
                    {

                        Designation = row["DESIGNATION"].ToString(),
                        TPCount = row["TO_COUNT"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportTourPlanStatus>> Export(DefaultParameterBEO model)
        {
       
            string vHeader = "";

            string Qry = " Select MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MONTH_NUMBER, YEAR,MARKET_NAME, REGION_CODE, REGION_NAME, TP_STATUS, DVR_STATUS, PWDS_STATUS, GWDS_STATUS" +
               " from VW_TP_DVR_PWDS_GWDS_STATUS Where  MONTH_NUMBER||YEAR ='" + model.MonthNumber + model.Year + "' ";
            
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " And TERRITORY_CODE= '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry += " And REGION_CODE= '" + model.RegionCode + "'";
            }
        

            Qry += " ORDER BY MARKET_NAME";


        

          

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanStatus> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanStatus
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        TPStatus = row["TP_STATUS"].ToString(),
                        DVRStatus = row["DVR_STATUS"].ToString(),
                        PWDSStatus = row["PWDS_STATUS"].ToString(),
                        GWDSStatus = row["GWDS_STATUS"].ToString(),
                       
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

        public List<ReportTourPlanStatus> GetViewDetailData(DefaultParameterBEO model)
        {
            string SetDate = model.DayNumber + "-" + model.MonthNumber + "-" + model.Year;
            string Qry = "Select  EMPID,EMPNAME,LOC_NAME,DESIGNATION " +
                " from  VW_TP_STATUS Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and DESIGNATION='" + model.Designation + "'";


            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
             
                Qry += " And TSM_ID= '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
             
                Qry += " And REGION_CODE= '" + model.RegionCode + "'";
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);


            List<ReportTourPlanStatus> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanStatus
                    {
                        EmployeeID = row["EMPID"].ToString(),
                        EmployeeName = row["EMPNAME"].ToString(),
                        MarketName = row["LOC_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                     
                    }).ToList();
            return item;
        }


        public List<ReportTourPlanStatus> GetTourPlanStatus(DefaultParameterBEO model)
        {
            string SetDate = model.DayNumber + "-" + model.MonthNumber + "-" + model.Year;
            string Qry = " Select MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MONTH_NUMBER, YEAR,MARKET_NAME, REGION_CODE, REGION_NAME, TP_STATUS, DVR_STATUS, PWDS_STATUS, GWDS_STATUS" +
                " from VW_TP_DVR_PWDS_GWDS_STATUS Where  YEAR =" + model.Year + " AND  MONTH_NUMBER='" + model.MonthNumber+"'";

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
                    if (model.RegionCode != null && model.RegionCode != "")
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
                    if (model.Designation != "" && model.Designation != null)
                    {
                        Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    }
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);


            List<ReportTourPlanStatus> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanStatus
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        TPStatus = row["TP_STATUS"].ToString(),
                        DVRStatus = row["DVR_STATUS"].ToString(),
                        PWDSStatus = row["PWDS_STATUS"].ToString(),
                        GWDSStatus = row["GWDS_STATUS"].ToString(),

                    }).ToList();
            return item;
        }
        public List<ReportTourPlanChangeFortnightStatus> GetChangeTpFortnightStatus(DefaultParameterBEO model)
        {
            string SetDate = model.DayNumber + "-" + model.MonthNumber + "-" + model.Year;

            string Qry = " Select MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MONTH_NUMBER, YEAR,NVL(MARKET_NAME,TERRITORY_NAME) MARKET_NAME, REGION_CODE, REGION_NAME, CHANGE_TP_STATUS, FORT_NIGHT_1_STATUS,FORT_NIGHT_2_STATUS, BILL_STATUS, STK_STATUS" +
                        " from VW_TP_CHN_FORT_BILL_STK_STATUS Where  MONTH_NUMBER||YEAR ='" + model.MonthNumber.Trim() + model.Year.Trim() + "' ";

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
                    if (model.RegionCode != null && model.RegionCode != "")
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
                    if (model.Designation != "" && model.Designation != null)
                    {
                        Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    }
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry = Qry + " and  REGION_CODE = '" + model.RegionCode + "' ";
                    }
                }
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);


            List<ReportTourPlanChangeFortnightStatus> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanChangeFortnightStatus
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        ChangeTPStatus = row["CHANGE_TP_STATUS"].ToString(),
                        Fortnight1Status = row["FORT_NIGHT_1_STATUS"].ToString(),
                        Fortnight2Status = row["FORT_NIGHT_2_STATUS"].ToString(),
                        BillStatus = row["BILL_STATUS"].ToString(),
                        StockCheckStatus = row["STK_STATUS"].ToString(),

                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportTourPlanChangeFortnightStatus>> ExportChangeTpFortnight(DefaultParameterBEO model)
        {

            string vHeader = "";


            string Qry = " Select MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MONTH_NUMBER, YEAR,NVL(MARKET_NAME,TERRITORY_NAME) MARKET_NAME, REGION_CODE, REGION_NAME, CHANGE_TP_STATUS, FORT_NIGHT_1_STATUS,FORT_NIGHT_2_STATUS, BILL_STATUS, STK_STATUS" +
                " from VW_TP_CHN_FORT_BILL_STK_STATUS Where  MONTH_NUMBER||YEAR ='" + model.MonthNumber.Trim() + model.Year.Trim() + "' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber.Trim()));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;
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
                    if (model.RegionCode != null && model.RegionCode != "")
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
                    if (model.Designation != "" && model.Designation != null)
                    {
                        Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                        vHeader = vHeader + ", DESIGNATION : " + model.Designation;
                    }
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        if (model.RegionName != "" && model.RegionName != null)
                        {
                            string[] Region = model.RegionName.Split(',');
                            string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                            vHeader = vHeader + ", Region : " + lastItem;
                        }
                        Qry += " And REGION_CODE= '" + model.RegionCode + "'";
                    }
                }
            }      


            Qry += " ORDER BY MARKET_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanChangeFortnightStatus> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanChangeFortnightStatus
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        ChangeTPStatus = row["CHANGE_TP_STATUS"].ToString(),
                        Fortnight1Status = row["FORT_NIGHT_1_STATUS"].ToString(),
                        Fortnight2Status = row["FORT_NIGHT_2_STATUS"].ToString(),
                        BillStatus = row["BILL_STATUS"].ToString(),
                        StockCheckStatus = row["STK_STATUS"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

    }
}
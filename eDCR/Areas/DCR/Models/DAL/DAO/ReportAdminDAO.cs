using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportAdminDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();
        public Tuple<string, DataTable, List<ReportAdminPromotionalItemExecuion>> ExportPromotionalItemExecution(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MP_GROUP,MARKET_NAME,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                          " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month-Year: " + month + " - " + model.Year;


            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                Qry += " AND PRODUCT_CODE LIKE '%" + model.ProductCode + "%'";
            }
            vHeader = vHeader + ", Print Date: " + CntDate;

            Qry = Qry + " Group by MP_GROUP,MARKET_NAME ";
            Qry = Qry + " Order by MARKET_NAME   ";
     
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportAdminPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportAdminPromotionalItemExecuion
                    {
                        SL = row["Col1"].ToString(),
                        MPGroup= row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        CarryQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        AddDefiQty = row["VARIABLE_QTY"].ToString(),
                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
        public Tuple<string, DataTable, List<ReportAdminPromotionalItemExecuion>> ExportDvrMonthly(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MP_GROUP,MARKET_NAME,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                          " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month-Year: " + month + " - " + model.Year;


            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                Qry += " AND PRODUCT_CODE LIKE '%" + model.ProductCode + "%'";
            }
            vHeader = vHeader + ", Print Date: " + CntDate;

            Qry = Qry + " Group by MP_GROUP,MARKET_NAME ";
            Qry = Qry + " Order by MARKET_NAME   ";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportAdminPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportAdminPromotionalItemExecuion
                    {
                        SL = row["Col1"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        CarryQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        AddDefiQty = row["VARIABLE_QTY"].ToString(),
                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }


    }
}
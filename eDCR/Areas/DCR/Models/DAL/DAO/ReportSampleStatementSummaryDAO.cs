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
    public class ReportSampleStatementSummaryDAO
    {
       

        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();

        public List<ReportSampleStatementSummaryBEO> GetMainGridData(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string Qry = " SELECT MARKET_NAME,MPO_NAME,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,Sum(REST_QTY)+Sum(Central_QTY)+Sum(Variable_QTY) Total_Qty,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                         " from VW_DCR_ITEM_EXE Where SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'"; 

            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {               
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            Qry = Qry + " Group by MARKET_NAME,MPO_NAME,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' ";
            Qry = Qry + " Order by MARKET_NAME,PRODUCT_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportSampleStatementSummaryBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportSampleStatementSummaryBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),                    

                        ReceiveQty = row["Total_Qty"].ToString(),                      
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()

                    }).ToList();
            return item;
        }



        public Tuple<string, DataTable, List<ReportSampleStatementSummaryBEO>> Export(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string vHeader = "";
            string Qry = " SELECT MARKET_NAME,MPO_NAME,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,Sum(REST_QTY)+Sum(Central_QTY)+Sum(Variable_QTY) Total_Qty,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                       " from VW_DCR_ITEM_EXE Where SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'"; 

            vHeader = vHeader + "Date Between: " + model.FromDate.Trim() + " to " + model.ToDate.Trim();
            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
            }
  
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }

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
            Qry = Qry + " Group by MARKET_NAME,MPO_NAME,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' ";
            Qry = Qry + " Order by MARKET_NAME,PRODUCT_NAME   ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);


            List<ReportSampleStatementSummaryBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportSampleStatementSummaryBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                      
                        ReceiveQty = row["Total_Qty"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()


                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
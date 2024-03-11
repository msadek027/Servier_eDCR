using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL
{
    public class ReportProductWiseCallSummaryDAO: ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportProductWiseCallSummaryBEO> GetData(DefaultParameterBEO model)
        {
            string Qry = " Select  MARKET_NAME,PRODUCT_CODE,PRODUCT_NAME,PACK_SIZE, PRODUCT_CALL, TOTAL_PRODUCT_CALL" +
                         " FROM VW_DOC_WISE_ITEM_WISE_CALL " +
                         " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }
            Qry = Qry + " ORDER BY MARKET_NAME,PRODUCT_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportProductWiseCallSummaryBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportProductWiseCallSummaryBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        PackSize = row["PACK_SIZE"].ToString(),
                        
                        TotalProductCall = row["TOTAL_PRODUCT_CALL"].ToString(),
                        ProductCall = row["PRODUCT_CALL"].ToString(),
                        Achi = row["TOTAL_PRODUCT_CALL"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["PRODUCT_CALL"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_PRODUCT_CALL"].ToString())).ToString("#.##"),



                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportProductWiseCallSummaryBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = "Select  MARKET_NAME,PRODUCT_CODE,PRODUCT_NAME,PACK_SIZE, PRODUCT_CALL, TOTAL_PRODUCT_CALL" +
             " FROM VW_DOC_WISE_ITEM_WISE_CALL " +
             " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;


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
                Qry += " And TSM_ID= '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }

            }
         
            Qry = Qry + " ORDER BY MARKET_NAME,PRODUCT_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportProductWiseCallSummaryBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportProductWiseCallSummaryBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        PackSize = row["PACK_SIZE"].ToString(),
                        TotalProductCall = row["TOTAL_PRODUCT_CALL"].ToString(),
                        ProductCall = row["PRODUCT_CALL"].ToString(),
                        Achi = row["TOTAL_PRODUCT_CALL"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["PRODUCT_CALL"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_PRODUCT_CALL"].ToString())).ToString("#.##"),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

      
    }
}
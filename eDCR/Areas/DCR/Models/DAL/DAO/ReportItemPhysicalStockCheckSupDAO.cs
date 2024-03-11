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
    public class ReportItemPhysicalStockCheckSupDAO 
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        public List<ReportItemPhysicalStockCheckSupBEO> GetData(DefaultParameterBEO model)
        {
            string Qry = " Select TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,MPO_NAME,MARKET_NAME, PRODUCT_CODE,PRODUCT_NAME,PACK_SIZE,ITEM_TYPE,ITEM_FOR, LOGICAL_QTY, PHYSICAL_QTY,  NVL(REMARKS,'empty') REMARKS" +
                         ",SUPERVISOR_ID,SUPERVISOR_NAME,DESIGNATION" +
                        " FROM VW_SUP_INV_ITEM_PHY_STOCK_CHK " +
                        " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
            Qry = Qry + " ORDER BY SET_DATE,MARKET_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportItemPhysicalStockCheckSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemPhysicalStockCheckSupBEO
                    {
                        SetDate = row["SET_DATE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                   
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        PackSize = row["PACK_SIZE"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_FOR"].ToString(),
                        PhysicalQty = row["PHYSICAL_QTY"].ToString(),
                        eDCRQty = row["LOGICAL_QTY"].ToString(),
                        Remarks = row["REMARKS"].ToString(),

                        CheckedByID = row["SUPERVISOR_ID"].ToString(),
                        CheckedByName = row["SUPERVISOR_NAME"].ToString(),
                        CheckedByDesignation = row["DESIGNATION"].ToString(),

                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportItemPhysicalStockCheckSupBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry =" Select TO_CHAR(SET_DATE,'dd/mm/yyyy') SET_DATE, MPO_NAME,MARKET_NAME,PRODUCT_CODE,PRODUCT_NAME,PACK_SIZE,ITEM_TYPE,ITEM_FOR, LOGICAL_QTY, PHYSICAL_QTY, NVL(REMARKS,'empty') REMARKS, SUPERVISOR_ID,SUPERVISOR_NAME,DESIGNATION" +
                        " FROM VW_SUP_INV_ITEM_PHY_STOCK_CHK " +
                        " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month " + month + ", " + model.Year;


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
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
          
            Qry = Qry + " ORDER BY SET_DATE,MARKET_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportItemPhysicalStockCheckSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemPhysicalStockCheckSupBEO
                    {
                        SetDate = row["SET_DATE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                    
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        PackSize = row["PACK_SIZE"].ToString(),
                        ItemType =row["ITEM_TYPE"].ToString() + row["ITEM_FOR"].ToString(),
                        PhysicalQty = row["PHYSICAL_QTY"].ToString(),
                        eDCRQty = row["LOGICAL_QTY"].ToString(),
                        Remarks = row["REMARKS"].ToString(),
                        CheckedByID = row["SUPERVISOR_ID"].ToString(),
                        CheckedByName = row["SUPERVISOR_NAME"].ToString(),
                        CheckedByDesignation = row["DESIGNATION"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportSelectedItemPlanDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        public List<ReportSelectedItemPlanBEL> GetGridData(DefaultParameterBEO model)
        {
            string qry = "Select distinct MARKET_NAME,MPO_NAME,NO_OF_DOC,PRODUCT_NAME ||' ('||PACK_SIZE||')' PRODUCT_NAME, REST_QTY,CENTRAL_QTY,CENTRAL_QTY+REST_QTY+VARIABLE_QTY Total_Qty,VARIABLE_QTY,BALANCE_QTY,DOCTOR_NAME, DOCTOR_ID " +
                " from VW_PWDS_GWDS_PLAN  Where   YEAR = " + model.Year + "  AND MONTH_NUMBER = '" + model.MonthNumber + "'";
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " AND TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }

            if (model.ItemType != "" && model.ItemType != null)
            {
                qry += " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                qry += " AND PRODUCT_CODE='" + model.ProductCode + "'";
            }
            qry = qry + " Order by MARKET_NAME,MPO_NAME,PRODUCT_NAME ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportSelectedItemPlanBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSelectedItemPlanBEL
                    {
                       
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                       
                        ProductName = row["PRODUCT_NAME"].ToString(),
                       
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["VARIABLE_QTY"].ToString(),                      
                       
                        BalanceQty = row["BALANCE_QTY"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
           


                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportSelectedItemPlanBEL>> Export(DefaultParameterBEO model)
        {

        
            string vHeader = "";

            string qry = "Select distinct MARKET_NAME,MPO_NAME,NO_OF_DOC,PRODUCT_NAME ||' ('||PACK_SIZE||')' PRODUCT_NAME, REST_QTY,CENTRAL_QTY,CENTRAL_QTY+REST_QTY+VARIABLE_QTY Total_Qty,VARIABLE_QTY,BALANCE_QTY,DOCTOR_NAME,DOCTOR_ID " +
            " from VW_PWDS_GWDS_PLAN Where   YEAR = " + model.Year + "  AND MONTH_NUMBER = '" + model.MonthNumber + "'";
           
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            if (model.ItemType != "" && model.ItemType != null)
            {
                vHeader = vHeader + ", ItemType: " + model.ItemType;
                qry += " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }
         
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF : " + model.MPOName;
                qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                qry += " And TERRITORY_CODE ='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
         
            qry = qry + " Order by MARKET_NAME,MPO_NAME,PRODUCT_NAME ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);

            List<ReportSelectedItemPlanBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSelectedItemPlanBEL
                    {

                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),                   
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["VARIABLE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),


                    }).ToList();

            return Tuple.Create(vHeader, dt, item);


        }





        public List<ReportSelectedItemPlanBEL> GetGridDataArchive(DefaultParameterBEO model)
        {
            string qry = "Select distinct MARKET_NAME,MPO_NAME,NO_OF_DOC,PRODUCT_NAME ||' ('||PACK_SIZE||')' PRODUCT_NAME, REST_QTY,CENTRAL_QTY,CENTRAL_QTY+REST_QTY+VARIABLE_QTY Total_Qty,VARIABLE_QTY,BALANCE_QTY,DOCTOR_NAME, DOCTOR_ID " +
                " from VW_ARC_PWDS_GWDS_PLAN Where   YEAR = " + model.Year + "  AND MONTH_NUMBER = '" + model.MonthNumber + "'";
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " AND TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }

            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }

            if (model.ItemType != "" && model.ItemType != null)
            {
                qry += " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                qry += " AND PRODUCT_CODE='" + model.ProductCode + "'";
            }
            qry = qry + " Order by MARKET_NAME,MPO_NAME,PRODUCT_NAME ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportSelectedItemPlanBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSelectedItemPlanBEL
                    {

                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),

                        ProductName = row["PRODUCT_NAME"].ToString(),
                       
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["VARIABLE_QTY"].ToString(),

                        BalanceQty = row["BALANCE_QTY"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),



                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportSelectedItemPlanBEL>> ExportArchive(DefaultParameterBEO model)
        {


            string vHeader = "";

            string qry = "Select distinct MARKET_NAME,MPO_NAME,NO_OF_DOC,PRODUCT_NAME ||' ('||PACK_SIZE||')' PRODUCT_NAME, REST_QTY,CENTRAL_QTY,CENTRAL_QTY+REST_QTY+VARIABLE_QTY Total_Qty,VARIABLE_QTY,BALANCE_QTY,DOCTOR_NAME,DOCTOR_ID " +
            " from VW_ARC_PWDS_GWDS_PLAN Where   YEAR = " + model.Year + "  AND MONTH_NUMBER = '" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            if (model.ItemType != "" && model.ItemType != null)
            {
                vHeader = vHeader + ", ItemType: " + model.ItemType;
                qry += " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }

            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF : " + model.MPOName;
                qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                qry += " And TERRITORY_CODE ='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }

            qry = qry + " Order by MARKET_NAME,MPO_NAME,PRODUCT_NAME ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);

            List<ReportSelectedItemPlanBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSelectedItemPlanBEL
                    {

                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),

                        ProductName = row["PRODUCT_NAME"].ToString(),
              
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["VARIABLE_QTY"].ToString(),

                        BalanceQty = row["BALANCE_QTY"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),



                    }).ToList();

            return Tuple.Create(vHeader, dt, item);


        }
    }
}
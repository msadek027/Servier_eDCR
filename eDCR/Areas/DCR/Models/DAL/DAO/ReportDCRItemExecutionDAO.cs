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
    public class ReportDCRItemExecutionDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        public List<ReportDCRItemExecutionBEL> GetGridData(ReportDCRItemExecutionBEL model)
        {
            string Qry = "SELECT MP_GROUP,MPO_CODE,MPO_NAME,TSM_ID,TSM_NAME,TERRITORY_CODE,TERRITORY_NAME,Sum(REST_QTY) REST_QTY,SUM(Central_QTY) Central_QTY,Sum(REST_QTY) +SUM(Central_QTY)+SUM(Variable_QTY) Total_Qty,SUM(Variable_QTY) Variable_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,SUM(BALANCE_QTY) BALANCE_QTY "+
                " FROM VW_DCR_ITEM_EXE Where 1=1 ";
         
          if (model.Year != "" && model.Year != null)
          {
              Qry = Qry + " and Year=" + model.Year + "";
          }
          if (model.MonthNumber != "" && model.MonthNumber != null)
          {
              Qry = Qry + " and MONTH_NUMBER='" + model.MonthNumber + "'";
           
          }
          if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
          {
              Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
          }
          if (model.MPGroup != "" && model.MPGroup != null)
          {
              Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
          }
          if (model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
          {
              Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
          }

          if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }
            Qry = Qry + " group by MP_GROUP,MPO_CODE,MPO_NAME,TSM_ID,TSM_NAME,TERRITORY_CODE,TERRITORY_NAME ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportDCRItemExecutionBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDCRItemExecutionBEL
                    {
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_Name"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        TerritoryManagerID = row["TSM_ID"].ToString(),
                        TerritoryManagerName = row["TSM_NAME"].ToString(),                       
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["Central_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),                        
                        VariableQty = row["Variable_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()


                    }).ToList();
            return item;
        }

        public List<ReportDCRItemExecutionDetail> GetItemDetail(ReportDCRItemExecutionBEL model)
        {

            string Qry = "SELECT To_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,MP_GROUP,MPO_CODE,MPO_NAME,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,REST_QTY,Central_QTY,REST_QTY+Central_QTY TOTAL_QTY, VARIABLE_QTY,EXECUTE_QTY,BALANCE_QTY"+
                " FROM VW_DCR_ITEM_EXE Where 1=1 ";
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " and Year=" + model.Year + "";
            }
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " and MONTH_NUMBER='" + model.MonthNumber + "'";
               
            }
            if ( model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }

            if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportDCRItemExecutionDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDCRItemExecutionDetail
                    {
                        SetDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["Central_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["Variable_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()

                     
                    }).ToList();
            return item;
        }



        public List<ReportDCRDetail> GetExecuteDetail(ReportDCRItemExecutionBEL model)
        {
            string Qry = "Select distinct to_char(SET_DATE,'dd-mm-yyyy') SET_DATE, MP_Group,DOCTOR_ID,DOCTOR_NAME,ITEM_TYPE,Product_Code,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' Product_Name,QUANTITY,STATUS" +
                " from VW_DOC_WISE_NO_OF_CALL_DTL Where 1=1";
          
             if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " and To_char(SET_DATE,'YYYY')=" + model.Year + "";
            }
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " and To_char(SET_DATE,'MM')='" + model.MonthNumber + "'";
               
            }
            if (model.ProductCode != " " &&  model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }

            if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
            }

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportDCRDetail> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRDetail
                    {
                        SetDate = row["SET_DATE"].ToString(),
                        MPGroup = row["MP_Group"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        DcrType = row["STATUS"].ToString(),

                    }).ToList();
            return item;
        }



        public Tuple<string, DataTable, List<ReportDCRItemExecutionBEL>> Export(DefaultParameterBEO model)
        {
      
            string vHeader = "";
            string Qry = "SELECT MP_GROUP,MPO_CODE,MPO_NAME,TSM_ID,TSM_NAME,TERRITORY_CODE,TERRITORY_NAME,Sum(REST_QTY) REST_QTY,SUM(Central_QTY) Central_QTY,Sum(REST_QTY) +SUM(Central_QTY)+SUM(Variable_QTY) Total_Qty,SUM(Variable_QTY) Variable_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,SUM(BALANCE_QTY) BALANCE_QTY "+
                " FROM VW_DCR_ITEM_EXE Where To_char(SET_DATE,'MMYYYY')='" + model.MonthNumber + model.Year + "' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month-Year: " + month + " - " + model.Year;
            if (model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }
            if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
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
            }
            Qry = Qry + " group by MP_GROUP,MPO_CODE,MPO_NAME,TSM_ID,TSM_NAME,TERRITORY_CODE,TERRITORY_NAME ";
 
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDCRItemExecutionBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDCRItemExecutionBEL
                    {
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_Name"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        TerritoryManagerID = row["TSM_ID"].ToString(),
                        TerritoryManagerName = row["TSM_NAME"].ToString(),
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["Central_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["Variable_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()


                    }).ToList();
            return Tuple.Create(vHeader, dt,item);

        }
    }
}
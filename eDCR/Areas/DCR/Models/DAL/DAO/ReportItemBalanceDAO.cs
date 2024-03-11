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
    public class ReportItemBalanceDAO
    {
       
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportItemBalanceBEL> GetGridData(ReportItemBalanceBEL model)
        {
            string Qry = "SELECT MP_GROUP,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' PRODUCT_NAME,Sum(REST_QTY) REST_QTY,Sum(Central_QTY) Central_QTY,Sum(REST_QTY)+Sum(Central_QTY) Total_Qty,Sum(Variable_QTY) Variable_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(BALANCE_QTY) BALANCE_QTY " +
                " from VW_INV_ITEM_BALANCE Where MP_GROUP='" + model.MPGroup + "' ";
          
            if (model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }

            if (model.ItemType != "All" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "' ";

            }

            Qry = Qry + " Group by MP_GROUP,PRODUCT_CODE,PRODUCT_NAME||' ('||PACK_SIZE||')' Order by PRODUCT_NAME";
           
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportItemBalanceBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportItemBalanceBEL
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        //ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_FOR"].ToString(),
                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["Central_QTY"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        VariableQty = row["Variable_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString()
                     
                    }).ToList();
            return item;
        }
    }
}
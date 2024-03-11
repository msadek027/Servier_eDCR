using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL
{
    public class ReportRemarksDAO:ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();

      
        public List<ReportRemarksBEO> GetMainGrid(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
           

            string qry = " Select PRODUCT_CODE, QUANTITY,TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE, MP_GROUP, YEAR, MONTH_NUMBER,  REMARKS, PRODUCT_NAME,Pack_Size,MPO_CODE,MPO_NAME" +
                " from VW_ITEM_REMARK Where REMARKS IS NOT NULL AND SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "' ";

            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != null && model.MPGroup != "")
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            qry += " ORDER BY SET_DATE ASC";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportRemarksBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportRemarksBEO
                    {
                        SL = row["Col1"].ToString(),
                        FromDate = row["SET_DATE"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),

                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        ItemType = row["PRODUCT_CODE"].ToString().Substring(0,3),

                        PackSize = row["Pack_Size"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        Remark = row["REMARKS"].ToString(),




                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportRemarksBEO>> Export(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string vHeader = "";
            string qry = " Select PRODUCT_CODE, QUANTITY,TO_Char(SET_DATE,'dd/mm/yyyy') SET_DATE, MP_GROUP, YEAR, MONTH_NUMBER,  REMARKS, PRODUCT_NAME,Pack_Size,MPO_CODE,MPO_NAME" +
                         " from VW_ITEM_REMARK Where REMARKS IS NOT NULL AND SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "' ";


            vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;
          
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
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

            qry += " ORDER BY SET_DATE ASC";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportRemarksBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportRemarksBEO
                    {
                        SL = row["Col1"].ToString(),
                        FromDate = row["SET_DATE"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),

                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        ItemType = row["PRODUCT_CODE"].ToString().Substring(0, 3),

                        PackSize = row["Pack_Size"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        Remark = row["REMARKS"].ToString(),

                    }).ToList();

            return Tuple.Create(vHeader, dt, item);

        }
    }
}
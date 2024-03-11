using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportProductWiseDoctorSampleSummaryDAO
    {     
        
         DBHelper dbHelper=new DBHelper();
         DBConnection dbConn = new DBConnection();
         string CntDate = DateTime.Now.ToString("dd-MM-yyyy");
         string CntTime = DateTime.Now.ToString("hh:mm");
        DateFormat dateFormat = new DateFormat();


      
         public List<ReportProductWiseDoctorSampleSummaryBEO> GetGridData(DefaultParameterBEO model)
         {

            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);          

            string qry = "SELECT MP_GROUP,MPO_Code,MPO_Name,MARKET_NAME, TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE, DOCTOR_ID, DOCTOR_NAME, SPECIALIZATION, PRODUCT_CODE, PRODUCT_NAME, TOTAL_SAMPLE, TOTAL_SELECTED, TOTAL_GIFT " +
                         " from VW_DCR_ITEM_WISE_SUMMARY WHERE SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";
           
             if (model.ItemType == "PI")
            {
                model.ItemType = "'SlR','SmR','SmI'";
                qry += " AND ITEM_TYPE||ITEM_FOR IN (" + model.ItemType + ")";
            }
            if (model.ItemType == "GI")
            {
                model.ItemType = "'GtR','SmI'";
                qry += " AND ITEM_TYPE||ITEM_FOR IN (" + model.ItemType + ")";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                qry += " AND PRODUCT_CODE2='" + model.ProductCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " AND TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.DoctorID != "" && model.DoctorID != " " && model.DoctorID != null)
            {
                qry += " AND DOCTOR_ID='" + model.DoctorID + "'";
            }
            if (model.DoctorName != "" && model.DoctorName != " " && model.DoctorName != null)
            {
                qry += " AND DOCTOR_NAME LIKE '%" + model.DoctorName + "%'";
            }
            qry = qry+ " Order by SET_DATE ,PRODUCT_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportProductWiseDoctorSampleSummaryBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportProductWiseDoctorSampleSummaryBEO
                    {
                        FromDate = row["SET_DATE"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_Name"].ToString(),
                        MarketName = row["MARKET_Name"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),                  
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Speciality = row["SPECIALIZATION"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        SampleQty = row["TOTAL_SAMPLE"].ToString(),
                        SelectedQty = row["TOTAL_SELECTED"].ToString(),
                        GiftQty =row["TOTAL_GIFT"].ToString(),
                        PPMQty="0",
                     }).ToList();
            return item;
        
        }
        public Tuple<string, DataTable, List<ReportProductWiseDoctorSampleSummaryBEO>> Export(DefaultParameterBEO model)
        {
          

            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string vHeader = "";
            string qry = "SELECT MP_GROUP,MPO_Code,MPO_Name,MARKET_NAME,SET_DATE, DOCTOR_ID, DOCTOR_NAME, SPECIALIZATION, PRODUCT_CODE, PRODUCT_NAME, TOTAL_SAMPLE, TOTAL_SELECTED, TOTAL_GIFT " +
                       " from VW_DCR_ITEM_WISE_SUMMARY WHERE SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";

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
                qry += " And TERRITORY_CODE= '" + model.TerritoryManagerID + "'";
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
            if (model.ItemType == "PI")
            {
                model.ItemType = "'SlR','SmR','SmI'";
                qry += " AND ITEM_TYPE||ITEM_FOR IN (" + model.ItemType + ")";
            }
            if (model.ItemType == "GI")
            {
                model.ItemType = "'GtR','SmI'";
                qry += " AND ITEM_TYPE||ITEM_FOR IN (" + model.ItemType + ")";
            }
            if (model.ProductCode != "" && model.ProductCode != " " && model.ProductCode != null)
            {
                qry += " AND PRODUCT_CODE2='" + model.ProductCode + "'";
            }
            if (model.DoctorID != "" && model.DoctorID != " " && model.DoctorID != null)
            {
                qry += " AND DOCTOR_ID='" + model.DoctorID + "'";
            }
            if (model.DoctorName != "" && model.DoctorName != " " && model.DoctorName != null)
            {
                qry += " AND DOCTOR_NAME LIKE '%" + model.DoctorName + "%'";
            }


            qry = qry + " Order by SET_DATE ,PRODUCT_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportProductWiseDoctorSampleSummaryBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportProductWiseDoctorSampleSummaryBEO
                    {
                        FromDate = row["SET_DATE"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_Name"].ToString(),
                        MarketName = row["MARKET_Name"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Speciality = row["SPECIALIZATION"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        SampleQty = row["TOTAL_SAMPLE"].ToString(),
                        SelectedQty = row["TOTAL_SELECTED"].ToString(),
                        GiftQty = row["TOTAL_GIFT"].ToString(),
                        PPMQty = "0",
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);

        }
    }
}
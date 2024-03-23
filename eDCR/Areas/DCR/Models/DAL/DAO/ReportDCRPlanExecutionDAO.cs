using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System.Globalization;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportDCRPlanExecutionDAO
    {
        ExportToAnother export = new ExportToAnother();
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();

        public List<ReportDCRPlanExecutionBEL> GetMainGrid(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            //string qry = @"SELECT  TSM_ID, SET_DATE, DOCTOR_ID, DOCTOR_NAME, DEGREES, MPO_COLLEAGUE, REMARK, DCR_TITLE, MARKET_NAME, WP_SELECTED, WP_SAMPLE, WP_GIFT, DCR_SELECTED, DCR_SAMPLE, DCR_GIFT, SHIFT_NAME FROM TEST_PLAN_VS_EXEC                      
            //            WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'";
            string qry = @" SELECT TSM_ID, TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES,MPO_COLLEAGUE,REMARK,DCR_TITLE, MARKET_NAME,
                          LISTAGG(WP_SELECTED, ', ') WITHIN GROUP(ORDER BY WP_SELECTED) WP_SELECTED,
                          LISTAGG(WP_SAMPLE, ', ') WITHIN GROUP(ORDER BY WP_SAMPLE) WP_SAMPLE,
                          LISTAGG(WP_GIFT, ', ') WITHIN GROUP(ORDER BY WP_GIFT) WP_GIFT,
                          LISTAGG(DCR_SELECTED, ', ') WITHIN GROUP(ORDER BY DCR_SELECTED) DCR_SELECTED,
                          LISTAGG(DCR_SAMPLE, ', ') WITHIN GROUP(ORDER BY DCR_SAMPLE) DCR_SAMPLE,
                          LISTAGG(DCR_GIFT, ', ') WITHIN GROUP(ORDER BY DCR_GIFT) DCR_GIFT,                  
                          SHIFT_NAME
               
            FROM VW_DCR_PLAN_VS_EXE  WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'";

            // FROM    MVN_VW_DCR_PLAN_VS_EXE  WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'";
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }

            if (model.MPGroup != null && model.MPGroup != "")
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }

            qry = qry + " GROUP BY TSM_ID,SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES,SHIFT_NAME, MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME";
            qry = qry + " ORDER BY MARKET_NAME, SET_DATE,SHIFT_NAME";


           
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDCRPlanExecutionBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRPlanExecutionBEL
                    {
                        SL = row["Col1"].ToString(),     
                        Date = row["SET_DATE"].ToString(), 
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        PlanSelected = row["WP_SELECTED"].ToString(),
                        PlanSample = row["WP_SAMPLE"].ToString(),
                        PlanGift = row["WP_GIFT"].ToString(),
                        DCRSelected = row["DCR_SELECTED"].ToString(),
                        DCRSample = row["DCR_SAMPLE"].ToString(),
                        DCRGift = row["DCR_GIFT"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        Accompany = row["MPO_COLLEAGUE"].ToString(),
                        Remarks = row["REMARK"].ToString(),
                        DcrType = row["DCR_TITLE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),                  

                    }).ToList();
            return item;
        }

         
        public Tuple<string, DataTable, List<ReportDCRPlanExecutionBEL>> Export(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string vHeader = "";

            // string qry2 = "Select * from TEMP_DCR31";

            string qry =" SELECT TSM_ID, TO_CHAR(SET_DATE,'dd/mm/yyyy') SET_DATE,DOCTOR_ID,DOCTOR_NAME,DEGREES,MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME, " +
                        " LISTAGG(WP_SELECTED, ', ') WITHIN GROUP(ORDER BY WP_SELECTED) WP_SELECTED," +
                        " LISTAGG(WP_SAMPLE, ', ') WITHIN GROUP(ORDER BY WP_SAMPLE) WP_SAMPLE," +
                        " LISTAGG(WP_GIFT, ', ') WITHIN GROUP(ORDER BY WP_GIFT) WP_GIFT," +
                        " LISTAGG(DCR_SELECTED, ', ') WITHIN GROUP(ORDER BY DCR_SELECTED) DCR_SELECTED," +
                        " LISTAGG(DCR_SAMPLE, ', ') WITHIN GROUP(ORDER BY DCR_SAMPLE) DCR_SAMPLE," +
                        " LISTAGG(DCR_GIFT, ', ') WITHIN GROUP(ORDER BY DCR_GIFT) DCR_GIFT," +
                        " SHIFT_NAME " +
                        " FROM VW_DCR_PLAN_VS_EXE WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'";
            
            vHeader = vHeader + "Date Between: " + model.FromDate.Trim() + " to " + model.ToDate.Trim();       
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF: " + model.MPOName.Trim();
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory: " + lastItem.Trim();
                }
                qry += " And TERRITORY_CODE= '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region: " + lastItem.Trim();
                }
                qry += " And REGION_CODE= '" + model.RegionCode + "'";
            }

            qry = qry + " GROUP BY TSM_ID,SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES, SHIFT_NAME, MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME";
            qry = qry + " ORDER BY MARKET_NAME, SET_DATE,SHIFT_NAME";
            

            
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);


            List<ReportDCRPlanExecutionBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRPlanExecutionBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        Date = row["SET_DATE"].ToString(),               
                
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        PlanSelected = row["WP_SELECTED"].ToString(),
                        PlanSample = row["WP_SAMPLE"].ToString(),
                        PlanGift = row["WP_GIFT"].ToString(),
                        DCRSelected = row["DCR_SELECTED"].ToString(),
                        DCRSample = row["DCR_SAMPLE"].ToString(),
                        DCRGift = row["DCR_GIFT"].ToString(),
                   
                        Accompany = row["MPO_COLLEAGUE"].ToString(),
                        Remarks = row["REMARK"].ToString(),
                        DcrType = row["DCR_TITLE"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);          
        }


        public List<ReportDCRPlanExecutionBEL> GetMainGridArchive(DefaultParameterBEO model)
        {
            string qry = " SELECT TSM_ID, TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES,MPO_COLLEAGUE,REMARK,DCR_TITLE, MARKET_NAME," +
                         " LISTAGG(WP_SELECTED, ', ') WITHIN GROUP(ORDER BY WP_SELECTED) WP_SELECTED," +
                         " LISTAGG(WP_SAMPLE, ', ') WITHIN GROUP(ORDER BY WP_SAMPLE) WP_SAMPLE," +
                         " LISTAGG(WP_GIFT, ', ') WITHIN GROUP(ORDER BY WP_GIFT) WP_GIFT," +
                         " LISTAGG(DCR_SELECTED, ', ') WITHIN GROUP(ORDER BY DCR_SELECTED) DCR_SELECTED," +
                         " LISTAGG(DCR_SAMPLE, ', ') WITHIN GROUP(ORDER BY DCR_SAMPLE) DCR_SAMPLE," +
                         " LISTAGG(DCR_GIFT, ', ') WITHIN GROUP(ORDER BY DCR_GIFT) DCR_GIFT," +
                         " SHIFT_NAME" +
                         " FROM  VW_ARC_DCR_PLAN_VS_EXE  WHERE TO_CHAR(SET_DATE,'MMYYYY')='"+ model.MonthNumber+model.Year+"'";

            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != null && model.MPGroup != "")
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            qry = qry + " GROUP BY TSM_ID,SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES,SHIFT_NAME, MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME";
            qry = qry + " ORDER BY MARKET_NAME, SET_DATE,SHIFT_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDCRPlanExecutionBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRPlanExecutionBEL
                    {
                        SL = row["Col1"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        PlanSelected = row["WP_SELECTED"].ToString(),
                        PlanSample = row["WP_SAMPLE"].ToString(),
                        PlanGift = row["WP_GIFT"].ToString(),
                        DCRSelected = row["DCR_SELECTED"].ToString(),
                        DCRSample = row["DCR_SAMPLE"].ToString(),
                        DCRGift = row["DCR_GIFT"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        Accompany = row["MPO_COLLEAGUE"].ToString(),
                        Remarks = row["REMARK"].ToString(),
                        DcrType = row["DCR_TITLE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                    }).ToList();
            return item;
        }


        public Tuple<string, DataTable, List<ReportDCRPlanExecutionBEL>> ExportArchive(DefaultParameterBEO model)
        {
            string vHeader = "";
            string qry = " SELECT TSM_ID, TO_CHAR(SET_DATE,'dd/mm/yyyy') SET_DATE,DOCTOR_ID,DOCTOR_NAME,DEGREES,MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME, " +
                            " LISTAGG(WP_SELECTED, ', ') WITHIN GROUP(ORDER BY WP_SELECTED) WP_SELECTED," +
                            " LISTAGG(WP_SAMPLE, ', ') WITHIN GROUP(ORDER BY WP_SAMPLE) WP_SAMPLE," +
                            " LISTAGG(WP_GIFT, ', ') WITHIN GROUP(ORDER BY WP_GIFT) WP_GIFT," +
                            " LISTAGG(DCR_SELECTED, ', ') WITHIN GROUP(ORDER BY DCR_SELECTED) DCR_SELECTED," +
                            " LISTAGG(DCR_SAMPLE, ', ') WITHIN GROUP(ORDER BY DCR_SAMPLE) DCR_SAMPLE," +
                            " LISTAGG(DCR_GIFT, ', ') WITHIN GROUP(ORDER BY DCR_GIFT) DCR_GIFT," +
                            " SHIFT_NAME" +
                            " FROM  VW_ARC_DCR_PLAN_VS_EXE  WHERE TO_CHAR(SET_DATE,'MMYYYY')='" + model.MonthNumber + model.Year + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF: " + model.MPOName.Trim();
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory: " + lastItem.Trim();
                }
                qry += " And TERRITORY_CODE= '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region: " + lastItem.Trim();
                }
                qry += " And REGION_CODE= '" + model.RegionCode + "'";
            }
            qry = qry + " GROUP BY TSM_ID,SET_DATE,DOCTOR_ID, DOCTOR_NAME,DEGREES, SHIFT_NAME, MPO_COLLEAGUE,REMARK,DCR_TITLE,MARKET_NAME";
            qry = qry + " ORDER BY MARKET_NAME, SET_DATE,SHIFT_NAME";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDCRPlanExecutionBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRPlanExecutionBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        Date = row["SET_DATE"].ToString(),

                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        PlanSelected = row["WP_SELECTED"].ToString(),
                        PlanSample = row["WP_SAMPLE"].ToString(),
                        PlanGift = row["WP_GIFT"].ToString(),
                        DCRSelected = row["DCR_SELECTED"].ToString(),
                        DCRSample = row["DCR_SAMPLE"].ToString(),
                        DCRGift = row["DCR_GIFT"].ToString(),

                        Accompany = row["MPO_COLLEAGUE"].ToString(),
                        Remarks = row["REMARK"].ToString(),
                        DcrType = row["DCR_TITLE"].ToString(),


                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

    }
}
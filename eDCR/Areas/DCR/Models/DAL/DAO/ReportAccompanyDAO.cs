using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System.Globalization;
using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportAccompanyDAO
    {
        DBHelper dbHelper=new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DateFormat dateFormat = new DateFormat();


        public List<ReportAccompanyBEL> GetDataMainView(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

          
            string vWhereMpoApp = "";
            string vWhereTmRsmApp = "";
           

         //string qry= " SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE, " +
         //           " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) MORNING,NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) EVENING," +
         //           " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) + NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) TOTAL_DCR" +
         //           " FROM(SELECT DISTINCT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, SET_DATE," +
         //           " SHIFT_NAME, DOCTOR_ID, regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) MPO_COLLEAGUE" +
         //           " FROM(SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, MPO_COLLEAGUE, SET_DATE, SHIFT_NAME, DOCTOR_ID " +
         //           " FROM VW_DCR_ACCOMPANY " +
         //           " Where   SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";

            string qry = " SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE, " +
                " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) MORNING,NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) EVENING," +
                " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) + NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) TOTAL_DCR" +
                " FROM(SELECT DISTINCT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, SET_DATE," +
                " SHIFT_NAME, DOCTOR_ID" +
                " FROM(SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, MPO_COLLEAGUE, SET_DATE, SHIFT_NAME, DOCTOR_ID " +
                " FROM VW_DCR_ACCOMPANY " +
                " Where   SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";

            if (model.Designation == null || model.LocCode == null)
            {
                qry += " AND MPO_COLLEAGUE LIKE '%null%' )";
            }
            else
            {
                if (model.LocCode != "" && model.LocCode != null)
                {
                    vWhereMpoApp = " AND MPO_COLLEAGUE LIKE '%" + model.LocCode + "%' )";
                    vWhereTmRsmApp = " AND MPO_COLLEAGUE LIKE '%" + model.LocName + "%' )";
                }
            }

            //string qryMpo = qry + vWhereMpoApp+ "  connect by regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) is not null ) WHERE MPO_COLLEAGUE = '"+ model.LocCode +"'" + 
            // " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";
          

            //string qryTmRsm =  qry + vWhereTmRsmApp + "  connect by regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) is not null ) WHERE MPO_COLLEAGUE = '" + model.LocName + "'" +
            // " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";


            string qryMpo = qry + vWhereMpoApp + "   ) " +
          " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";


            string qryTmRsm = qry + vWhereTmRsmApp + "  )" +
             " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";






            DataTable dtMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qryMpo);
            DataTable dtTmRsm = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qryTmRsm);

            dtMpo.Merge(dtTmRsm);
            DataTable dt = dbHelper.dtIncremented(dtMpo);         
            List<ReportAccompanyBEL> item;          
            item = (from DataRow row in dt.Rows
                    select new ReportAccompanyBEL
                    {
                        SL = row["Col1"].ToString(),
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        Accompany = dt.Rows.Count > 0 ? model.LocCode : "",
                        Morning = row["MORNING"].ToString(),
                        Evening = row["EVENING"].ToString(),
                        Total = row["TOTAL_DCR"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),

                    }).ToList();
           
            return item;
        }


            public Tuple<string, DataTable, List<ReportAccompanyBEL>> Export(DefaultParameterBEO model)
           {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string vHeader = "";
            string vWhereMpoApp = "";
            string vWhereTmRsmApp = "";
      


            //string qry = " SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, TO_CHAR(SET_DATE,'dd/mm/yyyy') SET_DATE, " +
            //           " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) MORNING,NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) EVENING," +
            //           " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) + NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) TOTAL_DCR" +
            //           " FROM(SELECT DISTINCT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, SET_DATE," +
            //           " SHIFT_NAME, DOCTOR_ID, regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) MPO_COLLEAGUE" +
            //           " FROM(SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, MPO_COLLEAGUE, SET_DATE, SHIFT_NAME, DOCTOR_ID " +
            //           " FROM VW_DCR_ACCOMPANY " +
            //      " Where   SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";

            string qry = " SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, TO_CHAR(SET_DATE,'dd/mm/yyyy') SET_DATE, " +
                    " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) MORNING,NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) EVENING," +
                    " NVL(CASE WHEN SHIFT_NAME = 'm' THEN COUNT(DOCTOR_ID) END, 0) + NVL(CASE WHEN SHIFT_NAME = 'e' THEN COUNT(DOCTOR_ID) END, 0) TOTAL_DCR" +
                    " FROM(SELECT DISTINCT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, SET_DATE," +
                    " SHIFT_NAME, DOCTOR_ID" +
                    " FROM(SELECT MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME, MPO_COLLEAGUE, SET_DATE, SHIFT_NAME, DOCTOR_ID " +
                    " FROM VW_DCR_ACCOMPANY " +
               " Where   SET_DATE BETWEEN  '" + model.FromDate + "' AND '" + model.ToDate + "'";


            vHeader = " Date : " + model.FromDate + " To " + model.ToDate;

            if (model.Designation == null || model.LocCode == null)
            {
                qry += " AND MPO_COLLEAGUE LIKE '%null%')";
            }
            else
            {
                if (model.LocCode != "" && model.LocCode != null)
                {
                    vHeader = vHeader + ", Accompany: " + model.LocName;


                    vWhereMpoApp = " AND MPO_COLLEAGUE LIKE '%" + model.LocCode + "%')";
                    vWhereTmRsmApp = " AND MPO_COLLEAGUE LIKE '%" + model.LocName + "%')";
                }
            }

            //string qryMpo = qry + vWhereMpoApp + "  connect by regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) is not null ) WHERE MPO_COLLEAGUE = '" + model.LocCode + "'" +
            // " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";


            //string qryTmRsm = qry + vWhereTmRsmApp + "  connect by regexp_substr(MPO_COLLEAGUE, '[^,]+', 1, level) is not null ) WHERE MPO_COLLEAGUE = '" + model.LocName + "'" +
            // " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";


            string qryMpo = qry + vWhereMpoApp + "   )" +
         " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";


            string qryTmRsm = qry + vWhereTmRsmApp + "  ) " +
             " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME, DESIGNATION,MARKET_CODE, MARKET_NAME, TERRITORY_CODE, TERRITORY_NAME, REGION_CODE, REGION_NAME,SET_DATE, SHIFT_NAME";




            DataTable dtMpo = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qryMpo);
            DataTable dtTmRsm = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qryTmRsm);

             dtMpo.Merge(dtTmRsm);


            DataTable dt = dbHelper.dtIncremented(dtMpo);
            List<ReportAccompanyBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportAccompanyBEL
                    {
                        SL = row["Col1"].ToString(),
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        Accompany = dt.Rows.Count>0 ? model.LocCode : "",
                        Morning = row["MORNING"].ToString(),
                        Evening = row["EVENING"].ToString(),
                        Total = row["TOTAL_DCR"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
           
        }
    }
}
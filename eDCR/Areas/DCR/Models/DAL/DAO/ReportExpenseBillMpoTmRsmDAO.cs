using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Common;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportExpenseBillMpoTmRsmDAO: ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        DateFormat dateFormat = new DateFormat();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

    
        public List<AllowanceNatuare> GetDA(DefaultParameterBEO model)
        {
            model.DayNumber = model.DayNumber.Trim().Length == 1 ? "0" + model.DayNumber.Trim() : model.DayNumber.Trim();
            string inputDate = model.DayNumber + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
    
            string SetDate = dateFormat.StringDateDdMonYYYY(inputDate);         

            string QryE =   " SELECT Round(RATE/ 2,2) RATE,DESIGNATION, TO_Char(EFFECT_FROM, 'dd-mm-yyyy') EFFECT_FROM, SHORT_NAME"+
                            " FROM(SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE, RANK() over(partition by DESIGNATION, SHORT_NAME  Order by EFFECT_FROM DESC) rnk"+
                            " FROM ACC_DA) WHERE rnk = (SELECT MIN(rnk) FROM(SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE,"+
                            " RANK() OVER(PARTITION BY DESIGNATION, SHORT_NAME Order by EFFECT_FROM DESC) rnk"+
                            " FROM ACC_DA) WHERE EFFECT_FROM IN(SELECT DISTINCT MAX(EFFECT_FROM) OVER(PARTITION BY DESIGNATION, SHORT_NAME"+
                            " ORDER BY EFFECT_FROM DESC) FROM ACC_DA WHERE   EFFECT_FROM <= '" + SetDate + "')) AND DESIGNATION = '" + model.Designation + "'" ;


    
            string eAllowenceRate = dbHelper.GetValue(QryE);

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryE);
            List<AllowanceNatuare> item;
            item = (from DataRow row in dt.Rows
                    select new AllowanceNatuare
                    {
                     
                        AllowenceRate = row["RATE"].ToString(),
                        Allowence = row["SHORT_NAME"].ToString(),

                    }).ToList();
            return item;
        }

        public List<AllowanceNatuare> GetDefaultDA(DefaultParameterBEO model)
        {
            model.DayNumber = model.DayNumber.Trim().Length == 1 ? "0" + model.DayNumber.Trim() : model.DayNumber.Trim();
            string inputDate = model.DayNumber + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
            //  string SetDate = dateFormat.StringDateDdMonYYYY(CntDate);
            string SetDate = dateFormat.StringDateDdMonYYYY(inputDate);

            string QryM = " SELECT  Round(RATE/2,2) RATE,DESIGNATION, TO_Char(EFFECT_FROM,'dd-mm-yyyy') EFFECT_FROM, SHORT_NAME FROM (SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE,RANK() over ( partition by DESIGNATION,SHORT_NAME  Order by EFFECT_FROM ) rnk" +
                         " FROM ACC_DA) " +
                         " WHERE rnk = (SELECT Max(rnk) FROM (SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE,RANK() OVER ( PARTITION BY DESIGNATION,SHORT_NAME  " +
                         " Order by EFFECT_FROM ) rnk FROM ACC_DA) WHERE  " +
                         " EFFECT_FROM IN (SELECT DISTINCT MAX(EFFECT_FROM) OVER ( PARTITION BY DESIGNATION,SHORT_NAME  ORDER BY EFFECT_FROM DESC)" +
                         " FROM ACC_DA WHERE  EFFECT_FROM <= '" + SetDate + "')) AND DESIGNATION='" + model.Designation + "' AND SHORT_NAME='" + model.MAllowence + "'";

            string QryE = " SELECT  Round(RATE/2,2) RATE,DESIGNATION, TO_Char(EFFECT_FROM,'dd-mm-yyyy') EFFECT_FROM, SHORT_NAME FROM (SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE,RANK() over ( partition by DESIGNATION,SHORT_NAME  Order by EFFECT_FROM ) rnk" +
                      " FROM ACC_DA) " +
                      " WHERE rnk = (SELECT Max(rnk) FROM (SELECT DESIGNATION, EFFECT_FROM, SHORT_NAME, RATE,RANK() OVER ( PARTITION BY DESIGNATION,SHORT_NAME  " +
                      " Order by EFFECT_FROM ) rnk FROM ACC_DA) WHERE  " +
                      " EFFECT_FROM IN (SELECT DISTINCT MAX(EFFECT_FROM) OVER ( PARTITION BY DESIGNATION,SHORT_NAME  ORDER BY EFFECT_FROM DESC)" +
                      " FROM ACC_DA WHERE  EFFECT_FROM <= '" + SetDate + "')) AND DESIGNATION='" + model.Designation + "' AND SHORT_NAME='" + model.EAllowence + "'";

            string mAllowenceRate = dbHelper.GetValue(QryM);
            string eAllowenceRate = dbHelper.GetValue(QryE);

   
            HttpContext.Current.Session["NDAM"] = mAllowenceRate;
            HttpContext.Current.Session["NDAE"] = eAllowenceRate;



            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryE);
            List<AllowanceNatuare> item;
            item = (from DataRow row in dt.Rows
                    select new AllowanceNatuare
                    {

                        AllowenceRate = row["RATE"].ToString(),
                        Allowence = row["SHORT_NAME"].ToString(),

                    }).ToList();
            return item;
        }
        public List<ReportExpenseBillMpoTmRsmDaTaOthersBEO> GetTA(DefaultParameterBEO model)
        {
            string MPGroup = model.LocCode;
            if (model.Designation=="MPO" || model.Designation == "SMPO")
            {
                string Qry2 = "Select MPO_CODE,MARKET_CODE from VW_HR_LOC_MAPPING Where MP_GROUP='" + model.LocCode + "' ";
                var tuple2 = GetTwoValues(Qry2);
                if (tuple2.Item1)
                {                  
                    model.LocCode = tuple2.Item3;
                }
            }
            model.DayNumber = model.DayNumber.Trim().Length == 1 ? "0" + model.DayNumber.Trim() : model.DayNumber.Trim();
            string inputDate = model.DayNumber + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
     
            string SetDate = dateFormat.StringDateDdMonYYYY(inputDate);
        
            string TA = " Select TO_Char(EFFECT_FROM,'dd-mm-yyyy') EFFECT_FROM,REGION_CODE, REGION_TYPE, REGION_RATE, MILEAGE_LIMIT, MILEAGE_RATE, FUEL_PRICE" +
            " FROM(SELECT EFFECT_FROM, REGION_CODE, REGION_TYPE, REGION_RATE, MILEAGE_LIMIT, MILEAGE_RATE, FUEL_PRICE, RANK() over(partition by REGION_CODE, REGION_TYPE  order by EFFECT_FROM) rnk" +
            " From ACC_TA)WHERE rnk = (SELECT Max(rnk)FROM(SELECT EFFECT_FROM, REGION_CODE, REGION_TYPE, REGION_RATE, MILEAGE_LIMIT, MILEAGE_RATE, FUEL_PRICE, RANK() OVER(PARTITION BY REGION_CODE, REGION_TYPE   Order by EFFECT_FROM) rnk FROM ACC_TA)" +
            " WHERE EFFECT_FROM IN(SELECT DISTINCT MAX(EFFECT_FROM) OVER(PARTITION BY REGION_CODE, REGION_TYPE ORDER BY EFFECT_FROM DESC)" +
            " FROM ACC_TA WHERE  EFFECT_FROM <=  '" + SetDate + "')AND REGION_CODE = '" + model.LocCode + "')  AND REGION_CODE ='" + model.LocCode + "'";
            var tuple5 = GetFiveValues(TA);

            string RegionType = tuple5.Item3;
            string RegionRate = tuple5.Item4;
            string MileageLimit = tuple5.Item5;
            string MileageRate = tuple5.Item6;
            string FuelRate = tuple5.Item7;


            string Qry = "Select MST_SL,TA_AMOUNT,DISTANCE from VW_EXPENSE_BILL_MPO_TM_RSM Where Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and DAY_NUMBER='" + model.DayNumber + "'";
            if (model.Designation == "MPO" || model.Designation == "SMPO")
            {
                Qry= Qry+" and LOC_CODE = '" + MPGroup + "' ";
            }
            else
            {
                Qry = Qry + " and LOC_CODE = '" + model.LocCode + "' ";
            }
               
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportExpenseBillMpoTmRsmDaTaOthersBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportExpenseBillMpoTmRsmDaTaOthersBEO
                    {
                        RegionCode = model.LocCode,
                        RegionType =  RegionType,
                        cmbRegionType= row["DISTANCE"].ToString() == "1" ? "MCCity" : "Mileage",
                        RegionRate = RegionRate,
                        MileageRate = MileageRate,
                        FuelRate    = FuelRate,

                        MasterSL = row["MST_SL"].ToString(),
                        TA = row["TA_AMOUNT"].ToString(),
                        Distance = row["DISTANCE"].ToString(),

                        //MShiftType = row["M_SHIFT_TYPE"].ToString(),
                        //EShiftType = row["E_SHIFT_TYPE"].ToString(),

                    }).ToList();
            return item;
        }
        private Tuple<Boolean, string, string> GetTwoValues(string Qry)
        {
            string GetValue1 = ""; string GetValue2 = ""; bool isTrue = false;

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
                GetValue1 = dt.Rows[0][0].ToString();
                GetValue2 = dt.Rows[0][1].ToString();
            }
            return Tuple.Create(isTrue, GetValue1, GetValue2);
        }
        private Tuple<Boolean, string, string, string, string, string, string> GetFiveValues(string Qry)
        {
            string GetValue1 = ""; string GetValue2 = ""; string GetValue3 = ""; string GetValue4 = ""; string GetValue5 = ""; string GetValue6 = ""; bool isTrue = false;

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
                GetValue1 = dt.Rows[0][0].ToString();
                GetValue2 = dt.Rows[0][2].ToString();
                GetValue3 = dt.Rows[0][3].ToString();
                GetValue4 = dt.Rows[0][4].ToString();
                GetValue5 = dt.Rows[0][5].ToString();
                GetValue6 = dt.Rows[0][6].ToString();
            }
            return Tuple.Create(isTrue, GetValue1, GetValue2, GetValue3, GetValue4, GetValue5, GetValue6);
        }


       
      


    }
}
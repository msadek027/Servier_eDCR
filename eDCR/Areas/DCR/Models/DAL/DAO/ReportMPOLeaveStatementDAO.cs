using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
        
    public class ReportMPOLeaveStatementDAO : ReturnData
    {
        DBHelper dbHelper=new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DateFormat dateFormat = new DateFormat();

 
        public List<ReportMPOLeaveStatementBEL> GetMainData(DefaultParameterBEO model)
        {
            //model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            //model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry = "SELECT  MPO_CODE, MPO_NAME,LOC_CODE,MARKET_NAME,DESIGNATION,TERRITORY_NAME,REGION_NAME,PHONE_NO,SUM(M_LEAVE) M_LEAVE,SUM(E_LEAVE) E_LEAVE, SUM(FULL_LEAVE) FULL_LEAVE" +
                " FROM  VW_TP_LEAVE_MPO_TP_RSM Where YEAR="+ model.Year + " AND MONTH_NUMBER='"+ model.MonthNumber + "'";

          
            if (model.LocCode != "" && model.LocCode != " " && model.LocCode != "null" && model.LocCode != null)
            {
                qry += " AND LOC_CODE='" + model.LocCode + "'";
            }
            else
            {
                if (HttpContext.Current.Session["Designation"].ToString() == "TM")
                {
                    qry = qry + " and TERRITORY_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Designation"].ToString() == "RSM")
                {
                    qry = qry + " and REGION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                if (model.Designation == "MPO")
                {
                    qry += " AND DESIGNATION IN ('MPO','SMPO')";

                }
                else if (model.Designation == "RSM" || model.Designation == "TM")
                {

                    qry += " AND DESIGNATION='" + model.Designation + "'";
                }
            }
                qry += " GROUP BY MPO_CODE, MPO_NAME,LOC_CODE,MARKET_NAME,DESIGNATION,TERRITORY_NAME,REGION_NAME,PHONE_NO ";


        
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportMPOLeaveStatementBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOLeaveStatementBEL
                    {                   
                        LocCode = row["LOC_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeCode = row["MPO_Code"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MobileNo = row["PHONE_NO"].ToString(),
                        MorningLeave = row["M_LEAVE"].ToString(),
                        EveningLeave = row["E_LEAVE"].ToString(),
                        FullLeave = row["FULL_LEAVE"].ToString(),
                    }).ToList();
            return item;
        }

        public List<ReportMPOLeaveStatementBEL> GetLeaveDetailLink(DefaultParameterBEO model)
        {
            
            string qry = " SELECT MPO_CODE, MPO_NAME,LOC_CODE,MARKET_NAME,DESIGNATION,PHONE_NO,To_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,SHIFT_NAME " +
               " FROM  VW_TP_LEAVE_MPO_TP_RSM Where YEAR = "+ model.Year + " AND MONTH_NUMBER = '"+ model.MonthNumber + "' AND LOC_CODE='"+ model.LocCode + "'";




            if (model.ShiftName != "")
            {
                qry += " AND SHIFT_NAME='" + model.ShiftName + "'";
            }

            qry = qry + " Order by SET_DATE";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportMPOLeaveStatementBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOLeaveStatementBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),                  
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeCode = row["MPO_Code"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                      
                        MobileNo = row["PHONE_NO"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),

                    }).ToList();
            return item;
        }

 
        public Tuple<string, DataTable, List<ReportMPOLeaveStatementBEL>> Export(DefaultParameterBEO model)
        {
            //model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            //model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string vHeader = "";

            string qry = "SELECT  MPO_CODE, MPO_NAME,LOC_CODE,MARKET_NAME,DESIGNATION,TERRITORY_NAME,REGION_NAME,PHONE_NO,SUM(M_LEAVE) M_LEAVE,SUM(E_LEAVE) E_LEAVE, SUM(FULL_LEAVE) FULL_LEAVE" +
                " FROM  VW_TP_LEAVE_MPO_TP_RSM Where YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";



      

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            if (model.LocCode != "" && model.LocCode != " " && model.LocCode != "null" && model.LocCode != null)
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                qry += " AND LOC_CODE='" + model.LocCode + "'";
            }
            else
            {
                if (HttpContext.Current.Session["Designation"].ToString() == "TM")
                {
                    qry = qry + " and TERRITORY_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                if (HttpContext.Current.Session["Designation"].ToString() == "RSM")
                {
                    qry = qry + " and REGION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                if (model.Designation == "MPO")
                {
                    qry += " AND DESIGNATION IN ('MPO','SMPO')";
                    vHeader = vHeader + ", Designation : " + model.Designation;

                }
                else if (model.Designation == "RSM" || model.Designation == "TM")
                {
                    vHeader = vHeader + ", Designation : " + model.Designation;
                    qry += " AND DESIGNATION='" + model.Designation + "'";
                }
            }


         

           
            qry += " GROUP BY MPO_CODE, MPO_NAME,LOC_CODE,MARKET_NAME,DESIGNATION,TERRITORY_NAME,REGION_NAME,PHONE_NO ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportMPOLeaveStatementBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOLeaveStatementBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeCode = row["MPO_Code"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MobileNo = row["PHONE_NO"].ToString(),
                        MorningLeave = row["M_LEAVE"].ToString(),
                        EveningLeave = row["E_LEAVE"].ToString(),
                        FullLeave = row["FULL_LEAVE"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
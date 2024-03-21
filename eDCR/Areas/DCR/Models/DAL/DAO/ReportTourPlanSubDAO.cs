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
    public class ReportTourPlanSubDAO: ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        public List<ReportTourPlanSupBEO> GetViewData(DefaultParameterBEO model)
        {
            string Qry = "";


            if (model.Designation=="MIO")
            {
                Qry = "SELECT MP_GROUP LOC_CODE,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
               "  FROM VW_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";               
                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And MP_GROUP = '" + model.LocCode + "'";
                }
            }
            if (model.Designation == "PM" || model.Designation == "ZM" || model.Designation == "RM")
            {
             Qry = " SELECT LOC_CODE,EMPID,EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
                   " FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
        
                if (model.LocCode != "" && model.LocCode != null)
               {
                 Qry += " And LOC_CODE = '" + model.LocCode + "'";
              }
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanSupBEO
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmpCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString()+ row["DAY_NAME"].ToString(),
                    

                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),

                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportTourPlanSupBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = "";
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;     
            if (model.Designation == "MIO")
            {
                Qry = "SELECT MP_GROUP LOC_CODE,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
               "  FROM VW_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And MP_GROUP = '" + model.LocCode + "'";
                }
            }
            if (model.Designation == "PM" || model.Designation == "ZM" || model.Designation == "RM")
            {
                 Qry = "SELECT LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                       "  FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
            }           
            vHeader = vHeader + ", Employee Name: " + model.LocName;
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
          

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanSupBEO
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmpCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString()+ row["DAY_NAME"].ToString(),
                  

                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }















        public List<ReportTourPlanSupBEO> GetViewDataArchive(DefaultParameterBEO model)
        {
            string Qry = "";


            if (model.Designation == "MIO")
            {
                Qry = "SELECT MP_GROUP LOC_CODE,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
               "  FROM VW_ARC_TP Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And MP_GROUP = '" + model.LocCode + "'";
                }
            }
            if (model.Designation == "PM" || model.Designation == "ZM" || model.Designation == "RM")
            {
                Qry = "SELECT LOC_CODE,EMPID,EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
                   "  FROM VW_ARC_SUP_TP Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
            }


            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanSupBEO
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmpCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString() + row["DAY_NAME"].ToString(),


                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),

                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportTourPlanSupBEO>> ExportArchive(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = "";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;


            if (model.Designation == "MIO")
            {
                Qry = "SELECT MP_GROUP LOC_CODE,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
               "  FROM VW_ARC_TP Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And MP_GROUP = '" + model.LocCode + "'";
                }
            }
            if (model.Designation == "PM" || model.Designation == "RM" || model.Designation == "ZM")
            {
                Qry = "SELECT LOC_CODE,EMPID,EMPNAME,Designation,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                      "  FROM VW_ARC_SUP_TP Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";

            vHeader = vHeader + ", Employee Name: " + model.LocName;

        


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportTourPlanSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportTourPlanSupBEO
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmpCode = row["EMPID"].ToString(),
                        DayNumber = row["DAY_NUMBER"].ToString()+ row["DAY_NAME"].ToString(),


                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        MAccompany = row["M_EMP_ID_WK_TYPE"].ToString(),

                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        EAccompany = row["E_EMP_ID_WK_TYPE"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
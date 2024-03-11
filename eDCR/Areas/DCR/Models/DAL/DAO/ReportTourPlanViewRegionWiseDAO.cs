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
    public class ReportTourPlanViewRegionWiseDAO :ReturnData
    {
        DBHelper dbHelper = new DBHelper();
    DBConnection dbConn = new DBConnection();
    string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
    string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);



    public List<ReportTourPlanViewRegionWiseBEO> GetViewData(DefaultParameterBEO model)
    {
        string Qry = "";

        string Str = model.RegionCode.Replace("[", "").Replace("]", "").Replace("\"", "'");
        model.RegionCode = Str;


        if (model.Designation == "MPO")
        {
            Qry = "SELECT MP_GROUP LOC_CODE,MARKET_NAME LOC_NAME,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
           "  FROM VW_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (model.LocCode != "" && model.LocCode != null)
            {
                Qry += " And MP_GROUP = '" + model.LocCode + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null && model.RegionCode != "All")
                {
                    Qry += " And MP_GROUP IN (Select MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE IN(" + model.RegionCode + "))";
            }
            }
        if ( model.Designation == "TM")
        {
            Qry = "SELECT LOC_CODE,LOC_NAME,EMPID,EMPNAME,DAY_NUMBER,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
               "  FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
                Qry = Qry + " And DESIGNATION='" + model.Designation + "'";
                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
                else
                {
                    if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null && model.RegionCode != "All")
                    {
                        Qry += " And LOC_CODE IN (Select TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE IN(" + model.RegionCode + "))";
                    }
                }
            }
            if (model.Designation == "RSM")
            {
                Qry = "SELECT LOC_CODE,LOC_NAME,EMPID,EMPNAME,DAY_NUMBER,' ('|| TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
                   "  FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
                Qry = Qry + " And DESIGNATION='" + model.Designation + "'";
                if (model.LocCode != "" && model.LocCode != null)
                {
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
                else
                {
                    if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null && model.RegionCode != "All")
                    {
                        Qry += " And LOC_CODE  IN(" + model.RegionCode + ")";
                    }
                }
            }


            if (model.DayNumber != "" && model.DayNumber != null)
            {
                Qry += " And DAY_NUMBER = '" + model.DayNumber + "'";
            }

            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
        DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
        List<ReportTourPlanViewRegionWiseBEO> item;

        item = (from DataRow row in dt.Rows
                select new ReportTourPlanViewRegionWiseBEO
                {

                    LocCode = row["LOC_CODE"].ToString(),
                    LocName = row["LOC_NAME"].ToString(),
                    EmployeeCode = row["EMPID"].ToString(),
                    EmployeeName = row["EMPNAME"].ToString(),
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
    public Tuple<string, DataTable, List<ReportTourPlanViewRegionWiseBEO>> Export(DefaultParameterBEO model)
    {
        string vHeader = "";
        string Qry = "";

        string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
        vHeader = vHeader + "Month: " + month + ", " + model.Year;

            string QryRegionCount = "Select Distinct REGION_CODE from VW_HR_LOC_MAPPING";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryRegionCount);
            if (model.RegionCode2 != null && model.RegionCode2 != "" && model.RegionCode2 != " " && model.RegionCode2 != "null")
            {
                string Str = model.RegionCode2.Replace(",", "','");
                model.RegionCode2 = "'" + Str + "'";
                model.RegionCode = model.RegionCode2;

                string[] count = Str.Split(',');

                if(count.Length== dt2.Rows.Count)
                {
                    model.RegionName = "All";
                }
            }
        if (model.Designation == "MPO")
        {
            Qry = "SELECT MP_GROUP LOC_CODE,MARKET_NAME LOC_NAME,MPO_CODE EMPID,MPO_NAME EMPNAME,DAY_NUMBER,' ('|| TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,'' M_EMP_ID_WK_TYPE,'' E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
           "  FROM VW_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (model.LocCode != "" && model.LocCode != null)            {

                Qry += " And MP_GROUP = '" + model.LocCode + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null)
            {
                if (model.RegionName != null && model.RegionName != "")
                {
                    string lastItem = model.RegionName;
                    vHeader = vHeader + ", Region Name: " + lastItem;
                }
                Qry += " And MP_GROUP IN (Select MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE IN(" + model.RegionCode + "))";
            }
        }
        if (model.Designation == "TM")
        {
            Qry = " SELECT LOC_CODE,LOC_NAME,EMPID,EMPNAME,Designation,DAY_NUMBER,' ('|| TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                  " FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
            if (model.LocCode != "" && model.LocCode != null)
            {
                if (model.LocName != null && model.LocName != "")
                {
                    string lastItem = model.LocName;
                    vHeader = vHeader + ", Employee Name: " + lastItem;
                }
                Qry += " And LOC_CODE = '" + model.LocCode + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null)
            {
                if (model.RegionName != null && model.RegionName != "")
                {
                    string lastItem = model.RegionName;
                    vHeader = vHeader + ", Region Name: " + lastItem;
                }
                Qry += " And LOC_CODE IN (Select TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE IN(" + model.RegionCode + "))";
            }
            }
            if (model.Designation == "RSM")
            {
                Qry = "SELECT LOC_CODE,LOC_NAME,EMPID,EMPNAME,Designation,DAY_NUMBER,' ('|| TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_EMP_ID_WK_TYPE,E_EMP_ID_WK_TYPE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                      "  FROM VW_SUP_TP_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' ";
                if (model.LocCode != "" && model.LocCode != null)
                {
                    if (model.LocName != null && model.LocName != "")
                    {
                        string lastItem = model.LocName;
                        vHeader = vHeader + ", Employee Name: " + lastItem;
                    }
                    Qry += " And LOC_CODE = '" + model.LocCode + "'";
                }
                if (model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null" && model.RegionCode != null)
                {
                    if (model.RegionName != null && model.RegionName != "")
                    {
                        string lastItem = model.RegionName;
                        vHeader = vHeader + ", Region Name: " + lastItem;
                    }
                    Qry += " And LOC_CODE IN (" + model.RegionCode + ")";
                }
            }

            if (model.DayNumber != "" && model.DayNumber != null)
            {
                Qry += " And DAY_NUMBER = '" + model.DayNumber + "'";
            }      

        Qry = Qry + " Order by To_Number(DAY_NUMBER)";
        DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
        List<ReportTourPlanViewRegionWiseBEO> item;

        item = (from DataRow row in dt.Rows
                select new ReportTourPlanViewRegionWiseBEO
                {
                    LocCode = row["LOC_CODE"].ToString(),
                    LocName = row["LOC_NAME"].ToString(),
                    EmployeeCode = row["EMPID"].ToString(),
                    EmployeeName = row["EMPNAME"].ToString(),
                    DayNumber = row["DAY_NUMBER"].ToString()+ row["DAY_NAME"].ToString(),
                   
                    MInstName = row["M_INSTI_NAME"].ToString(),
                    MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                    MSetTime = row["M_SET_TIME"].ToString(),
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
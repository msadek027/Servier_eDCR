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
    public class ReportActivityLogDAO
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        DateFormat dateFormat = new DateFormat();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        public List<ReportActivityLogBEO> GetActivityLog(DefaultParameterBEO model)
        {
            string Qry = " Select MP_GROUP, OP_TYPE, TITLE, MESSAGE, TO_CHAR (SET_DATE, 'dd-mm-yyyy HH24:MI:SS') SET_DATE, YEAR, MONTH_NUMBER,TERMINAL, SET_LOC_CODE,MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,SET_BY_ID,SET_BY_NAME " + 
                        " from VW_PUSH_NOTIFICATION Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            if (model.WhoWhom == "Act-by-him")
            {
                if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
                {
                    Qry = Qry + " AND SET_LOC_CODE='" + model.LocCode + "' ";
                }
                else
                {

                    if (model.Designation == "MPO")
                    {
                        Qry = Qry + " AND SET_LOC_CODE IN (SELECT MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "' )";
                    }
                    else if (model.Designation == "TM")
                    {
                        Qry = Qry + " AND SET_LOC_CODE IN (SELECT TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                    }
                    else if (model.Designation == "RSM" || model.Designation == "DSM")
                    {
                        Qry = Qry + " AND  SET_LOC_CODE='" + model.RegionCode + "'";
                    }

                }
            }
            if (model.WhoWhom == "Act-for-him")
            {
                if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
                {
                    Qry = Qry + " AND MP_GROUP='" + model.LocCode + "' ";
                }
                else
                {

                    if (model.Designation == "MPO")
                    {
                        Qry = Qry + " AND MP_GROUP IN (SELECT MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "' )";
                    }
                    else if (model.Designation == "TM")
                    {
                        Qry = Qry + " AND MP_GROUP IN (SELECT TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                    }
                    else if (model.Designation == "RSM")
                    {
                        Qry = Qry + " AND  MP_GROUP='" + model.RegionCode + "'";
                    }

                }
            }



            if (model.Operation == "TP")
            {                
                Qry = Qry + " AND OP_TYPE IN ('TP','Change TP')";
            }
            else if (model.Operation == "DVR")
            {
                Qry = Qry + " AND OP_TYPE='DVR'";
            }
            else if (model.Operation == "PWDS")
            {
                Qry = Qry + " AND OP_TYPE ='PWDS'";
            }
            else if (model.Operation == "GWDS")
            {
                Qry = Qry + " AND OP_TYPE ='GWDS'";
            }
            else if (model.Operation == "Sample")
            {
                Qry = Qry + " AND OP_TYPE IN ('SlR','SlI','SmR','SmI','GtR','GrI',)";
            }
            
            Qry = Qry + " ORDER BY SET_DATE DESC";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportActivityLogBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportActivityLogBEO
                    {
                        SL = row["Col1"].ToString(),
                        LocCode = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),           
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        Operation = row["OP_TYPE"].ToString()+", "+ row["TITLE"].ToString(),
                        Message =  row["MESSAGE"].ToString(),                   
                        Terminal = row["TERMINAL"].ToString(),
                        SetLocCode = row["SET_LOC_CODE"].ToString(),
                        SetByID = row["SET_BY_ID"].ToString(),
                        SetByName = row["SET_BY_NAME"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportActivityLogBEO>> GetExportActivityLog(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MST_SL, MP_GROUP, OP_TYPE, TITLE, MESSAGE, TO_CHAR (SET_DATE, 'dd-mm-yyyy HH24:MI:SS') SET_DATE, YEAR, MONTH_NUMBER,TERMINAL,SET_LOC_CODE,MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,SET_BY_ID,SET_BY_NAME " +
                         " from VW_PUSH_NOTIFICATION Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            if (model.WhoWhom == "Act-by-him")
            {
                if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
                {
                    if (model.LocName != null && model.LocName != "")
                    {
                        string lastItem = model.LocName;
                        vHeader = vHeader + ", Employee Name: " + lastItem;
                    }
                    if (model.RegionName != "" && model.RegionName != null)
                    {
                        string[] Region = model.RegionName.Split(',');
                        string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                        vHeader = vHeader + ", Region : " + lastItem;
                    }
                    Qry = Qry + " AND SET_LOC_CODE='" + model.LocCode + "' ";


                }
                else
                {
                    if (model.Designation == "MPO")
                    {
                        Qry = Qry + "  AND SET_LOC_CODE IN ( SELECT MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                    }
                    else if (model.Designation == "TM")
                    {
                        Qry = Qry + " AND SET_LOC_CODE IN ( SELECT TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";

                    }
                    else if (model.Designation == "RSM")
                    {
                        Qry = Qry + " AND SET_LOC_CODE='" + model.RegionCode + "'";
                    }

                    if (model.RegionName != "" && model.RegionName != null)
                    {
                        string[] Region = model.RegionName.Split(',');
                        string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                        vHeader = vHeader + ", Region : " + lastItem;
                    }
                    vHeader = vHeader + ", Designation : " + model.Designation;
                }
            }
            if (model.WhoWhom == "Act-for-him")
            {
                if (model.LocCode != "" && model.LocCode != null && model.LocCode != " ")
                {
                    if (model.LocName != null && model.LocName != "")
                    {
                        string lastItem = model.LocName;
                        vHeader = vHeader + ", Employee Name: " + lastItem;
                    }
                    if (model.RegionName != "" && model.RegionName != null)
                    {
                        string[] Region = model.RegionName.Split(',');
                        string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                        vHeader = vHeader + ", Region : " + lastItem;
                    }
                    Qry = Qry + " AND MP_GROUP='" + model.LocCode + "' ";
                }
                else
                {
                    if (model.Designation == "MPO")
                    {
                        Qry = Qry + "  AND MP_GROUP IN ( SELECT MP_GROUP from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";
                    }
                    else if (model.Designation == "TM")
                    {
                        Qry = Qry + " AND MP_GROUP IN ( SELECT TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE = '" + model.RegionCode + "')";

                    }
                    else if (model.Designation == "RSM")
                    {
                        Qry = Qry + " AND MP_GROUP='" + model.RegionCode + "'";
                    }

                    if (model.RegionName != "" && model.RegionName != null)
                    {
                        string[] Region = model.RegionName.Split(',');
                        string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                        vHeader = vHeader + ", Region : " + lastItem;
                    }
                    vHeader = vHeader + ", Designation : " + model.Designation;
                }
            }
            if (model.Operation == "TP")
            {
                Qry = Qry + " AND OP_TYPE IN ('TP','Change TP')";
            }
            else if (model.Operation == "DVR")
            {
                Qry = Qry + " AND OP_TYPE='DVR'";
            }
            else if (model.Operation == "PWDS")
            {
                Qry = Qry + " AND OP_TYPE ='PWDS'";
            }
            else if (model.Operation == "GWDS")
            {
                Qry = Qry + " AND OP_TYPE ='GWDS'";
            }
            else if (model.Operation == "Sample")
            {
                Qry = Qry + " AND OP_TYPE IN ('SlR','SlI','SmR','SmI','GtR','GrI',)";
            }

            Qry = Qry + " ORDER BY SET_DATE DESC";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportActivityLogBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportActivityLogBEO
                    {
                        SL = row["Col1"].ToString(),
                        LocCode = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        Operation = row["OP_TYPE"].ToString() + ", " + row["TITLE"].ToString(),
                        Message = row["MESSAGE"].ToString(),
                        Terminal = row["TERMINAL"].ToString(),
                        SetLocCode = row["SET_LOC_CODE"].ToString(),
                        SetByID = row["SET_BY_ID"].ToString(),
                        SetByName = row["SET_BY_NAME"].ToString(),
                    }).ToList();
       
            return Tuple.Create(vHeader, dt, item);
        }
       
    }
}
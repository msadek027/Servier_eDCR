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
    public class ReportSupervisorDCRDAO : ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportSupervisorDCRBEO> GetViewData(DefaultParameterBEO model)
        {
            string Qry = "SELECT  distinct TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE, LOC_CODE,MARKET_NAME, DESIGNATION,DOCTOR_ID,SHIFT_NAME,CALL_TYPE,ACCOMPANY_STR,TP_FOLLOWED,REMARKS,DCR_TYPE,DCR_SUB_TYPE,KEY_DOCTOR_NAME,DEGREE,NO_OF_INTERN,ADDRESS_WORD " +
         "  FROM VW_SUP_DCR Where SET_DATE BETWEEN TO_DATE('" + model.FromDate + "','dd-mm-yyyy') AND TO_DATE('" + model.ToDate + "','dd-mm-yyyy')  ";

            if (model.Designation == "" || model.Designation == " " || model.Designation == null)
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry += " And LOC_CODE IN (Select TERRITORY_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "' UNION  Select REGION_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode+"')";
                }
            }
            else
            {
                if (model.Designation == "RM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";

                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";
                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            Qry += " And LOC_CODE IN (Select TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "')";
                        }
                    }
                }

                if (model.Designation == "ZM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";
                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            Qry += " And LOC_CODE ='" + model.RegionCode + "'";
                        }
                    }
                }
                if (model.Designation == "PM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";
                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            Qry += " And LOC_CODE ='" + model.RegionCode + "'";
                        }
                    }
                }

            }
            Qry = Qry + " ORDER BY MARKET_NAME,TO_DATE(SET_DATE ,'dd-mm-yyyy')";
       

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportSupervisorDCRBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSupervisorDCRBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["KEY_DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREE"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        CallType = row["CALL_TYPE"].ToString(),
                        Accompany = row["ACCOMPANY_STR"].ToString(),
                        LocationFollowed = row["TP_FOLLOWED"].ToString(),                      
                        Remarks = row["REMARKS"].ToString(),
                        DCRType = row["DCR_TYPE"].ToString(),
                        DCRSubType = row["DCR_SUB_TYPE"].ToString(),
                        AddressWord = row["ADDRESS_WORD"].ToString(),
                        NoOfInterns = row["NO_OF_INTERN"].ToString(),
                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportSupervisorDCRBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";

            string Qry = "SELECT  distinct TRUNC(SET_DATE) SET_DATE, LOC_CODE,MARKET_NAME, DESIGNATION,DOCTOR_ID,SHIFT_NAME,CALL_TYPE,ACCOMPANY_STR,TP_FOLLOWED,REMARKS,DCR_TYPE,DCR_SUB_TYPE,KEY_DOCTOR_NAME,DEGREE,NO_OF_INTERN,ADDRESS_WORD " +
             "  FROM VW_SUP_DCR Where SET_DATE BETWEEN TO_DATE('" + model.FromDate + "','dd-mm-yyyy') AND TO_DATE('" + model.ToDate + "','dd-mm-yyyy')  ";

            vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;


            if (model.Designation == "" || model.Designation == " " || model.Designation == null)
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry += " And LOC_CODE IN ( Select TERRITORY_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "' UNION  Select REGION_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "')";
                }
            }
            else
            {
                if (model.Designation == "RM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";

                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        if (model.LocName != null && model.LocName != "")
                        {
                            string lastItem = model.LocName;
                            vHeader = vHeader + ", Employee: " + lastItem;
                        }
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";

                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            if (model.RegionName != null && model.RegionName != "")
                            {
                                string lastItem = model.RegionName;
                                vHeader = vHeader + ", Region: " + lastItem;
                            }
                            Qry += " And LOC_CODE IN (Select TERRITORY_CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "')";

                        }
                    }
                }

                if (model.Designation == "ZM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        if (model.LocName != null && model.LocName != "")
                        {
                            string lastItem = model.LocName;
                            vHeader = vHeader + ", Employee Name: " + lastItem;
                        }
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";

                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            if (model.RegionName != null && model.RegionName != "")
                            {
                                string lastItem = model.RegionName;
                                vHeader = vHeader + ", Region: " + lastItem;
                            }
                            Qry += " And LOC_CODE ='" + model.RegionCode + "'";
                        }
                    }
                }
                if (model.Designation == "PM")
                {
                    Qry = Qry + " and DESIGNATION='" + model.Designation + "'";
                    if (model.LocCode != "" && model.LocCode != null)
                    {
                        Qry = Qry + " and LOC_CODE='" + model.LocCode + "'";
                    }
                    else
                    {
                        if (model.RegionCode != "" && model.RegionCode != null)
                        {
                            Qry += " And LOC_CODE ='" + model.RegionCode + "'";
                        }
                    }
                }
            }

            Qry = Qry + " ORDER BY MARKET_NAME,TRUNC(SET_DATE)";


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportSupervisorDCRBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportSupervisorDCRBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        Date = row["SET_DATE"].ToString() ,
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["KEY_DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREE"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),                      
                        CallType = row["CALL_TYPE"].ToString(),
                        Accompany = row["ACCOMPANY_STR"].ToString(),
                        LocationFollowed = row["TP_FOLLOWED"].ToString(),
                        Remarks = row["REMARKS"].ToString(),

                        DCRType = row["DCR_TYPE"].ToString(),
                        DCRSubType = row["DCR_SUB_TYPE"].ToString(),
                        AddressWord = row["ADDRESS_WORD"].ToString(),
                        NoOfInterns = row["NO_OF_INTERN"].ToString(),

              



                    }).ToList();
        

            return Tuple.Create(vHeader, dt, item);
        }

    }
}
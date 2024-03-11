using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.DAO;
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
    public class ReportFortnightMonitoringRemarksSupDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();

        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);


        public List<ReportFortnightMonitoringRemarksSupBEO> GetViewData(DefaultParameterBEO model)
        {
            string Qry = "SELECT DAY_NUMBER, MONTH_NUMBER,MONTH_NAME, YEAR,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME, LOC_CODE,LOC_NAME, DESIGNATION,INST_NAME,REVIEW, ALLOWANCE_NATURE, EMP_ID_WK_TYPE, SHIFT_NAME, M_SINGLE, E_SINGLE, M_DOUBLE, E_DOUBLE, CHEMIST_COUNT  " +
                  "  FROM VW_SUP_FORTNIGHT_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and LOC_CODE = '" + model.LocCode + "'";
            if (model.FortNightType == "First")
            {
                Qry = Qry + " and To_Number(DAY_NUMBER) Between 1 and 15";
            }
            if (model.FortNightType == "Second")
            {
                Qry = Qry + " and To_Number(DAY_NUMBER) Between 16 and 31";
            }           
            if (model.FortNightType == "Full")
            {
                Qry = Qry + " and To_Number(DAY_NUMBER) Between 1 and 31";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightMonitoringRemarksSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportFortnightMonitoringRemarksSupBEO
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["LOC_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),

                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),

                        SetDate = row["DAY_NUMBER"].ToString() + "-" + row["MONTH_NUMBER"].ToString() + "-" + row["YEAR"].ToString() + row["DAY_NAME"].ToString(),
                        LocName = row["LOC_NAME"].ToString(),
                        InstName = row["INST_NAME"].ToString(),
                        AllownceNature = row["ALLOWANCE_NATURE"].ToString(),
                        Review = row["REVIEW"].ToString() == "No" ? "Yes" : "No",
                        ColleagueName = row["EMP_ID_WK_TYPE"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MDouble = row["M_DOUBLE"].ToString(),
                        EDouble = row["E_DOUBLE"].ToString(),
                        MSingle = row["M_SINGLE"].ToString(),
                        ESingle = row["E_SINGLE"].ToString(),
                        ChemistCount = row["CHEMIST_COUNT"].ToString(),


                    }).ToList();
            return item;
        }
        public List<ReportFortnightMonitoring> GetViewDataMonitoring(DefaultParameterBEO model)
        {

            string Qry =" SELECT MPO_CODE, MPO_NAME,DESIGNATION, LOC_CODE,MARKET_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,PAPER_AUDIT,PROBLEMS_IDENTIFIED,REMARKS FROM VW_SUP_FORTNIGHT_MONITORING ";
            Qry = Qry + " Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' and SET_LOC_CODE = '" + model.LocCode + "'";
            Qry = Qry + " Order by SET_DATE,MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightMonitoring> item;

            item = (from DataRow row in dt.Rows
                    select new ReportFortnightMonitoring
                    {
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["MARKET_NAME"].ToString(),

                        SetDate = row["SET_DATE"].ToString(),
                        PaperAudit = row["PAPER_AUDIT"].ToString(),
                        ProblemsIdentifed = row["PROBLEMS_IDENTIFIED"].ToString(),
                        Remarks = row["REMARKS"].ToString()
                    }).ToList();
            return item;
        }
        public List<ReportFortnightOverallPer> GetViewDataOverallPer(DefaultParameterBEO model)
        {

            string Qry =" SELECT MPO_CODE, MPO_NAME,DESIGNATION, LOC_CODE,MARKET_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,SALES_ACHI,MARKET_SHARE,LEADERSHIP,REMARKS FROM VW_SUP_FORTNIGHT_OVERALL ";
            Qry = Qry + " Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' and SET_LOC_CODE = '" + model.LocCode + "'";
            Qry = Qry + " Order by SET_DATE,MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightOverallPer> item;

            item = (from DataRow row in dt.Rows
                    select new ReportFortnightOverallPer
                    {
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation= row["DESIGNATION"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["MARKET_NAME"].ToString(),

                        SetDate = row["SET_DATE"].ToString(),
                        SalesAchi = row["SALES_ACHI"].ToString(),
                        MarketShare = row["MARKET_SHARE"].ToString(),
                        Leadership = row["LEADERSHIP"].ToString(),
                        Remarks = row["REMARKS"].ToString()

                    }).ToList();
            return item;
        }
        public List<ReportFortnightHistory> GetViewDataHistory(DefaultParameterBEO model)
        {

            string Qry =" SELECT MPO_CODE, MPO_NAME,DESIGNATION, LOC_CODE,MARKET_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,PARA_TYPE,REMARKS " +
                        " FROM VW_SUP_FORTNIGHT_HISTORY ";
            Qry = Qry + " Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' and SET_LOC_CODE = '" + model.LocCode + "'";
            Qry = Qry + " Order by SET_DATE,MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightHistory> item = new List<ReportFortnightHistory>();

            if (model.Designation == "TM")
            {
                item = (from DataRow row in dt.Rows
                        select new ReportFortnightHistory
                        {

                            EmployeeCode = row["MPO_CODE"].ToString(),
                            EmployeeName = row["MPO_NAME"].ToString(),
                            Designation = row["DESIGNATION"].ToString(),
                            LocCode = row["LOC_CODE"].ToString(),
                            LocName = row["MARKET_NAME"].ToString(),
                            SetDate = row["SET_DATE"].ToString(),
                            ParaType = row["PARA_TYPE"].ToString(),
                            Remarks = row["REMARKS"].ToString()

                        }).ToList();
            }
            if (model.Designation == "RSM" || model.Designation == "DSM")
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable TempTable = new DataView(dt).ToTable(true, "PARA_TYPE", "REMARKS");
                    item = (from DataRow row in TempTable.Rows
                            select new ReportFortnightHistory
                            {
                                ParaType = row["PARA_TYPE"].ToString(),
                                Remarks = row["REMARKS"].ToString()

                            }).ToList();
                }
                else
                {
                    item = (from DataRow row in dt.Rows
                            select new ReportFortnightHistory
                            {
                                ParaType = row["PARA_TYPE"].ToString(),
                                Remarks = row["REMARKS"].ToString()

                            }).ToList();
                }
            }

            return item;
        }
        public DataTable dtGetViewDataHistory(DefaultParameterBEO model)
        {

            string Qry = "SELECT MPO_CODE, MPO_NAME,DESIGNATION, LOC_CODE,MARKET_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,PARA_TYPE,REMARKS " +
                  "  FROM VW_SUP_FORTNIGHT_HISTORY ";
            Qry = Qry + "  Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' and SET_LOC_CODE = '" + model.LocCode + "'";
            Qry = Qry + " Order by SET_DATE,MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);       
            return dt;
        }

        public List<ReportFortnightHistory> GetViewDataHistory(DataTable dtAll,string ParaType,string Designation)
        {
            DataTable dt = new DataTable();
            List<ReportFortnightHistory> item = new List<ReportFortnightHistory>();
            dt = dtAll;
            if (dtAll.Rows.Count > 0)
            {
                if (Designation == "TM")
                {
                    DataTable viewSubDTL = new DataView(dtAll).ToTable(true, "MPO_CODE", "MPO_NAME", "DESIGNATION", "LOC_CODE", "MARKET_NAME", "SET_DATE", "PARA_TYPE", "REMARKS");
                    DataRow[] drArraySubDTL = viewSubDTL.Select("PARA_TYPE = '" + ParaType + "'");

                    dt = viewSubDTL.Clone();
                    foreach (DataRow row in drArraySubDTL)
                    {
                        dt.ImportRow(row);
                    }

                    item = (from DataRow row in dt.Rows
                            select new ReportFortnightHistory
                            {

                                EmployeeCode = row["MPO_CODE"].ToString(),
                                EmployeeName = row["MPO_NAME"].ToString(),
                                Designation = row["DESIGNATION"].ToString(),
                                LocCode = row["LOC_CODE"].ToString(),
                                LocName = row["MARKET_NAME"].ToString(),

                                SetDate = row["SET_DATE"].ToString(),
                                ParaType = row["PARA_TYPE"].ToString(),
                                Remarks = row["REMARKS"].ToString()

                            }).ToList();
                }
                if (Designation == "RSM" || Designation == "DSM")
                {
                    DataTable viewSubDTL = new DataView(dtAll).ToTable(true, "PARA_TYPE", "REMARKS");
                    DataRow[] drArraySubDTL = viewSubDTL.Select("PARA_TYPE = '" + ParaType + "'");

                    dt = viewSubDTL.Clone();
                    foreach (DataRow row in drArraySubDTL)
                    {
                        dt.ImportRow(row);
                    }

                    item = (from DataRow row in dt.Rows
                            select new ReportFortnightHistory
                            {                              
                                ParaType = row["PARA_TYPE"].ToString(),
                                Remarks = row["REMARKS"].ToString()

                            }).ToList();
                }
            }
           
           
            return item;
        }


        public List<ReportFortnightSupervisorComments> GetViewDataSupervisorComments(DefaultParameterBEO model)
        {

            string Qry = " SELECT MPO_CODE, MPO_NAME,DESIGNATION, LOC_CODE,MARKET_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,REMARKS,MANAGER_REMARKS " +
                         " FROM VW_SUP_FORTNIGHT_RSM_COMMENTS ";
            Qry = Qry + "  Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' and LOC_CODE = '" + model.LocCode + "'";
            Qry = Qry + " Order by SET_DATE,MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightSupervisorComments> item;

            item = (from DataRow row in dt.Rows
                    select new ReportFortnightSupervisorComments
                    {
                        EmployeeCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["MARKET_NAME"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        Remarks = row["REMARKS"].ToString(),
                        ManagerRemarks = row["MANAGER_REMARKS"].ToString()

                    }).ToList();
            return item;
        }

      

        public Tuple<string, DataTable, List<ReportFortnightMonitoringRemarksSupBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";


            string Qry = "SELECT DAY_NUMBER, MONTH_NUMBER,MONTH_NAME, YEAR,' ('||TO_CHAR(TO_DATE(DAY_NUMBER||MONTH_NUMBER || YEAR, 'DD/MM/YYYY') , 'fmDay')||')' DAY_NAME, LOC_CODE,LOC_NAME, DESIGNATION,INST_NAME,REVIEW, ALLOWANCE_NATURE, EMP_ID_WK_TYPE, SHIFT_NAME, M_SINGLE, E_SINGLE, M_DOUBLE, E_DOUBLE, CHEMIST_COUNT  " +
                    "  FROM VW_SUP_FORTNIGHT_RPT Where YEAR=" + model.Year + " And MONTH_NUMBER='" + model.MonthNumber + "' and LOC_CODE = '" + model.LocCode + "'";
            if (model.FortNightType == "First")
            {
                Qry = Qry + " and  To_Number(DAY_NUMBER) Between 1 and 15";
            }
            if (model.FortNightType == "Second")
            {
                Qry = Qry + " and  To_Number(DAY_NUMBER) Between 16 and 31";
            }
            if (model.FortNightType == "Full")
            {
                Qry = Qry + " and  To_Number(DAY_NUMBER) Between 1 and 31";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            vHeader = vHeader + ", Fortnight: " + model.FortNightType;
            vHeader = vHeader + ", Employee Name: " + model.LocName;

            if (model.RegionName != null && model.RegionName != "")
            {
                string lastItem = model.RegionName;
                vHeader = vHeader + ", Region: " + lastItem;
            }

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportFortnightMonitoringRemarksSupBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportFortnightMonitoringRemarksSupBEO
                    {

                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["LOC_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),

                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),


                        SetDate = row["DAY_NUMBER"].ToString() + "/" + row["MONTH_NUMBER"].ToString() + "/" + row["YEAR"].ToString() + row["DAY_NAME"].ToString(),
                        LocName = row["LOC_NAME"].ToString(),
                        InstName = row["INST_NAME"].ToString(),
                        AllownceNature = row["ALLOWANCE_NATURE"].ToString(),
                        Review = row["REVIEW"].ToString() == "No" ? "Yes" : row["REVIEW"].ToString(),
                        ColleagueName = row["EMP_ID_WK_TYPE"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MDouble = row["M_DOUBLE"].ToString(),
                        EDouble = row["E_DOUBLE"].ToString(),
                        MSingle = row["M_SINGLE"].ToString(),
                        ESingle = row["E_SINGLE"].ToString(),
                        ChemistCount = row["CHEMIST_COUNT"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }


        public bool SaveUpdate(ReportFortnightMonitoringRemarksSupBEO model)
        {
            string QryType = "Select LOC_CODE from  SA_EMPLOYEE_VW Where EMPID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";
            string UserID = dbHelper.GetValue(QryType);

            string DesignationSession = HttpContext.Current.Session["Designation"].ToString();


            bool isTrue = false;
            string Qry = "";
            string QryIsExists = "Select * from SUP_FORTNIGHT_RSM_COMMENTS Where YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and LOC_CODE='" + model.LocCode + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' ";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
            if (tuple1.Item1)
            {
               if(DesignationSession == "RSM")
                {
                    Qry = " Update SUP_FORTNIGHT_RSM_COMMENTS Set REMARKS='" + model.Remarks.Replace("'", "''") + "'" +
                          " Where YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'  and LOC_CODE='" + model.LocCode + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' ";

                }
                if (DesignationSession == "Manager" || DesignationSession == "DSM")
                {
                    Qry = " Update SUP_FORTNIGHT_RSM_COMMENTS Set MANAGER_REMARKS='" + model.Remarks.Replace("'", "''") + "'" +
                          " Where YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'  and LOC_CODE='" + model.LocCode + "' and FORTNIGHT_TYPE='" + model.FortNightType + "' ";

                }

            }
            else
            {
                if (DesignationSession == "RSM")
                {

                    Qry = " INSERT INTO SUP_FORTNIGHT_RSM_COMMENTS(YEAR, MONTH_NUMBER, LOC_CODE,SET_LOC_CODE, FORTNIGHT_TYPE,SET_DATE, REMARKS) " +
                          " Values(" + model.Year + ",'" + model.MonthNumber + "','" + model.LocCode + "','" + UserID + "','" + model.FortNightType + "',TO_Date('" + CntDate + "','dd-mm-yyyy'),'" + model.Remarks.Replace("'", "''") + "')";
                }
                if (DesignationSession == "Manager" || DesignationSession == "DSM")
                {

                    Qry = " INSERT INTO SUP_FORTNIGHT_RSM_COMMENTS(YEAR, MONTH_NUMBER, LOC_CODE,SET_LOC_CODE, FORTNIGHT_TYPE,SET_DATE, MANAGER_REMARKS) " +
                          " Values(" + model.Year + ",'" + model.MonthNumber + "','" + model.LocCode + "','" + UserID + "','" + model.FortNightType + "',TO_Date('" + CntDate + "','dd-mm-yyyy'),'" + model.Remarks.Replace("'", "''") + "')";
                }
            }

            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
            {
                IUMode = "U";
                isTrue = true;
            }
            return isTrue;
        }
    }
}
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
    public class TourPlanCreationMpoDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();
        public List<TourPlanCreationMpoBEO> GetViewData(DefaultParameterBEO model)
        {
            DataTable dt = new DataTable();
            int daysInMonth = DateTime.DaysInMonth(Convert.ToInt16(model.Year), Convert.ToInt16(model.MonthNumber));
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));      

            string Qry =@" SELECT MPO_CODE,MP_Group,DAY_NUMBER ,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW,M_SHIFT_TYPE,E_SHIFT_TYPE   " +
                         " FROM VW_TP Where MP_Group = '" + model.MPGroup + "'";
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " And MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " And YEAR=" + model.Year + "";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
             dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanCreationMpoBEO> item;
            if (dt.Rows.Count>0)
            {
                item = (from DataRow row in dt.Rows
                        select new TourPlanCreationMpoBEO
                        {
                            DayNumber = row["DAY_NUMBER"].ToString(),

                            mShiftType = row["M_SHIFT_TYPE"].ToString(),
                            mLocation = row["M_INSTI_NAME"].ToString(),
                            mLocationMultiple = row["M_INSTI_NAME"].ToString().Replace(","," | "),
                            
                            mAddress = row["M_MEETING_PLACE"].ToString(),
                            mReportTime = row["M_SET_TIME"].ToString(),
                            mDailyAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                            mReview = row["M_REVIEW"].ToString(),

                            eShiftType = row["E_SHIFT_TYPE"].ToString(),
                            eLocation = row["E_INSTI_NAME"].ToString(),
                            eLocationMultiple = row["E_INSTI_NAME"].ToString().Replace(",", " | "),
                            eAddress = row["E_MEETING_PLACE"].ToString(),
                            eReportTime = row["E_SET_TIME"].ToString(),
                            eDailyAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                            eReview = row["E_REVIEW"].ToString(),

                        }).ToList();
            }
            else
            {
                dt.Clear();
                dt.Columns.Add("DayNumber");
                dt.Columns.Add("mShiftType");
                dt.Columns.Add("mLocation");
                dt.Columns.Add("mAddress");
                dt.Columns.Add("mReportTime");
                dt.Columns.Add("mDailyAllowence");
                dt.Columns.Add("mReview");

                dt.Columns.Add("eShiftType");
                dt.Columns.Add("eLocation");
                dt.Columns.Add("eAddress");
                dt.Columns.Add("eReportTime");
                dt.Columns.Add("eDailyAllowence");
                dt.Columns.Add("eReview");

                for (int day = 1; day <= daysInMonth; day++)
                {
                    DataRow dr = dt.NewRow();
                    dr["DayNumber"] = day.ToString("D2"); // Format the day with leading zeros if needed

                    dr["mShiftType"] = "Working";// "--select--";
                    dr["mLocation"] = "--select--";
                    dr["mAddress"] = "Test Morning Address";
                    dr["mReportTime"] = "08:30";
                    dr["mDailyAllowence"] = "HQ";// "--select--";
                    dr["mReview"] = "No";

                    dr["eShiftType"] = "Working";// "--select--";
                    dr["eLocation"] = "--select--";
                    dr["eAddress"] = "Test Evening Address";
                    dr["eReportTime"] = "16:00";
                    dr["eDailyAllowence"] = "HQ";// "--select--";
                    dr["eReview"] = "No";

                    dt.Rows.Add(dr);
                }
                item = (from DataRow row in dt.Rows
                        select new TourPlanCreationMpoBEO
                        {
                            DayNumber = row["DayNumber"].ToString(),

                            mShiftType = row["mShiftType"].ToString(),
                            mLocation = row["mLocation"].ToString(),
                            mAddress = row["mAddress"].ToString(),
                            mReportTime = row["mReportTime"].ToString(),
                            mDailyAllowence = row["mDailyAllowence"].ToString(),
                            mReview = row["mReview"].ToString(),

                            eShiftType = row["eShiftType"].ToString(),
                            eLocation = row["eLocation"].ToString(),
                            eAddress = row["eAddress"].ToString(),
                            eReportTime = row["eReportTime"].ToString(),
                            eDailyAllowence = row["eDailyAllowence"].ToString(),
                            eReview = row["eReview"].ToString(),

                        }).ToList();

            }
          
            return item;
        }

        public bool TpCreation(TourPlanCreationMpoModelBEO model)
        {
            bool isTrue = false;
            string QryIsExistsMST = "Select MST_SL from Tour_MST Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);
            if (tuple1.Item1)
            {
                string SL = tuple1.Item2;
                foreach (TourPlanCreationMpoBEO detail in model.DetailList)
                {
                    int calenderCell = Convert.ToInt16(detail.DayNumber);
                    string mAndroidDetailSL = model.Year + model.MonthNumber + calenderCell + "0";
                    string eAndroidDetailSL = model.Year + model.MonthNumber + calenderCell + "1";

                    detail.mAddress = detail.mAddress?.Replace("'", "''") ?? string.Empty;
                    detail.eAddress = detail.eAddress?.Replace("'", "''") ?? string.Empty;

                    string QryDTLm = "";
                    string QryDTLe = "";
                    string Qrym = "Select * from TOUR_DTL Where MST_SL=" + SL + " and DAY_NUMBER='" + detail.DayNumber + "' and SHIFT_NAME='m'";
                    string Qrye = "Select * from TOUR_DTL Where MST_SL=" + SL + " and DAY_NUMBER='" + detail.DayNumber + "' and SHIFT_NAME='e'";

                    string QryDetail = "Select * from TOUR_DTL Where MST_SL=" + SL + " and DAY_NUMBER='" + detail.DayNumber + "'";
                    DataTable dt=  dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryDetail);
                    DataView dataViewm = new DataView(dt);      
                    dataViewm.RowFilter = "SHIFT_NAME = 'm'";            
                    DataTable mdt = dataViewm.ToTable(true, "MST_SL", "SHIFT_NAME");

                    DataView dataViewe = new DataView(dt);
                    dataViewe.RowFilter = "SHIFT_NAME = 'e'";
                    DataTable edt = dataViewe.ToTable(true, "MST_SL", "SHIFT_NAME");

                    var tuplem = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), Qrym);
                    var tuplee = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), Qrye);
                    if (tuplem.Item1)
                    {
                        QryDTLm =" Update TOUR_DTL Set SHIFT_TYPE='"+ detail.mShiftType + "',SET_TIME='" + detail.mReportTime + "',ALLOWANCE_NATURE='" + detail.mDailyAllowence + "',MEETING_PLACE='" + detail.mAddress + "'" +
                                 " Where MST_SL=" + SL + " and DAY_NUMBER='" + detail.DayNumber + "' and SHIFT_NAME='m'";
                    }
                    else
                    {
                        QryDTLm = "INSERT INTO TOUR_DTL(AN_DTL_SL,MST_SL,DAY_NUMBER,SHIFT_NAME,SET_TIME,CALENDER_CELL,ALLOWANCE_NATURE,MEETING_PLACE,SHIFT_TYPE,DTL_STATUS) Values(" + mAndroidDetailSL + "," + SL + ",'" + detail.DayNumber + "','m','" + detail.mReportTime + "'," + calenderCell + ",'" + detail.mDailyAllowence + "','" + detail.mAddress + "','" + detail.mShiftType + "','Waiting')";
                    }
                    if (tuplee.Item1)
                    {
                        QryDTLe =" Update TOUR_DTL Set SHIFT_TYPE='" + detail.eShiftType + "',SET_TIME='" + detail.eReportTime + "',ALLOWANCE_NATURE='" + detail.eDailyAllowence + "',MEETING_PLACE='" + detail.eAddress + "'" +
                                 " Where MST_SL=" + SL + " and DAY_NUMBER='" + detail.DayNumber + "' and SHIFT_NAME='e'";
                                               
                    }
                    else
                    {
                        QryDTLe = "INSERT INTO TOUR_DTL(AN_DTL_SL,MST_SL,DAY_NUMBER,SHIFT_NAME,SET_TIME,CALENDER_CELL,ALLOWANCE_NATURE,MEETING_PLACE,SHIFT_TYPE,DTL_STATUS) Values(" +eAndroidDetailSL + "," + SL + ",'" + detail.DayNumber + "','e','" + detail.eReportTime + "'," + calenderCell + ",'" + detail.eDailyAllowence + "','" + detail.eAddress + "','" + detail.eShiftType + "','Waiting')";
                    }
                    dbHelper.CmdExecute(QryDTLm);
                    dbHelper.CmdExecute(QryDTLe);
                    //if (dbHelper.CmdExecute(QryDTLm) > 0 || dbHelper.CmdExecute(QryDTLe) > 0)
                    //{
                        string detailSLm = "m" + detail.DayNumber + SL;
                        string detailSLe = "e" + detail.DayNumber + SL;
                        string QryDelM = "Delete from Tour_SUB_DTL Where DTL_SL='"+ detailSLm + "'";
                        string QryDelE = "Delete from Tour_SUB_DTL Where DTL_SL='" + detailSLe + "'";
                        dbHelper.CmdExecute(QryDelM);
                        dbHelper.CmdExecute(QryDelE);
                        string[] mLocation = detail.mLocationMultiple.Split('|');
                        string[] eLocation = detail.eLocationMultiple.Split('|');
                        foreach (string location in mLocation)
                        {                            
                            string QrySubDTL = "INSERT INTO Tour_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS, ADDITION) Values('" + detailSLm + "','" + location + "','Waiting','No')";
                            if (dbHelper.CmdExecute(QrySubDTL) > 0)
                            {
                                isTrue = true;
                            }
                        }
                        foreach (string location in eLocation)
                        {                         
                            string QrySubDTL = "INSERT INTO Tour_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS, ADDITION) Values('" + detailSLe + "','" + location + "','Waiting','No')";
                            if (dbHelper.CmdExecute(QrySubDTL) > 0)
                            {
                                isTrue = true;
                            }
                        }
                    //}                   
                }
            }
            else
            {                    
              Int64 SL = idGenerated.getMAXSL("Tour_MST", "MST_SL");
              string QryMST = "INSERT INTO Tour_MST(MST_SL,MP_GROUP,YEAR,MONTH_NUMBER) Values(" + SL + ",'" + model.MPGroup + "'," + model.Year + ",'" + model.MonthNumber + "')";
                if (dbHelper.CmdExecute(QryMST)>0)
                {
                    foreach (TourPlanCreationMpoBEO detail in model.DetailList)
                    {
                        int calenderCell =Convert.ToInt16(detail.DayNumber);
                        string mAndroidDetailSL = model.Year + model.MonthNumber + calenderCell + "0";
                        string eAndroidDetailSL = model.Year + model.MonthNumber + calenderCell + "1";

                        detail.mAddress = detail.mAddress?.Replace("'", "''") ?? string.Empty;
                        detail.eAddress = detail.eAddress?.Replace("'", "''") ?? string.Empty;
                        string QryDtlMorningValues = "Select " + mAndroidDetailSL + " AN_DTL_SL," + SL + " MST_SL,'" + detail.DayNumber + "' DAY_NUMBER,'m' SHIFT_NAME,'" + detail.mReportTime + "' SET_TIME," + calenderCell + " CALENDER_CELL,'" + detail.mDailyAllowence + "' ALLOWANCE_NATURE,'" + detail.mAddress + "' MEETING_PLACE,'" + detail.mShiftType + "' SHIFT_TYPE,'Waiting' DTL_STATUS from Dual";
                        string QryDtlEveningValues = "Select " + eAndroidDetailSL + " AN_DTL_SL," + SL + " MST_SL,'" + detail.DayNumber + "' DAY_NUMBER,'e' SHIFT_NAME,'" + detail.eReportTime + "' SET_TIME," + calenderCell + " CALENDER_CELL,'" + detail.eDailyAllowence + "' ALLOWANCE_NATURE,'" + detail.eAddress + "' MEETING_PLACE,'" + detail.eShiftType + "' SHIFT_TYPE,'Waiting' DTL_STATUS from Dual";

                        string QryDTL = @"INSERT INTO Tour_DTL(AN_DTL_SL,MST_SL,DAY_NUMBER,SHIFT_NAME,SET_TIME,CALENDER_CELL,ALLOWANCE_NATURE,MEETING_PLACE,SHIFT_TYPE,DTL_STATUS) ";
                        QryDTL = QryDTL + QryDtlMorningValues + " Union All " + QryDtlEveningValues;
                        if (dbHelper.CmdExecute(QryDTL)>0)
                        {
                            string[] mLocation = detail.mLocationMultiple.Split('|');
                            string[] eLocation = detail.eLocationMultiple.Split('|');
                            foreach (string location in mLocation)
                            {                              
                                string detailSL = "m" + detail.DayNumber + SL;                              
                                string QrySubDTL = "INSERT INTO Tour_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS, ADDITION) Values('" + detailSL + "','" + location + "','Waiting','No')";

                                if (dbHelper.CmdExecute(QrySubDTL)>0)
                                {
                                    isTrue = true;
                                }
                            }
                            foreach (string location in eLocation)
                            {
                                string detailSL = "e" + detail.DayNumber + SL;                              
                                string QrySubDTL = "INSERT INTO Tour_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS, ADDITION) Values('" + detailSL + "','" + location + "','Waiting','No')";

                                if (dbHelper.CmdExecute(QrySubDTL) > 0)
                                {
                                    isTrue = true;
                                }
                            }
                        }
                    }
                }                
            }
            return isTrue;
        }


    }
}
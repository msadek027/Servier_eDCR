using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
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

namespace eDCR.Areas.DCR.Models.DAO
{

    public class TourPlanDAO : ReturnData
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
        public bool SaveUpdateNEW(TourPlanningMaster model)
        {
            bool isTrue = false;

            int month = ((   Convert.ToInt16(model.Year)- Convert.ToInt16(DateTime.Now.Year)) * 12) +  Convert.ToInt16(model.MonthNumber)- Convert.ToInt16(DateTime.Now.Month);

            //if (month >= 0)
            //{
                if (model.ViewReview == "View")
                {
                    if (SaveUpdateView(model))
                    {
                        isTrue = true;                       
                    }
                }
                if (model.ViewReview == "Review")
                {
                    if (SaveUpdateReview(model))
                    {
                        isTrue = true;
                   
                    }

               // }            

            string QryTourPlanPartialAllow = "Update Tour_Partial_Allow set Is_Partial_Allow='No' Where MP_GROUP='" + model.MPGroup + "' ";
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTourPlanPartialAllow);

            }
            return isTrue;
        }

      
        private bool SaveUpdateView(TourPlanningMaster model)
        {

            bool isTrue = false;
            string QryIsExistsMST = "Select MST_SL from TOUR_MST Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and MST_STATUS='Approved'";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);

            string QryTempDel = "Delete from  OP_TP_VW Where MP_GROUP='" + model.MPGroup + "'";
           // string QryTempIns = "Insert Into Tour_VW_OP (Select * from VW_TOUR Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
             string QryTempIns = "Insert Into OP_TP_VW (MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE,DTL_STATUS,REVIEW,DTL_SL,INST_NAME,Sub_DTL_STATUS)" +
                " (Select MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE,DTL_STATUS,REVIEW,DTL_SL,INST_NAME,Sub_DTL_STATUS from VW_TOUR Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";

            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempDel);
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempIns);           
          
            if (tuple1.Item1)
            {
                IUMode = "U";
                isTrue = true;
            }
            else
            {
               
                string QryMaster = "Update  TOUR_MST Set MST_STATUS='Approved'  Where MST_SL in (Select distinct  MST_SL from OP_TP_VW Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
                string QryDetail = "Update TOUR_DTL Set DTL_STATUS ='Approved' Where SHIFT_NAME||DAY_NUMBER||MST_SL in  (Select distinct SHIFT_NAME||DAY_NUMBER||MST_SL from OP_TP_VW Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
                string QrySubDetail = "Update TOUR_SUB_DTL Set Sub_DTL_STATUS='Approved' Where DTL_SL in (Select distinct DTL_SL from OP_TP_VW Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryMaster))
                {
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail))
                    {
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail))
                        {
                            IUMode = "I";
                            isTrue = true;
                        }
                    }
                }

            }

            if (pushNotification.IsConnectedToInternet())
            {
                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
                pushNotification.SaveToDatabase(model.MPGroup, "TP", "TP Approved", "TP of " + MonthName + " Approved.", model.Year, model.MonthNumber);
                pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.MPGroup), "TP", "TP Approved", "TP of " + MonthName + " Approved.");            
            
            }
            return isTrue;
        }
        private bool SaveUpdateReview(TourPlanningMaster model)
        {
            bool isTrue = false;
            string QryIsExistsMST = "Select MST_SL from TOUR_MST Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and MST_STATUS='Approved' ";
            var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);

           string QryTempDel = "Delete from  OP_TP_VW Where MP_GROUP='" + model.MPGroup + "'";

           string QryTempIns = " Insert Into OP_TP_VW (MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE,DTL_STATUS,REVIEW,DTL_SL,INST_NAME,Sub_DTL_STATUS)" +
                               " (Select MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE,DTL_STATUS,REVIEW,DTL_SL,INST_NAME,Sub_DTL_STATUS from VW_TOUR Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "')";
           dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempDel);
           dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempIns);  
       
            DVRApprovedReview(model);    

            string QryDetail = "Update TOUR_DTL Set DTL_STATUS ='Approved',Review='Approved' Where SHIFT_NAME||DAY_NUMBER||MST_SL in  (Select distinct SHIFT_NAME||DAY_NUMBER||MST_SL from OP_TP_VW Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and Review='Yes')";
            string QrySubDetail = "Update TOUR_SUB_DTL Set Sub_DTL_STATUS='Approved' Where DTL_SL in (Select distinct DTL_SL from OP_TP_VW Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' and Review='Yes')";

            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail))
            {
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail))
                {
                    IUMode = "I";
                    isTrue = true;
                }
            }



            if (pushNotification.IsConnectedToInternet())
            {
                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
                pushNotification.SaveToDatabase(model.MPGroup, "TP", "Change TP Approved", "Change TP of " + MonthName + " Approved.", model.Year, model.MonthNumber);

                pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.MPGroup), "Change TP", "Change TP Approved", "TP of " + MonthName + " Approved.");
              
            }
            return isTrue;
        }

        private bool DVRApprovedReview(TourPlanningMaster master)
        {
            bool isTrue = false;
            string IsExistApprovedDVR = "Select MST_SL from DVR_MST Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' and MST_STATUS='Approved'";
            var tupleApprovedDVR = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExistApprovedDVR);      

            if (tupleApprovedDVR.Item1)
            {
                string mstSL = tupleApprovedDVR.Item2;

                string QrySubDTLDel = "Delete from DVR_SUB_DTL Where DTL_SL IN (Select distinct Shift_Name||DAY_NUMBER||MST_SL from VW_TOUR_INST_DOC_MAPP_EFF_DVR Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' and MST_STATUS='Approved' and Review='Yes') ";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDTLDel);

                string QryDel = "Delete from DVR_DTL Where Shift_Name||DAY_NUMBER||MST_SL IN (Select distinct Shift_Name||DAY_NUMBER||MST_SL from VW_TOUR_INST_DOC_MAPP_EFF_DVR Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' and Review='Yes')";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDel);

                string QryDetail = "Insert into DVR_DTL(MST_SL, DAY_NUMBER, SHIFT_NAME, SET_TIME, DTL_STATUS) (Select distinct MST_SL, DAY_NUMBER, SHIFT_NAME, SET_TIME, 'Waiting' DTL_STATUS from VW_TOUR_INST_DOC_MAPPING Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' and SHIFT_TYPE='Working' and Review='Yes')";
                string QrySubDetail = "Insert into DVR_SUB_DTL (DTL_SL, DOCTOR_ID, SUB_DTL_STATUS)(Select distinct DTL_SL, DOCTOR_ID,'Waiting' SUB_DTL_STATUS from VW_TOUR_INST_DOC_MAPPING Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' and SHIFT_TYPE='Working' and Review='Yes')";
              
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail))
                    {
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail))
                        {
                            IUMode = "I";
                            isTrue = true;
                        }
                    }
                   
            }
            return isTrue;
        }
       

        public List<TourPlanningReport> GetReviewData(DefaultParameterBEO model)
        {
            string Qry = "SELECT MPO_CODE,MP_Group,DAY_NUMBER,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,M_REVIEW,E_REVIEW  " +
               "  FROM VW_TP_RPT Where YEAR||MONTH_NUMBER||DAY_NUMBER IN"+
                  " (Select distinct YEAR||MONTH_NUMBER||DAY_NUMBER from TOUR_MST A JOIN TOUR_DTL B ON A.MST_SL= B.MST_SL Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'  and Review='Yes')";

            Qry = Qry + " and MP_Group = '" + model.MPGroup + "'";
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " And MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " And YEAR=" + model.Year + "";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanningReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanningReport
                    {

                        //MPGroup = row["MP_Group"].ToString(),                      
                        DayNumber = row["DAY_NUMBER"].ToString(),                
                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }

        public List<TourPlanningReport> GetViewData(DefaultParameterBEO model)
        {
            string Qry = "SELECT MPO_CODE,MP_Group,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE,M_REVIEW,E_REVIEW  " +
                "  FROM VW_TP_RPT Where MP_Group = '" + model.MPGroup + "'";
            if (model.MonthNumber != "" && model.MonthNumber != null)
            {
                Qry = Qry + " And MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            if (model.Year != "" && model.Year != null)
            {
                Qry = Qry + " And YEAR=" + model.Year + "";
            }
            Qry = Qry + " Order by To_Number(DAY_NUMBER)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanningReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanningReport
                    {
                        //MPGroup = row["MP_Group"].ToString(),                   
                        DayNumber = row["DAY_NUMBER"].ToString(),                                   
                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),                   
                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),               
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                        MReview = row["M_REVIEW"].ToString(),
                        EReview = row["E_REVIEW"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<TourPlanningReport>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";


            string Qry = "SELECT MPO_CODE,MP_Group,DAY_NUMBER,M_SET_TIME,M_INSTI_NAME,M_MEETING_PLACE ,E_SET_TIME,E_INSTI_NAME,E_MEETING_PLACE,ALLOWANCE_NATURE,M_ALLOWANCE_NATURE,E_ALLOWANCE_NATURE  " +
                "  FROM VW_TP_RPT Where MONTH_NUMBER='" + model.MonthNumber + "' And YEAR=" + model.Year + "";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;

            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split('|');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split('|');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
            }

            Qry = Qry + " Order by To_Number(DAY_NUMBER)";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanningReport> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanningReport
                    {
                        DayNumber = row["DAY_NUMBER"].ToString(),                   
                        MSetTime = row["M_SET_TIME"].ToString(),
                        MInstName = row["M_INSTI_NAME"].ToString(),
                        MMeetingPlace = row["M_MEETING_PLACE"].ToString(),
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        EInstName = row["E_INSTI_NAME"].ToString(),
                        EMeetingPlace = row["E_MEETING_PLACE"].ToString(),
                        ESetTime = row["E_SET_TIME"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }



        public List<TourPlanningMaster> GetPopupView(DefaultParameterBEO model)
        {
            string Qry = "SELECT distinct  MST_SL,MPO_CODE,MPO_NAME,MP_Group,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_Tour  Where VW='View'";
            Qry = Qry + " AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";          
            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }        

            Qry = Qry + " Order by MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanningMaster> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanningMaster
                    {
                        MasterSL = row["MST_SL"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_Group"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        MasterStatus = row["MST_STATUS"].ToString(),
                        VW = row["VW"].ToString()

                    }).ToList();
            return item;
        }
        public List<TourPlanningMaster> GetPopupReview(DefaultParameterBEO model)
        {
            string Qry = "SELECT distinct  MST_SL,MPO_CODE,MPO_NAME,MP_Group,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS,VW From VW_Tour  Where Review='Yes' and MST_STATUS='Approved' ";
            Qry = Qry + " AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";
            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }     
            Qry = Qry + " Order by MPO_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TourPlanningMaster> item;

            item = (from DataRow row in dt.Rows
                    select new TourPlanningMaster
                    {
                        MasterSL = row["MST_SL"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_Group"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        MasterStatus = row["MST_STATUS"].ToString(),
                        RMasterStatus = row["MST_STATUS"].ToString() == "Approved" ? "Waiting" : row["MST_STATUS"].ToString(),
                        VW = "ReView"
                    }).ToList();
            return item;
        }

        public List<DefaultParameterBEO> GetAllowanceNature()
        {
            string Qry = "Select SHORT_NAME,FULL_NAME from ACC_ALLOWANCE_NATURE";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultParameterBEO> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultParameterBEO
                    {
                        Allowence = row["SHORT_NAME"].ToString(),

                      
                    }).ToList();
            return item;
        }

        public List<TPShiftWiseLocation> MorningInst(TourPlanningMaster model)
        {
            string Qry = "Select a.INST_NAME,a.YetAssigned from " +
                " ((Select distinct INST_NAME,'true' YetAssigned from VW_TOUR  Where MP_Group='" + model.MPGroup + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and   SHIFT_NAME='m')" +
               " UNION " +
               "(Select distinct INST_NAME,'false' YetAssigned from VW_DOC_INST_MPO_MAPPING  Where MP_Group='" + model.MPGroup + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and SHIFT_NAME='m')) a" +
               " Order by  CASE WHEN a.YetAssigned = 'true' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TPShiftWiseLocation> item;

            item = (from DataRow row in dt.Rows
                    select new TPShiftWiseLocation
                    {
                        MInstCode = row["INST_NAME"].ToString(),
                        MInstName = row["INST_NAME"].ToString(),
                        YetAssigned = row["YetAssigned"].ToString()

                    }).ToList();
            return item;
        }

        public List<TPShiftWiseLocation> EveningInst(TourPlanningMaster model)
        {
            string Qry = "Select a.INST_NAME,a.YetAssigned from " +
                     " ((Select distinct INST_NAME,'true' YetAssigned from VW_TOUR  Where MP_Group='" + model.MPGroup + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "'  and SHIFT_NAME='e')" +
                    " UNION " +
                    "(Select distinct INST_NAME,'false' YetAssigned from VW_DOC_INST_MPO_MAPPING  Where MP_Group='" + model.MPGroup + "' and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and SHIFT_NAME='e')) a" +
                    " Order by  CASE WHEN a.YetAssigned = 'true' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TPShiftWiseLocation> item;

            item = (from DataRow row in dt.Rows
                    select new TPShiftWiseLocation
                    {
                        EInstCode = row["INST_NAME"].ToString(),
                        EInstName = row["INST_NAME"].ToString(),
                        YetAssigned = row["YetAssigned"].ToString()

                    }).ToList();
            return item;
        }

        public List<TPShiftWiseLocation> DetailValue(TourPlanningMaster model)
        {
            string Qry = "Select distinct a.SHIFT_TYPE M_SHIFT_TYPE,a.ALLOWANCE_NATURE M_ALLOWANCE_NATURE,b.SHIFT_TYPE E_SHIFT_TYPE,b.ALLOWANCE_NATURE E_ALLOWANCE_NATURE" +
                " from"+
                " (Select Day_Number,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE from VW_TOUR Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and  MP_Group='" + model.MPGroup + "' and SHIFT_NAME='m') a," +
                " (Select Day_Number,SHIFT_NAME,SHIFT_TYPE,ALLOWANCE_NATURE from VW_TOUR Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and Day_Number='" + model.DayNumber + "' and  MP_Group='" + model.MPGroup + "' and SHIFT_NAME='e')b" +
                " Where a.Day_Number=b.Day_Number(+)";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<TPShiftWiseLocation> item;

            item = (from DataRow row in dt.Rows
                    select new TPShiftWiseLocation
                    {
                        MAllowence = row["M_ALLOWANCE_NATURE"].ToString(),
                        EAllowence = row["E_ALLOWANCE_NATURE"].ToString(),

                        MShiftType = row["M_SHIFT_TYPE"].ToString(),
                        EShiftType = row["E_SHIFT_TYPE"].ToString(),

                    }).ToList();
            return item;
        }

      public bool SaveUpdateEdit(DefaultParameterBEO model)
        {
            bool isTrue = false;
            int month = ((Convert.ToInt16(model.Year) - Convert.ToInt16(DateTime.Now.Year)) * 12) + Convert.ToInt16(model.MonthNumber) - Convert.ToInt16(DateTime.Now.Month);

            if (month >= 0)
            {
                string QryIsExistsMST = "Select MST_SL from Tour_MST Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'";
                var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);
                string mDTLSL = "m" + model.DayNumber + tuple1.Item2;
                string eDTLSL = "e" + model.DayNumber + tuple1.Item2;

                string QrymDTL = "Update TOUR_DTL Set ALLOWANCE_NATURE='" + model.MAllowence + "',SHIFT_TYPE='" + model.MShiftType + "' Where SHIFT_NAME||DAY_NUMBER||MST_SL='" + mDTLSL + "'";
                string QryeDTL = "Update TOUR_DTL Set ALLOWANCE_NATURE='" + model.EAllowence + "',SHIFT_TYPE='" + model.EShiftType + "' Where SHIFT_NAME||DAY_NUMBER||MST_SL='" + eDTLSL + "'";

                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrymDTL);
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryeDTL);



                string QryIsExistsMSTStatus = "Select MST_STATUS from Tour_MST Where MP_GROUP='" + model.MPGroup + "' and YEAR=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "'";
                var tupleStatus = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMSTStatus);

                string mDel = "Delete from TOUR_SUB_DTL Where DTL_SL='" + mDTLSL + "'";
                string eDel = "Delete from TOUR_SUB_DTL Where DTL_SL='" + eDTLSL + "'";

                dbHelper.CmdExecute(dbConn.SAConnStrReader(), mDel);
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), eDel);

           
                string StrMInst = model.MInstCode.Replace("[", "").Replace("]", "").Replace("\"", "");
                String[] SubStrMInst = StrMInst.Split(',');
                for (int i = 0; i < SubStrMInst.Length; i++)
                {

                    //string Qry = "Update TOUR_SUB_DTL set INST_CODE='"+SubStrMInst[i].ToString()+"' Where DTL_SL='" + mDTLSL + "'";
                    string Qry = "INSERT INTO TOUR_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS) Values ('" + mDTLSL + "','" + SubStrMInst[i].ToString() + "','" + tupleStatus.Item2 + "')";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                    {
                        isTrue = true;
                        IUMode = "U";
                    }

                }
                string StrEInst = model.EInstCode.Replace("[", "").Replace("]", "").Replace("\"", "");
                String[] SubStrEInst = StrEInst.Split(',');
                for (int i = 0; i < SubStrEInst.Length; i++)
                {
                    //string Qry = "Update TOUR_SUB_DTL set INST_CODE='" + SubStrEInst[i].ToString() + "' Where DTL_SL='" + eDTLSL + "'";
                    string Qry = "INSERT INTO TOUR_SUB_DTL(DTL_SL, INST_NAME, SUB_DTL_STATUS) Values ('" + eDTLSL + "','" + SubStrEInst[i].ToString() + "','" + tupleStatus.Item2 + "')";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                    {
                        isTrue = true;
                        IUMode = "U";
                    }

                }
            }
            return  isTrue;
            }



      public List<DefaultBEL> TourPartialAllow()
      {
          string Qry = "SELECT distinct MARKET_CODE,MARKET_NAME,MPO_CODE,MPO_NAME,MP_GROUP,Is_Partial_Allow from SA_USERCREDENTIAL_VW ";
            Qry = Qry + " Where DESIGNATION IN ('MPO','SMPO') ";
            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }
            Qry = Qry + "  Order by MARKET_CODE,MP_GROUP";

          DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
          List<DefaultBEL> item;

          item = (from DataRow row in dt.Rows
                  select new DefaultBEL
                  {
                      MPOCode = row["MPO_CODE"].ToString(),
                      MPOName = row["MPO_NAME"].ToString(),
                      MarketName = row["MARKET_NAME"].ToString(),
                      MarketCode = row["MARKET_CODE"].ToString(),
                      MPGroup = row["MP_GROUP"].ToString(),
                      IsPartialAllow = row["Is_Partial_Allow"].ToString(),
                  }).ToList();
          return item;
      }

      public bool SaveUpdateTourPlanPartialAllow(DefaultBEL model)
      {
          bool isTrue = false;
          string Qry = "";
          string QryIsExists = "Select MP_GROUP From Tour_Partial_Allow Where MP_GROUP='" + model.MPGroup + "'  ";
          var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
          if (tuple1.Item1)
          {             
             Qry = "Update Tour_Partial_Allow set Is_Partial_Allow='"+model.IsPartialAllow+"',MARKET_CODE='" + model.MarketCode + "', MPO_CODE='" + model.MPOCode + "' Where MP_GROUP='" + model.MPGroup + "' ";
             IUMode = "U";
       
            }
             else
              {
                  Qry = "Insert Into Tour_Partial_Allow(MP_GROUP,MARKET_CODE,MPO_CODE,Is_Partial_Allow) Values('" + model.MPGroup + "', '" + model.MarketCode + "','" + model.MPOCode + "','" + model.IsPartialAllow + "')";

                  IUMode = "I";
                 
              }
          if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
          {
              isTrue = true;
          }
              return isTrue;
          } 
      }
    }


    

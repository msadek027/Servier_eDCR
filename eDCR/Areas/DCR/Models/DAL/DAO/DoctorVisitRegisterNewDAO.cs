using eDCR.Areas.DCR.Models.BEL;
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
    public class DoctorVisitRegisterNewDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();


        public Tuple<DataTable, List<ReportDVRBEL>> GetMainGridData(DefaultParameterBEO model)
        {
            if (model.MasterStatus == " " || model.MasterStatus == "" || model.MasterStatus == null)
            {
                string QryStatus = "Select  MST_STATUS from DVR_MST Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "'  and MP_GROUP='" + model.MPGroup + "'";
                model.MasterStatus=dbHelper.GetValue(QryStatus);
            }

            string Qry = " Select DOCTOR_ID,DOCTOR_NAME ,MPO_CODE, MPO_NAME, MP_GROUP,POTENTIAL,SHIFT_NAME," +
            " md01, md02, md03, md04, md05, md06, md07, md08, md09, md10, md11, md12, md13, md14, md15, md16, md17, md18, md19, md20, md21, md22, md23, md24, md25, md26, md27, md28, md29, md30, md31," +
            " ed01, ed02, ed03, ed04, ed05, ed06, ed07, ed08, ed09, ed10, ed11, ed12, ed13, ed14, ed15, ed16, ed17, ed18, ed19, ed20, ed21, ed22, ed23, ed24, ed25, ed26, ed27, ed28, ed29, ed30, ed31," +

          "   CASE WHEN md01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md31 LIKE '%D%' THEN 1 ELSE 0 END MD,  " +
          "   CASE WHEN ed01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%D%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed31 LIKE '%D%' THEN 1 ELSE 0END ED,  " +

          "   CASE WHEN md01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md31 LIKE '%P%' THEN 1 ELSE 0 END MP,  " +
          "   CASE WHEN ed01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%P%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed31 LIKE '%P%' THEN 1 ELSE 0END EP,  " +


          "   CASE WHEN md01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md31 LIKE '%E%' THEN 1 ELSE 0 END ME,  " +
          "   CASE WHEN ed01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%E%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed31 LIKE '%E%' THEN 1 ELSE 0END EE,  " +

           "  CASE WHEN md01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN md31 LIKE '%A%' THEN 1 ELSE 0 END MA,  " +
          "   CASE WHEN ed01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%A%' THEN 1 ELSE 0 END " +
          " + CASE WHEN ed31 LIKE '%A%' THEN 1 ELSE 0END EA  " +

          " From VW_DVR_RPT_V03 Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + "and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                if (model.MasterStatus=="Approved")
                {
                    Qry = Qry + " and  SubStr(MST_Status,1,8)='" + model.MasterStatus + "'";
                }
                if (model.MasterStatus == "Waiting")
                {
                    Qry = Qry + " and  SubStr(MST_Status,1,7)='" + model.MasterStatus + "'";
                }               
            }
            if (model.MasterStatus == "Waiting")
            {
                Qry = Qry + " AND WAITING_STATUS IS NOT NULL";
            }           
        

            Qry = Qry + " Order by DOCTOR_ID";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDVRBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDVRBEL
                    {
                        SL = row["Col1"].ToString(),
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        Potential = row["POTENTIAL"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MPOCode = row["MPO_Code"].ToString(),
                        // MPOName = row["MPO_Name"].ToString(),
                        MPGroup = row["MP_Group"].ToString(),
                        MD = row["MD"].ToString(),
                        MED = Convert.ToString(Convert.ToInt16(row["MD"].ToString()) + Convert.ToInt16(row["ED"].ToString())),
                        //Add New
                        MEP = Convert.ToString(Convert.ToInt16(row["MP"].ToString()) + Convert.ToInt16(row["EP"].ToString())),
                        MEE = Convert.ToString(Convert.ToInt16(row["ME"].ToString()) + Convert.ToInt16(row["EE"].ToString())),
                        MEA = Convert.ToString(Convert.ToInt16(row["MA"].ToString()) + Convert.ToInt16(row["EA"].ToString())),


                        md01 = row["md01"].ToString(),
                        md02 = row["md02"].ToString(),
                        md03 = row["md03"].ToString(),
                        md04 = row["md04"].ToString(),
                        md05 = row["md05"].ToString(),
                        md06 = row["md06"].ToString(),
                        md07 = row["md07"].ToString(),
                        md08 = row["md08"].ToString(),
                        md09 = row["md09"].ToString(),
                        md10 = row["md10"].ToString(),
                        md11 = row["md11"].ToString(),
                        md12 = row["md12"].ToString(),
                        md13 = row["md13"].ToString(),
                        md14 = row["md14"].ToString(),
                        md15 = row["md15"].ToString(),
                        md16 = row["md16"].ToString(),
                        md17 = row["md17"].ToString(),
                        md18 = row["md18"].ToString(),
                        md19 = row["md19"].ToString(),
                        md20 = row["md20"].ToString(),
                        md21 = row["md21"].ToString(),
                        md22 = row["md22"].ToString(),
                        md23 = row["md23"].ToString(),
                        md24 = row["md24"].ToString(),
                        md25 = row["md25"].ToString(),
                        md26 = row["md26"].ToString(),
                        md27 = row["md27"].ToString(),
                        md28 = row["md28"].ToString(),
                        md29 = row["md29"].ToString(),
                        md30 = row["md30"].ToString(),
                        md31 = row["md31"].ToString(),

                        ED = row["ED"].ToString(),
                        ed01 = row["ed01"].ToString(),
                        ed02 = row["ed02"].ToString(),
                        ed03 = row["ed03"].ToString(),
                        ed04 = row["ed04"].ToString(),
                        ed05 = row["ed05"].ToString(),
                        ed06 = row["ed06"].ToString(),
                        ed07 = row["ed07"].ToString(),
                        ed08 = row["ed08"].ToString(),
                        ed09 = row["ed09"].ToString(),
                        ed10 = row["ed10"].ToString(),
                        ed11 = row["ed11"].ToString(),
                        ed12 = row["ed12"].ToString(),
                        ed13 = row["ed13"].ToString(),
                        ed14 = row["ed14"].ToString(),
                        ed15 = row["ed15"].ToString(),
                        ed16 = row["ed16"].ToString(),
                        ed17 = row["ed17"].ToString(),
                        ed18 = row["ed18"].ToString(),
                        ed19 = row["ed19"].ToString(),
                        ed20 = row["ed20"].ToString(),
                        ed21 = row["ed21"].ToString(),
                        ed22 = row["ed22"].ToString(),
                        ed23 = row["ed23"].ToString(),
                        ed24 = row["ed24"].ToString(),
                        ed25 = row["ed25"].ToString(),
                        ed26 = row["ed26"].ToString(),
                        ed27 = row["ed27"].ToString(),
                        ed28 = row["ed28"].ToString(),
                        ed29 = row["ed29"].ToString(),
                        ed30 = row["ed30"].ToString(),
                        ed31 = row["ed31"].ToString(),
                    }).ToList();

            return Tuple.Create(dt, item);
        }


        public Tuple<List<WorkPlan>, List<WorkPlan>, List<WorkPlan>> GetDvrWpProductList(DefaultParameterBEO model)
        {
            string DayNumber = model.DayNumber.Length == 1 ? "0" + model.DayNumber : model.DayNumber;
            string SetDate = DayNumber.Trim() + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
            string QryDvr = "Select distinct MP_GROUP,DAY_NUMBER||'-'||MONTH_NUMBER||'-'||Year SET_DATE, Doctor_ID,Doctor_Name from  VW_DVR  Where TO_Date(DAY_NUMBER||'-'||Month_Number||'-'||Year,'dd-mm-yyyy')=To_Date('" + SetDate + "','dd-mm-yyyy') and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";
            DataTable dtDvr = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryDvr);        
            var dvr = dtDvr?.AsEnumerable().Select(row => new WorkPlan
            {
                DoctorID = row["Doctor_ID"].ToString(),
                DoctorName = row["Doctor_Name"].ToString(),
                MPGroup = row["MP_GROUP"].ToString(),
                SetDate = row["SET_DATE"].ToString(),
            }).ToList();
            //---------------------------------
            string QryWp = "Select distinct TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,MP_GROUP,PRODUCT_CODE,PRODUCT_NAME,QUANTITY from VW_WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";
            DataTable dtWp = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryWp);
            var wp = dtWp?.AsEnumerable().Select(row => new WorkPlan
            {
                MPGroup = row["MP_GROUP"].ToString(),
                SetDate = row["SET_DATE"].ToString(),
                ProductCode = row["PRODUCT_CODE"].ToString(),
                ProductName = row["PRODUCT_NAME"].ToString(),
                Quantity = row["QUANTITY"].ToString(),

            }).ToList();
            //-----------------------------------
            string Parameter = "MP_GROUP='" + model.MPGroup + "' and Month_Number='" + model.MonthNumber + "' and Year=" + model.Year + "";
            string DoctorType = model.DoctorID.Trim().Substring(0, 1);
            string ItemType = string.Empty;
            string QryTop = "Select a.Product_Code,a.Product_Name from (";
            string QryFirst = "Select '0' Product_Code,'' Product_Name from Dual  Union ";
            string QryProduct = "";
            if (DoctorType == "I")
            {
                ItemType = "'SmI','GtI'";
                QryProduct = "Select distinct Product_Code,Product_Name||' |'||ITEM_TYPE||ITEM_FOR  Product_Name from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR in ('SmI','GtI')  and ";
                QryProduct = QryTop + QryFirst + QryProduct + Parameter + " ";

            }
            else
            {
                ItemType = "'SlR','SmR','GtR'";
                QryProduct = "Select distinct Product_Code,Product_Name||' |'||ITEM_TYPE||ITEM_FOR  Product_Name from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR in ('SlR','SmR','GtR')  and ";
                QryProduct = QryTop + QryFirst + QryProduct + Parameter + " and Product_Code In (Select Product_Code from VW_GWDS Where Doctor_ID='" + model.DoctorID + "' and " + Parameter +
                " Union All " +
                " Select Product_Code from VW_PWDS Where Doctor_ID='" + model.DoctorID + "' and " + Parameter + " " +
                " Union All Select distinct Product_Code from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR='SmR' and " + Parameter + " )";

            }
            QryProduct = QryProduct + ") a Order by  CASE WHEN a.Product_Code = '0' THEN 1 ELSE 2 END,a.Product_Name";
            DataTable dtProduct = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryProduct);
            var product = dtProduct?.AsEnumerable().Select(row => new WorkPlan
            {
                ProductCode = row["PRODUCT_CODE"].ToString(),
                ProductName = row["PRODUCT_NAME"].ToString(),

            }).ToList();
            //--------------------------------------
            return Tuple.Create(dvr, wp, product);
        }

        public List<WorkPlan> GetDVR(DefaultParameterBEO model)
        {
            string DayNumber = model.DayNumber.Length == 1 ? "0" + model.DayNumber : model.DayNumber;
            string SetDate = DayNumber.Trim() + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
            string Qry = "Select distinct MP_GROUP,DAY_NUMBER||'-'||MONTH_NUMBER||'-'||Year SET_DATE, Doctor_ID,Doctor_Name from  VW_DVR  Where TO_Date(DAY_NUMBER||'-'||Month_Number||'-'||Year,'dd-mm-yyyy')=To_Date('" + SetDate + "','dd-mm-yyyy') and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<WorkPlan> item;

            item = (from DataRow row in dt.Rows
                    select new WorkPlan
                    {
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),

                    }).ToList();
            return item;
        }

        public List<WorkPlan> GetWP(DefaultParameterBEO model)
        {
            string SetDate = model.DayNumber.Trim() + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
            string Qry = "Select distinct TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,MP_GROUP,PRODUCT_CODE,PRODUCT_NAME,QUANTITY from VW_WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<WorkPlan> item;
            item = (from DataRow row in dt.Rows
                    select new WorkPlan
                    {
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                    }).ToList();
            return item;
        }

        public List<WorkPlan> GetProduct(DefaultParameterBEO model)
        {

            string SetDate = model.DayNumber.Trim() + "-" + model.MonthNumber.Trim() + "-" + model.Year.Trim();
            string  Parameter=  "MP_GROUP='" + model.MPGroup + "' and Month_Number='" + model.MonthNumber + "' and Year=" + model.Year + "";
            string DoctorType = model.DoctorID.Trim().Substring(0, 1);

            string ItemType = string.Empty;

            string QryTop = "Select a.Product_Code,a.Product_Name from (";
            string QryFirst = "Select '0' Product_Code,'' Product_Name from Dual  Union ";
            string Qry = "";
            if(DoctorType=="I")
            {
                ItemType = "'SmI','GtI'";

                Qry = "Select distinct Product_Code,Product_Name||' |'||ITEM_TYPE||ITEM_FOR  Product_Name from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR in ('SmI','GtI')  and ";
                Qry =QryTop+ QryFirst + Qry + Parameter + " ";
                
            }
            else
            {
                ItemType = "'SlR','SmR','GtR'";

               
                Qry =  "Select distinct Product_Code,Product_Name||' |'||ITEM_TYPE||ITEM_FOR  Product_Name from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR in ('SlR','SmR','GtR')  and ";
                Qry = QryTop + QryFirst + Qry + Parameter + " and Product_Code In (Select Product_Code from VW_GWDS Where Doctor_ID='" + model.DoctorID + "' and " + Parameter +
                " Union All " +
                " Select Product_Code from VW_PWDS Where Doctor_ID='" + model.DoctorID + "' and " + Parameter + " "+
                " Union All Select distinct Product_Code from VW_INV_ITEM_BALANCE Where ITEM_TYPE||ITEM_FOR='SmR' and " + Parameter + " )";  

            }
            Qry = Qry + ") a Order by  CASE WHEN a.Product_Code = '0' THEN 1 ELSE 2 END,a.Product_Name";     

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<WorkPlan> item;
            item = (from DataRow row in dt.Rows
                    select new WorkPlan
                    {                  
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
              
                    }).ToList();
            return item;
        }

      

        public bool SaveUpdateWPVoid(WorkPlan model)
        {
            bool isTrue = false;

          
            string Qry = "Delete from WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and Shift_Name='" + model.ShiftName + "'  and DOCTOR_ID='" + model.DoctorID + "' and Product_Code='" + model.ProductCode + "'";
       
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                {
                    isTrue = true;
                    IUMode = "D";
                }
            
            
            return isTrue;
        }

        public bool SaveUpdateWPUpdate(WorkPlan model)
        {
            bool isTrue = false;
            string Qry = "";
        

            string IsExDetailSL = "Select * from VW_WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and Shift_Name='" + model.ShiftName + "' and DOCTOR_ID='" + model.DoctorID + "' and Product_Code='" + model.ProductCode + "'";
            var tupleIsExDetailSL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExDetailSL);

            if (tupleIsExDetailSL.Item1)
            {
                Qry = "Update WORK_PLAN Set QUANTITY=" + model.Quantity + " Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and Shift_Name='" + model.ShiftName + "' and DOCTOR_ID='" + model.DoctorID + "' and Product_Code='" + model.ProductCode + "'";
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                {
                    IUMode = "U";
                    isTrue = true;
                }
            }
           
       
           
        
        return isTrue;
        }

        public bool SaveUpdateWP(WorkPlan model)
        {
            bool isTrue = false; 
            SaveUpdateDVR(model);
          
            if (DoctorOnShift(model))
            {
               
                string IsExDetailSL = "Select * from WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and Shift_Name='" + model.ShiftName + "' and Product_Code='" + model.ProductCode + "'";
                var tupleIsExDetailSL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExDetailSL);

             
                    if (tupleIsExDetailSL.Item1)//When Master and Detail Exists
                    {
                      
                    }
                    else
                    {
                    string SubDTL = "Insert Into WORK_PLAN(SET_DATE, MP_GROUP, DOCTOR_ID, SHIFT_NAME, PRODUCT_CODE, QUANTITY, INST_NAME, MARKET_CODE) Values ('" + model.SetDate + "','" + model.MPGroup + "','" + model.DoctorID + "','" + model.ShiftName + "','" + model.ProductCode + "'," + model.Quantity + ",'" + model.DoctorName + "','" + model.MPGroup + "')";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                    {
                        IUMode = "I";
                        isTrue = true;
                    }

                }
                 

            }
           
            return isTrue;
        }


        public bool DoctorOnShift(WorkPlan model)
        {
            bool isTrue = false;
            string QryDoctor = "Select distinct Doctor_ID from VW_DOC_INTRN_INST_MPO_MAPPING Where MP_GROUP='" + model.MPGroup + "' AND Doctor_ID='" + model.DoctorID + "'  and lower(Substr(Shift_Name,1,1))='" + model.ShiftName + "'";

            DataTable dtDoctor = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryDoctor);
            if (dtDoctor.Rows.Count > 0)
            {
                isTrue = true;
              
            }
            return isTrue;
        }

        public bool SaveUpdateDVR(WorkPlan model)
        {
            bool isTrue = false;

            if (DoctorOnShift(model))
            {

            //TO_Date(DAY_NUMBER||'-'||Month_Number||'-'||Year,'dd-mm-yyyy')=To_Date('" + model.SetDate + "','dd-mm-yyyy') and

            string MonthYear = model.SetDate.Substring(3, 7);
            string MasterSLnStatus = "Select  MST_SL,MST_Status from DVR_MST  Where  MP_GROUP='" + model.MPGroup + "' and Month_Number||'-'||Year='" + MonthYear + "'";
            var tupleMasterSLnStatus = dbHelper.IsExistsWithGetSLnID(dbConn.SAConnStrReader(), MasterSLnStatus);


            string IsExMstDTL = "Select distinct B.SHIFT_NAME || B.DAY_NUMBER || A.MST_SL DTL_SL from DVR_MST A,DVR_DTL B Where A.MST_SL=B.MST_SL Where TO_Date(B.DAY_NUMBER||'-'||A.Month_Number||'-'||A.Year,'dd-mm-yyyy')=To_Date('" + model.SetDate + "','dd-mm-yyyy') and A.MP_GROUP='" + model.MPGroup + "' and B.Shift_Name='" + model.ShiftName + "'";
            var tupleIsExMstDTL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExMstDTL);

            if (tupleMasterSLnStatus.Item3 == "Approved")
            {
                if(tupleIsExMstDTL.Item1)
                {
                    string DTLQry = "Update DVR_DTL Set DTL_STATUS='" + tupleMasterSLnStatus.Item3 + "' Where Shift_Name||Day_number||MST_SL='" + tupleIsExMstDTL.Item2 + "'";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), DTLQry))
                    {
                        IUMode = "U";
                        isTrue = true;
                        string IsExDTL = "Select distinct C.Doctor_ID from  DVR_DTL B,DVR_SUB_DTL C Where B.SHIFT_NAME || B.DAY_NUMBER || B.MST_SL = C.DTL_SL AND C.DTL_SL='" + tupleIsExMstDTL.Item2 + "' and C.Doctor_ID='" + model.DoctorID + "'";
                        var tupleIsExDTL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExDTL);
                        if (tupleIsExDTL.Item1)
                        {
                            string SubDTL = "Update DVR_SUB_DTL Set SUB_DTL_STATUS='" + tupleMasterSLnStatus.Item3 + "' Where DTL_SL='" + tupleIsExMstDTL.Item2 + "' and Doctor_ID='" + model.DoctorID + "'";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                            {
                                IUMode = "U";
                                isTrue = true;
                            }
                        }
                        else
                        {
                            string SubDTL = "Insert Into DVR_SUB_DTL(DTL_SL,DOCTOR_ID,SUB_DTL_STATUS) Values ('" + tupleIsExMstDTL.Item2 + "','" + model.DoctorID + "','" + tupleMasterSLnStatus.Item3 + "')";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                            {
                                IUMode = "I";
                                isTrue = true;
                            }
                        }

                    }
                
                }
                else
                {
                    string DetailSL = model.ShiftName + model.SetDate.Substring(0, 2) + tupleMasterSLnStatus.Item2;
                    string DayNumber = model.SetDate.Substring(0, 2);
                    string SetTime = CntTime;
                    string DTLQry = "INSERT INTO DVR_DTL (MST_SL,DAY_NUMBER,SHIFT_NAME,SET_TIME,DTL_STATUS) VALUES(" + tupleMasterSLnStatus.Item2 + ",'" + DayNumber + "','" + model.ShiftName + "', '" + SetTime + "','" + tupleMasterSLnStatus.Item3 + "')";

                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), DTLQry))
                    {
                        string SubDTL = "Insert Into DVR_SUB_DTL(DTL_SL,DOCTOR_ID,Sub_DTL_STATUS,Addition) Values ('" + DetailSL + "','" + model.DoctorID + "','" + tupleMasterSLnStatus.Item3 + "','Yes')";
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                        {
                            IUMode = "I";
                            isTrue = true;
                        }
                    }
                }
                if (pushNotification.IsConnectedToInternet())
                {
                    string MonthName = Convert.ToDateTime(model.SetDate).ToString("MMMM");
                        pushNotification.SaveToDatabase(model.MPGroup, "DVR", "DVR Approved," + MonthName, "You requested monthly DVR for " + MonthName + " is Approved. Please sync it.", Convert.ToDateTime(model.SetDate).ToString("YYYY"), Convert.ToDateTime(model.SetDate).ToString("MM"));

                        pushNotification.SinglePushNotification(dbHelper.GetSingleToken(model.MPGroup), "DVR", "DVR Approved," + MonthName, "You requested monthly DVR for " + MonthName + " is Approved. Please sync it.");
                 
                
                }

            }
            else
            {


                if (tupleIsExMstDTL.Item1)
                {
                    string DTLQry = "Update DVR_DTL Set Shift_Name='" + model.ShiftName + "',DTL_STATUS='" + tupleMasterSLnStatus.Item3 + "' Where SHIFT_NAME || DAY_NUMBER || MST_SL='" + tupleIsExMstDTL.Item2 + "'";
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), DTLQry))
                    {
                        IUMode = "U";
                        string IsExDTL = "Select distinct C.Doctor_ID from  DVR_DTL B,DVR_SUB_DTL C Where B.SHIFT_NAME || B.DAY_NUMBER || B.MST_SL = C.DTL_SL AND C.DTL_SL='" + tupleIsExMstDTL.Item2 + "' and C.Doctor_ID='" + model.DoctorID + "'";
                        var tupleIsExDTL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExDTL);
                        if (tupleIsExDTL.Item1 == false)
                        {
                            string SubDTL = "Insert Into DVR_SUB_DTL(DTL_SL,DOCTOR_ID) Values ('" + tupleIsExMstDTL.Item2 + "','" + model.DoctorID + "')";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                            {
                                IUMode = "I";
                                isTrue = true;
                            }
                        }
                        else
                        {
                            IUMode = "U";
                            isTrue = true;
                        }
                    }
                }
                else
                {
                    string DetailSL = model.ShiftName + model.SetDate.Substring(0, 2) + tupleMasterSLnStatus.Item2;
                    string DayNumber = model.SetDate.Substring(0, 2);
                    string SetTime = CntTime;
                    string DTLQry = "INSERT INTO DVR_DTL (MST_SL,DAY_NUMBER,SHIFT_NAME,SET_TIME,DTL_STATUS) VALUES(" + tupleMasterSLnStatus.Item2 + ",'" + DayNumber + "','" + model.ShiftName + "', '" + SetTime + "','" + tupleMasterSLnStatus.Item3 + "')";

                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), DTLQry))
                    {
                        string SubDTL = "Insert Into DVR_SUB_DTL(DTL_SL,DOCTOR_ID,Sub_DTL_STATUS,Addition) Values ('" + DetailSL + "','" + model.DoctorID + "','" + tupleMasterSLnStatus.Item3 + "','Yes')";
                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), SubDTL))
                        {
                            IUMode = "I";
                            isTrue = true;
                        }
                    }
                }
            
            }

            }
            return isTrue;
        }

        public bool SaveUpdateDVRWP(DefaultParameterBEO model)
        {
            bool isTrue = false;

            int month = ((Convert.ToInt16(model.Year) - Convert.ToInt16(DateTime.Now.Year)) * 12) + Convert.ToInt16(model.MonthNumber) - Convert.ToInt16(DateTime.Now.Month);

            if (month >= 0)
            {
                if (DVRSaveUpdate(model))
                {
                    isTrue = true;
                }
               
            }
            return isTrue;
        }

        private bool DVRSaveUpdate(DefaultParameterBEO master)
        {
             bool isTrue = false;
             string QryIsExistsMST = "Select MST_SL from DVR_MST Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "' --and MST_STATUS='Approved'";
             var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);
             string QryTempDel = "Delete from  OP_DVR_VW Where MP_GROUP='" + master.MPGroup + "'";
            string QryTempIns = " Insert Into OP_DVR_VW (MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,DTL_STATUS,REVIEW,DTL_SL,DOCTOR_ID,Sub_DTL_STATUS)" +
                                " (Select MST_SL,MP_GROUP,YEAR,MONTH_NUMBER,MST_STATUS,DAY_NUMBER,SHIFT_NAME,DTL_STATUS,REVIEW,DTL_SL,DOCTOR_ID,Sub_DTL_STATUS from VW_DVR Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "')";

            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempDel);
             dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryTempIns);           
             if (tuple1.Item1)
             {                
                 string QryMaster = "Update  DVR_MST Set MST_STATUS='Approved'  Where MST_SL in (Select distinct  MST_SL from OP_DVR_VW Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "')";
                 string QryDetail = "Update DVR_DTL Set DTL_STATUS ='Approved' Where SHIFT_NAME||DAY_NUMBER||MST_SL in  (Select distinct SHIFT_NAME||DAY_NUMBER||MST_SL from OP_DVR_VW Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "')";
                 string QrySubDetail = "Update DVR_SUB_DTL Set Sub_DTL_STATUS='Approved' Where DTL_SL in (Select distinct DTL_SL from OP_DVR_VW Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "')";
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
                 string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(master.MonthNumber));
                pushNotification.SaveToDatabase(master.MPGroup, "DVR", "DVR Approved", "DVR of " + MonthName + " Approved.", master.Year, master.MonthNumber);
                pushNotification.SinglePushNotification(dbHelper.GetSingleToken(master.MPGroup), "DVR", "DVR Approved", "DVR of " + MonthName + " Approved.");

             }
            return isTrue;
        }




        public List<TourPlanningMaster> GetPopupView(TourPlanningMaster model)
        {
            string Qry = "SELECT distinct A.MST_SL,D.MPO_CODE,D.MPO_NAME,A.MP_GROUP,A.MONTH_NUMBER,F.MONTH_NAME,A.YEAR,A.MST_STATUS from DVR_MST A,DVR_DTL B,VW_HR_LOC_MAPPING D,GEN_MONTH F  ";
            Qry = Qry + " Where A.MST_SL=B.MST_SL AND A.MP_GROUP=D.MP_GROUP  AND A.MONTH_NUMBER = F.MONTH_NUMBER ";
            if (model.Year != null && model.Year != "" && model.MonthNumber != null && model.MonthNumber != "")
            {
                Qry = Qry + " And A.YEAR=" + model.Year + " and A.MONTH_NUMBER='" + model.MonthNumber + "'";
            }
            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }          
         
            Qry = Qry + " Order by D.MPO_NAME";
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
                       

                    }).ToList();
            return item;
        }

        public bool DeleteDVRWP(WorkPlan model)
        {
            bool isTrue = false;
            if (DeleteWP(model))
            {
                isTrue = true;
            }
            if (DeleteDVR(model))
            {
                isTrue = true;
            }           
            return isTrue;
        }

        private bool DeleteDVR(WorkPlan model)
        {
            bool isTrue = false;

            string IsExMst = "Select B.SHIFT_NAME || B.DAY_NUMBER || A.MST_SL DTL_SL from  DVR_MST A,DVR_DTL B Where A.MST_SL=B.MST_SL AND TO_Date(B.DAY_NUMBER||'-'||A.MONTH_NUMBER||'-'||A.YEAR,'dd-mm-yyyy')=To_Date('" + model.SetDate + "','dd-mm-yyyy') and A.MP_GROUP='" + model.MPGroup + "' and B.Shift_Name='" + model.ShiftName + "'";
            var tupleIsExMst = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExMst);
            if(tupleIsExMst.Item1)
            {            
            string QryDetail = "Delete from DVR_DTL Where SHIFT_NAME||DAY_NUMBER||MST_SL='" + tupleIsExMst.Item2 + "'";
            string QrySubDetail = "Delete from DVR_SUB_DTL Where DTL_SL ='" + tupleIsExMst.Item2 + "'";
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QrySubDetail);
            dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail);
            isTrue = true;
            }
            return isTrue;
        }

        private bool DeleteWP(WorkPlan model)
        {
            bool isTrue = false;        
          
            string IsExMasterSL = "Select * from WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and Shift_Name='" + model.ShiftName + "' and DOCTOR_ID='" + model.DoctorID + "' and Product_Code='" + model.ProductCode + "'";
            var tupleIsExMasterSL = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), IsExMasterSL);
            if (tupleIsExMasterSL.Item1)
            {
                isTrue = true;     
             
                string delMST = "Delete from  WORK_PLAN  Where TO_Char(SET_DATE,'dd-mm-yyyy')='" + model.SetDate + "' and MP_GROUP='" + model.MPGroup + "' and Shift_Name='" + model.ShiftName + "' and DOCTOR_ID='" + model.DoctorID + "' and Product_Code='" + model.ProductCode + "'";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), delMST);
                IUMode = "D";
            }

            return isTrue;
        }

        

        public bool DeleteOnlyWP(WorkPlan model)
        {
            bool isTrue = false;
            if (DeleteWP(model))
            {
                isTrue = true;
            }
            return isTrue;
        }
      
        public Tuple<string, DataTable, List<ReportDVRBEL>> Export(DefaultParameterBEO model)
        {
            if (model.MasterStatus == " " || model.MasterStatus == "" || model.MasterStatus == null)
            {
                string QryStatus = "Select  MST_STATUS from DVR_MST Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "'  and MP_GROUP='" + model.MPGroup + "'";
                model.MasterStatus = dbHelper.GetValue(QryStatus);
            }
            string vHeader = "";
            string Qry = " Select DOCTOR_ID,DOCTOR_NAME ,MPO_CODE, MPO_NAME, MP_GROUP,POTENTIAL,SHIFT_NAME," +
          " md01, md02, md03, md04, md05, md06, md07, md08, md09, md10, md11, md12, md13, md14, md15, md16, md17, md18, md19, md20, md21, md22, md23, md24, md25, md26, md27, md28, md29, md30, md31," +
          " ed01, ed02, ed03, ed04, ed05, ed06, ed07, ed08, ed09, ed10, ed11, ed12, ed13, ed14, ed15, ed16, ed17, ed18, ed19, ed20, ed21, ed22, ed23, ed24, ed25, ed26, ed27, ed28, ed29, ed30, ed31," +

        "   CASE WHEN md01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md31 LIKE '%D%' THEN 1 ELSE 0 END MD,  " +
        "   CASE WHEN ed01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%D%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed31 LIKE '%D%' THEN 1 ELSE 0END ED,  " +

        "   CASE WHEN md01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md31 LIKE '%P%' THEN 1 ELSE 0 END MP,  " +
        "   CASE WHEN ed01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%P%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed31 LIKE '%P%' THEN 1 ELSE 0END EP,  " +


        "   CASE WHEN md01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md31 LIKE '%E%' THEN 1 ELSE 0 END ME,  " +
        "   CASE WHEN ed01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%E%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed31 LIKE '%E%' THEN 1 ELSE 0END EE,  " +

         "  CASE WHEN md01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN md31 LIKE '%A%' THEN 1 ELSE 0 END MA,  " +
        "   CASE WHEN ed01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%A%' THEN 1 ELSE 0 END " +
        " + CASE WHEN ed31 LIKE '%A%' THEN 1 ELSE 0END EA  " +

        " From VW_DVR_RPT_V03 Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + " " + model.Year;
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF : " + model.MPOName;
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
                Qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
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
            if (model.MasterStatus != "" && model.MasterStatus != null)
            {
                if (model.MasterStatus == "Approved")
                {
                    Qry = Qry + " and  SubStr(MST_Status,1,8)='" + model.MasterStatus + "'";
                }
                if (model.MasterStatus == "Waiting")
                {
                    Qry = Qry + " and  SubStr(MST_Status,1,7)='" + model.MasterStatus + "'";
                }
            }
            if (model.MasterStatus == "Waiting")
            {
                Qry = Qry + " AND WAITING_STATUS IS NOT NULL";
            }         
            Qry = Qry + " Order by DOCTOR_ID,POTENTIAL";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDVRBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDVRBEL
                    {

                        SL = row["Col1"].ToString(),
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        Potential = row["POTENTIAL"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MPOCode = row["MPO_Code"].ToString(),
                    
                        MPGroup = row["MP_Group"].ToString(),
                        MD = row["MD"].ToString(),
                        MED = Convert.ToString(Convert.ToInt16(row["MD"].ToString()) + Convert.ToInt16(row["ED"].ToString())),
                        //Add New
                        MEP = Convert.ToString(Convert.ToInt16(row["MP"].ToString()) + Convert.ToInt16(row["EP"].ToString())),
                        MEE = Convert.ToString(Convert.ToInt16(row["ME"].ToString()) + Convert.ToInt16(row["EE"].ToString())),
                        MEA = Convert.ToString(Convert.ToInt16(row["MA"].ToString()) + Convert.ToInt16(row["EA"].ToString())),


                        md01 = row["md01"].ToString(),
                        md02 = row["md02"].ToString(),
                        md03 = row["md03"].ToString(),
                        md04 = row["md04"].ToString(),
                        md05 = row["md05"].ToString(),
                        md06 = row["md06"].ToString(),
                        md07 = row["md07"].ToString(),
                        md08 = row["md08"].ToString(),
                        md09 = row["md09"].ToString(),
                        md10 = row["md10"].ToString(),
                        md11 = row["md11"].ToString(),
                        md12 = row["md12"].ToString(),
                        md13 = row["md13"].ToString(),
                        md14 = row["md14"].ToString(),
                        md15 = row["md15"].ToString(),
                        md16 = row["md16"].ToString(),
                        md17 = row["md17"].ToString(),
                        md18 = row["md18"].ToString(),
                        md19 = row["md19"].ToString(),
                        md20 = row["md20"].ToString(),
                        md21 = row["md21"].ToString(),
                        md22 = row["md22"].ToString(),
                        md23 = row["md23"].ToString(),
                        md24 = row["md24"].ToString(),
                        md25 = row["md25"].ToString(),
                        md26 = row["md26"].ToString(),
                        md27 = row["md27"].ToString(),
                        md28 = row["md28"].ToString(),
                        md29 = row["md29"].ToString(),
                        md30 = row["md30"].ToString(),
                        md31 = row["md31"].ToString(),

                        ED = row["ED"].ToString(),
                        ed01 = row["ed01"].ToString(),
                        ed02 = row["ed02"].ToString(),
                        ed03 = row["ed03"].ToString(),
                        ed04 = row["ed04"].ToString(),
                        ed05 = row["ed05"].ToString(),
                        ed06 = row["ed06"].ToString(),
                        ed07 = row["ed07"].ToString(),
                        ed08 = row["ed08"].ToString(),
                        ed09 = row["ed09"].ToString(),
                        ed10 = row["ed10"].ToString(),
                        ed11 = row["ed11"].ToString(),
                        ed12 = row["ed12"].ToString(),
                        ed13 = row["ed13"].ToString(),
                        ed14 = row["ed14"].ToString(),
                        ed15 = row["ed15"].ToString(),
                        ed16 = row["ed16"].ToString(),
                        ed17 = row["ed17"].ToString(),
                        ed18 = row["ed18"].ToString(),
                        ed19 = row["ed19"].ToString(),
                        ed20 = row["ed20"].ToString(),
                        ed21 = row["ed21"].ToString(),
                        ed22 = row["ed22"].ToString(),
                        ed23 = row["ed23"].ToString(),
                        ed24 = row["ed24"].ToString(),
                        ed25 = row["ed25"].ToString(),
                        ed26 = row["ed26"].ToString(),
                        ed27 = row["ed27"].ToString(),
                        ed28 = row["ed28"].ToString(),
                        ed29 = row["ed29"].ToString(),
                        ed30 = row["ed30"].ToString(),
                        ed31 = row["ed31"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }


        public List<ReportDVRBEL> GetGrandTotalSum(DefaultParameterBEO model, DataTable dtDtl)
        {
            var newDt = dtDtl.AsEnumerable()
               // .GroupBy(r => r.Field<string>("MP_GROUP")) //  OR
               //.GroupBy(g => new { Col1 = g["MP_GROUP"] }) //  OR
               .GroupBy(g => new { Col1 = "" })   //Where Group is Empty Group
                   .Select(g =>
                   {
                       var row = dtDtl.NewRow();
                       row["MP_GROUP"] = g.Key;
                       row["MD"] = g.Sum(r => (decimal)r["MD"]);
                       row["ED"] = g.Sum(r => (decimal)r["ED"]);
                       row["MP"] = g.Sum(r => (decimal)r["MP"]);
                       row["EP"] = g.Sum(r => (decimal)r["EP"]);
                       row["ME"] = g.Sum(r => (decimal)r["ME"]);
                       row["EE"] = g.Sum(r => (decimal)r["EE"]);
                       row["MA"] = g.Sum(r => (decimal)r["MA"]);
                       row["EA"] = g.Sum(r => (decimal)r["EA"]);

                       return row;
                   }).CopyToDataTable();

            DataTable dt = newDt;
            List<ReportDVRBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDVRBEL
                    {
                        MED = Convert.ToString(Convert.ToInt16(row["MD"].ToString()) + Convert.ToInt16(row["ED"].ToString())),
                        MEP = Convert.ToString(Convert.ToInt16(row["MP"].ToString()) + Convert.ToInt16(row["EP"].ToString())),
                        MEE = Convert.ToString(Convert.ToInt16(row["ME"].ToString()) + Convert.ToInt16(row["EE"].ToString())),
                        MEA = Convert.ToString(Convert.ToInt16(row["MA"].ToString()) + Convert.ToInt16(row["EA"].ToString())),
                        MD = row["MD"].ToString(),
                        ED = row["ED"].ToString(),

                        DoctorName = "Grand Total: ",
                        md01 = "",
                        md02 = "",
                        md03 = "",
                        md04 = "",
                        md05 = "",
                        md06 = "",
                        md07 = "",
                        md08 = "",
                        md09 = "",
                        md10 = "",
                        md11 = "",
                        md12 = "",
                        md13 = "",
                        md14 = "",
                        md15 = "",
                        md16 = "",
                        md17 = "",
                        md18 = "",
                        md19 = "",
                        md20 = "",
                        md21 = "",
                        md22 = "",
                        md23 = "",
                        md24 = "",
                        md25 = "",
                        md26 = "",
                        md27 = "",
                        md28 = "",
                        md29 = "",
                        md30 = "",
                        md31 = "",
                        ed01 = "",
                        ed02 = "",
                        ed03 = "",
                        ed04 = "",
                        ed05 = "",
                        ed06 = "",
                        ed07 = "",
                        ed08 = "",
                        ed09 = "",
                        ed10 = "",
                        ed11 = "",
                        ed12 = "",
                        ed13 = "",
                        ed14 = "",
                        ed15 = "",
                        ed16 = "",
                        ed17 = "",
                        ed18 = "",
                        ed19 = "",
                        ed20 = "",
                        ed21 = "",
                        ed22 = "",
                        ed23 = "",
                        ed24 = "",
                        ed25 = "",
                        ed26 = "",
                        ed27 = "",
                        ed28 = "",
                        ed29 = "",
                        ed30 = "",
                        ed31 = "",
                    }).ToList();
            return item;
        }


    }
}
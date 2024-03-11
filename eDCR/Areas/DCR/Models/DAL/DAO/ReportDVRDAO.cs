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
    public class ReportDVRDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();
        public string IsDvrStatus { get;  set; }
        public string DvrWpDcrStatus { get; set; }
        public Tuple<DataTable, List<ReportDVRBEL>> GetMainGridData(DefaultParameterBEO model)
        {
            string Qry =" Select DOCTOR_ID,DOCTOR_NAME ,MPO_CODE, MPO_NAME, MP_GROUP,POTENTIAL,SHIFT_NAME," +
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
                   " + CASE WHEN ed31 LIKE '%D%' THEN 1 ELSE 0 END ED,  " +

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
                   " + CASE WHEN ed31 LIKE '%P%' THEN 1 ELSE 0 END EP,  " +


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
                   " + CASE WHEN ed31 LIKE '%E%' THEN 1 ELSE 0 END EE,  " +

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
                   " + CASE WHEN ed31 LIKE '%A%' THEN 1 ELSE 0 END EA  " +

                   " From VW_DVR_RPT_V03 Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";

            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
           
            if (model.MPGroup != "" && model.MPGroup!=null)
            {
             Qry=Qry+"and MP_GROUP='" + model.MPGroup + "'";
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


            return Tuple.Create(dt, item);
         
        }

        public Tuple<string, DataTable, List<ReportDVRBEL>> Export(DefaultParameterBEO model)
        {
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
                   " + CASE WHEN ed31 LIKE '%D%' THEN 1 ELSE 0 END ED,  " +

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
                   " + CASE WHEN ed31 LIKE '%P%' THEN 1 ELSE 0 END EP,  " +


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
                   " + CASE WHEN ed31 LIKE '%E%' THEN 1 ELSE 0 END EE,  " +

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
                   " + CASE WHEN ed31 LIKE '%A%' THEN 1 ELSE 0 END EA  " +
                  " From VW_DVR_RPT_V03 Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";



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
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
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


            return Tuple.Create(vHeader, dt, item);
        }


        public List<ReportDVRBEL> GetGrandTotalSum(DefaultParameterBEO model,DataTable dtDtl)
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

                         DoctorName="Grand Total: ",
                         md01 ="",
                         md02 ="",
                         md03 ="",
                         md04 ="",
                         md05 ="",
                         md06 ="",
                         md07 ="",
                         md08 ="",
                         md09 ="",
                         md10 ="",
                         md11 ="",
                         md12 ="",
                         md13 ="",
                         md14 ="",
                         md15 ="",
                         md16 ="",
                         md17 ="",
                         md18 ="",
                         md19 ="",
                         md20 ="",
                         md21 ="",
                         md22 ="",
                         md23 ="",
                         md24 ="",
                         md25 ="",
                         md26 ="",
                         md27 ="",
                         md28 ="",
                         md29 ="",
                         md30 ="",
                         md31 ="",
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



   

        public List<WorkPlan> GetDvrWpDcr(DefaultParameterBEO model)
        {
        

            IsDvrStatus = "";
            string DayNumber = model.DayNumber.Length == 1 ? "0" + model.DayNumber : model.DayNumber;
            string SetDate = DayNumber + "-" + model.MonthNumber + "-" + model.Year;
            SetDate = dateFormat.StringDateDdMonYYYY(SetDate);



            string QryDvr = "Select distinct MP_GROUP,DAY_NUMBER||'-'||MONTH_NUMBER||'-'||Year SET_DATE, Doctor_ID,Doctor_Name "+
                " from  VW_DVR  "+
                " Where TO_Date(DAY_NUMBER||'-'||Month_Number||'-'||Year,'dd-mm-yyyy')='" + SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";
           
          

            string QryWp = "Select distinct TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,MP_GROUP,PRODUCT_CODE,PRODUCT_NAME,QUANTITY ,'WP' Type " +
                " from VW_WORK_PLAN  "+
                " Where SET_DATE='" + SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";


            string QryDcr = "Select distinct TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,MP_GROUP,PRODUCT_CODE,PRODUCT_NAME,QUANTITY,'DCR' Type "+
                " from  VW_DCR "+
                " Where SET_DATE='" + SetDate + "' and MP_GROUP='" + model.MPGroup + "' and DOCTOR_ID='" + model.DoctorID + "' and SHIFT_NAME='" + model.ShiftName + "'";

            DataTable dtDvr = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryDvr);
            DataTable dtWp = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryWp);
            DataTable dtDcr = dbHelper.GetDataTable(dbConn.SAConnStrReader(), QryDcr);

            IsDvrStatus = dtDvr.Rows.Count > 0 ? "DVR: YES" : "DVR: NO";
            DvrWpDcrStatus = (dtDvr.Rows.Count > 0 || dtWp.Rows.Count > 0 || dtDcr.Rows.Count > 0) ? "YES" : "NO";

          var  itemDvr = (from DataRow row in dtDvr.Rows
                    select new WorkPlan
                    {
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),

                    }).ToList();


          var  itemWp = (from DataRow row in dtWp.Rows
                    select new WorkPlan
                    {
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        Type = row["Type"].ToString(),

                    }).ToList();



           var itemDcr = (from DataRow row in dtDcr.Rows
                    select new WorkPlan
                    {
                        MPGroup = row["MP_GROUP"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        Type = row["Type"].ToString(),

                    }).ToList();




            itemWp = itemWp.Concat(itemDcr).ToList();

            return itemWp;
        }
        public Tuple<string, DataTable, List<ReportDvrWpDcrBEL>> ExportDvrWpDcrSummary(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = @" Select  A.DOCTOR_ID, A.DOCTOR_NAME, A.MP_GROUP, A.SHIFT_NAME, A.YM, A.MD, A.ED, A.MP, A.EP, A.ME, A.EE,A.MA, A.EA, B.PRODUCT_GROUP,
                            B.MARKET_CODE,B.MARKET_NAME,B.REGION_CODE,B.REGION_NAME
                            from MVN_DVR_WP_DCR_MANUAL A,VW_HR_LOC_MAPPING B 
                            Where A.MP_GROUP=B.MP_GROUP AND  A.YM='" + model.Year+ model.MonthNumber + "' AND B.PRODUCT_GROUP='"+model.SBU+"'";



            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month: " + month + ", " + model.Year+", SBU: "+ model.SBU;
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND A.MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                //Qry += " And B.TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                Qry += " And B.REGION_CODE = '" + model.RegionCode + "'";
            }

            Qry = Qry + " Order by A.DOCTOR_ID";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDvrWpDcrBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDvrWpDcrBEL
                    {
                        SL = row["Col1"].ToString(),
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),                
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MarketCode = row["MARKET_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                      
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        MD = row["MD"].ToString(),
                        ED = row["ED"].ToString(),
                        MP = row["MP"].ToString(),
                        EP = row["EP"].ToString(),
                        ME = row["ME"].ToString(),
                        EE = row["EE"].ToString(),
                        MA = row["MA"].ToString(),
                        EA = row["EA"].ToString(),
                        MED = Convert.ToString(Convert.ToInt16(row["MD"].ToString()) + Convert.ToInt16(row["ED"].ToString())),
                  
                        MEP = Convert.ToString(Convert.ToInt16(row["MP"].ToString()) + Convert.ToInt16(row["EP"].ToString())),
                        MEE = Convert.ToString(Convert.ToInt16(row["ME"].ToString()) + Convert.ToInt16(row["EE"].ToString())),
                        MEA = Convert.ToString(Convert.ToInt16(row["MA"].ToString()) + Convert.ToInt16(row["EA"].ToString())),
                        

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

    }
}
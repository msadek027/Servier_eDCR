using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL
{
    public class ReportMPOWiseMonthlyDCRDAO : ReturnData
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportMPOWiseMonthlyDCRBEO> GetData(DefaultParameterBEO model)
        {

            string Qry =" SELECT  DOCTOR_ID,DOCTOR_NAME ||', '||SPECIALIZATION DOCTOR_NAME, POTENTIAL,TOTAL_PLAN,  " +
                        " D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31," +
                        " CASE WHEN d01='.' THEN 0 ELSE 1 END + CASE WHEN d02='.' THEN 0 ELSE 1 END+ CASE WHEN d03='.' THEN 0 ELSE 1 END + CASE WHEN d04='.' THEN 0 ELSE 1 END+ CASE WHEN d05='.' THEN 0 ELSE 1 END" +
                        " + CASE WHEN d06='.' THEN 0 ELSE 1 END + CASE WHEN d07='.' THEN 0 ELSE 1 END+ CASE WHEN d08='.' THEN 0 ELSE 1 END + CASE WHEN d09='.' THEN 0 ELSE 1 END+ CASE WHEN d10='.' THEN 0 ELSE 1 END + CASE WHEN d11='.' THEN 0 ELSE 1 END	   " +
                        " + CASE WHEN d12='.' THEN 0 ELSE 1 END + CASE WHEN d13='.' THEN 0 ELSE 1 END+ CASE WHEN d14='.' THEN 0 ELSE 1 END + CASE WHEN d15='.' THEN 0 ELSE 1 END+ CASE WHEN d16='.' THEN 0 ELSE 1 END + CASE WHEN d17='.' THEN 0 ELSE 1 END  " +
                        " + CASE WHEN d18='.' THEN 0 ELSE 1 END + CASE WHEN d19='.' THEN 0 ELSE 1 END+ CASE WHEN d20='.' THEN 0 ELSE 1 END + CASE WHEN d21='.' THEN 0 ELSE 1 END+ CASE WHEN d22='.' THEN 0 ELSE 1 END + CASE WHEN d23='.' THEN 0 ELSE 1 END  " +
                        " + CASE WHEN d24='.' THEN 0 ELSE 1 END + CASE WHEN d25='.' THEN 0 ELSE 1 END+ CASE WHEN d26='.' THEN 0 ELSE 1 END + CASE WHEN d27='.' THEN 0 ELSE 1 END+ CASE WHEN d28='.' THEN 0 ELSE 1 END + CASE WHEN d29='.' THEN 0 ELSE 1 END  " +
                        " + CASE WHEN d30='.' THEN 0 ELSE 1 END + CASE WHEN d31='.' THEN 0 ELSE 1 END DCR  " +
                        " FROM VW_DCR_DAY_WISE_MONTHLY " +
                        " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportMPOWiseMonthlyDCRBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseMonthlyDCRBEO
                    {
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Potential = row["POTENTIAL"].ToString(),
                        d01 = row["d01"].ToString(),
                        d02 = row["d02"].ToString(),
                        d03 = row["d03"].ToString(),
                        d04 = row["d04"].ToString(),
                        d05 = row["d05"].ToString(),
                        d06 = row["d06"].ToString(),
                        d07 = row["d07"].ToString(),
                        d08 = row["d08"].ToString(),
                        d09 = row["d09"].ToString(),
                        d10 = row["d10"].ToString(),
                        d11 = row["d11"].ToString(),
                        d12 = row["d12"].ToString(),
                        d13 = row["d13"].ToString(),
                        d14 = row["d14"].ToString(),
                        d15 = row["d15"].ToString(),
                        d16 = row["d16"].ToString(),
                        d17 = row["d17"].ToString(),
                        d18 = row["d18"].ToString(),
                        d19 = row["d19"].ToString(),
                        d20 = row["d20"].ToString(),
                        d21 = row["d21"].ToString(),
                        d22 = row["d22"].ToString(),
                        d23 = row["d23"].ToString(),
                        d24 = row["d24"].ToString(),
                        d25 = row["d25"].ToString(),
                        d26 = row["d26"].ToString(),
                        d27 = row["d27"].ToString(),
                        d28 = row["d28"].ToString(),
                        d29 = row["d29"].ToString(),
                        d30 = row["d30"].ToString(),
                        d31 = row["d31"].ToString(),

                        TotalDCR = row["DCR"].ToString(),
                        TotalPLAN = row["TOTAL_PLAN"].ToString(),
                        Achi = row["TOTAL_PLAN"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["DCR"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_PLAN"].ToString())).ToString("#.##"),

                      

                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportMPOWiseMonthlyDCRBEO>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = "SELECT  DOCTOR_ID,DOCTOR_NAME ||', '||SPECIALIZATION DOCTOR_NAME, POTENTIAL,TOTAL_PLAN,  " +
             " D01, D02, D03, D04, D05, D06, D07, D08, D09, D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D20, D21, D22, D23, D24, D25, D26, D27, D28, D29, D30, D31," +
             " CASE WHEN d01='.' THEN 0 ELSE 1 END + CASE WHEN d02='.' THEN 0 ELSE 1 END+ CASE WHEN d03='.' THEN 0 ELSE 1 END + CASE WHEN d04='.' THEN 0 ELSE 1 END+ CASE WHEN d05='.' THEN 0 ELSE 1 END" +
             " + CASE WHEN d06='.' THEN 0 ELSE 1 END + CASE WHEN d07='.' THEN 0 ELSE 1 END+ CASE WHEN d08='.' THEN 0 ELSE 1 END + CASE WHEN d09='.' THEN 0 ELSE 1 END+ CASE WHEN d10='.' THEN 0 ELSE 1 END + CASE WHEN d11='.' THEN 0 ELSE 1 END	   " +
             " + CASE WHEN d12='.' THEN 0 ELSE 1 END + CASE WHEN d13='.' THEN 0 ELSE 1 END+ CASE WHEN d14='.' THEN 0 ELSE 1 END + CASE WHEN d15='.' THEN 0 ELSE 1 END+ CASE WHEN d16='.' THEN 0 ELSE 1 END + CASE WHEN d17='.' THEN 0 ELSE 1 END  " +
             " + CASE WHEN d18='.' THEN 0 ELSE 1 END + CASE WHEN d19='.' THEN 0 ELSE 1 END+ CASE WHEN d20='.' THEN 0 ELSE 1 END + CASE WHEN d21='.' THEN 0 ELSE 1 END+ CASE WHEN d22='.' THEN 0 ELSE 1 END + CASE WHEN d23='.' THEN 0 ELSE 1 END  " +
             " + CASE WHEN d24='.' THEN 0 ELSE 1 END + CASE WHEN d25='.' THEN 0 ELSE 1 END+ CASE WHEN d26='.' THEN 0 ELSE 1 END + CASE WHEN d27='.' THEN 0 ELSE 1 END+ CASE WHEN d28='.' THEN 0 ELSE 1 END + CASE WHEN d29='.' THEN 0 ELSE 1 END  " +
             " + CASE WHEN d30='.' THEN 0 ELSE 1 END + CASE WHEN d31='.' THEN 0 ELSE 1 END DCR  " +
             " FROM VW_DCR_DAY_WISE_MONTHLY " +
             " WHERE  Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";


            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));
            vHeader = vHeader + "Month: " + month + ", " + model.Year;


            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", FF : " + model.MPOName;
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
                Qry += " And TSM_ID= '" + model.TerritoryManagerID + "'";
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
     

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportMPOWiseMonthlyDCRBEO> item;

            item = (from DataRow row in dt.Rows
                    select new ReportMPOWiseMonthlyDCRBEO
                    {
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Potential = row["POTENTIAL"].ToString(),
                        d01 = row["d01"].ToString(),
                        d02 = row["d02"].ToString(),
                        d03 = row["d03"].ToString(),
                        d04 = row["d04"].ToString(),
                        d05 = row["d05"].ToString(),
                        d06 = row["d06"].ToString(),
                        d07 = row["d07"].ToString(),
                        d08 = row["d08"].ToString(),
                        d09 = row["d09"].ToString(),
                        d10 = row["d10"].ToString(),
                        d11 = row["d11"].ToString(),
                        d12 = row["d12"].ToString(),
                        d13 = row["d13"].ToString(),
                        d14 = row["d14"].ToString(),
                        d15 = row["d15"].ToString(),
                        d16 = row["d16"].ToString(),
                        d17 = row["d17"].ToString(),
                        d18 = row["d18"].ToString(),
                        d19 = row["d19"].ToString(),
                        d20 = row["d20"].ToString(),
                        d21 = row["d21"].ToString(),
                        d22 = row["d22"].ToString(),
                        d23 = row["d23"].ToString(),
                        d24 = row["d24"].ToString(),
                        d25 = row["d25"].ToString(),
                        d26 = row["d26"].ToString(),
                        d27 = row["d27"].ToString(),
                        d28 = row["d28"].ToString(),
                        d29 = row["d29"].ToString(),
                        d30 = row["d30"].ToString(),
                        d31 = row["d31"].ToString(),

                        TotalDCR = row["DCR"].ToString(),
                        TotalPLAN = row["TOTAL_PLAN"].ToString(),
                        Achi = row["TOTAL_PLAN"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["DCR"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_PLAN"].ToString())).ToString("#.##"),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
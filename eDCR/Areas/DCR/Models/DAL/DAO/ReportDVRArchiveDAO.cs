using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System.Globalization;

using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportDVRArchiveDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();

        public List<ReportDVRArchiveBEL> GetDvrSummary(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string qry = "select MARKET_NAME,MP_GROUP,MPO_NAME,MPO_CODE,TO_CHAR(SET_DATE, 'dd-mm-yyyy') SET_DATE,to_char(SET_DATE, 'DAY') DayName,"+
                  " SUM(TOTAL_DOT) TOTAL_DOT,SUM(M_DOT) M_DOT,SUM(E_DOT) E_DOT,SUM(INTERN_DOT) INTERN_DOT" +
                  " from VW_DVR_SUMMARY WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'"; 
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
               
            }            
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {               
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " AND REGION_CODE='" + model.RegionCode + "'";                
            }
            qry += " GROUP BY MARKET_NAME,MP_GROUP,MPO_NAME,MPO_CODE,SET_DATE";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportDVRArchiveBEL> item;
             item = (from DataRow row in dt.Rows
                    select new ReportDVRArchiveBEL
                    {
                       
                        Date = row["SET_DATE"].ToString(),
                        DayName = row["DayName"].ToString(),                    
                        MarketName = row["MARKET_NAME"].ToString(),                      
                        MPGroup = row["MP_GROUP"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MorningDOT = row["M_DOT"].ToString(),
                        EveningDOT = row["E_DOT"].ToString(),
                        TotalDOT = row["TOTAL_DOT"].ToString(),
                        InternDOT = row["INTERN_DOT"].ToString(),
                    }).ToList();
            return item;
        }
        public Tuple<string, DataTable, List<ReportDVRArchiveBEL>> Export(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string vHeader = "";
            string qry = "select MARKET_NAME,MP_GROUP,MPO_NAME,MPO_CODE,TO_CHAR(SET_DATE, 'dd/mm/yyyy') SET_DATE,to_char(SET_DATE, 'DAY') DayName," +
                     " SUM(TOTAL_DOT) TOTAL_DOT,SUM(M_DOT) M_DOT,SUM(E_DOT) E_DOT,SUM(INTERN_DOT) INTERN_DOT" +
                     " from VW_DVR_SUMMARY WHERE SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'";
            vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;
              
            
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                qry += " And TERRITORY_CODE ='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
               
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                qry += " AND REGION_CODE='" + model.RegionCode + "'";
            }

            qry += " GROUP BY MARKET_NAME,MP_GROUP,MPO_NAME,MPO_CODE,SET_DATE";


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
           
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDVRArchiveBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDVRArchiveBEL
                    {
                        SL = row["Col1"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        DayName = row["DayName"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MorningDOT = row["M_DOT"].ToString(),
                        EveningDOT = row["E_DOT"].ToString(),
                        TotalDOT = row["TOTAL_DOT"].ToString(),
                        InternDOT = row["INTERN_DOT"].ToString(),
                    }).ToList();
            return Tuple.Create(vHeader, dt,item);
        }






        public Tuple<string, DataTable, List<ArchiveReportDVRBEL>> ExportDvrMonthly(DefaultParameterBEO model)
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

            string Qry2 = " Select DOCTOR_ID, DOCTOR_NAME, MARKET_NAME, MP_GROUP, SHIFT_NAME, SUM(MD) MD, SUM(ED) ED, SUM(MP) MP, SUM(EP) EP, SUM(ME) ME, SUM(EE) EE, SUM(MA) MA, SUM(EA) EA" +
 " from(Select DOCTOR_ID, DOCTOR_NAME, MARKET_NAME, MP_GROUP, SHIFT_NAME," +
 " CASE WHEN md01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN md31 LIKE '%D%' THEN 1 ELSE 0 END MD," +
 " CASE WHEN ed01 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed07 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed13 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed19 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed25 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%D%' THEN 1 ELSE 0 END + CASE WHEN ed31 LIKE '%D%' THEN 1 ELSE 0 END ED," +
 " CASE WHEN md01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN md31 LIKE '%P%' THEN 1 ELSE 0 END MP," +
 " CASE WHEN ed01 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed07 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed13 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed19 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed25 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%P%' THEN 1 ELSE 0 END + CASE WHEN ed31 LIKE '%P%' THEN 1 ELSE 0 END EP," +
 " CASE WHEN md01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN md31 LIKE '%E%' THEN 1 ELSE 0 END ME," +
 " CASE WHEN ed01 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed07 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed13 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed19 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed25 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%E%' THEN 1 ELSE 0 END + CASE WHEN ed31 LIKE '%E%' THEN 1 ELSE 0 END EE," +
 " CASE WHEN md01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md06 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md12 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md18 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md24 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md30 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN md31 LIKE '%A%' THEN 1 ELSE 0 END MA," +
 " CASE WHEN ed01 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed02 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed03 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed04 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed05 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed06 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed07 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed08 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed09 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed10 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed11 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed12 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed13 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed14 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed15 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed16 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed17 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed18 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed19 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed20 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed21 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed22 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed23 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed24 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed25 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed26 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed27 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed28 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed29 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed30 LIKE '%A%' THEN 1 ELSE 0 END + CASE WHEN ed31 LIKE '%A%' THEN 1 ELSE 0 END EA " +
 " From VW_DVR_RPT_V03 Where Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and REGION_CODE = '" + model.RegionCode + "') Group By DOCTOR_ID,DOCTOR_NAME ,MARKET_NAME, MP_GROUP,SHIFT_NAME";



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


            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry2);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ArchiveReportDVRBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ArchiveReportDVRBEL
                    {
                        SL = row["Col1"].ToString(),
                        DoctorID = row["Doctor_ID"].ToString(),
                        DoctorName = row["Doctor_Name"].ToString(),
                     
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                       
                        MPGroup = row["MP_Group"].ToString(),
                        MD = row["MD"].ToString(),
                        MED = Convert.ToString(Convert.ToInt16(row["MD"].ToString()) + Convert.ToInt16(row["ED"].ToString())),
                        //Add New
                        MEP = Convert.ToString(Convert.ToInt16(row["MP"].ToString()) + Convert.ToInt16(row["EP"].ToString())),
                        MEE = Convert.ToString(Convert.ToInt16(row["ME"].ToString()) + Convert.ToInt16(row["EE"].ToString())),
                        MEA = Convert.ToString(Convert.ToInt16(row["MA"].ToString()) + Convert.ToInt16(row["EA"].ToString())),
                       

                        ED = row["ED"].ToString(),
                       

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

    }
}
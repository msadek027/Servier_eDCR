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
    public class ReportDoctorCoverageDAO 
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        public List<ReportDoctorCoverageBEL> GetDoctorCoverage(DefaultParameterBEO model)
        {
            string Qry =@" SELECT  MARKET_NAME,MPO_CODE, MPO_NAME,
                          NVL(SUM(TOTAL_DOC),0) TOTAL_DOC,NVL(SUM(TOTAL_PLAN),0) TOTAL_DVR,NVL(SUM(TOTAL_PLAN_DOC),0) TOTAL_DVR_DOC,
                          NVL(SUM(VISITED_PLAN),0) TOTAL_PLAN_DCR,NVL(SUM(VISITED_DOC),0) TOTAL_DCR_DOC,
                          NVL(SUM(DOC_WITHOUT_PLAN),0) DOC_WITHOUT_PLAN,
                          NVL(SUM(EXISTING_DOC),0) EXISTING_DOC,  NVL(SUM(NEW_DOC),0) NEW_DOC,
                          NVL(SUM(UK_EXISTING_DOC),0) UK_EXISTING_DOC,  NVL(SUM(TOTAL_NEW_DOC),0) TOTAL_NEW_DOC,
                          NVL(SUM(INTERN_DOC),0) INTERN_DOC
                          FROM VW_DOC_COVERAGE Where Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

                if (model.MPGroup != "" && model.MPGroup != null)
                {
                    Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
                }                
                if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
                {
                    Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
                }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
            Qry = Qry + " group by MARKET_NAME,MPO_CODE, MPO_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDoctorCoverageBEL> item;    
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorCoverageBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOID = row["MPO_CODE"].ToString(),                        
                        MPOName = row["MPO_NAME"].ToString(),
                        TotalNoOfDoctor = row["TOTAL_DOC"].ToString(),
                        TotalNoOfDOTPlanDoctor = row["TOTAL_DVR_DOC"].ToString(),

                        DoctorPlannedPer = row["TOTAL_DOC"].ToString()=="0"?"0":(Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DOC"].ToString())).ToString("#.##"),

                        TotalNoOfVisitedDoctor = row["TOTAL_DCR_DOC"].ToString(),
                        DoctorCoveragePer = row["TOTAL_DVR_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_DCR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString())).ToString("#.##"),  


                        TotalNoOfDOTPlan = row["TOTAL_DVR"].ToString(),
                        TotalExecutionOfDOTPlan=  row["TOTAL_PLAN_DCR"].ToString(),
                        TotalExecutionOfDOTDoctorWithoutDOTPlan = row["DOC_WITHOUT_PLAN"].ToString(),

                        PlanVsExecutionPer = row["TOTAL_DVR"].ToString() == "0" ? "0" : ((Convert.ToDecimal(row["TOTAL_PLAN_DCR"].ToString()) + Convert.ToDecimal(row["DOC_WITHOUT_PLAN"].ToString())) * 100 / Convert.ToDecimal(row["TOTAL_DVR"].ToString())).ToString("#.##"),

                        TotalExecutionOfNonDOTDoctor = row["EXISTING_DOC"].ToString(),
                        TotalNoOfNonDOTDoctorVisited = row["UK_EXISTING_DOC"].ToString(),


                        TotalExecutionOfNewDoctor = row["TOTAL_NEW_DOC"].ToString(),

                        TotalNoOfNewDoctorVisited = row["NEW_DOC"].ToString(),
                        TotalExecutionOfInternDoctor = row["INTERN_DOC"].ToString()

                    }).ToList();
            return item;
        }



        public Tuple<string, DataTable, List<ReportDoctorCoverageBEL>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " SELECT  MARKET_NAME,MPO_CODE, MPO_NAME," +
                         " NVL(SUM(TOTAL_DOC),0) TOTAL_DOC,NVL(SUM(TOTAL_PLAN),0) TOTAL_DVR,NVL(SUM(TOTAL_PLAN_DOC),0) TOTAL_DVR_DOC," +
                         " NVL(SUM(VISITED_PLAN),0) TOTAL_PLAN_DCR,NVL(SUM(VISITED_DOC),0) TOTAL_DCR_DOC," +
                         " NVL(SUM(DOC_WITHOUT_PLAN),0) DOC_WITHOUT_PLAN," +               
                         " NVL(SUM(EXISTING_DOC),0) EXISTING_DOC,  NVL(SUM(NEW_DOC),0) NEW_DOC,"+
                         " NVL(SUM(UK_EXISTING_DOC),0) UK_EXISTING_DOC,  NVL(SUM(TOTAL_NEW_DOC),0) TOTAL_NEW_DOC," +
                         " NVL(SUM(INTERN_DOC),0) INTERN_DOC" +
                         " FROM VW_DOC_COVERAGE Where Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

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
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }



            Qry = Qry + " group by MARKET_NAME,MPO_CODE, MPO_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDoctorCoverageBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorCoverageBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOID = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        TotalNoOfDoctor = row["TOTAL_DOC"].ToString(),
                        TotalNoOfDOTPlanDoctor = row["TOTAL_DVR_DOC"].ToString(),

                        DoctorPlannedPer = row["TOTAL_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DOC"].ToString())).ToString("#.##"),

                        TotalNoOfVisitedDoctor = row["TOTAL_DCR_DOC"].ToString(),
                        DoctorCoveragePer = row["TOTAL_DVR_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_DCR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString())).ToString("#.##"),


                        TotalNoOfDOTPlan = row["TOTAL_DVR"].ToString(),
                        TotalExecutionOfDOTPlan = row["TOTAL_PLAN_DCR"].ToString(),
                        TotalExecutionOfDOTDoctorWithoutDOTPlan = row["DOC_WITHOUT_PLAN"].ToString(),

                        PlanVsExecutionPer = row["TOTAL_DVR"].ToString() == "0" ? "0" : ((Convert.ToDecimal(row["TOTAL_PLAN_DCR"].ToString()) + Convert.ToDecimal(row["DOC_WITHOUT_PLAN"].ToString())) * 100 / Convert.ToDecimal(row["TOTAL_DVR"].ToString())).ToString("#.##"),

                        TotalExecutionOfNonDOTDoctor = row["EXISTING_DOC"].ToString(),
                        TotalNoOfNonDOTDoctorVisited = row["UK_EXISTING_DOC"].ToString(),


                        TotalExecutionOfNewDoctor = row["TOTAL_NEW_DOC"].ToString(),

                        TotalNoOfNewDoctorVisited = row["NEW_DOC"].ToString(),
                        TotalExecutionOfInternDoctor = row["INTERN_DOC"].ToString()

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }


        public List<ReportDoctorCoverageBEL> GetDoctorCoverageArchive(DefaultParameterBEO model)
        {

            string Qry = " SELECT  MARKET_NAME,MPO_CODE, MPO_NAME," +
                         " NVL(SUM(TOTAL_DOC),0) TOTAL_DOC,NVL(SUM(TOTAL_PLAN),0) TOTAL_DVR,NVL(SUM(TOTAL_PLAN_DOC),0) TOTAL_DVR_DOC," +
                         " NVL(SUM(VISITED_PLAN),0) TOTAL_PLAN_DCR,NVL(SUM(VISITED_DOC),0) TOTAL_DCR_DOC," +
                         " NVL(SUM(DOC_WITHOUT_PLAN),0) DOC_WITHOUT_PLAN," +
                         " NVL(SUM(EXISTING_DOC),0) EXISTING_DOC,  NVL(SUM(NEW_DOC),0) NEW_DOC," +
                         " NVL(SUM(UK_EXISTING_DOC),0) UK_EXISTING_DOC,  NVL(SUM(TOTAL_NEW_DOC),0) TOTAL_NEW_DOC," +
                         " NVL(SUM(INTERN_DOC),0) INTERN_DOC" +
                         " FROM VW_ARC_DOC_COVERAGE Where Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TERRITORY_CODE='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
            Qry = Qry + " group by MARKET_NAME,MPO_CODE, MPO_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDoctorCoverageBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorCoverageBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOID = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        TotalNoOfDoctor = row["TOTAL_DOC"].ToString(),
                        TotalNoOfDOTPlanDoctor = row["TOTAL_DVR_DOC"].ToString(),

                        DoctorPlannedPer = row["TOTAL_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DOC"].ToString())).ToString("#.##"),

                        TotalNoOfVisitedDoctor = row["TOTAL_DCR_DOC"].ToString(),
                        DoctorCoveragePer = row["TOTAL_DVR_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_DCR_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DVR_DOC"].ToString())).ToString("#.##"),


                        TotalNoOfDOTPlan = row["TOTAL_DVR"].ToString(),
                        TotalExecutionOfDOTPlan = row["TOTAL_PLAN_DCR"].ToString(),
                        TotalExecutionOfDOTDoctorWithoutDOTPlan = row["DOC_WITHOUT_PLAN"].ToString(),

                        PlanVsExecutionPer = row["TOTAL_DVR"].ToString() == "0" ? "0" : ((Convert.ToDecimal(row["TOTAL_PLAN_DCR"].ToString()) + Convert.ToDecimal(row["DOC_WITHOUT_PLAN"].ToString())) * 100 / Convert.ToDecimal(row["TOTAL_DVR"].ToString())).ToString("#.##"),

                        TotalExecutionOfNonDOTDoctor = row["EXISTING_DOC"].ToString(),
                        TotalNoOfNonDOTDoctorVisited = row["UK_EXISTING_DOC"].ToString(),


                        TotalExecutionOfNewDoctor = row["TOTAL_NEW_DOC"].ToString(),

                        TotalNoOfNewDoctorVisited = row["NEW_DOC"].ToString(),
                        TotalExecutionOfInternDoctor = row["INTERN_DOC"].ToString()

                    }).ToList();
            return item;
        }



        public Tuple<string, DataTable, List<ReportDoctorCoverageBEL>> ExportArchive(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " SELECT  MARKET_NAME,MPO_CODE, MPO_NAME," +
                         " NVL(SUM(TOTAL_DOC),0) TOTAL_DOC,NVL(SUM(TOTAL_PLAN_DOC),0) TOTAL_PLAN_DOC," +
                         " NVL(SUM(VISITED_DOC),0) VISITED_DOC,NVL(SUM(TOTAL_PLAN),0) TOTAL_PLAN," +
                         " NVL(SUM(VISITED_PLAN),0) VISITED_PLAN,NVL(SUM(DOC_WITHOUT_PLAN),0) DOC_WITHOUT_PLAN,"+
                         " NVL(SUM(EXISTING_DOC),0) EXISTING_DOC,  NVL(SUM(NEW_DOC),0) NEW_DOC,"+
                         " NVL(SUM(UK_EXISTING_DOC),0) UK_EXISTING_DOC,  NVL(SUM(TOTAL_NEW_DOC),0) TOTAL_NEW_DOC," +
                         " NVL(SUM(INTERN_DOC),0) INTERN_DOC" +
                         " FROM VW_ARC_DOC_COVERAGE Where Year=" + model.Year + " and MONTH_NUMBER='" + model.MonthNumber + "' ";

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
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }



            Qry = Qry + " group by MARKET_NAME,MPO_CODE, MPO_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportDoctorCoverageBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorCoverageBEL
                    {
                        SL = row["Col1"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPOID = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        TotalNoOfDoctor = row["TOTAL_DOC"].ToString(),
                        TotalNoOfDOTPlanDoctor = row["TOTAL_PLAN_DOC"].ToString(),

                        DoctorPlannedPer = row["TOTAL_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["TOTAL_PLAN_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_DOC"].ToString())).ToString("#.##"),
                        TotalNoOfVisitedDoctor = row["VISITED_DOC"].ToString(),
                        DoctorCoveragePer = row["TOTAL_PLAN_DOC"].ToString() == "0" ? "0" : (Convert.ToDecimal(row["VISITED_DOC"].ToString()) * 100 / Convert.ToDecimal(row["TOTAL_PLAN_DOC"].ToString())).ToString("#.##"),


                        TotalNoOfDOTPlan = row["TOTAL_PLAN"].ToString(),
                        TotalExecutionOfDOTPlan = row["VISITED_PLAN"].ToString(),
                        TotalExecutionOfDOTDoctorWithoutDOTPlan = row["DOC_WITHOUT_PLAN"].ToString(),

                        PlanVsExecutionPer = row["TOTAL_PLAN"].ToString() == "0" ? "0" : ((Convert.ToDecimal(row["VISITED_PLAN"].ToString()) + Convert.ToDecimal(row["DOC_WITHOUT_PLAN"].ToString())) * 100 / Convert.ToDecimal(row["TOTAL_PLAN"].ToString())).ToString("#.##"),
                       
                        TotalExecutionOfNonDOTDoctor = row["EXISTING_DOC"].ToString(),
                        TotalNoOfNonDOTDoctorVisited = row["UK_EXISTING_DOC"].ToString(),
                       
                        TotalExecutionOfNewDoctor = row["TOTAL_NEW_DOC"].ToString(),
                        TotalNoOfNewDoctorVisited = row["NEW_DOC"].ToString(),
                        TotalExecutionOfInternDoctor = row["INTERN_DOC"].ToString()

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
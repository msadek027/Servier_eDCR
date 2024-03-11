using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using System.Globalization;
using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportDCRSummaryDAO
    {
        DBHelper dbHelper=new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        DateFormat dateFormat = new DateFormat();
        public List<ReportDCRSummaryBEL> GetDataForDCRSummary(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry =" SELECT MARKET_NAME,MP_GROUP, MPO_CODE, MPO_NAME,TSM_ID,  REGION_CODE,  " +
                        " LISTAGG(MPO_COLLEAGUE,', ') WITHIN GROUP(ORDER BY SET_DATE) MPO_COLLEAGUE,TO_CHAR(SET_DATE,'dd-mm-yyyy')  SET_DATE, " +
                        " SUM(R_SELECTED_QTY) R_SELECTED_QTY,  SUM(R_SAMPLE_QTY) R_SAMPLE_QTY,  SUM(I_SAMPLE_QTY) I_SAMPLE_QTY,  SUM(R_GIFT_QTY) R_GIFT_QTY,  SUM(I_GIFT_QTY) I_GIFT_QTY,  " +
                        " SUM(TOTAL_DOT) TOTAL_DOT,SUM(REGULAR_DOT) REGULAR_DOT,SUM(INTERN_DOT) INTERN_DOT,SUM(M_WP) M_WP, SUM(E_WP) E_WP, SUM(TOTAL_DCR) TOTAL_DCR,SUM(M_DCR) M_DCR, SUM(E_DCR) E_DCR, SUM(INTERN_DCR) INTERN_DCR,SUM(OTHER_DCR) OTHER_DCR, SUM(M_ABSENT) M_ABSENT, SUM(E_ABSENT) E_ABSENT" +
                        " FROM  VW_DCR_SUMMARY " +
                        " Where  SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' ";

            if (model.RegionCode != "" && model.RegionCode != null)
            {
                qry += " AND REGION_CODE = '" + model.RegionCode + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                qry += " AND TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }            
            qry += " GROUP BY SET_DATE,MARKET_NAME,MP_GROUP,MPO_CODE, MPO_NAME, TSM_ID, REGION_CODE ";       

            qry = qry + " Order by SET_DATE,MARKET_NAME";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDCRSummaryBEL> item;
               
                item = (from DataRow row in dt.Rows
                        select new ReportDCRSummaryBEL
                        {
                            SL = row["Col1"].ToString(),
                            Date = row["SET_DATE"].ToString(),                          
                            MarketName = row["MARKET_NAME"].ToString(),
                            Accompany = row["MPO_COLLEAGUE"].ToString(),
                            SelectedRegularQty = row["R_SELECTED_QTY"].ToString(),                           
                            SampleRegularQty = row["R_SAMPLE_QTY"].ToString(),
                            SampleInternQty = row["I_SAMPLE_QTY"].ToString(),
                            GiftRegularQty = row["R_GIFT_QTY"].ToString(),
                            GiftInternQty = row["I_GIFT_QTY"].ToString(),

                            RegularDOT= row["REGULAR_DOT"].ToString(),
                            InternDOT = row["INTERN_DOT"].ToString(),
                            TotalDOT = row["TOTAL_DOT"].ToString(),

                            MorningWP = row["M_WP"].ToString(),
                            EveningWP = row["E_WP"].ToString(),

                            TotalDCR = row["TOTAL_DCR"].ToString(),
                            MorningDCR = row["M_DCR"].ToString(),
                            EveningDCR = row["E_DCR"].ToString(),
                            InternDCR= row["INTERN_DCR"].ToString(),
                            OtherDCR = row["OTHER_DCR"].ToString(),

                            MorningAbsent = row["M_ABSENT"].ToString(),
                            EveningAbsent = row["E_ABSENT"].ToString(),                          

                        }).ToList();
            
            return item;
        }


           public Tuple<string, DataTable, List<ReportDCRSummaryBEL>> Export(DefaultParameterBEO model)
           {
            string vHeader = "";
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry = " SELECT MARKET_NAME,MP_GROUP, MPO_CODE, MPO_NAME,TSM_ID,  REGION_CODE,  " +
               " LISTAGG(MPO_COLLEAGUE,', ') WITHIN GROUP(ORDER BY SET_DATE) MPO_COLLEAGUE,TO_CHAR(SET_DATE,'dd/mm/yyyy')  SET_DATE, " +
               " SUM(R_SELECTED_QTY) R_SELECTED_QTY,  SUM(R_SAMPLE_QTY) R_SAMPLE_QTY,  SUM(I_SAMPLE_QTY) I_SAMPLE_QTY,  SUM(R_GIFT_QTY) R_GIFT_QTY,  SUM(I_GIFT_QTY) I_GIFT_QTY,  " +
               " SUM(TOTAL_DOT) TOTAL_DOT,SUM(REGULAR_DOT) REGULAR_DOT,SUM(INTERN_DOT) INTERN_DOT,SUM(M_WP) M_WP, SUM(E_WP) E_WP, SUM(TOTAL_DCR) TOTAL_DCR,SUM(M_DCR) M_DCR, SUM(E_DCR) E_DCR, SUM(INTERN_DCR) INTERN_DCR,SUM(OTHER_DCR) OTHER_DCR, SUM(M_ABSENT) M_ABSENT, SUM(E_ABSENT) E_ABSENT" +
               " FROM  VW_DCR_SUMMARY " +
               " Where SET_DATE Between '" + model.FromDate + "' AND '" + model.ToDate + "' ";

            vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;
           /*
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
                qry += " And TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
            }       
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                if (model.RegionName != "" && model.RegionName != null)
                {
                    string[] Region = model.RegionName.Split(',');
                    string lastItem = Region.Length > 1 ? Region[Region.Length - 1] : Region[0];
                    vHeader = vHeader + ", Region : " + lastItem;
                }
                qry += " AND REGION_CODE = '" + model.RegionCode + "'";
            }            
            */
            qry += " GROUP BY SET_DATE,MARKET_NAME,MP_GROUP,MPO_CODE, MPO_NAME, TSM_ID,  REGION_CODE ";

     
            qry = qry + " Order by SET_DATE,MARKET_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDCRSummaryBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDCRSummaryBEL
                    {
                        SL = row["Col1"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),                     

                        Accompany = row["MPO_COLLEAGUE"].ToString(),
                        SelectedRegularQty = row["R_SELECTED_QTY"].ToString(),
                        SampleRegularQty = row["R_SAMPLE_QTY"].ToString(),
                        SampleInternQty = row["I_SAMPLE_QTY"].ToString(),
                        GiftRegularQty = row["R_GIFT_QTY"].ToString(),
                        GiftInternQty = row["I_GIFT_QTY"].ToString(),
                        RegularDOT = row["REGULAR_DOT"].ToString(),
                        InternDOT = row["INTERN_DOT"].ToString(),
                        TotalDOT = row["TOTAL_DOT"].ToString(),
                        MorningWP = row["M_WP"].ToString(),
                        EveningWP = row["E_WP"].ToString(),

                        TotalDCR = row["TOTAL_DCR"].ToString(),
                        MorningDCR = row["M_DCR"].ToString(),
                        EveningDCR = row["E_DCR"].ToString(),
                        InternDCR = row["INTERN_DCR"].ToString(),
                        OtherDCR = row["OTHER_DCR"].ToString(),
                        MorningAbsent = row["M_ABSENT"].ToString(),
                        EveningAbsent = row["E_ABSENT"].ToString(),

                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),

                    }).ToList();
            return Tuple.Create(vHeader, dt, item);
          
        }


     

        public List<ReportDcrSummaryTmRsm> GetDcrSummaryTmRsm(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string Qry = " Select  TO_CHAR(SET_DATE,'dd-mm-yyyy')  SET_DATE,LOC_CODE, MARKET_NAME,MPO_CODE ,MPO_NAME, " +
                        " SUM(SINGLE_CALL) SINGLE_CALL, SUM(DOUBLE_CALL) DOUBLE_CALL, SUM(MORNING_DCR) MORNING_DCR, SUM(EVENING_DCR)EVENING_DCR,"+
                         " SUM(TOTAL_DCR) TOTAL_DCR," +
                        " SUM(NEW_DOCTOR_DCR) NEW_DOCTOR_DCR,SUM(INTERN_DOCTOR_DCR) INTERN_DOCTOR_DCR,SUM(OTHER_DCR) OTHER_DCR,SUM(NO_OF_INTERN) NO_OF_INTERN, SUM(CHEMIST_COUNT) CHEMIST_COUNT, " +
                        //" LISTAGG(ACCOMPANY, ', ') WITHIN GROUP(ORDER BY SET_DATE) ACCOMPANY" +
                        " RTRIM(XMLAGG(XMLELEMENT(E, ACCOMPANY, ',').EXTRACT('//text()') ORDER BY SET_DATE).GetClobVal(), ',') AS ACCOMPANY " +
                        " FROM  VW_SUP_TM_RSM_DCR_SUMMARY " +
                        " Where  SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' ";

            if (model.Designation == "" || model.Designation == " " || model.Designation == null)
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry += " And LOC_CODE IN ( Select TERRITORY_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "' UNION  Select REGION_CODE CODE from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "')";
                }
            }
            else
            {
                if (model.Designation == "TM")
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

                if (model.Designation == "RSM")
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
            Qry += " GROUP BY SET_DATE,LOC_CODE, MARKET_NAME,MPO_CODE ,MPO_NAME ";

    

            Qry = Qry + " Order by SET_DATE,MARKET_NAME";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDcrSummaryTmRsm> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDcrSummaryTmRsm
                    {
                        SL = row["Col1"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        SingleCall = row["SINGLE_CALL"].ToString(),
                        DoubleCall = row["DOUBLE_CALL"].ToString(),
                        TotalDCR = row["TOTAL_DCR"].ToString(),
                        MorningDCR = row["MORNING_DCR"].ToString(),
                        EveningDCR = row["EVENING_DCR"].ToString(),
                        NewDoctorDCR = row["NEW_DOCTOR_DCR"].ToString(),
                        InternDoctorDCR = row["INTERN_DOCTOR_DCR"].ToString(),
                        NoOfInternDoctorVisited = row["NO_OF_INTERN"].ToString(),
                        OtherDCR = row["OTHER_DCR"].ToString(),
                        NoOfChemist = row["CHEMIST_COUNT"].ToString(),
                        Accompany = row["ACCOMPANY"].ToString(),
                        //  Accompany = ByteArrayToString((byte[])row["ACCOMPANY"]),

                    }).ToList();

            return item;
        }


        public Tuple<string, DataTable, List<ReportDcrSummaryTmRsm>> ExportTmRsm(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);
            string vHeader = "";
            string Qry = " Select  TO_CHAR(SET_DATE,'dd/mm/yyyy')  SET_DATE,LOC_CODE, MARKET_NAME,MPO_CODE ,MPO_NAME, " +
                                 " SUM(SINGLE_CALL) SINGLE_CALL, SUM(DOUBLE_CALL) DOUBLE_CALL, SUM(MORNING_DCR) MORNING_DCR, SUM(EVENING_DCR) EVENING_DCR," +
                                 " SUM(TOTAL_DCR) TOTAL_DCR," +
                                 " SUM(NEW_DOCTOR_DCR) NEW_DOCTOR_DCR,SUM(INTERN_DOCTOR_DCR) INTERN_DOCTOR_DCR,SUM(OTHER_DCR) OTHER_DCR,SUM(NO_OF_INTERN) NO_OF_INTERN, SUM(CHEMIST_COUNT) CHEMIST_COUNT, " +
                                 // " LISTAGG(ACCOMPANY, ', ') WITHIN GROUP(ORDER BY SET_DATE) ACCOMPANY" +
                                 " RTRIM(XMLAGG(XMLELEMENT(E, ACCOMPANY, ',').EXTRACT('//text()') ORDER BY SET_DATE).GetClobVal(), ',') AS ACCOMPANY " +
                                 " FROM  VW_SUP_TM_RSM_DCR_SUMMARY " +
                                 " Where  SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' ";

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
                if (model.Designation == "TM")
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

                if (model.Designation == "RSM")
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
            }
            Qry += " GROUP BY SET_DATE,LOC_CODE,MARKET_NAME,MPO_CODE ,MPO_NAME ";

            Qry = Qry + " Order by SET_DATE,MARKET_NAME";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportDcrSummaryTmRsm> item;

            item = (from DataRow row in dt.Rows
                    select new ReportDcrSummaryTmRsm
                    {
                        SL = row["Col1"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        SingleCall = row["SINGLE_CALL"].ToString(),
                        DoubleCall = row["DOUBLE_CALL"].ToString(),
                        TotalDCR = row["TOTAL_DCR"].ToString(),
                        MorningDCR = row["MORNING_DCR"].ToString(),
                        EveningDCR = row["EVENING_DCR"].ToString(),
                        NewDoctorDCR = row["NEW_DOCTOR_DCR"].ToString(),
                        InternDoctorDCR = row["INTERN_DOCTOR_DCR"].ToString(),
                        NoOfInternDoctorVisited = row["NO_OF_INTERN"].ToString(),
                        OtherDCR = row["OTHER_DCR"].ToString(),
                        NoOfChemist = row["CHEMIST_COUNT"].ToString(),
                        Accompany = row["ACCOMPANY"].ToString(),


                    }).ToList();
            return Tuple.Create(vHeader, dt, item);

        }



    }
}

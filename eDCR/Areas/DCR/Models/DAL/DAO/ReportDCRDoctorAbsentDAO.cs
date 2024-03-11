using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class ReportDCRDoctorAbsentDAO
    {
       DBHelper dbHelper=new DBHelper();
        DBConnection dbConn = new DBConnection();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

        DateFormat dateFormat = new DateFormat();

        public List<ReportDCRDoctorAbsentBEL> GetAbsentSummary(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string qry =" SELECT MP_GROUP, MPO_CODE, MPO_NAME,MARKET_NAME,TERRITORY_NAME,  REGION_CODE, REGION_NAME,  DOCTOR_ID,DOCTOR_NAME,DEGREES, SPECIALIZATION,SHIFT_NAME ,Sum(ABSENT) ABSENT,Sum(NOT_ALLOWED) NOT_ALLOWED,Sum(MISSED) MISSED" +
                        " FROM VW_DCR_ABSENT_DOCTOR " +
                        " Where  SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' AND ABSENT>0 ";
                
                       if (model.TerritoryManagerID != "")
                        {
                         qry += " AND TERRITORY_CODE= '" + model.TerritoryManagerID + "'";               
                        }                        
                        if (model.MPGroup != "")
                        {                
                          qry += " AND MP_GROUP='" + model.MPGroup + "'";                
                        }
                        qry += " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME,MARKET_NAME,TERRITORY_NAME,  REGION_CODE, REGION_NAME,  DOCTOR_ID,DOCTOR_NAME,DEGREES, SPECIALIZATION,SHIFT_NAME ";


               
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);           
            List<ReportDCRDoctorAbsentBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRDoctorAbsentBEL
                    {
                      
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degrees = row["DEGREES"].ToString(),
                        
                        Speciality = row["SPECIALIZATION"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        Absent = row["ABSENT"].ToString(),
                        NotAllowed = row["NOT_ALLOWED"].ToString(),
                        Missed = row["MISSED"].ToString(),
                    }).ToList();
            return item;
        }







        public List<ReportDCRDoctorAbsentDetail> GetAbsentDetail(DefaultParameterBEO model)
        {
            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string Qry = "Select MP_Group,TO_Char(SET_DATE,'dd-mm-yyyy') SET_DATE,ABSENT,NOT_ALLOWED,MISSED"+
                " from VW_DCR_ABSENT_DOCTOR " +
                " Where SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "' "+
                " and DOCTOR_ID='" + model.DoctorID + "' and DOCTOR_NAME='" + model.DoctorName + "'" +
                " and MP_GROUP='" + model.MPGroup + "'"+
                " ORDER BY SET_DATE";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ReportDCRDoctorAbsentDetail> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRDoctorAbsentDetail
                    {
                        MPGroup = row["MP_Group"].ToString(),
                        Date = row["SET_DATE"].ToString(),
                        Absent = row["ABSENT"].ToString(),
                        NotAllowed = row["NOT_ALLOWED"].ToString(),
                        Missed = row["MISSED"].ToString(),
                    }).ToList();
            return item;
        }




        
        public Tuple<string, DataTable, List<ReportDCRDoctorAbsentBEL>> Export(DefaultParameterBEO model)
        {

            model.FromDate = dateFormat.StringDateDdMonYYYY(model.FromDate);
            model.ToDate = dateFormat.StringDateDdMonYYYY(model.ToDate);

            string vHeader = "";
            string qry = " SELECT MP_GROUP, MPO_CODE, MPO_NAME,MARKET_NAME,TERRITORY_NAME,  REGION_CODE, REGION_NAME,  DOCTOR_ID,DOCTOR_NAME,DEGREES, SPECIALIZATION,SHIFT_NAME ,Sum(ABSENT) ABSENT,Sum(NOT_ALLOWED) NOT_ALLOWED,Sum(MISSED) MISSED" +
                          " FROM VW_DCR_ABSENT_DOCTOR " +
                          " Where  SET_DATE BETWEEN '" + model.FromDate + "' AND '" + model.ToDate + "'  AND ABSENT>0 ";

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
            }
         
            qry += " GROUP BY MP_GROUP, MPO_CODE, MPO_NAME,MARKET_NAME, TERRITORY_NAME, REGION_CODE, REGION_NAME,  DOCTOR_ID,DOCTOR_NAME,DEGREES, SPECIALIZATION,SHIFT_NAME ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
            List<ReportDCRDoctorAbsentBEL> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDCRDoctorAbsentBEL
                    {

                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MPOCode = row["MPO_Code"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degrees = row["DEGREES"].ToString(),
                        
                        Speciality = row["SPECIALIZATION"].ToString(),
                        ShiftName = row["SHIFT_NAME"].ToString(),
                        Absent = row["ABSENT"].ToString(),
                        NotAllowed = row["NOT_ALLOWED"].ToString(),
                        Missed = row["MISSED"].ToString(),
                    }).ToList();
            return Tuple.Create(vHeader, dt,item);

        }



    }
    
}
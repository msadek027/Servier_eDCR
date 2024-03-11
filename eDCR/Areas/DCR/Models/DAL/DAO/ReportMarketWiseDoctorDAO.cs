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
    public class ReportMarketWiseDoctorDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      

        public List<ReportMarketWiseDoctorBEL> GetMarketWiseDoctor(DefaultParameterBEO model)
        {
            string Qry = "Select distinct A.DOCTOR_ID,A.DOCTOR_NAME,A.REG_NO,A.PHONE,A.EMAIL,A.ADDRESS1,A.ADDRESS2,A.ADDRESS3,A.ADDRESS4,A.RELIGION,A.DESIGNATION,A.SPECIALIZATION,A.POTENTIAL,A.DEGREES,A.GENDER,A.MORNING_LOCATION,A.EVENING_LOCATION,A.MARKET_CODE,A.MARKET,A.Year,A.MONTH_NUMBER,B.TERRITORY_NAME,B.REGION_NAME " +
                  " From DOC_DETAIL A,VW_HR_LOC_MAPPING B Where A.MARKET_CODE =B.MARKET_CODE and A.Year=" + model.Year + " and A.Month_Number='" + model.MonthNumber + "' ";
         
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and B.TERRITORY_CODE = '" + model.TerritoryManagerID + "' ";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and B.MP_Group = '" + model.MPGroup + "' ";
            }
            Qry = Qry + " ORDER BY MARKET";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);


            List<ReportMarketWiseDoctorBEL> item = new List<ReportMarketWiseDoctorBEL>();
            foreach (DataRow row in dt.Rows)
            {
                ReportMarketWiseDoctorBEL dData = new ReportMarketWiseDoctorBEL();
                dData.SL = row["Col1"].ToString();
                dData.DoctorID = row["DOCTOR_ID"].ToString();
                dData.DoctorName = row["DOCTOR_NAME"].ToString();
                dData.RegNo = row["REG_NO"].ToString();
                dData.Phone = row["PHONE"].ToString();
                dData.Email = row["EMAIL"].ToString();
                dData.Address1 = row["ADDRESS1"].ToString();
                dData.Address2 = row["ADDRESS2"].ToString();
                dData.Address3 = row["ADDRESS3"].ToString();
                dData.Address4 = row["ADDRESS4"].ToString();
                dData.Religion = row["RELIGION"].ToString();
                dData.Designation = row["DESIGNATION"].ToString();
                dData.Specialization = row["SPECIALIZATION"].ToString();
                dData.Potential = row["POTENTIAL"].ToString();
                dData.Degrees = row["DEGREES"].ToString();
                dData.Gender = row["GENDER"].ToString();
                dData.MorningLocation = row["Morning_Location"].ToString();
                dData.EveningLocation = row["Evening_Location"].ToString();
                dData.MarketCode = row["Market_Code"].ToString();
                dData.MarketName = row["MARKET"].ToString();
                dData.Territory = row["TERRITORY_NAME"].ToString();
                dData.Region = row["REGION_NAME"].ToString();

                //if (!string.IsNullOrEmpty(dData.DoctorID) && !string.IsNullOrEmpty(dData.DoctorName) && !string.IsNullOrEmpty(dData.MarketCode) && (!string.IsNullOrEmpty(dData.MorningLocation) || !string.IsNullOrEmpty(dData.EveningLocation)))
                //{
                item.Add(dData);
               // }
            }
            return item;
        }


        public Tuple<string, DataTable, List<ReportMarketWiseDoctorBEL>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select A.DOCTOR_ID,A.DOCTOR_NAME,A.REG_NO,A.PHONE,A.EMAIL,A.ADDRESS1,A.ADDRESS2,A.ADDRESS3,A.ADDRESS4,A.RELIGION,A.DESIGNATION,A.SPECIALIZATION,A.POTENTIAL,A.DEGREES,A.GENDER,A.MORNING_LOCATION,A.EVENING_LOCATION,A.MARKET_CODE,A.MARKET,A.Year,A.MONTH_NUMBER,B.TERRITORY_NAME,B.REGION_NAME " +
                         " From DOC_DETAIL A,VW_HR_LOC_MAPPING B Where A.MARKET_CODE =B.MARKET_CODE and A.Year=" + model.Year + " and A.Month_Number='" + model.MonthNumber + "' ";
                        
            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month: " + month + ", " + model.Year;
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND B.MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " And B.TERRITORY_CODE = '" + model.TerritoryManagerID + "'";
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
            Qry = Qry + " ORDER BY MARKET";


            //Qry = "Select * from Temp_DOC_Rpt_Group ";

            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);
            List<ReportMarketWiseDoctorBEL> item;

            item = (from DataRow row in dt.Rows
                    select new ReportMarketWiseDoctorBEL
                    {
                       SL = row["Col1"].ToString(),
                       DoctorID = row["DOCTOR_ID"].ToString(),
                       DoctorName = row["DOCTOR_NAME"].ToString(),
                       RegNo = row["REG_NO"].ToString(),
                       Phone = row["PHONE"].ToString(),
                       Email = row["EMAIL"].ToString(),
                       Address1 = row["ADDRESS1"].ToString(),
                       Address2 = row["ADDRESS2"].ToString(),
                       Address3 = row["ADDRESS3"].ToString(),
                       Address4 = row["ADDRESS4"].ToString(),
                       Designation = row["DESIGNATION"].ToString(),
                       Specialization = row["SPECIALIZATION"].ToString(),
                       Potential = row["POTENTIAL"].ToString(),
                       Degrees = row["DEGREES"].ToString(),
                       Religion = row["RELIGION"].ToString(),
                       Gender = row["GENDER"].ToString(),
                       MorningLocation = row["Morning_Location"].ToString(),
                       EveningLocation = row["Evening_Location"].ToString(),
                       MarketCode = row["Market_Code"].ToString(),
                       MarketName = row["MARKET"].ToString(),
                       Territory = row["TERRITORY_NAME"].ToString(),
                       Region = row["REGION_NAME"].ToString(),

        }).ToList();

          

            return Tuple.Create(vHeader, dt, item);
        }
    }
}
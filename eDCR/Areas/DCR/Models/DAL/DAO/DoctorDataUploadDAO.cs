using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class DoctorDataUploadDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();



        public List<DoctorDataUploadInfo> GetDoctorGridData(DoctorDataUploadInfo model)
        {

            string Qry = "Select DOCTOR_ID,DOCTOR_NAME,REG_NO,PHONE,EMAIL,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,RELIGION,DESIGNATION,SPECIALIZATION,POTENTIAL,DEGREES,GENDER,MORNING_LOCATION,EVENING_LOCATION,MARKET_CODE,MARKET,Region, MONTH_NUMBER,YEAR"+
                " From DOC_DETAIL Where  MONTH_NUMBER='" + model.MonthNumber + "' AND YEAR=" + model.Year + "";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
   

            List<DoctorDataUploadInfo> item = new List<DoctorDataUploadInfo>();
            foreach (DataRow row in dt.Rows)
            {
                DoctorDataUploadInfo dData = new DoctorDataUploadInfo();
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
                        dData.Market = row["MARKET"].ToString();
                        dData.Region = row["Region"].ToString();
                        dData.MonthNumber = row["MONTH_NUMBER"].ToString();
                        dData.Year = row["YEAR"].ToString();
                //if (!string.IsNullOrEmpty(dData.DoctorID) && !string.IsNullOrEmpty(dData.DoctorName) && !string.IsNullOrEmpty(dData.MarketCode) && (!string.IsNullOrEmpty(dData.MorningLocation) || !string.IsNullOrEmpty(dData.EveningLocation)))
                //{
                    item.Add(dData);
                //}
            }
            return item;
        }



        public bool SaveUpdate(DoctorDataUploadBEL model)
        {
            bool isTrue = false;
            string query = "";

            if (model.doctorDataUploadInfoList != null)
            {
                foreach (DoctorDataUploadInfo detail in model.doctorDataUploadInfoList)
                {
                    if (DoctorIDIsExist(detail))
                    {
                        MaxID = ""; IUMode = "U";
                        query = " Update DOC_DETAIL set DOCTOR_NAME = '" + detail.DoctorName + "',REG_NO = '" + detail.RegNo + "',PHONE = '" + detail.Phone + "',EMAIL = '" + detail.Email + "',ADDRESS1 = '" + detail.Address1 + "',ADDRESS2 = '" + detail.Address2 + "',ADDRESS3 = '" + detail.Address3 + "',ADDRESS4 = '" + detail.Address4+ "',RELIGION = '" + detail.Religion + "',DESIGNATION = '" + detail.Designation + "',SPECIALIZATION = '" + detail.Specialization + "',POTENTIAL = '" + detail.Potential + "',DEGREES = '" + detail.Degrees + "',GENDER = '" + detail.Gender + "',Morning_Location='" + detail.MorningLocation + "',Evening_Location='" + detail.EveningLocation + "' " +
                                " Where  YEAR = " + detail.Year + " and MONTH_NUMBER='" + detail.MonthNumber + "' AND DOCTOR_ID = '" + detail.DoctorID + "' and market_Code='" + detail.MarketCode + "'";
                        if (detail.MorningLocation != "" && detail.MorningLocation != null)
                        {
                            query = query + " and Morning_Location='" + detail.MorningLocation + "'";
                        }
                        if (detail.EveningLocation != "" && detail.EveningLocation != null)
                        {
                            query = query + " and Evening_Location='" + detail.EveningLocation + "'";
                        }                    
                    }
                    else
                    {
                        MaxID = ""; IUMode = "I";
                        query = "Insert into DOC_DETAIL(DOCTOR_ID,DOCTOR_NAME,REG_NO,PHONE,EMAIL,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,RELIGION,DESIGNATION,SPECIALIZATION,POTENTIAL,DEGREE,GENDER,Morning_Location,Evening_Location,Market_Code,MARKET,REGION,YEAR,MONTH_NUMBER) Values('" + detail.DoctorID + "','" + detail.DoctorName + "','" + detail.RegNo + "','" + detail.Phone + "','" + detail.Email + "','" + detail.Address1 + "','" + detail.Address2 + "','" + detail.Address3 + "','" + detail.Address4 + "','" + detail.Religion + "','" + detail.Designation + "','" + detail.Specialization + "','" + detail.Potential + "','" + detail.Degrees + "','" + detail.Gender + "','" + detail.MorningLocation + "','" + detail.EveningLocation + "','" + detail.MarketCode + "','" + detail.Market + "','" + detail.Region + "'," + detail.Year + ",'" + detail.MonthNumber + "')";

                    }
                    if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), query))
                    {
                        isTrue = true;
                    }
                }

            }


            return isTrue;
        }


        private bool DoctorIDIsExist(DoctorDataUploadInfo detail)
        {
            bool isTrue = false;
            string Qry = "SELECT DOCTOR_ID FROM DOC_DETAIL WHERE YEAR = " + detail.Year + " and MONTH_NUMBER='" + detail.MonthNumber + "' AND DOCTOR_ID = '" + detail.DoctorID + "' and market_Code='" + detail.MarketCode + "' ";

            if (detail.MorningLocation != "" && detail.MorningLocation != null)
            {
                Qry = Qry + " and Morning_Location='" + detail.MorningLocation + "'";
            }
            if (detail.EveningLocation != "" && detail.EveningLocation != null)
            {
                Qry = Qry + " and Evening_Location='" + detail.EveningLocation + "'";
            }
                
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            if (dt2.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }



        public List<DoctorDataUploadInfo> GetDoctor(DataTable dt)
        {
            List<DoctorDataUploadInfo> item = new List<DoctorDataUploadInfo>();
            foreach (DataRow row in dt.Rows)
            {
                    DoctorDataUploadInfo dData = new DoctorDataUploadInfo();
                    dData.DoctorID = row["DoctorID"].ToString();
                    dData.DoctorName = row["DoctorName"].ToString();
                    dData.RegNo = row["RegNo"].ToString();
                    dData.Phone = row["Phone"].ToString() == "" ? "" : row["Phone"].ToString();
                    dData.Email = row["Email"].ToString();
                    dData.Address1 = row["Address1"].ToString();
                    dData.Address2 = row["Address2"].ToString();
                    dData.Address3 = row["Address3"].ToString();
                    dData.Address4 = row["Address4"].ToString();
                    dData.Religion = row["Religion"].ToString();
                    dData.Designation = row["Designation"].ToString();
                    dData.Specialization = row["Specialization"].ToString();
                    dData.Potential = row["Potential"].ToString();
                    dData.Degrees = row["Degrees"].ToString();
                    dData.Gender = row["Gender"].ToString();
                    dData.MorningLocation = row["MorningLocation"].ToString();
                    dData.EveningLocation = row["EveningLocation"].ToString();
                    dData.MarketCode = row["MarketCode"].ToString();
                    dData.Market = row["Market"].ToString();
                    dData.Region = row["Region"].ToString();
                if (!string.IsNullOrEmpty(dData.DoctorID) && !string.IsNullOrEmpty(dData.DoctorName) && !string.IsNullOrEmpty(dData.MarketCode) && (!string.IsNullOrEmpty(dData.MorningLocation) || !string.IsNullOrEmpty(dData.EveningLocation)))
                {
                    item.Add(dData);
                }
            }
            return item;
        }

    }
}
using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class GiftWiseDoctorSelectDAO: ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();



        public List<GiftWiseDoctorSelectDetail> GetDetailDataForSaveUpdate(GiftWiseDoctorSelectBEL model)
        {
            string Qry = " SELECT distinct MST_SL,PRODUCT_CODE,Gift_Name ||' ('||GENERIC_NAME||')' Gift_Name,REST_QTY+CENTRAL_QTY Total_Qty,DOCTOR_ID,DOCTOR_NAME,SPECIALIZATION " +
                " from VW_GWDS Where  Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' and MP_Group = '" + model.MPGroup + "'";             
                Qry = Qry + " Order by MST_SL,PRODUCT_CODE ";                 
           
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GiftWiseDoctorSelectDetail> item;

            item = (from DataRow row in dt.Rows
                    select new GiftWiseDoctorSelectDetail
                    {
                        MasterSL = row["MST_SL"].ToString(),
                        GiftCode = row["PRODUCT_CODE"].ToString(),
                        GiftName = row["Gift_Name"].ToString(),
                        TotalQty = row["Total_Qty"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString()

                    }).ToList();
            return item;
        }

        public List<GiftWiseDoctorSelectDetail> GetDoctor(ProductWiseDoctorSelectMaster model)
        {
            string Qry = "Select distinct MPO_CODE,DOCTOR_ID,DOCTOR_NAME,DEGREES,SPECIALIZATION,POTENTIAL "+
                " from VW_DOC_INTRN_INST_MPO_MAPPING  Where MP_Group = '" + model.MPGroup + "'  and Year=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ORDER BY DOCTOR_NAME";
           
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GiftWiseDoctorSelectDetail> item;

            item = (from DataRow row in dt.Rows
                    select new GiftWiseDoctorSelectDetail
                    {                       
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString()
                    }).ToList();
            return item;
        }


        public List<GiftWiseDoctorSelectDetail> GetGiftItem(string MPGroup)
        {
            string Qry = "SELECT distinct PRODUCT_CODE,PRODUCT_NAME ||' ('||GENERIC_NAME||')' PRODUCT_NAME,REST_QTY+CENTRAL_QTY TOTAL_QTY From VW_INV_ITEM_BALANCE Where ITEM_TYPE='Gt' and BALANCE_QTY >0";

            if (MPGroup != null && MPGroup != "")
            {
                Qry = Qry + "  and MP_Group = '" + MPGroup + "'";
            }
            Qry = Qry + "  ORDER BY PRODUCT_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GiftWiseDoctorSelectDetail> item;

            item = (from DataRow row in dt.Rows
                    select new GiftWiseDoctorSelectDetail
                    {
                        GiftCode = row["PRODUCT_CODE"].ToString(),
                        GiftName = row["PRODUCT_NAME"].ToString(),
                        TotalQty = row["TOTAL_QTY"].ToString()
                    }).ToList();
            return item;
        }



        public bool SaveUpdate(GiftWiseDoctorSelectBEL master)
        {
            bool isTrue = false;
            string masterQry = "";
            string masterSL;
            string MaxID = "";

            int month = ((Convert.ToInt16(master.Year) - Convert.ToInt16(DateTime.Now.Year)) * 12) + Convert.ToInt16(master.MonthNumber) - Convert.ToInt16(DateTime.Now.Month);

            if (month >= 0)
            {
                string QryIsExistsMST = "Select MST_SL from GWDS_MST Where MP_GROUP='" + master.MPGroup + "' and YEAR=" + master.Year + " and MONTH_NUMBER='" + master.MonthNumber + "'";
                var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExistsMST);

                if (tuple1.Item1)
                {
                    masterSL = tuple1.Item2;
                    MaxID = masterSL;
                    IUMode = "U";
                    masterQry = "Update GWDS_MST Set MST_STATUS='Approved' Where MST_SL=" + MaxID + "";
                }
                else
                {
                    IUMode = "I";
                    MaxID = master.MasterSL;
                    masterQry = "Insert Into GWDS_MST(MST_SL,MP_Group,YEAR,MONTH_NUMBER,MST_STATUS) Values(" + MaxID + ",'" + master.MPGroup + "'," + master.Year + ",'" + master.MonthNumber + "','Approved')";

                }
                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), masterQry))
                {
                    string DelDetail = "Delete from GWDS_DTL Where MST_SL=" + MaxID + "";
                    dbHelper.CmdExecute(dbConn.SAConnStrReader(), DelDetail);

                    foreach (GiftWiseDoctorSelectDetail detail in master.DetailList)
                    {
                        string QryDetail = "Insert Into GWDS_DTL(MST_SL,PRODUCT_CODE,DOCTOR_ID,DTL_STATUS) Values(" + MaxID + ",'" + detail.GiftCode + "','" + detail.DoctorID + "','Approved') ";

                        if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), QryDetail))
                        {
                            isTrue = true;
                        }
                    }

                }
                if (pushNotification.IsConnectedToInternet())
                {
                    pushNotification.SaveToDatabase(master.MPGroup, "GWDS", "GWDS Approved," + master.MonthName, "GWDS of " + master.MonthName + " Approved.", master.Year, master.MonthNumber);

                    pushNotification.SinglePushNotification(dbHelper.GetSingleToken(master.MPGroup), "GWDS", "GWDS Approved", " GWDS of " + master.MonthName + "  Approved. ");
                  
                }
            }
       
            return isTrue;
        }



        public List<GiftWiseDoctorSelectBEL> GetPopupView(DefaultParameterBEO model)
        {
            string Qry = "SELECT distinct  MST_SL,MPO_CODE,MPO_NAME,MP_Group,MONTH_NUMBER,MONTH_NAME,YEAR,MST_STATUS From VW_GWDS ";
            Qry = Qry + " Where  YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";

            string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }
            // Qry = Qry + " And MP_Group in(" + HttpContext.Current.Session["MPGroupForQry"].ToString() + ")";

            Qry = Qry + "Order by MPO_NAME";
            
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GiftWiseDoctorSelectBEL> item;

            item = (from DataRow row in dt.Rows
                    select new GiftWiseDoctorSelectBEL
                    {

                        MasterSL = row["MST_SL"].ToString(),
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                        MPGroup = row["MP_Group"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        MonthName = row["MONTH_NAME"].ToString(),
                        Year = row["YEAR"].ToString(),
                        MasterStatus = row["MST_STATUS"].ToString(),
                    }).ToList();
            return item;
        }





    }
}
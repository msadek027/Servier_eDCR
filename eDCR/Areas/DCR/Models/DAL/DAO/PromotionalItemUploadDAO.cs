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
    public class PromotionalItemUploadDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        public List<PromotionalItemUploadDetail> GetSampleData(string MonthNumber, string Year,string ItemType, string ItemFor)
        {
            string TableName = ItemType == "Gt" ? "INV_GIFT" : "INV_PRODUCT";
            string Qry = " SELECT distinct A.PRODUCT_CODE,B.PRODUCT_NAME,A.MP_GROUP,A.ITEM_FOR,A.QUANTITY,To_Char(A.SET_DATE,'dd-mm-yyyy') SET_DATE,A.MONTH_NUMBER,A.YEAR "+
                         " from INV_ITEM A,"+ TableName+ " B Where A.PRODUCT_CODE = B.PRODUCT_CODE "+
                         " And A.MONTH_NUMBER = '" + MonthNumber + "' AND A.YEAR = '" + Year + "' AND A.ITEM_TYPE='" + ItemType + "' AND A.ITEM_FOR = '" + ItemFor + "' "+
                         " Order by PRODUCT_CODE";
      

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<PromotionalItemUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemUploadDetail
                    {
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        ItemFor = row["ITEM_FOR"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),  
                    }).ToList();
            return item;
        }



      

       

        public bool SaveUpdate(PromotionalItemUploadBEL model)
        {
            try
            {

                foreach (PromotionalItemUploadDetail detailModel in model.ItemList)
                {
                    string cntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
                    string inputMonthYear = detailModel.MonthNumber + "-" + detailModel.Year;
                    if (cntMonthYear == inputMonthYear)
                    {
                        detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? CntDate : detailModel.SetDate;
                    }
                    if (cntMonthYear != inputMonthYear)
                    {
                        string activeDate = "01-" + detailModel.MonthNumber + "-" + detailModel.Year;
                        detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? activeDate : detailModel.SetDate;
                    }
                    if (Convert.ToInt16(detailModel.Quantity) > 0)
                    {

                        string Qry = "";
                        string QryIsExists = "Select PRODUCT_CODE From INV_ITEM Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "'  and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";
                        var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                        if (tuple1.Item1)
                        {
                            IUMode = "U";
                            Qry = "Update INV_ITEM set SET_DATE=To_Date('" + detailModel.SetDate + "','dd-MM-yyyy'), QUANTITY=" + detailModel.Quantity + " Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "' and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";

                        }
                        else
                        {
                            IUMode = "I";
                            Qry = "INSERT Into INV_ITEM(PRODUCT_CODE, QUANTITY, SET_DATE, MP_GROUP, YEAR, MONTH_NUMBER, ITEM_TYPE, ITEM_FOR, REMARKS) VALUES('" + detailModel.ProductCode + "'," + detailModel.Quantity + ",To_Date('" + detailModel.SetDate + "','dd-MM-yyyy'),'" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "','" + detailModel.ItemType + "','" + detailModel.ItemFor + "','')";

                        }
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                    }
                }

                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }


        }

       

        public List<PromotionalItemUploadDetail> GetSampleItemMap(DataTable dt)
        {
            List<PromotionalItemUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemUploadDetail
                    {                        
                        ProductCode = row["ProductCode"].ToString(),
                        ProductName = row["ProductName"].ToString(),                       
                        Quantity = row["Quantity"].ToString(),
                        MPGroup = row["MPGroup"].ToString(),

                    }).ToList();
            return item;
        }

      


        public bool SaveUpdateSampleFormatData(PromotionalItemFormatDataUploadBEO model)
        {
            try
            {

                string delQry = "Delete from OP_INV_SAMPLE Where FILE_NAME='"+model.FileName+"'";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), delQry);
                foreach (PromotionalItemFormatDataUploadDetail detailModel in model.ItemList)
                {
                    string cntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
                    string inputMonthYear = detailModel.MonthNumber + "-" + detailModel.Year;
                    //if (cntMonthYear == inputMonthYear)
                    //{
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? CntDate : detailModel.SetDate;
                    //}
                    //if (cntMonthYear != inputMonthYear)
                    //{
                    //    string activeDate = "01-" + detailModel.MonthNumber + "-" + detailModel.Year;
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? activeDate : detailModel.SetDate;
                    //}
                    if (Convert.ToInt16(detailModel.Quantity) > 0)
                    {

                        string Qry = "";
                        //string QryIsExists = "Select PRODUCT_CODE From INV_ITEM Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "'  and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";
                        //var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                        //if (tuple1.Item1)
                        //{
                        //    IUMode = "U";
                        //    Qry = "Update INV_ITEM set SET_DATE=To_Date('" + detailModel.SetDate + "','dd-MM-yyyy'), QUANTITY=" + detailModel.Quantity + " Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "' and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";

                        //}
                        //else
                        //{
                            IUMode = "I";
                            Qry = "INSERT Into OP_INV_SAMPLE(PRODUCT_CODE, QUANTITY, MP_GROUP, ITEM_FOR, FILE_NAME) VALUES('" + detailModel.ProductCode + "'," + detailModel.Quantity + ",'" + detailModel.MPGroup + "','" + detailModel.ItemFor + "','" + detailModel.FileName + "')";

                        //}
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                    }
                }
                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public List<PromotionalItemUploadDetail> GetSampleFormatDataMap(DataTable dt)
        {
            List<PromotionalItemUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemUploadDetail
                    {
                        ProductCode = row["ProductCode"].ToString(),             
                        Quantity = row["Quantity"].ToString(),
                        MPGroup = row["MPGroup"].ToString(),
                        //Date = 

                    }).ToList();
            return item;
        }



        public bool SaveUpdateGiftFormatData(PromotionalItemFormatDataUploadBEO model)
        {
            try
            {

                string delQry = "Delete from OP_INV_GIFT Where FILE_NAME='" + model.FileName + "'";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), delQry);
                foreach (PromotionalItemFormatDataUploadDetail detailModel in model.ItemList)
                {
                    string cntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
                    string inputMonthYear = detailModel.MonthNumber + "-" + detailModel.Year;
                    //if (cntMonthYear == inputMonthYear)
                    //{
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? CntDate : detailModel.SetDate;
                    //}
                    //if (cntMonthYear != inputMonthYear)
                    //{
                    //    string activeDate = "01-" + detailModel.MonthNumber + "-" + detailModel.Year;
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? activeDate : detailModel.SetDate;
                    //}
                    if (Convert.ToInt16(detailModel.Quantity) > 0)
                    {

                        string Qry = "";
                        //string QryIsExists = "Select PRODUCT_CODE From INV_ITEM Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "'  and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";
                        //var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                        //if (tuple1.Item1)
                        //{
                        //    IUMode = "U";
                        //    Qry = "Update INV_ITEM set SET_DATE=To_Date('" + detailModel.SetDate + "','dd-MM-yyyy'), QUANTITY=" + detailModel.Quantity + " Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "' and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";

                        //}
                        //else
                        //{
                        IUMode = "I";
                        Qry = "INSERT Into OP_INV_GIFT(PRODUCT_Name,GIFT_NAME, QUANTITY, MP_GROUP, ITEM_FOR, FILE_NAME) VALUES('" + detailModel.ProductName.Trim() + "','" + detailModel.GiftName.Trim() + "'," + detailModel.Quantity + ",'" + detailModel.MPGroup + "','" + detailModel.ItemFor + "','" + detailModel.FileName + "')";

                        //}
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                    }
                }
                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public List<PromotionalItemFormatDataUploadDetail> GetGiftFormatDataMap(DataTable dt)
        {
            List<PromotionalItemFormatDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemFormatDataUploadDetail
                    {
                        ProductName = row["ProductName"].ToString(),
                        GiftName = row["GiftName"].ToString(),
                        Quantity = row["Quantity"].ToString(),
                        MPGroup = row["MPGroup"].ToString(),
                        //Date = 

                    }).ToList();
            return item;
        }



        public bool SaveUpdateStarFormatData(PromotionalItemFormatDataUploadBEO model)
        {
            try
            {

                string delQry = "Delete from OP_INV_STAR ";
                dbHelper.CmdExecute(dbConn.SAConnStrReader(), delQry);
                foreach (PromotionalItemFormatDataUploadDetail detailModel in model.ItemList)
                {
                    string cntMonthYear = DateTime.Now.ToString("MM-yyyy", CultureInfo.CurrentCulture);
                    string inputMonthYear = detailModel.MonthNumber + "-" + detailModel.Year;
                    //if (cntMonthYear == inputMonthYear)
                    //{
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? CntDate : detailModel.SetDate;
                    //}
                    //if (cntMonthYear != inputMonthYear)
                    //{
                    //    string activeDate = "01-" + detailModel.MonthNumber + "-" + detailModel.Year;
                    //    detailModel.SetDate = (detailModel.SetDate == "" || detailModel.SetDate == null) ? activeDate : detailModel.SetDate;
                    //}
                   
                        string Qry = "";
                        //string QryIsExists = "Select PRODUCT_CODE From INV_ITEM Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "'  and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";
                        //var tuple1 = dbHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                        //if (tuple1.Item1)
                        //{
                        //    IUMode = "U";
                        //    Qry = "Update INV_ITEM set SET_DATE=To_Date('" + detailModel.SetDate + "','dd-MM-yyyy'), QUANTITY=" + detailModel.Quantity + " Where PRODUCT_CODE='" + detailModel.ProductCode + "' and Month_Number='" + detailModel.MonthNumber + "' and Year=" + detailModel.Year + " and MP_GROUP='" + detailModel.MPGroup + "' and Item_Type='" + detailModel.ItemType + "' and Item_For='" + detailModel.ItemFor + "' and To_Char(SET_DATE,'dd-mm-yyyy')='" + detailModel.SetDate + "'";

                        //}
                        //else
                        //{
                        IUMode = "I";
                        Qry = "INSERT Into OP_INV_STAR(PRODUCT_CODE, SBU, MARKET_GROUP) VALUES('" + detailModel.ProductCode + "','" + detailModel.SBU + "','" + detailModel.MarketGroup + "')";

                        //}
                        dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                    
                }
                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public List<PromotionalItemFormatDataUploadDetail> GetStarFormatDataMap(DataTable dt)
        {
            List<PromotionalItemFormatDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemFormatDataUploadDetail
                    {
                        ProductCode = row["ProductCode"].ToString(),
                        SBU = row["SBU"].ToString(),
                        MarketGroup = row["MarketGroup"].ToString(),
               

                    }).ToList();
            return item;
        }


        public List<PromotionalItemUploadDetail> GetSampleAllData(string MonthNumber, string Year)
        {
           
            string Qry = " SELECT  A.PRODUCT_CODE,B.PRODUCT_NAME,A.MP_GROUP,A.ITEM_TYPE,A.ITEM_FOR,A.QUANTITY,To_Char(A.SET_DATE,'dd-mm-yyyy') SET_DATE,A.MONTH_NUMBER,A.YEAR " +
                         " from INV_ITEM A,VW_INV_ITEM B Where A.PRODUCT_CODE = B.PRODUCT_CODE AND SUBSTR (A.MP_GROUP,INSTR (A.MP_GROUP, 'R1', -1) + 2,3)=B.SBU" +
                         " And A.MONTH_NUMBER = '" + MonthNumber + "' AND A.YEAR = '" + Year + "'  " +
                         " Order by A.PRODUCT_CODE";


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<PromotionalItemUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemUploadDetail
                    {
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        Quantity = row["QUANTITY"].ToString(),
                        ItemFor = row["ITEM_FOR"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString(),
                        SetDate = row["SET_DATE"].ToString(),
                    }).ToList();
            return item;
        }


    }
}
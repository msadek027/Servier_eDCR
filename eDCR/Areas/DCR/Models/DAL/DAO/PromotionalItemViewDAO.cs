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
    public class PromotionalItemViewDAO : ReturnData
    {
        PushNotification pushNotification = new PushNotification();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);

        DefaultDAO defaultDAO = new DefaultDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();



        public List<PromotionalItemViewDetail> MainGridDataNew(DefaultParameterBEO model)
        {
             // MySqlConnection
            //string QryForMPGroup = HttpContext.Current.Session["MPGroupForQry"].ToString();

            string Qry = " Select A.MP_GROUP, A.YEAR, A.MONTH_NUMBER, A.PRODUCT_CODE,A.PRODUCT_NAME || ' ('||A.PACK_SIZE||')' PRODUCT_NAME, A.ITEM_TYPE, A.ITEM_FOR,Sum(A.REST_QTY) REST_QTY,Sum(A.CENTRAL_QTY) CENTRAL_QTY,Sum(A.VARIABLE_QTY) VARIABLE_QTY,Sum(A.VARIABLE_QTY)+Sum(A.CENTRAL_QTY) GIVEN_QTY,Sum(A.EXECUTE_QTY) EXECUTE_QTY,Sum(A.VARIABLE_QTY)+Sum(A.CENTRAL_QTY)+Sum(A.REST_QTY) TOTAL_QTY,Sum(A.BALANCE_QTY) BALANCE_QTY" +
                        " from VW_INV_ITEM_BALANCE A,VW_HR_LOC_MAPPING B Where  A.MP_GROUP=B.MP_GROUP AND A.PRODUCT_CODE!='SmRPPM' AND  A.YEAR=" + model.Year + "  And A.MONTH_NUMBER='" + model.MonthNumber + "'";
              
            if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + " And A.ITEM_TYPE='" + model.ItemType + "'";
            }
            if (model.ItemFor != "" && model.ItemFor != null)
            {
                Qry = Qry + " And A.ITEM_FOR='" + model.ItemFor + "'";
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " And A.MP_GROUP='" + model.MPGroup + "'";
            }
            else 
            {
                string WhereClause = defaultDAO.GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + " AND " + WhereClause;
                }
               
            }


            Qry = Qry + " Group by A.MP_GROUP, YEAR, MONTH_NUMBER, PRODUCT_CODE, PRODUCT_NAME,PACK_SIZE, ITEM_TYPE, ITEM_FOR";
            Qry = Qry + " Order by PRODUCT_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<PromotionalItemViewDetail> item;

            item = (from DataRow row in dt.Rows
                    select new PromotionalItemViewDetail
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString(),
                        ItemFor = row["ITEM_For"].ToString(),

                        RestQty = row["REST_QTY"].ToString(),
                        CentralQty = row["CENTRAL_QTY"].ToString(),
                        TotalQty =row["TOTAL_QTY"].ToString() ,
                        VariableQty = row["VARIABLE_QTY"].ToString(),

                        GivenQty = row["GIVEN_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                        LossQty = "0",
                        GainQty = "0",

                        Remark = "",
                        MPGroup = row["MP_GROUP"].ToString(),
                        MonthNumber = row["MONTH_NUMBER"].ToString(),
                        Year = row["YEAR"].ToString()

                    }).ToList();
            return item;
        }


  
        public List<PromotionalItemViewDetail> GetProduct(PromotionalItemViewDetail model)
        {
             string Qry ="";       
             if (model.ItemType == "Gt")
             {
                 Qry = "Select distinct Product_Code,Product_Name||' | '||HIGHLIGHTING_PRODUCT Product_Name from INV_GIFT Where MARKET_GROUP||SBU IN (Select MP from VW_HR_LOC_MAPPING Where MP_GROUP='" + model.MPGroup + "')";
             }
             else
             {
                 Qry = " Select distinct Product_Code,Product_Name||' | '||PACK_SIZE Product_Name from INV_PRODUCT Where MARKET_GROUP||SBU IN (Select MP from VW_HR_LOC_MAPPING Where MP_GROUP='" + model.MPGroup + "')";
             }

            Qry=Qry+" and Product_Code not IN (Select Product_Code from INV_ITEM Where MP_GROUP='"+model.MPGroup+"' and YEAR=" + model.Year + " and Month_Number='" + model.MonthNumber + "' ";
           
            if (model.ItemType != "" && model.ItemType != null)
            {
                Qry = Qry + " and ITEM_TYPE='" + model.ItemType + "'";
            }       
            if (model.ItemFor != "" && model.ItemFor != null)
            {
                Qry = Qry + " and ITEM_FOR='" + model.ItemFor + "'";
            }
            Qry = Qry + ") order by Product_Name";         

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<PromotionalItemViewDetail> item;
            item = (from DataRow row in dt.Rows
                    select new PromotionalItemViewDetail
                    {
                        ProductCode = row["PRODUCT_CODE"].ToString(),
                        ProductName = row["PRODUCT_NAME"].ToString(),

                    }).ToList();
            return item;
        }

        public bool SaveUpdateSingle(PromotionalItemViewBEL model)
        {
            bool isTrue = false;
            try
            {               
                if (model.ItemList != null)
                {
                    foreach (PromotionalItemViewDetail detailModel in model.ItemList)
                    {                    

                        if (detailModel.ItemType!="Gt")
                        {
                            int index1 = detailModel.ProductCode.IndexOf('S', 0);
                            //if (detailModel.ProductCode.Length == 10)
                            //{
                            if (index1 < 0)
                            {
                                detailModel.ProductCode = detailModel.ItemType + detailModel.ItemFor + detailModel.ProductCode;
                            }
                        }
                      if (detailModel.ItemType == "Gt")
                        {
                            int index1 = detailModel.ProductCode.IndexOf('G', 0);
                            if (index1<0)
                            {
                                detailModel.ProductCode = detailModel.ItemType + detailModel.ItemFor + detailModel.ProductCode;
                            }
                                 
                       }
                        string ItemTypeItemFor = detailModel.ItemType + detailModel.ItemFor;

                        Int64 phyQty = Convert.ToInt64(detailModel.PhysicalQty) - Convert.ToInt64(detailModel.BalanceQty);
                        if (phyQty>0)
                        {
                         detailModel.GainQty=phyQty.ToString();
                        }
                        if (phyQty<0)
                        {
                            detailModel.LossQty = (phyQty*(-1)).ToString();
                        }

                        if (Convert.ToInt64(detailModel.LossQty) > 0 && (Convert.ToInt64(detailModel.BalanceQty) >= Convert.ToInt64(detailModel.LossQty)))
                            {
                                detailModel.AdjustmentType = "Loss";
                                string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.LossQty + ",'" + detailModel.Remark + "')";
                                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                                {
                                    IUMode = "I";
                                    isTrue = true;
                                }                        
                            }
                            if (Convert.ToInt64(detailModel.GainQty) > 0)
                            {
                                detailModel.AdjustmentType = "Gain";
                                string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.GainQty + ",'" + detailModel.Remark + "')";
                                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                                {
                                    IUMode = "I";
                                    isTrue = true;
                                }
                            }



                  
                          if (pushNotification.IsConnectedToInternet())
                          {
                            pushNotification.SaveToDatabase(detailModel.MPGroup, ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.", detailModel.Year, detailModel.MonthNumber);
                            pushNotification.SinglePushNotification(dbHelper.GetSingleToken(detailModel.MPGroup), ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.");

                            }
                        }
                    
                }

                return isTrue;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }

        public bool SaveUpdateNew(PromotionalItemViewBEL model)
        {
            bool isTrue = false;
            try
            {

                if (model.ItemList != null)
                {
                    foreach (PromotionalItemViewDetail detailModel in model.ItemList)
                    {

                        string ItemTypeItemFor = detailModel.ProductCode.Substring(0, 3);
                        detailModel.ProductCode =detailModel.ItemType+detailModel.ItemFor+detailModel.ProductCode;
                        if (Convert.ToInt64(detailModel.GainQty) > 0)
                        {
                            detailModel.AdjustmentType = "Gain";
                            string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.GainQty + ",'" + detailModel.Remark + "')";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                            {
                                IUMode = "I";
                                isTrue = true;
                            }

                        }
                        if (pushNotification.IsConnectedToInternet())
                        {
                            pushNotification.SaveToDatabase(detailModel.MPGroup, ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.", detailModel.Year, detailModel.MonthNumber);

                            pushNotification.SinglePushNotification(dbHelper.GetSingleToken(detailModel.MPGroup), ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.");
                          

                        }
                    }

                }

                return isTrue;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public bool SaveUpdateVoidSingle(PromotionalItemViewBEL model)
        {
            bool isTrue = false;
            try
            {

                if (model.ItemList != null)
                {
                    foreach (PromotionalItemViewDetail detailModel in model.ItemList)
                    {

                        string ItemTypeItemFor = detailModel.ProductCode.Substring(0, 3);
                        
                            detailModel.AdjustmentType = "Loss";
                            if (detailModel.BalanceQty != "" && detailModel.BalanceQty != null)
                            {
                                string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.BalanceQty + ",'" + detailModel.Remark + "')";
                                if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                                {
                                    IUMode = "D";
                                    isTrue = true;
                                }
                            }
                        if (pushNotification.IsConnectedToInternet())
                        {
                            pushNotification.SaveToDatabase(detailModel.MPGroup, ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.", detailModel.Year, detailModel.MonthNumber);

                            pushNotification.SinglePushNotification(dbHelper.GetSingleToken(detailModel.MPGroup), ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.");
                           

                        }
                    }

                }

                return isTrue;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public bool DirtyAllSaveUpdate(PromotionalItemViewBEL model)
        {
            bool isTrue = false;
            try
            {
                if (model.ItemList != null)
                {
                    foreach (PromotionalItemViewDetail detailModel in model.ItemList)
                    {

                        string ItemTypeItemFor = detailModel.ProductCode.Substring(0, 3);

                        if (Convert.ToInt64(detailModel.LossQty) > 0 && (Convert.ToInt64(detailModel.BalanceQty) >= Convert.ToInt64(detailModel.LossQty)))
                        {
                            detailModel.AdjustmentType = "Loss";
                            string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.LossQty + ",'" + detailModel.Remark + "')";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                            {
                                IUMode = "I";
                                isTrue = true;
                            }


                        }
                        if (Convert.ToInt64(detailModel.GainQty) > 0)
                        {
                            detailModel.AdjustmentType = "Gain";
                            string Qry = "INSERT INTO INV_ITEM_ADJUSTMENT(MP_GROUP, YEAR, MONTH_NUMBER, SET_DATE, PRODUCT_CODE, ADJUSTMENT_TYPE, GIVEN_QTY,REMARKS) VALUES('" + detailModel.MPGroup + "'," + detailModel.Year + ",'" + detailModel.MonthNumber + "',TO_Date('" + CntDate + "','dd-mm-yyyy') ,'" + detailModel.ProductCode + "','" + detailModel.AdjustmentType + "'," + detailModel.GainQty + " ,'" + detailModel.Remark + "')";
                            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                            {
                                IUMode = "I";
                                isTrue = true;
                            }

                        }
                        if (pushNotification.IsConnectedToInternet())
                        {
                            pushNotification.SaveToDatabase(detailModel.MPGroup, ItemTypeItemFor, ItemTypeItemFor + " Updated.", "Please sync it.", detailModel.Year, detailModel.MonthNumber);

                            pushNotification.SinglePushNotification(dbHelper.GetSingleToken(detailModel.MPGroup), detailModel.ItemType, detailModel.ItemType + " Updated.", "Please sync it.");
                         

                        }

                    }
                }
                return isTrue;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }


        public List<ReportPromotionalItemExecuion> ReportPromotionalItemExecution(DefaultParameterBEO model)
        {
            string Qry = " Select MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME,PRODUCT_CODE,PRODUCT_NAME || ' ('||PACK_SIZE||')' PRODUCT_NAME, ITEM_TYPE, ITEM_FOR,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                         " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

       
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }
            if (model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }

            if (model.ItemType != "All" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "' ";

            }
            Qry = Qry + " Group by MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME, PRODUCT_CODE, PRODUCT_NAME,PACK_SIZE, ITEM_TYPE, ITEM_FOR";
            Qry = Qry + " Order by REGION_NAME   ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);

            List<ReportPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportPromotionalItemExecuion
                    {
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MpoCode = row["MPO_CODE"].ToString(),
                        MpoName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        SBU = row["PRODUCT_GROUP"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_For"].ToString(),

                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportPromotionalItemExecuion>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME,PRODUCT_CODE,PRODUCT_NAME || ' ('||PACK_SIZE||')' PRODUCT_NAME, ITEM_TYPE, ITEM_FOR,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                          " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month-Year: " + month + " - " + model.Year;
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
                Qry += " And TSM_ID = '" + model.TerritoryManagerID + "'";
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
            if (model.ProductCode != " " && model.ProductCode != "" && model.ProductCode != null)
            {
                Qry = Qry + " and PRODUCT_CODE='" + model.ProductCode + "'";
            }
            if (model.ItemType != "All" && model.ItemType != null)
            {
                Qry = Qry + "  and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "' ";

            }
            vHeader = vHeader + ", Print Date: " + CntDate;

            Qry = Qry + " Group by MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME, PRODUCT_CODE, PRODUCT_NAME,PACK_SIZE, ITEM_TYPE, ITEM_FOR";
            Qry = Qry + " Order by REGION_NAME   ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);

            List<ReportPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportPromotionalItemExecuion
                    {
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MpoCode = row["MPO_CODE"].ToString(),
                        MpoName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        SBU = row["PRODUCT_GROUP"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_For"].ToString(),

               

                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }


        public List<ReportPromotionalItemExecuion> ReportPromotionalItemCost(DefaultParameterBEO model)
        {
            string Qry = " Select MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME,PRODUCT_CODE,PRODUCT_NAME || ' ('||PACK_SIZE||')' PRODUCT_NAME, ITEM_TYPE, ITEM_FOR,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                         " ,Sum(EXECUTE_QTY)* COST_PER_UNIT Achievement" +
                         " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " and ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " and ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
            }
            if (model.MPGroup != "" && model.MPGroup != null)
            {
                Qry = Qry + " and MP_GROUP='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry = Qry + " and TSM_ID='" + model.TerritoryManagerID + "'";
            }
            if (model.RegionCode != "" && model.RegionCode != null)
            {
                Qry = Qry + " and REGION_CODE='" + model.RegionCode + "'";
            }

            Qry = Qry + " Group by MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME, PRODUCT_CODE, PRODUCT_NAME,PACK_SIZE, ITEM_TYPE, ITEM_FOR,COST_PER_UNIT";
            Qry = Qry + " Order by REGION_NAME   ";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportPromotionalItemExecuion
                    {
                        SL = row["Col1"].ToString(),
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MpoCode = row["MPO_CODE"].ToString(),
                        MpoName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        SBU = row["PRODUCT_GROUP"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_For"].ToString(),

                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),
                        BalanceQty = row["BALANCE_QTY"].ToString(),

                        PromotionalCost = row["Achievement"].ToString(),
                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<ReportPromotionalItemExecuion>> ExportCost(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME,PRODUCT_CODE,PRODUCT_NAME || ' ('||PACK_SIZE||')' PRODUCT_NAME, ITEM_TYPE, ITEM_FOR,Sum(REST_QTY) REST_QTY,Sum(CENTRAL_QTY) CENTRAL_QTY,Sum(VARIABLE_QTY) VARIABLE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY) GIVEN_QTY,Sum(EXECUTE_QTY) EXECUTE_QTY,Sum(VARIABLE_QTY)+Sum(CENTRAL_QTY)+Sum(REST_QTY) TOTAL_QTY,Sum(BALANCE_QTY) BALANCE_QTY" +
                         " ,Sum(EXECUTE_QTY)* COST_PER_UNIT Achievement" +
                         " from VW_INV_ITEM_BALANCE_HR Where PRODUCT_CODE!='SmRPPM' AND YEAR=" + model.Year + "  And MONTH_NUMBER='" + model.MonthNumber + "'";

            string month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt16(model.MonthNumber));

            vHeader = vHeader + "Month-Year: " + month + " - " + model.Year;
            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND MP_GROUP='" + model.MPGroup + "'";
            }

            string Item = "All";
            if (model.ItemType != "All" && model.ItemType != null)
            {
                if (model.ItemType == "SlSmRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Sl','Sm') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "GtRI")
                {
                    Qry = Qry + " AND ITEM_TYPE IN ('Gt') AND ITEM_FOR IN ('R','I')";
                }
                if (model.ItemType == "SlR" || model.ItemType == "SmR" || model.ItemType == "SmI" || model.ItemType == "GtR" || model.ItemType == "GtI")
                {
                    Qry = Qry + " AND ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
                Item = model.ItemTypeName;
            }
            vHeader = vHeader + ", Item Type : " + Item;
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
                Qry += " And TSM_ID = '" + model.TerritoryManagerID + "'";
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

            vHeader = vHeader + ", Print Date: " + CntDate;

            Qry = Qry + " Group by MPO_CODE, MPO_NAME,MARKET_NAME,PRODUCT_GROUP,TERRITORY_CODE, TERRITORY_NAME, REGION_CODE,REGION_NAME, PRODUCT_CODE, PRODUCT_NAME,PACK_SIZE, ITEM_TYPE, ITEM_FOR,COST_PER_UNIT";
            Qry = Qry + " Order by REGION_NAME   ";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            DataTable dt = dbHelper.dtIncremented(dt2);

            List<ReportPromotionalItemExecuion> item;

            item = (from DataRow row in dt.Rows
                    select new ReportPromotionalItemExecuion
                    {
                        SL = row["Col1"].ToString(),
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        MpoCode = row["MPO_CODE"].ToString(),
                        MpoName = row["MPO_NAME"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),

                        SBU = row["PRODUCT_GROUP"].ToString(),
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        ItemType = row["ITEM_TYPE"].ToString() + row["ITEM_For"].ToString(),

                        TotalQty = row["TOTAL_QTY"].ToString(),
                        ExecuteQty = row["EXECUTE_QTY"].ToString(),

                        BalanceQty = row["BALANCE_QTY"].ToString(),

                        PromotionalCost = row["Achievement"].ToString(),
                        
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }

    }
}
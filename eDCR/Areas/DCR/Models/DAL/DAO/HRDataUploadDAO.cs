using eDCR.Areas.DCR.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace eDCR.Areas.DCR.Models.DAL.DAO
{
    public class HRDataUploadDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        public List<HRDataUploadMPO> GetMPOGridData()
        {
            string Qry = " Select MPO_CODE, MPO_NAME, DESIGNATION, PHONE, PRODUCT_GROUP, MARKET_GROUP, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, REGION_CODE, REGION_NAME, DIVISION_CODE, COMPANY_CODE, TERRITORY_NAME, DEPOT_CODE, DEPOT_NAME, DIVISION_NAME from HR_LOC_MAPPING ";
            Qry = Qry + " Where MPO_CODE IS NOT NULL Order by MPO_CODE";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<HRDataUploadMPO> item;

            item = (from DataRow row in dt.Rows
                    select new HRDataUploadMPO
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        Phone = row["PHONE"].ToString(),
                        ProductGroup = row["PRODUCT_GROUP"].ToString(),

                        MarketGroup = row["MARKET_GROUP"].ToString(),
                        MarketCode = row["MARKET_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotCode = row["DEPOT_CODE"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),
                        DivisionCode = row["DIVISION_CODE"].ToString(),
                        DivisionName = row["DIVISION_NAME"].ToString(),
                        CompanyCode = row["COMPANY_CODE"].ToString(),


                    }).ToList();
            return item;
        }

        public bool SaveUpdate(HRDataUploadBEL model)
        {
            bool isTrue = false;



            using (OracleConnection connection = new OracleConnection(dbConn.SAConnStrReader()))
            {
                connection.Open();
                OracleCommand command = connection.CreateCommand();
                //OracleTransaction transaction;
                // Start a local transaction
                // OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                OracleTransaction transaction = connection.BeginTransaction();

                // Assign transaction object for a pending local transaction
                command.Transaction = transaction;

                try
                {
                    int i = 0;
                    if (model.hRDataUploadMPOList != null)
                    {
                        foreach (HRDataUploadMPO detail in model.hRDataUploadMPOList)
                        {

                            MaxID = "";
                            IUMode = "I";
                            detail.CompanyCode = "001";

                            detail.EmployeeID = detail.EmployeeID == null ? "" : detail.EmployeeID.Trim();
                            detail.Designation = detail.Designation == null ? "" : detail.Designation.Trim();
                            detail.ProductGroup = detail.ProductGroup == null ? "" : detail.ProductGroup.Trim();
                            detail.MarketGroup = detail.MarketGroup == null ? "" : detail.MarketGroup.Trim();
                            detail.MarketCode = detail.MarketCode == null ? "" : detail.MarketCode.Trim();
                            detail.TerritoryCode = detail.TerritoryCode == null ? "" : detail.TerritoryCode.Trim();
                            detail.RegionCode = detail.RegionCode == null ? "" : detail.RegionCode.Trim();
                            detail.DepotCode = detail.DepotCode == null ? "" : detail.DepotCode.Trim();
                            detail.DivisionCode = detail.DivisionCode == null ? "" : detail.DivisionCode.Trim();
                            detail.Phone = detail.Phone == null ? "" : detail.Phone.Trim().Replace("'", "''");

                            string QryUpdate = " Update HR_LOC_MAPPING Set MPO_CODE='',MPO_NAME='', PHONE = '' Where MPO_CODE='" + detail.EmployeeID + "'";
                            command.CommandText = QryUpdate;
                            command.ExecuteNonQuery();
                            string QryInsert = "Insert Into HR_LOC_MAPPING(MPO_CODE, MPO_NAME, DESIGNATION, PHONE, PRODUCT_GROUP, MARKET_GROUP, MARKET_CODE, MARKET_NAME, TERRITORY_CODE,TERRITORY_NAME, REGION_CODE, REGION_NAME,DEPOT_CODE,DEPOT_NAME, DIVISION_CODE,DIVISION_NAME, COMPANY_CODE) " +
                                  " Values('" + detail.EmployeeID + "','" + detail.EmployeeName + "','" + detail.Designation + "','" + detail.Phone + "','" + detail.ProductGroup + "','" + detail.MarketGroup + "'," +
                                  " '" + detail.MarketCode + "','" + detail.MarketName + "','" + detail.TerritoryCode + "','" + detail.TerritoryName + "','" + detail.RegionCode + "','" + detail.RegionName + "'," +
                                  " '" + detail.DepotCode + "','" + detail.DepotName + "','" + detail.DivisionCode + "','" + detail.DivisionName + "','" + detail.CompanyCode + "')";

                            if (detail.Designation.Trim() == "MPO" || detail.Designation.Trim() == "SMPO")
                            {
                                string QryDel = @"Delete From HR_LOC_MAPPING Where DESIGNATION IN ('MPO','SMPO') AND MARKET_CODE='" + detail.MarketCode + "'";

                                command.CommandText = QryDel;
                                command.ExecuteNonQuery();
                            }
                            if (detail.Designation.Trim() == "TM")
                            {
                                string QryDel = @" Delete From HR_LOC_MAPPING Where DESIGNATION = 'TM' AND TERRITORY_CODE='" + detail.TerritoryCode + "'";
                                command.CommandText = QryDel;
                                command.ExecuteNonQuery();
                            }
                            if (detail.Designation.Trim() == "RSM")
                            {
                                string QryDel = @" Delete From HR_LOC_MAPPING Where DESIGNATION = 'RSM' AND REGION_CODE='" + detail.RegionCode + "'";
                                command.CommandText = QryDel;
                                command.ExecuteNonQuery();

                            }

                            try
                            {
                                isTrue = false;
                                command.CommandText = QryInsert;
                                command.ExecuteNonQuery();
                                i = i + 1;
                                if (i == model.hRDataUploadMPOList.Count)
                                {
                                    isTrue = true;
                                    transaction.Commit();
                                }

                            }
                            catch (Exception)
                            {
                                transaction.Rollback();
                            }
                        }//foreach
                    }//List

                } //End try

                catch (Exception)
                {
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }//End connection
            return isTrue;
        }

        private bool IsExistsLocation(string CompositeKey)
        {
            bool isTrue = false;
            string Qry = "SELECT MPO_CODE FROM HR_LOC_MAPPING WHERE MARKET_CODE||MARKET_GROUP||PRODUCT_GROUP||TERRITORY_CODE||REGION_CODE = '" + CompositeKey + "'";
            DataTable dt2 = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            if (dt2.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        public List<HRDataUploadMPO> GetMPO(DataTable dt)
        {
            List<HRDataUploadMPO> item;

            item = (from DataRow row in dt.Rows
                    select new HRDataUploadMPO
                    {
                        EmployeeID = row["EmployeeID"].ToString(),
                        EmployeeName = row["EmployeeName"].ToString(),
                        Designation = row["Designation"].ToString(),
                        Phone = row["Phone"].ToString(),

                        ProductGroup = row["ProductGroup"].ToString(),
                        MarketGroup = row["MarketGroup"].ToString(),

                        MarketCode = row["MarketCode"].ToString(),
                        MarketName = row["MarketName"].ToString(),
                        TerritoryCode = row["TerritoryCode"].ToString(),
                        TerritoryName = row["TerritoryCode"].ToString(),
                        RegionCode = row["RegionCode"].ToString(),
                        RegionName = row["RegionName"].ToString(),
                        DepotCode = row["DepotCode"].ToString(),
                        DepotName = row["DepotName"].ToString(),
                        DivisionCode = row["DivisionCode"].ToString(),
                        DivisionName = row["TerritoryCode"].ToString(),

                        CompanyCode = "001",
                    }).ToList();
            return item;
        }

        public List<HRDataUploadMPO> GetMPOGridDataArchive(DefaultParameterBEO model)
        {
            string Qry = "";
            Qry = @" Select MPO_CODE, MPO_NAME, DESIGNATION, PHONE, PRODUCT_GROUP, MARKET_GROUP, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, REGION_CODE, REGION_NAME, DIVISION_CODE, COMPANY_CODE, TERRITORY_NAME, DEPOT_CODE, DEPOT_NAME, DIVISION_NAME from ARC_HR_LOC_MAPPING 
                             Where YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' AND MPO_CODE IS NOT NULL Order by MPO_CODE";
            int month = ((Convert.ToInt16(model.Year) - Convert.ToInt16(DateTime.Now.Year)) * 12) + Convert.ToInt16(model.MonthNumber) - Convert.ToInt16(DateTime.Now.Month);


            if (month == 0)
            {
                Qry = @" Select MPO_CODE, MPO_NAME, DESIGNATION, PHONE, PRODUCT_GROUP, MARKET_GROUP, MARKET_CODE, MARKET_NAME, TERRITORY_CODE, REGION_CODE, REGION_NAME, DIVISION_CODE, COMPANY_CODE, TERRITORY_NAME, DEPOT_CODE, DEPOT_NAME, DIVISION_NAME from HR_LOC_MAPPING 
                             Where  MPO_CODE IS NOT NULL Order by MPO_CODE";

            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<HRDataUploadMPO> item;

            item = (from DataRow row in dt.Rows
                    select new HRDataUploadMPO
                    {
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        Phone = row["PHONE"].ToString(),
                        ProductGroup = row["PRODUCT_GROUP"].ToString(),

                        MarketGroup = row["MARKET_GROUP"].ToString(),
                        MarketCode = row["MARKET_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotCode = row["DEPOT_CODE"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),
                        DivisionCode = row["DIVISION_CODE"].ToString(),
                        DivisionName = row["DIVISION_NAME"].ToString(),
                        CompanyCode = row["COMPANY_CODE"].ToString(),


                    }).ToList();
            return item;
        }
    }
}
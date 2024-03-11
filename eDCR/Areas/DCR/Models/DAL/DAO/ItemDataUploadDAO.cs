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
    public class ItemDataUploadDAO : ReturnData
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();


        public List<ItemDataUploadDetail> GetInvProductData()
        {
            string Qry = "Select PRODUCT_CODE,PRODUCT_NAME,GENERIC_NAME,BRAND_NAME,PACK_SIZE,Market_Group,SBU,NVL(COST_PER_UNIT,0) COST_PER_UNIT from Inv_Product";
          
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ItemDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ItemDataUploadDetail
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        GenericName = row["Generic_Name"].ToString(),
                        PackSize = row["Pack_Size"].ToString(),
                        MarketGroup = row["Market_Group"].ToString(),
                        SBU = row["SBU"].ToString(),
                        BrandName = row["BRAND_NAME"].ToString(),
                        CostPerUnit = row["COST_PER_UNIT"].ToString(),
                    }).ToList();
            return item;
        }
        public List<ItemDataUploadDetail> GetInvGiftData()
        {
            string Qry  = "Select  PRODUCT_CODE, PRODUCT_NAME,HIGHLIGHTING_PRODUCT,Market_Group,SBU,NVL(COST_PER_UNIT,0) COST_PER_UNIT from INV_GIFT";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ItemDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ItemDataUploadDetail
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),
                        HilightingProduct = row["HIGHLIGHTING_PRODUCT"].ToString(),
                       
                        MarketGroup = row["Market_Group"].ToString(),
                        SBU = row["SBU"].ToString(),
                      
                        CostPerUnit = row["COST_PER_UNIT"].ToString(),
                    }).ToList();
            return item;
        }
       
       

        public bool SaveUpdateGift(ItemDataUploadBEL model)
        {
            try
            {
                    if (model.ItemList != null)
                    {
                        foreach (ItemDataUploadDetail detail in model.ItemList)
                        {
                            if (detail != null)
                            {
                            detail.ProductName = detail.ProductName == null ? "" : detail.ProductName.Replace("'", "''").Trim();
                            detail.PackSize = detail.PackSize == null ? "" : detail.PackSize.Replace("'", "''").Trim();
                            detail.GenericName = detail.GenericName == null ? "" : detail.GenericName.Replace("'", "''").Trim();
                            detail.BrandName = detail.BrandName == null ? "" : detail.BrandName.Replace("'", "''").Trim();
                            detail.HilightingProduct = detail.HilightingProduct == null ? "" : detail.HilightingProduct.Replace("'", "''").Trim();

                            detail.CostPerUnit = (detail.CostPerUnit == "" || detail.CostPerUnit == null) ? "0" : detail.CostPerUnit;
                           
                            
                            string Qry = "";
                                string Select = "Select Product_CODE From INV_Gift Where PRODUCT_CODE='" + detail.ProductCode + "' and  Market_Group='" + detail.MarketGroup + "' and SBU='" + detail.SBU + "'";
                                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Select);
                                if (dt.Rows.Count > 0)
                                {
                                    Qry = "Update INV_Gift set PRODUCT_NAME='" + detail.ProductName + "',HIGHLIGHTING_PRODUCT='" + detail.HilightingProduct + "',Market_Group='" + detail.MarketGroup + "',SBU='" + detail.SBU + "',COST_PER_UNIT=" + detail.CostPerUnit + " Where PRODUCT_CODE='" + detail.ProductCode + "' and  Market_Group='" + detail.MarketGroup + "' and SBU='" + detail.SBU + "'";
                                    IUMode = "U";
                                }
                                else
                                {
                                    Qry = "Insert into INV_Gift(PRODUCT_CODE,PRODUCT_NAME,HIGHLIGHTING_PRODUCT,Market_Group,SBU,COST_PER_UNIT) " +
                                          " Values('" + detail.ProductCode + "','" + detail.ProductName + "','" + detail.HilightingProduct + "','" + detail.MarketGroup + "','" + detail.SBU + "'," + detail.CostPerUnit + ")";

                                    IUMode = "I";
                                }

                                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                            }
                        }
                    }
                

                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public bool DeleteProduct(ItemDataUploadBEL model)
        {
            try
            {
                if (model.ItemList != null)
                {
                    foreach (ItemDataUploadDetail detailModel in model.ItemList)
                    {
                        if (detailModel != null)
                        {
                           
                            string Qry = "Delete From INV_PRODUCT Where PRODUCT_CODE='" + detailModel.ProductCode + "' and  Market_Group='" + detailModel.MarketGroup + "' and SBU='" + detailModel.SBU + "'";
                          
                             IUMode = "Del";                          
                            dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        }
                    }
                }


                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }

        public bool DeleteGift(ItemDataUploadBEL model)
        {
            try
            {
                if (model.ItemList != null)
                {
                    foreach (ItemDataUploadDetail detailModel in model.ItemList)
                    {
                        if (detailModel != null)
                        {
                            string Qry = "Delete From INV_GIFT Where PRODUCT_CODE='" + detailModel.ProductCode + "' and  Market_Group='" + detailModel.MarketGroup + "' and SBU='" + detailModel.SBU + "'";

                            IUMode = "Del";                          
                            dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                        }
                    }
                }


                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }

        public bool SaveUpdateProduct(ItemDataUploadBEL model)
        {
            try
            {
                    if (model.ItemList != null)
                    {
                        foreach (ItemDataUploadDetail detail in model.ItemList)
                        {

                            if (detail != null)
                            {
                            detail.ProductCode = detail.ProductCode == null ? "" : detail.ProductCode.Trim();
                            detail.ProductName = detail.ProductName == null ? "" : detail.ProductName.Replace("'", "''").Trim();
                            detail.PackSize = detail.PackSize == null ? "" : detail.PackSize.Replace("'", "''").Trim();
                            detail.GenericName = detail.GenericName == null ? "" : detail.GenericName.Replace("'", "''").Trim();              
                            detail.BrandName = detail.BrandName == null ? "" : detail.BrandName.Replace("'", "''").Trim();                          
                            detail.CostPerUnit = (detail.CostPerUnit == "" || detail.CostPerUnit == null) ? "0" : detail.CostPerUnit;

                            detail.MarketGroup = detail.MarketGroup == null ? "" : detail.MarketGroup.Trim();
                            detail.SBU = detail.SBU == null ? "" : detail.SBU.Trim();
                            string Qry = "";
                                string Select = "Select PRODUCT_CODE From INV_PRODUCT Where PRODUCT_CODE='" + detail.ProductCode + "' and  Market_Group='" + detail.MarketGroup + "' and SBU='" + detail.SBU + "'";
                                DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Select);
                                if (dt.Rows.Count > 0)
                                {
                                    IUMode = "U";
                                  
                                    Qry = @"Update INV_PRODUCT set PRODUCT_NAME='" + detail.ProductName + "',GENERIC_NAME='" + detail.GenericName + "',BRAND_NAME='" + detail.BrandName + "',PACK_SIZE='" + detail.PackSize + "',COST_PER_UNIT="+ detail.CostPerUnit + "   Where PRODUCT_CODE='" + detail.ProductCode + "' and  Market_Group='" + detail.MarketGroup + "' and SBU='" + detail.SBU + "'";

                                }
                                else
                                {
                                    IUMode = "I";
                                    Qry = "Insert into INV_PRODUCT(PRODUCT_CODE,PRODUCT_NAME,GENERIC_NAME,BRAND_NAME,PACK_SIZE,Market_Group,SBU,COST_PER_UNIT) " +
                                          " Values('" + detail.ProductCode + "','" + detail.ProductName+ "','" + detail.GenericName + "','" + detail.BrandName + "','" + detail.PackSize + "','" + detail.MarketGroup + "','" + detail.SBU + "'," + detail.CostPerUnit + ")";

                                }
                                dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry);
                            }
                        }
                    }
                return true;
            }

            catch (Exception errorException)
            {
                throw errorException;
            }
        }


        public List<ItemDataUploadDetail> ProductFromExcel(DataTable dt)
        {
            List<ItemDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ItemDataUploadDetail
                    {
                        ProductCode = row["ProductCode"].ToString(),
                        ProductName = row["ProductName"].ToString(),                       
                        GenericName = row["GenericName"].ToString(),
                        BrandName = row["BrandName"].ToString(),
                        PackSize = row["PackSize"].ToString(),
                        MarketGroup = row["MarketGroup"].ToString(),
                        SBU = row["SBU"].ToString(),
                        CostPerUnit = row["CostPerUnit"].ToString(),

                    }).ToList();
            return item;
        }

        public List<ItemDataUploadDetail> GiftItemFromExcel(DataTable dt)
        {
            List<ItemDataUploadDetail> item;

            item = (from DataRow row in dt.Rows
                    select new ItemDataUploadDetail
                    {
                        ProductCode = row["ProductCode"].ToString(),
                        ProductName = row["ProductName"].ToString(),
                        HilightingProduct = row["HilightingProduct"].ToString(),
                        MarketGroup = row["MarketGroup"].ToString(),
                        SBU = row["SBU"].ToString(),
                        CostPerUnit = row["CostPerUnit"].ToString(),
                    }).ToList();
            return item;
        }






        public List<ItemDataUploadDetail> GetReportProductGiftData(DefaultParameterBEO model)
        {
            string Qry = "";
        
            if (model.ItemTypeName == "Product")
            {
                Qry = "Select PRODUCT_CODE,PRODUCT_NAME,PACK_SIZE,GENERIC_NAME,BRAND_NAME,Market_Group,SBU,COST_PER_UNIT from Inv_Product";
            }
            if (model.ItemTypeName == "Gift")
            {
                Qry = "Select PRODUCT_CODE, PRODUCT_NAME,HIGHLIGHTING_PRODUCT PACK_SIZE,'' GENERIC_NAME,'' BRAND_NAME,Market_Group,SBU,COST_PER_UNIT from INV_GIFT";
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ItemDataUploadDetail> item;
            item = (from DataRow row in dt.Rows
                        select new ItemDataUploadDetail
                        {
                            ProductCode = row["Product_Code"].ToString(),
                            ProductName = row["Product_Name"].ToString(),
                            GenericName = row["Generic_Name"].ToString(),
                            PackSize = row["PACK_SIZE"].ToString(),
                            MarketGroup = row["Market_Group"].ToString(),
                            SBU = row["SBU"].ToString(),
                            BrandName = row["BRAND_NAME"].ToString(),
                            CostPerUnit = row["COST_PER_UNIT"].ToString(),
                        }).ToList();
            
         
       

            return item;
        }


    }
}
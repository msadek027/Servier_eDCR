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
    public class DefaultDAO
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();   
        string LocCodeSession = HttpContext.Current.Session["LocCode"].ToString();


        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntMonthNumber = DateTime.Now.ToString("MM", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);
        public List<GenMonth> GetMonth()
        {
            string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();

            string Qry = "Select Month_Number,Month_Name from Gen_Month Order by CASE WHEN Month_Number= '" + month + "' THEN 1 ELSE 2 END,Month_Number";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GenMonth> item;
            item = (from DataRow row in dt.Rows
                    select new GenMonth
                    {
                        MonthNumber = row["Month_Number"].ToString(),
                        MonthName = row["Month_Name"].ToString(),
                    }).ToList();
            return item;
        }

        public List<GenMonth> GetPreviousMonth()
        {
            string month = DateTime.Now.AddMonths(-1).Month.ToString().Length == 1 ? "0" + DateTime.Now.AddMonths(-1).Month.ToString() : DateTime.Now.AddMonths(-1).Month.ToString();

            string Qry = "Select Month_Number,Month_Name from Gen_Month Where Month_Number= '" + month + "'";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<GenMonth> item;
            item = (from DataRow row in dt.Rows
                    select new GenMonth
                    {
                        MonthNumber = row["Month_Number"].ToString(),
                        MonthName = row["Month_Name"].ToString(),
                    }).ToList();
            return item;
        }

        public string GetWhereClauseDefineAsUserLocCode(string LocCode,string EmpType)
        {
            string WhereClause = "";
            if (EmpType == "MPO" || EmpType == "SMPO" || EmpType == "MIO" || EmpType == "SMIO")
            {
                WhereClause = "  MARKET_CODE='" + LocCode + "'";
            }
            else if (EmpType == "TM" || EmpType == "RM")
            {
                WhereClause = "  TERRITORY_CODE='" + LocCode + "'";
            }
            else if (EmpType == "RSM" || EmpType == "ZM")
            {
                WhereClause = "  REGION_CODE='" + LocCode + "'";
            }
            else if (EmpType == "DSM" || EmpType == "PM")
            {
                WhereClause = "  DIVISION_CODE='" + LocCode + "'";
            }
            else if ( EmpType == "Executive" || EmpType == "Sr. Executive")
            {
                WhereClause = "  DIVISION_CODE='" + LocCode + "'";
            }
            else if (EmpType == "Manager" || EmpType == "Sr. Manager" || EmpType == "CM")
            {
                WhereClause = "  M_ID='" + LocCode + "'";
            }
            else if (EmpType == "EMA" )
            {
                WhereClause = "  1=1 ";
            }
            return WhereClause;
        }
        public string GetImmediateSubordinate_WhereClauseDefineAsUserLocCode(string LocCode, string EmpType)
        {
            string WhereClause = "";
            if (EmpType == "MPO" || EmpType == "SMPO" || EmpType == "MIO" || EmpType == "SMIO")
            {
                WhereClause = "  MARKET_CODE='" + LocCode + "'";
            }
            else if (EmpType == "TM" || EmpType == "RM")
            {
                WhereClause = "  TERRITORY_CODE='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
            }
            else if (EmpType == "RSM" || EmpType == "ZM")
            {
                WhereClause = "  REGION_CODE='" + LocCode + "' AND DESIGNATION='RM'";
            }
            else if (EmpType == "DSM" || EmpType == "PM")
            {
                WhereClause = "  DIVISION_CODE='" + LocCode + "' AND DESIGNATION='ZM'";
            }
            else if (EmpType == "Executive" || EmpType == "Sr. Executive")
            {
                WhereClause = "  DIVISION_CODE='" + LocCode + "' AND DESIGNATION='PM'";
            }
            else if (EmpType == "Manager" || EmpType == "Sr. Manager" || EmpType == "RM")
            {
                WhereClause = "  M_ID='" + LocCode + "' AND DESIGNATION IN ('ZM','PM') ";
            }
            else if (EmpType == "EMA")
            {
                WhereClause = "  1=1 ";
            }
            return WhereClause;
        }
        public List<DefaultBEL> GetMPOPopupList(string TerritoryManagerID)
        {
            string Qry = "SELECT  LOC_CODE,MPO_NAME||' - '||MPO_CODE||' | '||MARKET_NAME||' - '||MARKET_CODE||'R1'||PRODUCT_GROUP MPO_NAME from VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('MPO','SMPO','MIO','SMIO') ";
            if (TerritoryManagerID != "" && TerritoryManagerID != null)
            {
                Qry = Qry + " AND TERRITORY_CODE= '" + TerritoryManagerID + "'";
            }
            else
            {
                string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + " AND " + WhereClause;
                }
            }
            Qry = Qry + " ORDER BY MPO_NAME"; 

           
            
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        MPGroup = row["LOC_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                       
                    }).ToList();
            return item;
        }

        public List<DefaultBEL> GetAccompanyWith(string TerritoryManagerID)
        {         
            string Qry = " SELECT  MPO_CODE,MPO_NAME||' - '||MPO_CODE ||' , '||DESIGNATION||' | 'MARKET_NAME||' - '||MARKET_CODE EmpName,LOC_CODE from VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('MPO','SMPO','TM','RSM','DSM','Manager','MIO','SMIO','RM','ZM','PM','CM')";
            if (TerritoryManagerID != "" && TerritoryManagerID != null)
            {
                Qry = Qry + " AND MPO_CODE= '" + TerritoryManagerID + "'";
            }
            else
            {
                string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + " AND " + WhereClause;
                }
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        MPOCode = row["MPO_CODE"].ToString(),
                        MPOName = row["EmpName"].ToString(),
                        MPGroup = row["LOC_CODE"].ToString(),
                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetUserWiseMPOList()
        {
            string Qry = "SELECT LOC_CODE,MPO_NAME||' - '||MPO_CODE||' , '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE MPO_NAME  from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";

           string WhereClause= GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if(WhereClause!="" && WhereClause!=null)
            {
                Qry = Qry + " AND " + WhereClause;
            }          
             Qry = Qry + "Order by MPO_NAME";      
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        MPGroup = row["LOC_CODE"].ToString(),
                        MPOName = row["MPO_NAME"].ToString(),
                      
                    }).ToList();
            return item;
        }

       

        public List<ProductInfo> GetProductItem(ProductInfo model)
        {
            string WhereClause = "";
            string Designation = HttpContext.Current.Session["Designation"].ToString();
            if (EmpTypeSession == "EMA")
            {
                if (model.ItemType != "All")
                {
                    WhereClause =WhereClause+ " Where ITEM_TYPE||ITEM_FOR='" + model.ItemType + "'";
                }
            }
            else
            {
                if (Designation == "TM" || Designation == "RM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where TSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if (Designation == "RSM" || Designation == "ZM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where RSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if(Designation == "DSM" || Designation == "PM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where DSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if (Designation == "Manager" || Designation == "Sr. Manager" || Designation == "CM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where M_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if(model.ItemType != "All")
                {
                    WhereClause = WhereClause + " AND ITEM_TYPE||ITEM_For='" + model.ItemType + "'";
                }

            }

            string Qry = "Select * from ( Select ' ' Product_Code,' ' Product_Name from Dual Union Select Distinct OP_CODE Product_Code,Product_Name||' | '||OP_CODE Product_Name from VW_INV_ITEM "+WhereClause+" ) " +
                "a  ORDER BY CASE WHEN a.Product_Name = ' ' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ProductInfo> item;

            item = (from DataRow row in dt.Rows
                    select new ProductInfo
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),

                    }).ToList();
            return item;
        }


        public List<ProductInfo> GetProductItemMaster(ProductInfo model)
        {
            string Designation = HttpContext.Current.Session["Designation"].ToString();

            if (model.ItemType=="PI")
            {
                model.ItemType = "'SlR','SmR','SmI'";
            }
            if (model.ItemType == "GI")
            {
                model.ItemType = "'GtR','SmI'";
            }
            string WhereClause = "";
            if (EmpTypeSession == "EMA")
            {
                if (model.ItemType != "All")
                {
                    WhereClause = WhereClause + " Where ITEM_TYPE||ITEM_For IN (" + model.ItemType + ")";
                }
            }
            else
            {
                if(Designation == "TM" || Designation == "RM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where TSM_ID='"+ HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if (Designation == "RSM" || Designation == "ZM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where RSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if(Designation == "DSM" || Designation == "PM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where DSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if (Designation == "Manager" || Designation == "Sr. Manager" || Designation == "CM")
                {
                    WhereClause = " Where SBU IN (Select Distinct PRODUCT_GROUP from VW_HR_LOC_MAPPING Where DSM_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "')";
                }
                else if(model.ItemType != "All")
                {
                    WhereClause = WhereClause + " AND ITEM_TYPE||ITEM_For IN (" + model.ItemType + ")";
                }

            }

            string Qry = "Select * from ( Select ' ' Product_Code,' ' Product_Name from Dual Union Select Distinct  Product_Code,Product_Name||' | '||Product_Code Product_Name from VW_INV_ITEM " + WhereClause + " ) " +
                "a  ORDER BY CASE WHEN a.Product_Name = ' ' THEN 1 ELSE 2 END";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<ProductInfo> item;

            item = (from DataRow row in dt.Rows
                    select new ProductInfo
                    {
                        ProductCode = row["Product_Code"].ToString(),
                        ProductName = row["Product_Name"].ToString(),

                    }).ToList();
            return item;
        }
          public List<DefaultBEL> GetEmpForSup()
           {
            string Qry = "";
            if (EmpTypeSession == "TM" || EmpTypeSession == "RM")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
            }
            if (EmpTypeSession == "RSM" || EmpTypeSession == "ZM")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE ='" + LocCodeSession + "' AND DESIGNATION ='RM'";
            }
            else if (EmpTypeSession == "DSM" || EmpTypeSession == "PM")
            {
                Qry = "Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION ='ZM'";
            }
            else if(EmpTypeSession == "Manager" || EmpTypeSession == "Sr. Manager" || EmpTypeSession == "CM")
            {
                Qry = "Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName,DESIGNATION from VW_HR_LOC_MAPPING_ALL Where (DIVISION_CODE ='" + LocCodeSession + "' OR M_ID ='" + LocCodeSession + "') AND LOC_CODE !='"+LocCodeSession+"' AND DESIGNATION  IN ('ZM','PM','Manager','Sr. Manager','CM')";
              
                string Qry2 = @"MINUS
                            Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName,DESIGNATION 
                            from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='RSM'  AND  DIVISION_CODE IN (Select DIVISION_CODE from HR_DSM Where M_ID_MAPPING='" + LocCodeSession + "' AND DSM_ID IS NOT NULL)";
                Qry ="Select * from ("+ Qry + Qry2 +")";
            }
            else if(EmpTypeSession == "Executive")
            {
                Qry = "Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION ='ZM'";
            }
            Qry = Qry + " ORDER BY DESIGNATION ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),
                       
                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForOwnSup()
        {       
            string Qry = "Select  '' LOC_CODE,'' EmpName from Dual";
            if (EmpTypeSession == "RM")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE ='" + LocCodeSession + "' AND DESIGNATION ='RM'";
          
            }
            else if(EmpTypeSession == "ZM")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('RM','ZM')";
            }
            else if(EmpTypeSession == "PM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('RM','ZM','PM')";
            }
           
            else if(EmpTypeSession == "CM" || EmpTypeSession == "Sr. Manager")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('RM','ZM','PM','CM','Sr. Manager')";

                string Qry2 = @"MINUS
                            Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName 
                            from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='RSM'  AND  DIVISION_CODE IN (Select DIVISION_CODE from HR_DSM Where M_ID_MAPPING='" + LocCodeSession + "' AND DSM_ID IS NOT NULL)";
                Qry =  Qry + Qry2;
            }
          
            else if (EmpTypeSession == "EMA")
            {
                Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL";

            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }

        public List<DefaultBEL> GetEmpForOwnSupfrmDesignation(string Designation)
        {
            string Qry = "Select  '' LOC_CODE,'' EmpName from Dual";       
       
            if (EmpTypeSession == "TM" || EmpTypeSession == "RM")
            {
                if (Designation == "TM" || EmpTypeSession == "RM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION='"+Designation+"' ";
                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO') ";
                }
            }
            if (EmpTypeSession == "RSM" || EmpTypeSession == "ZM")
            {
               
                if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO') ";
                }
            }
            if (EmpTypeSession == "DSM" || EmpTypeSession == "PM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {
                if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "DSM" || Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "'  AND DESIGNATION IN ('DSM','PM','Manager','Sr. Manager')";
                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "'  AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
                }
            }
            if (EmpTypeSession == "Manager" || EmpTypeSession == "Sr. Manager" || EmpTypeSession == "CM" )
            {
                if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";

                }
                if (Designation == "DSM" || Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "'  AND DESIGNATION IN ('DSM','PM','Manager','Sr. Manager')";
                }
                
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "'  AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
                }
            }
            if (EmpTypeSession == "EMA")
            {
                if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "DSM" || Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('DSM', 'PM','Manager','Sr. Manager')";
                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
                }
            }          
           
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }

        public List<DefaultBEL> GetEmpCodeForOwnSupfrmDesignation(string Designation)
        {
            string Qry = "Select  '' MPO_CODE,'' EmpName from Dual";
            if (EmpTypeSession == "TM" || EmpTypeSession == "RM")
            {
                if (Designation == "TM" || Designation == "RM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO') ";
                }
            }
            if (EmpTypeSession == "RSM" || EmpTypeSession == "ZM")
            {

                if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";

                }
                if (Designation == "MPO" || Designation == "MIO")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO') ";

                }
            }
            if (EmpTypeSession == "DSM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {
                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "ZM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager') ";
                }
                if (Designation == "MIO")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "'  AND DESIGNATION IN ('MIO','SMIO')";
                }
            }
            if (EmpTypeSession == "CM" || EmpTypeSession == "Sr. Manager")
            {
                if (Designation == "RM" || Designation == "ZM" )
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "PM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||', '||MARKET_NAME EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager') ";
                }
                if (Designation == "MIO")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "'  AND DESIGNATION IN ('MIO','SMIO')";
                }
            }
            if (EmpTypeSession == "EMA")
            {
                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where   DESIGNATION='" + Designation + "' ";
                }
                if (Designation == "PM")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where   DESIGNATION IN ('PM','CM','Sr. Manager') ";
                }
                if (Designation == "MIO")
                {
                    Qry = " Select MPO_CODE,MPO_NAME||' - '||MPO_CODE||', '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where   DESIGNATION IN ('MIO','SMIO')";
                }
            }            
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["MPO_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForOwnSupfrmRegionDesignation(string RegionCode, string Designation)
        {
            string Qry = "Select  '' LOC_CODE,'' EmpName from Dual";  

            string Str = RegionCode.Replace("[", "").Replace("]", "").Replace("\"", "'");      

            if (EmpTypeSession == "RM")
            {
                if (Designation == "RM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession+"' AND DESIGNATION='"+Designation+"'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                }
                
            }
            if (EmpTypeSession == "ZM")
            {
                if (Designation == "RM"|| Designation == "ZM" )
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                }
              
                if (Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                }

               

            }
            if (EmpTypeSession == "PM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {
                if (Designation == "RM" || Designation == "ZM" || Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " AND REGION_CODE='" + RegionCode + "'";                       
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                      Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE = '" + RegionCode + "')";
                       
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                } 
                
            }
           
            if (EmpTypeSession == "CM" || EmpTypeSession == "Sr. Manager")
            {
                if (Designation == "RM" || Designation == "ZM" )
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                       Qry = Qry + " AND REGION_CODE='" + RegionCode + "'";
                        
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE = '" + RegionCode + "')";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " and REGION_CODE='" + RegionCode + "'";
                    }
                }              

            }

            if (EmpTypeSession == "EMA")
            {
                RegionCode = Str;
                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                     Qry = Qry + " AND REGION_CODE='" + RegionCode + "'";                        
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                       Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE = '" + RegionCode + "')";
                    }
                  }
                  if (Designation == "MIO")
                  {
                    Qry = " Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " ")
                    {
                        Qry = Qry + " AND REGION_CODE in ('" + RegionCode + "')";
                    }
                }
                


            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),                      
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForOwnSupfrmRegionDesignationFnIn(string RegionCode, string Designation)
        {
            string Qry = "Select  '' LOC_CODE,'' EmpName from Dual";       

            string Str = RegionCode.Replace("[", "").Replace("]", "").Replace("\"", "'");
            if (Str!=null && Str!="null")
            {
                RegionCode = Str;
            }
         
            if (EmpTypeSession == "RM")
            {
                if (Designation == "RM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION='"+Designation+"'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
               

            }
            if (EmpTypeSession == "ZM")
            {

                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }              

            }
            if (EmpTypeSession == "PM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {

                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }             
            }
            
            if (EmpTypeSession == "CM" || EmpTypeSession == "Sr. Manager")
            {
                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION IN ('PM','Manager','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }              

            }
            if (EmpTypeSession == "EMA")
            {
                if (Designation == "RM" || Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }



            }

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForExpenseBillfrmRegionDesignationFnIn(string RegionCode, string Designation)
        {
            string Qry = "Select  '' LOC_CODE,'' EmpName from Dual";          

            string Str = RegionCode.Replace("[", "").Replace("]", "").Replace("\"", "'");
            if (Str != null && Str != "null")
            {
                RegionCode = Str;
            }

            if (EmpTypeSession == "RM")
            {
                if (Designation == "RM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE ='" + LocCodeSession+"' AND DESIGNATION='"+Designation+"'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
               

            }
            if (EmpTypeSession == "ZM")
            {

                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE ='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where REGION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }

               

            }
            if (EmpTypeSession == "PM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive")
            {

                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + "  AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where DIVISION_CODE ='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }

               

            }
            if (EmpTypeSession == "EMA")
            {

                if (Designation == "RM" || Designation == "ZM" )
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + "  AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where  DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
            }
            if (EmpTypeSession == "CM" || EmpTypeSession == "Sr. Manager")
            {
                if (Designation == "RM" || Designation == "ZM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID ='" + LocCodeSession + "' AND DESIGNATION='" + Designation + "'";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In(" + RegionCode + ")";
                    }
                }
                if (Designation == "PM")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID ='" + LocCodeSession + "' AND DESIGNATION IN ('PM','CM','Sr. Manager')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + "  AND DIVISION_CODE IN (Select DISTINCT DIVISION_CODE from HR_LOC_MAPPING Where REGION_CODE In(" + RegionCode + "))";
                    }
                }
                if (Designation == "MIO")
                {
                    Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName from VW_HR_LOC_MAPPING_ALL Where M_ID ='" + LocCodeSession + "' AND DESIGNATION IN ('MIO','SMIO')";
                    if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null")
                    {
                        Qry = Qry + " and REGION_CODE In (" + RegionCode + ")";
                    }
                }


            }
           

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["EmpName"].ToString(),
                 

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForDesignationLocCodeSession(string RegionCode, string Designation)
        {

            
            
            string Qry = " Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE ||' | '|| MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION EmpName from VW_HR_LOC_MAPPING_ALL";
           // string WhereClause = GetImmediateSubordinate_WhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " Where " + WhereClause + " AND LOC_CODE IS NOT NULL";
            }
           
            if (Designation == "MIO")
            {
                Qry = Qry + " AND DESIGNATION IN ('MPO', 'SMPO','MIO','SMIO') ";               
            }
            else
            {
                if(Designation != "" && Designation != null)
                {
                    Qry = Qry + " AND DESIGNATION='" + Designation + "'";
                }
              
            }
            if (RegionCode != null && RegionCode != "" && RegionCode != " " && RegionCode != "null" && Designation != "DSM" && Designation != "PM")
            {
                Qry = Qry + " AND REGION_CODE ='" + RegionCode + "'";
            }
          


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["EmpName"].ToString(),


                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForDesignation(string Designation,string OperationMode)
        {
            string Qry = "Select  LOC_CODE,MARKET_NAME||' - '||MARKET_CODE||' | '||MPO_NAME||' - '|| MPO_CODE||', '||DESIGNATION  EmpName  From VW_HR_LOC_MAPPING_ALL";             
           
            if (Designation == "TM" || Designation == "RSM" || Designation == "RM" || Designation == "ZM")
             {
                Qry = Qry+ " Where DESIGNATION='" + Designation + "' ";
             }
            if (Designation == "DSM" || Designation == "PM")
            {
                Qry = Qry + " Where DESIGNATION IN ('PM','CM','Sr. Manager') ";
            }
            if (Designation == "MIO")
             {
               Qry = Qry+ " Where DESIGNATION IN ('MIO','SMIO') ";
             }
            string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + "  AND " + WhereClause;
            }
            Qry = Qry + " ORDER BY MARKET_NAME";



            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetRegion()
        {   //Check
            string Qry = "Select  DISTINCT REGION_CODE,REGION_NAME  From VW_HR_LOC_MAPPING_ALL Where REGION_CODE IS NOT NULL";
            string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + "  AND " + WhereClause;
            }          
            Qry = Qry + " ORDER BY REGION_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {                       
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),                       

                    }).ToList();
            return item;
        }

 

     
        public List<DefaultBEL> GetTerritoryFromRegion(string RegionCode)
        {
            string Qry = "Select  LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName From VW_HR_LOC_MAPPING_ALL Where DESIGNATION IN ('TM','RM')  ";

            if (RegionCode != "" && RegionCode != null)
            {
                Qry = Qry + "  AND REGION_CODE='" + RegionCode + "'";
                string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + "  AND " + WhereClause;
                }
            }
            else
            {
                string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + "  AND " + WhereClause;
                }
            }          
            Qry = Qry + " ORDER BY MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        TerritoryManagerID = row["LOC_CODE"].ToString(),
                        TerritoryManagerName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }

       
        public List<DefaultBEL> GetMPOFromTM(string TerritoryManagerID)
        {
            string Qry = "Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||' , '||MARKET_NAME EmpName From VW_HR_LOC_MAPPING_ALL Where DESIGNATION='TM'";
            if (TerritoryManagerID != "" && TerritoryManagerID != null)
            {
                Qry = Qry + "  AND REGION_CODE='" + TerritoryManagerID + "'";
            }
            else
            {
                string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
                if (WhereClause != "" && WhereClause != null)
                {
                    Qry = Qry + "  AND " + WhereClause;
                }
            }
            Qry = Qry + " ORDER BY MPO_NAME";



            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {                       
                        MPOName = row["EmpName"].ToString(),                       
                        MPGroup = row["LOC_CODE"].ToString(),

                    }).ToList();
            return item;
        }



        public List<DefaultBEL> GetDepot()
        {        
            string Qry = "Select distinct TRIM(DEPOT_CODE) DEPOT_CODE,TRIM(DEPOT_NAME)||', '||TRIM(DEPOT_CODE) DEPOT_NAME From VW_HR_LOC_MAPPING";

            string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + "  Where " + WhereClause;
            }

            Qry = Qry + " ORDER BY DEPOT_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        DepotCode = row["DEPOT_CODE"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),

                    }).ToList();
            return item;
        }


      
        
        public List<DefaultBEL> GetLeftEmpForDepotDesignation(string DepotCode,  string MonthYear)
        {
          
            MonthYear = (MonthYear == "" || MonthYear == null)? DateTime.Now.AddMonths(-1).ToString("MM-yyyy", CultureInfo.CurrentCulture): MonthYear ;

            string Qry = "Select DISTINCT EMP_CODE,EMP_NAME ||' - '||EMP_CODE||', '||DESIGNATION||' | '||MARKET_NAME  EmpName from VW_EXPENSE_BILL_MPO_TM_RSM Where EMP_CODE NOT IN (Select MPO_CODE FROM VW_HR_LOC_MAPPING_ALL) AND MONTH_NUMBER||'-'||YEAR='" + MonthYear + "' ";
           
            if (DepotCode != null && DepotCode != "" && DepotCode != " " && DepotCode != "null")
            {                     
              Qry = Qry + " AND DEPOT_CODE='" + DepotCode + "'";
            }                   
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode= row["EMP_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetSpecificEmpForDepotDesignation(string DepotCode, string Year, string MonthNumber)
        {        

            MonthNumber = (MonthNumber == "" || MonthNumber == null) ? DateTime.Now.AddMonths(-1).ToString("MM", CultureInfo.CurrentCulture) : MonthNumber;
            Year = (Year == "" || Year == null) ? DateTime.Now.AddMonths(-1).ToString("yyyy", CultureInfo.CurrentCulture) : Year;
                      
            string Qry = "Select DISTINCT EMP_CODE,EMP_NAME ||' - '||EMP_CODE||', '||DESIGNATION||' | '||MARKET_NAME  EmpName from VW_EXPENSE_BILL_MPO_TM_RSM Where  MONTH_NUMBER='" + MonthNumber + "' AND YEAR=" +Year + " ";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["EMP_CODE"].ToString(),
                        EmployeeName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetEmpForOwnSupfrmDepotDesignation(string DepotCode, string Designation, string SBU)
        {
            bool isTrue = false;
            string Qry = "Select  '' LOC_CODE,'' LocName from Dual";       
            if (EmpTypeSession == "EMA")
            {
                if (Designation == "" || Designation == " ")
                {
                    Qry = " Select LOC_CODE,LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from( " +
                        " Select distinct REGION_CODE LOC_CODE,REGION_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from HR_LOC_MAPPING  Where DESIGNATION='ZM'" +
                        " UNION ALL" +
                        " Select distinct TERRITORY_CODE LOC_CODE,TERRITORY_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from HR_LOC_MAPPING Where DESIGNATION='RM'" +
                       " UNION ALL" +
                       " Select distinct MARKET_CODE || MARKET_GROUP || PRODUCT_GROUP LOC_CODE,MARKET_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU from HR_LOC_MAPPING Where DESIGNATION IN ('MPO','SMPO','MIO','SMIO') ) ";

                    if (DepotCode != null && DepotCode != "" && DepotCode != " " && DepotCode != "null")
                    {
                        isTrue = true;
                        Qry = Qry + " Where DEPOT_CODE='" + DepotCode + "'";
                    }
                    if (SBU != "" && SBU != null)
                    {
                        if (isTrue == true)
                        {
                            Qry = Qry + " AND SBU='" + SBU + "' ";
                        }
                        if (isTrue == false)
                        {
                            Qry = Qry + " Where SBU='" + SBU + "' ";
                        }

                    }
                }
                else
                {

                    if (Designation == "RSM" || Designation == "TM" || Designation == "RM" || Designation == "ZM")
                    {
                        Qry = "Select distinct REGION_CODE LOC_CODE,REGION_NAME LocName  from HR_LOC_MAPPING Where DESIGNATION='"+ Designation + "'";
                    }
                   
                    if (Designation == "MPO" || Designation == "MIO")
                    {
                        Qry = "Select distinct MARKET_CODE || MARKET_GROUP || PRODUCT_GROUP LOC_CODE,MARKET_NAME LocName  from HR_LOC_MAPPING Where DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
                    }


                    if (DepotCode != null && DepotCode != "" && DepotCode != " " && DepotCode != "null")
                    {
                        isTrue = true;
                        Qry = Qry + " AND DEPOT_CODE='" + DepotCode + "'";
                    }
                    if (SBU != "" && SBU != null)
                    {
                        Qry = Qry + " AND SBU='" + SBU + "' ";
                    
                    }
                }

            }

            if (EmpTypeSession == "DM" || EmpTypeSession == "DIC" || EmpTypeSession == "ADIC" || EmpTypeSession == "Sr.DIC")
            {
                if (Designation == "" || Designation == " ")
                {
                    Qry = " Select LOC_CODE,LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from( " +
                         " Select distinct REGION_CODE LOC_CODE,REGION_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from HR_LOC_MAPPING  Where DESIGNATION='RSM'" +
                         " UNION ALL" +
                         " Select distinct TERRITORY_CODE LOC_CODE,TERRITORY_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU  from HR_LOC_MAPPING Where DESIGNATION='TM'" +
                         " UNION ALL" +
                         " Select distinct MARKET_CODE || MARKET_GROUP || PRODUCT_GROUP LOC_CODE,MARKET_NAME LocName,DEPOT_CODE,PRODUCT_GROUP,SBU from HR_LOC_MAPPING Where DESIGNATION IN ('MPO','SMPO') ) ";

                    if (DepotCode != null && DepotCode != "" && DepotCode != " " && DepotCode != "null")
                    {
                        isTrue = true;
                        Qry = Qry + " Where DEPOT_CODE='" + DepotCode + "'";
                    }
                    if (SBU != "" && SBU != null)
                    {
                        if (isTrue == true)
                        {
                            Qry = Qry + " AND SBU='" + SBU + "' ";
                        }
                        if (isTrue == false)
                        {
                            Qry = Qry + " Where SBU='" + SBU + "' ";
                        }

                    }
                }
                else
                {
                    if (Designation == "RSM" || Designation == "TM" || Designation == "ZM" || Designation == "RM")
                    {
                        Qry = "Select distinct REGION_CODE LOC_CODE,REGION_NAME LocName  from HR_LOC_MAPPING Where DESIGNATION ='"+ Designation + "'";
                    }
                  
                    if (Designation == "MPO" || Designation == "MIO")
                    {
                        Qry = "Select distinct MARKET_CODE || MARKET_GROUP || PRODUCT_GROUP LOC_CODE,MARKET_NAME LocName  from HR_LOC_MAPPING Where DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";
                    }


                    if (DepotCode != null && DepotCode != "" && DepotCode != " " && DepotCode != "null")
                    {
                        isTrue = true;
                        Qry = Qry + " AND DEPOT_CODE='" + DepotCode + "'";
                    }
                    if (SBU != "" && SBU != null)
                    {
                        Qry = Qry + " AND SBU='" + SBU + "' ";
                        
                    }
                }

            }
             
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),                       
                        LocName = row["LocName"].ToString(),

                    }).ToList();
            return item;
        }



        //For Archive
        public List<DefaultBEL> GetRegionForArchive(DefaultBEL model)
        {
            string cntMonthNumber = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            model.Year = (model.Year == "" || model.Year == null) ? DateTime.Now.Year.ToString() : model.Year;
            model.MonthNumber = (model.MonthNumber == "" || model.MonthNumber == null) ? cntMonthNumber : model.MonthNumber;

            string Qry = "Select distinct REGION_CODE,REGION_NAME From VW_ARC_HR_LOC_MAPPING Where YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' ORDER BY REGION_NAME";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {

                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),


                    }).ToList();
            return item;
        }

        public List<DefaultBEL> GetEmployeeForArchive(DefaultBEL model)
        {
            string cntMonthNumber = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            model.Year = (model.Year == "" || model.Year == null) ? DateTime.Now.Year.ToString() : model.Year;
            model.MonthNumber = (model.MonthNumber == "" || model.MonthNumber == null) ? cntMonthNumber : model.MonthNumber;

            string Qry = "Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName from VW_ARC_HR_LOC_MAPPING_ALL Where  YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "'";

            string WhereClause = GetWhereClauseDefineAsUserLocCode(LocCodeSession, EmpTypeSession);
            if (WhereClause != "" && WhereClause != null)
            {
                Qry = Qry + " AND " + WhereClause;
            }

            if (model.Designation == "MPO" || model.Designation == "SMPO" || model.Designation == "MIO" || model.Designation == "SMIO")
            {
                Qry = Qry + " AND DESIGNATION IN ('MPO','SMPO','MIO','SMIO')";

                if (model.RegionCode != null && model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null")
                {
                    Qry = Qry + " and REGION_CODE ='" + model.RegionCode + "'";
                }
            }
            if (model.Designation == "TM" || model.Designation == "RSM" || model.Designation == "RM" || model.Designation == "ZM")
            {
                Qry = Qry + " AND DESIGNATION='"+ model.Designation + "'";

                if (model.RegionCode != null && model.RegionCode != "" && model.RegionCode != " " && model.RegionCode != "null")
                {
                    Qry = Qry + " and REGION_CODE ='" + model.RegionCode + "'";
                }
            }
           
            if (model.Designation == "DSM" || model.Designation == "PM")
            {
                Qry = Qry + " AND DESIGNATION='" + model.Designation + "'";
            }
           


            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        LocCode = row["LOC_CODE"].ToString(),
                        LocName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }


        public List<DefaultBEL> GetTerritoryForArchive(DefaultBEL model)
        {
            string cntMonthNumber = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            model.Year = (model.Year == "" || model.Year == null) ? DateTime.Now.Year.ToString() : model.Year;
            model.MonthNumber = (model.MonthNumber == "" || model.MonthNumber == null) ? cntMonthNumber : model.MonthNumber;

            string Qry = "Select LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||' - '||MARKET_CODE EmpName From VW_ARC_HR_LOC_MAPPING_ALL Where DESIGNATION='TM' AND YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' ";

            if (EmpTypeSession == "EMA")
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + "  AND REGION_CODE='" + model.RegionCode + "'";
                }
            }
            if (EmpTypeSession == "Manager" || EmpTypeSession == "Sr. Manager" || EmpTypeSession == "CM")
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + "  AND REGION_CODE='" + model.RegionCode + "'";
                }               
            }
            if (EmpTypeSession == "DSM" || EmpTypeSession == "Executive" || EmpTypeSession == "Sr. Executive" || EmpTypeSession == "PM")
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + "  AND REGION_CODE='" + model.RegionCode + "'";
                }               
            }
            if (EmpTypeSession == "ZM")
            {
                if (model.RegionCode != "" && model.RegionCode != null)
                {
                    Qry = Qry + "  AND REGION_CODE='" + model.RegionCode + "'";
                }               
            }
            Qry = Qry + " ORDER BY MPO_NAME";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        TerritoryManagerID = row["LOC_CODE"].ToString(),
                        TerritoryManagerName = row["EmpName"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetMpoForArchive(DefaultBEL model)
        {
            string cntMonthNumber = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            model.Year = (model.Year == "" || model.Year == null) ? DateTime.Now.Year.ToString() : model.Year;
            model.MonthNumber = (model.MonthNumber == "" || model.MonthNumber == null) ? cntMonthNumber : model.MonthNumber;

            string Qry = "SELECT distinct LOC_CODE,MPO_NAME||' - '||MPO_CODE||', '||DESIGNATION||' | '||MARKET_NAME||'-'||MARKET_CODE EmpName  from VW_ARC_HR_LOC_MAPPING_ALL Where YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' ";
            if (EmpTypeSession == "EMA")
            {
                if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
                {
                    Qry = Qry + " AND TERRITORY_CODE ='" + model.TerritoryManagerID + "'";
                }
            }
            else
            {
                if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
                {
                    Qry = Qry + " AND TERRITORY_CODE ='" + model.TerritoryManagerID + "'";
                }
                
            }
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;

            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        MPGroup = row["LOC_CODE"].ToString(),
                        MPOName = row["EmpName"].ToString(),


                    }).ToList();
            return item;
        }








        public List<DefaultBEL> GetDivision(DefaultBEL model)
        {
            string Qry = "Select Distinct DIVISION_CODE, DIVISION_NAME from VW_HR_LOC_MAPPING";

            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        DivisionCode = row["DIVISION_CODE"].ToString(),
                        DivisionName = row["DIVISION_NAME"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetDepot(DefaultBEL model)
        {
            string Qry = "Select Distinct  DEPOT_CODE, DEPOT_NAME from VW_HR_LOC_MAPPING Where DIVISION_CODE='" + model.DivisionCode + "' ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        DepotCode = row["DEPOT_CODE"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetRegion(DefaultBEL model)
        {
            string Qry = "Select Distinct REGION_CODE, REGION_NAME from VW_HR_LOC_MAPPING Where DEPOT_CODE='" + model.DepotCode + "' ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),

                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetTerritory(DefaultBEL model)
        {
            string Qry = "Select Distinct TERRITORY_CODE,TERRITORY_NAME from VW_HR_LOC_MAPPING Where REGION_CODE='" + model.RegionCode + "' ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        TerritoryCode = row["TERRITORY_CODE"].ToString(),
                        TerritoryName = row["TERRITORY_NAME"].ToString(),
                    }).ToList();
            return item;
        }
        public List<DefaultBEL> GetMarket(DefaultBEL model)
        {
            string Qry = "Select Distinct MARKET_CODE, MARKET_NAME from VW_HR_LOC_MAPPING Where TERRITORY_CODE='" + model.TerritoryCode+"' ";
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<DefaultBEL> item;
            item = (from DataRow row in dt.Rows
                    select new DefaultBEL
                    {
                        MarketCode = row["MARKET_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                    }).ToList();
            return item;
        }

    }
}
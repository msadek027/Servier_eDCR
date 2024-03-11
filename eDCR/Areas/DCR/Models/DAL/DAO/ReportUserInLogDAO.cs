using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.SA.Models.BEL;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

using System.Web;
using Systems.Universal;

namespace eDCR.Areas.DCR.Models.DAL
{
    public class ReportUserInLogDAO
    {
        DBConnection dbConn = new DBConnection();
        Encryption encryption = new Encryption();
        // SaHelper saHelper = new SaHelper();
        DBHelper saHelper = new DBHelper();
        string CntDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.CurrentCulture);
        string CntTime = DateTime.Now.ToString("hh:mm", CultureInfo.CurrentCulture);

      
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
    
        string LocCodeSession = HttpContext.Current.Session["LocCodeForQry"].ToString();
        public List<UserInRoleAllRelationalData> GetUser()
        {
            string Qry = @" Select a.EMPID, a.EMPNAME, a.DESIGNATION, a.MP_GROUP,a.LOC_CODE,a.LOC_NAME, b.USERID, c.NEWPASSWORD 
                          from SA_EMPLOYEE_VW a,SA_USERINROLE b,SA_USERCREDENTIAL c,Sa_Role d
                          Where a.EMPID=b.EMPID and b.USERID=C.USERID and b.ROLEID=D.ROLEID and b.ROLEID<=50";


            if (EmpTypeSession == "EMA")
            {

                Qry = Qry + " And  b.ROLEID>='" + HttpContext.Current.Session["RoleID"].ToString() + "'";
            }
            else
            {
                Qry = Qry + " And a.MP_GROUP in (" + LocCodeSession + ") and b.ROLEID>='" + HttpContext.Current.Session["RoleID"].ToString() + "'";

               

                if (HttpContext.Current.Session["EmpType"].ToString() == "RSM")
                {
                    Qry = Qry + " UNION ALL " + " Select a.EMPID, a.EMPNAME, a.DESIGNATION, a.MP_GROUP,a.LOC_CODE,a.LOC_NAME, b.USERID, c.NEWPASSWORD " +
                                 " from SA_EMPLOYEE_VW a,SA_USERINROLE b,SA_USERCREDENTIAL c,Sa_Role d" +
                                 " Where a.EMPID=b.EMPID and b.USERID=C.USERID and b.ROLEID=D.ROLEID And a.LOC_CODE in (" + LocCodeSession + ") ";

                }
            }
         
            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleAllRelationalData> item;

            item = (from DataRow row in dt.Rows
                    select new UserInRoleAllRelationalData
                    {
                        UserID = encryption.Decrypt(row["USERID"].ToString()),
                        EmpID = row["EMPID"].ToString(),
                        EmpName = row["EMPNAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),                       
                        Password = encryption.Decrypt(row["NEWPASSWORD"].ToString()),
                        MPGroup = row["MP_GROUP"].ToString(),
                        MarketCode = row["LOC_CODE"].ToString(),
                        MarketName = row["LOC_NAME"].ToString(),
               

                    }).ToList();
            return item;
        }

        public List<UserInRoleAllRelationalData> GetUserInfo(DefaultParameterBEO model)
        {
           
        
            string Qry = "Select Distinct LOC_CODE,MARKET_CODE,MARKET_NAME,MPO_CODE,MPO_NAME,DESIGNATION,PHONE_NO,USERID,NEWPASSWORD,PRODUCT_GROUP,REGION_CODE,REGION_NAME,DEPOT_CODE,DEPOT_NAME,DIVISION_CODE,DIVISION_NAME From SA_USERCREDENTIAL_VW Where 1=1 ";
            if (EmpTypeSession == "EMA")
            {
                if (HttpContext.Current.Session["RoleID"].ToString() == "01")
                {
                    //Qry = Qry + " And  ROLEID>='" + HttpContext.Current.Session["RoleID"].ToString() + "'";
                    Qry = Qry + " And  ROLEID>='15'";
                }
                else
                {
                    if (model.RegionCode != null && model.RegionCode != "")
                    {
                        Qry += " And REGION_CODE = '" + model.RegionCode + "'";
                    }

                    if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
                    {
                        Qry += " And TERRITORY_CODE  = '" + model.TerritoryManagerID + "'";
                    }
                    Qry = Qry + " And  ROLEID>='17'";
                }
            }
            else
            {
                if (model.RegionCode !=null && model.RegionCode != "")
                {
                    Qry += " And REGION_CODE = '" + model.RegionCode + "'";
                }

                if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
                {
                    Qry += " And TERRITORY_CODE  = '" + model.TerritoryManagerID + "'";
                }
            }
        
            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleAllRelationalData> item;

            item = (from DataRow row in dt.Rows
                    select new UserInRoleAllRelationalData
                    {
                        UserID = encryption.Decrypt(row["USERID"].ToString()),
                        EmpID = row["MPO_CODE"].ToString(),
                        EmpName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        PhoneNo = row["PHONE_NO"].ToString(),
                        Password = encryption.Decrypt(row["NEWPASSWORD"].ToString()),
                        MPGroup = row["LOC_CODE"].ToString(),
                        MarketCode = row["MARKET_CODE"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        SBU = row["PRODUCT_GROUP"].ToString(),


                        RegionCode = row["REGION_CODE"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        DepotCode = row["DEPOT_CODE"].ToString(),
                        DepotName = row["DEPOT_NAME"].ToString(),
                        DivisionCode = row["DIVISION_CODE"].ToString(),
                        DivisionName = row["DIVISION_NAME"].ToString(),

                    }).ToList();
            return item;
        }

        public List<UserInLoginLog> GetUserInLoginLog(DefaultParameterBEO model)
        {
            string Qry = " Select MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,REGION_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,TO_CHAR(SET_DATE,' HH:MI:SS AM') SET_DATE_TIME,DECODE(TERMINAL,'Android','A','W') TERMINAL" +
                " from SA_USERCREDENTIALLOG_VW Where SET_DATE BETWEEN TO_DATE('" + model.FromDate + "','dd-mm-yyyy') AND TO_DATE('" + model.ToDate + "','dd-mm-yyyy')";

            if (model.RegionCode != null && model.RegionCode != "")
            {
                Qry += " And REGION_CODE = '" + model.RegionCode + "'";
            }
            //if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            //{
            //    Qry += " And TERRITORY_CODE IN ( Select distinct TERRITORY_CODE from VW_HR_LOC_MAPPING Where TSM_ID= '" + model.TerritoryManagerID + "')";
            //}
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                Qry += " And TERRITORY_CODE  = '" + model.TerritoryManagerID + "'";
            }
            if (model.MPGroup != null && model.MPGroup != "")
            {
                Qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }

            Qry += " ORDER BY SET_DATE ASC";
            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInLoginLog> item;

            item = (from DataRow row in dt.Rows
                    select new UserInLoginLog
                    {
                       
                        EmpID = row["MPO_CODE"].ToString(),
                        EmpName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),      
                        MarketName = row["MARKET_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        LoginDate = row["SET_DATE"].ToString(),
                        LoginTime = row["SET_DATE_TIME"].ToString(),
                        AppWeb = row["TERMINAL"].ToString(),
                    }).ToList();
            return item;
        }

        public Tuple<string, DataTable, List<UserInLoginLog>> Export(DefaultParameterBEO model)
        {
            string vHeader = "";
            string Qry = " Select MPO_CODE,MPO_NAME,DESIGNATION,MARKET_NAME,REGION_NAME,TO_CHAR(SET_DATE,'dd-mm-yyyy') SET_DATE,TO_CHAR(SET_DATE,' HH:MI:SS AM') SET_DATE_TIME,DECODE(TERMINAL,'Android','A','W') TERMINAL from SA_USERCREDENTIALLOG_VW Where SET_DATE BETWEEN TO_DATE('" + model.FromDate + "','dd-mm-yyyy') AND TO_DATE('" + model.ToDate + "','dd-mm-yyyy')";

            vHeader = vHeader + "Date Between: " + model.FromDate + " To " + model.ToDate;

            if (model.MPGroup != null && model.MPGroup != "")
            {
                vHeader = vHeader + ", MPO : " + model.MPOName;
                Qry += " AND LOC_CODE='" + model.MPGroup + "'";
            }
            if (model.TerritoryManagerID != "" && model.TerritoryManagerID != null)
            {
                if (model.TerritoryManagerName != null && model.TerritoryManagerName != "")
                {
                    string[] Territory = model.TerritoryManagerName.Split(',');
                    string lastItem = Territory.Length > 1 ? Territory[Territory.Length - 1] : Territory[0];
                    vHeader = vHeader + ", Territory : " + lastItem;
                }
               // Qry += " And  TERRITORY_CODE IN ( Select distinct TERRITORY_CODE from VW_HR_LOC_MAPPING Where TSM_ID= '" + model.TerritoryManagerID + "')";
                Qry += " And TERRITORY_CODE  = '" + model.TerritoryManagerID + "'";
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
           
            Qry += " ORDER BY SET_DATE ASC";
            DataTable dt = saHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<UserInLoginLog> item;

            item = (from DataRow row in dt.Rows
                    select new UserInLoginLog
                    {
                     
                        EmpID = row["MPO_CODE"].ToString(),
                        EmpName = row["MPO_NAME"].ToString(),
                        Designation = row["DESIGNATION"].ToString(),
                        MarketName = row["MARKET_NAME"].ToString(),
                        RegionName = row["REGION_NAME"].ToString(),
                        LoginDate = row["SET_DATE"].ToString(),
                        LoginTime = row["SET_DATE_TIME"].ToString(),
                        AppWeb = row["TERMINAL"].ToString(),
                    }).ToList();


            return Tuple.Create(vHeader, dt, item);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.Areas.SA.Models.BEL;
using eDCR.DAL.Gateway;
using Systems.Universal;
using eDCR.Universal.Common;
using System.Data.SqlClient;
using eDCR.DAL.DAO;

namespace eDCR.Areas.SA.Models.DAL.DAO
{
    public class UserInRoleDAO : ReturnData
    {
        DBConnection  dbConn=new DBConnection();
        Encryption encryption = new Encryption();
        // SaHelper saHelper = new SaHelper();
        DBHelper saHelper = new DBHelper();
        LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();
        string EmpTypeSession = HttpContext.Current.Session["EmpType"].ToString();
        public bool SaveUpdate(UserInRoleBEL master)
        {
            try
            {

                string tt = encryption.Encrypt(master.UserID);
                string conn = dbConn.SAConnStrReader();
                string Qry = "Select MAX(UserID) ID from Sa_UserInRole";
                var tuple = saHelper.ProcedureExecuteTFn5(dbConn.SAConnStrReader(), Qry, "Sa_UserInRole_SSP", "p_RoleID", "p_UserID", "p_EmpID", "p_NewPassword", "p_IsActive", master.RoleID, encryption.Encrypt(master.UserID), master.EmpID, encryption.Encrypt(master.Password), master.IsActive.ToString());
                             
                if (tuple.Item1)
                {
                    MaxID = tuple.Item2;
                    IUMode = tuple.Item3;
                    if (master.BuyerID != "" && master.BuyerID != null && master.BuyerID!="null")
                    {
                     EmpBuyerMapping(master);
                    }
                    loginRegistrationDAO.UserLoginLOG(master.EmpID, encryption.Encrypt(master.UserID), encryption.Encrypt(master.Password), master.RoleID, "U");
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception errorException)
            {
                throw errorException;
            }
        }
        public bool Delete(UserInRoleBEL master)
        {
            string Qry = "";
            var tuple = saHelper.ProcedureExecuteFn1(dbConn.SAConnStrReader(), Qry, "Sa_EmpUser_Del_SP", "pEmpID", master.EmpID);
           
            if (tuple)
            {
                MaxID ="";
                IUMode = "D";
                return true;
            }
            else
            {
                return false;
            }
           
        }
        private Boolean EmpBuyerMapping(UserInRoleBEL master)
        {
            bool isTrue = false;
            string QryDelete = "Delete from SA_EMP_BUYER_MAPPING Where Emp_ID='" + master.EmpID + "'";
            if (saHelper.CmdExecute(dbConn.SAConnStrReader(), QryDelete))
            {                   
            string Str = master.BuyerID.Replace("[", "").Replace("]", "").Replace("\"", "");
            String[] SubStr = Str.Split(',');
            for (int i = 0; i < SubStr.Length; i ++)
            {
                string Qry = "Insert Into SA_EMP_BUYER_MAPPING(Buyer_ID,Emp_ID) Values ('" + SubStr[i].ToString() + "','" + master.EmpID + "')";          
                if(saHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                {
                 isTrue = true;
                }
            }
            }
            else
            {
                string Str = master.BuyerID.Replace("[", "").Replace("]", "").Replace("\"", "");
                String[] SubStr = Str.Split(',');
                for (int i = 0; i < SubStr.Length; i++)
                {
                    string Qry = "Insert Into SA_EMP_BUYER_MAPPING(Buyer_ID,Emp_ID) Values ('" + SubStr[i].ToString() + "','" + master.EmpID + "')";
                    if (saHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
                    {
                        isTrue = true;
                    }
                }
            }
            return isTrue;          
        }

        public List<UserInRoleBEL> GetUserInRoleList()
        {        
                string Qry1 = "SELECT ur.UserID,ur.RoleID,r.RoleName,ur.EmpID,E.MPO_NAME,e.LOC_CODE,e.DESIGNATION,u.NewPassword,u.OldPassword,ur.IsActive FROM Sa_UserInRole ur, Sa_UserCredential u,Sa_Role r,VW_HR_LOC_MAPPING_ALL e Where ur.UserID=u.UserID and ur.RoleID=r.RoleID and ur.EmpID=E.MPO_CODE and ur.EmpID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";

                string Qry = "SELECT ur.UserID,ur.RoleID,r.RoleName,ur.EmpID,E.MPO_NAME,e.LOC_CODE,e.DESIGNATION,u.NewPassword,u.OldPassword,ur.IsActive FROM Sa_UserInRole ur, Sa_UserCredential u,Sa_Role r,VW_HR_LOC_MAPPING_ALL e Where ur.UserID=u.UserID and ur.RoleID=r.RoleID and ur.EmpID=E.MPO_CODE and ur.RoleID>='" + HttpContext.Current.Session["RoleID"].ToString() + "'";

                if(EmpTypeSession == "DSM")
                {
                    Qry = Qry + " AND e.DIVISION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
               else if (EmpTypeSession == "RSM")
                {
                    Qry = Qry + " AND e.REGION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                else if (EmpTypeSession == "TM")
                {
                    Qry = Qry + " AND e.TERRITORY_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                else if (EmpTypeSession == "Manager")
                {
                    Qry = Qry + " AND e.M_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";
                }
                else if (EmpTypeSession == "Executive")
                {
                    Qry = Qry + " AND e.M_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";
                }
                Qry = Qry + " Order by ur.RoleID,ur.EmpID";
                DataTable dt1 = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry1);
                DataTable dt2 = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
                DataTable dt = new DataTable();
                dt.Merge(dt1);
                dt.Merge(dt2);

                var item = dt?.AsEnumerable().Select(row => new UserInRoleBEL
                {
                    RoleID = row["RoleID"].ToString(),
                    RoleName = row["RoleName"].ToString(),
                    UserID = encryption.Decrypt(row["UserID"].ToString()),
                    EmpID = row["EmpID"].ToString(),
                    EmpName = row["MPO_NAME"].ToString(),
                    MPGroup = row["LOC_CODE"].ToString(),
                    Designation = row["DESIGNATION"].ToString(),
                    Password = encryption.Decrypt(row["NewPassword"].ToString()),
                    IsActive = Convert.ToBoolean(row["IsActive"].ToString())
                }).ToList();
                return item;
          //  }




          
        }

        public List<UserInRoleBEL> GetEmployeeList()
        {
            string Qry = "Select EmpID,EmpName From Sa_Employee ";//-- where Upper(STATUS)=Upper('true')
            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleBEL> item;

            item = (from DataRow row in dt.Rows
                    select new UserInRoleBEL
                    {
                        EmpID = row["EmpID"].ToString(),
                        EmpName = row["EmpName"].ToString()
                       

                    }).ToList();
            return item;
        }
        
        public  List<UserInRoleBEL> GetEmployeeNotYetAssignedList()
        {

            string Qry = @" Select A.MPO_CODE,A.MPO_NAME,A.DESIGNATION,B.ROLEID,B.USERID,CASE WHEN B.USERID IS NULL THEN 'true' ELSE 'false' END YET_ASSIGNED
                                 from VW_HR_LOC_MAPPING_ALL A LEFT JOIN SA_USERINROLE B ON A.MPO_CODE = B.EMPID ";


            if (Convert.ToInt16(HttpContext.Current.Session["RoleID"].ToString()) > 17)
            {
                if (EmpTypeSession == "DSM")
                {
                    Qry = Qry + "AND A.DIVISION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                else if (EmpTypeSession == "RSM")
                {
                    Qry = Qry + "AND A.REGION_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                else if (EmpTypeSession == "TM")
                {
                    Qry = Qry + "AND A.TERRITORY_CODE='" + HttpContext.Current.Session["LocCode"].ToString() + "'";
                }
                else if (EmpTypeSession == "Manager")
                {
                    Qry = Qry + "AND A.M_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";
                }
                else if (EmpTypeSession == "Executive")
                {
                    Qry = Qry + "AND A.M_ID='" + HttpContext.Current.Session["EmpID"].ToString() + "'";
                }

            }     

            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleBEL> item;
            item = (from DataRow row in dt.Rows
                    select new UserInRoleBEL
                    {
                        EmpID = row["MPO_CODE"].ToString(),
                        EmpName = row["MPO_NAME"].ToString()+" | " + row["MPO_CODE"].ToString(),
                        DataOwner = row["DESIGNATION"].ToString(),
                        YetAssigned = row["YET_ASSIGNED"].ToString()

                    }).ToList();
            return item;
        }

        public List<UserInRoleBEL> GetBuyerList()
        {


            string Qry =" Select a.EmpID,a.EmpName||' | '||a.EmpID||' | '||a.DESIGNATION EmpName,a.DESIGNATION,a.YetAssigned from " +
                        " (Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.DESIGNATION='TM' and b.EmpID Not in (Select EmpID from Sa_UserInRole Where Upper(isActive)=Upper('true') and EmpID is not null) " +
                        " Union all Select a.EmpID,b.EmpName,b.DESIGNATION, 'true' YetAssigned From Sa_UserInRole a,SA_EMPLOYEE_VW b Where b.DESIGNATION='TM'  and Upper(a.isActive)=Upper('true') and a.EMPID=b.EMPID ) a Order by  CASE WHEN upper(a.YetAssigned) = upper('false') THEN 1 ELSE 2 END,CASE WHEN a.DESIGNATION='EMA' THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName,a.DESIGNATION ";

            if (Convert.ToInt16(HttpContext.Current.Session["RoleID"].ToString()) > 15)
            {
                Qry = "Select '' EmpID,'' EmpName,'' DESIGNATION from Dual";
            }

            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleBEL> item;

            item = (from DataRow row in dt.Rows
                    select new UserInRoleBEL
                    {
                        BuyerID = row["EmpID"].ToString(),
                        BuyerName = row["EmpName"].ToString(),
                        DataOwner = row["DESIGNATION"].ToString(),


                    }).ToList();
            return item;
        }
       public List<UserInRoleBEL> GetUserList()
        {
            string Qry = "Select UserID,EmpID,GetName(EmpID,'EM') EmpName From Sa_UserInRole Where Upper(IsActive)=Upper('true')";
            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInRoleBEL> item;

            item = (from DataRow row in dt.Rows
                    select new UserInRoleBEL
                    {
                        UserID = encryption.Decrypt(row["UserID"].ToString()),
                        EmpName = row["EmpName"].ToString()


                    }).ToList();
            return item;
        }

       public List<UserInRoleBEL> GetBuyerYetAssignedList(string EmpID)
       {
           //string Qry = " select a.EmpID,a.EmpName,a.YetAssigned from (  Select b.EmpID,b.EmpName ,b.DESIGNATION,'true' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW where EmpID='" + EmpID + "') b  Union all " +
           //            "  Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.EmpID Not in (Select EmpID from Sa_UserInRole Where  EmpID='" + EmpID + "') ) a " +
           //            " Order by  CASE WHEN Upper(a.YetAssigned) = upper('true') THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName ";


           string Qry = " select a.EMPID,a.EMPNAME,a.YetAssigned from (Select a.BUYER_ID EMPID,b.EMPNAME,'true' YetAssigned  From SA_EMP_BUYER_MAPPING a,SA_EMPLOYEE_VW b Where A.BUYER_ID=B.EMPID and b.DESIGNATION='TM' and A.Emp_ID='" + EmpID + "' Union all " +
                        " Select EMPID,EMPNAME ,'false' YetAssigned From SA_EMPLOYEE_VW where DESIGNATION='TM' and  EMPID Not In (Select distinct BUYER_ID  from SA_EMP_BUYER_MAPPING where Emp_ID='" + EmpID + "')) a " +
                        " Order by  CASE WHEN Upper(a.YetAssigned) = upper('true') THEN 1 ELSE 2 END, a.YetAssigned,a.EMPNAME ";
    
           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleBEL
                   {
                       BuyerID = row["EmpID"].ToString(),
                       BuyerName = row["EmpName"].ToString(),
                       YetAssigned = row["YetAssigned"].ToString()

                   }).ToList();
           return item;
       }
















       public List<UserInRoleBEL> GetEmployeeAll()
       {
           string Qry = " Select a.EmpID,a.EmpName||' | '||a.EmpID||' | '||a.DESIGNATION EmpName,a.DESIGNATION,a.YetAssigned from " +
                      " (Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.EmpID Not in (Select EmpID from Sa_UserInRole Where Upper(isActive)=Upper('true')  and EMPID is not null) " +
                      " Union all Select a.EmpID,b.EmpName,b.DESIGNATION, 'true' YetAssigned From Sa_UserInRole a,SA_EMPLOYEE_VW b Where Upper(a.isActive)=Upper('true') and a.EMPID=b.EMPID ) a Order by  CASE WHEN upper(a.YetAssigned) = upper('false') THEN 1 ELSE 2 END,CASE WHEN a.DESIGNATION='EMA' THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName ";
          
      

           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleBEL
                   {
                       EmpID = row["EmpID"].ToString(),
                       EmpName = row["EmpName"].ToString(),
                       DataOwner = row["DESIGNATION"].ToString(),
                       YetAssigned = row["YetAssigned"].ToString()

                   }).ToList();
           return item;
       }
       public List<UserInRoleBEL> GetUserType()
       {
           string Qry = " Select  a.EmpID,a.EmpName||' | '||a.EmpID||' | '||a.DESIGNATION EmpName,a.DESIGNATION,a.YetAssigned from " +
                  " (Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.EmpID Not in (Select EmpID from Sa_UserInRole Where Upper(isActive)=Upper('true')  and EMPID is not null) " +
                  " Union all Select a.EmpID,b.EmpName,b.DESIGNATION, 'true' YetAssigned From Sa_UserInRole a,SA_EMPLOYEE_VW b Where Upper(a.isActive)=Upper('true') and a.EMPID=b.EMPID ) a Order by  CASE WHEN upper(a.YetAssigned) = upper('false') THEN 1 ELSE 2 END,CASE WHEN a.DESIGNATION='EMA' THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName ";



           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleBEL
                   {
                       EmpID = row["EmpID"].ToString(),
                       EmpName = row["EmpName"].ToString(),
                       DataOwner = row["DESIGNATION"].ToString(),
                       YetAssigned = row["YetAssigned"].ToString()

                   }).ToList();
           return item;
       }

       public List<UserInRoleBEL> GetDistinctUserType()
       {
           string Qry = " Select distinct p.DESIGNATION from (Select  a.EmpID,a.EmpName||' | '||a.EmpID||' | '||a.DESIGNATION EmpName,a.DESIGNATION,a.YetAssigned from " +
                  " (Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.EmpID Not in (Select EmpID from Sa_UserInRole Where Upper(isActive)=Upper('true')  and EMPID is not null) " +
                  " Union all Select a.EmpID,b.EmpName,b.DESIGNATION, 'true' YetAssigned From Sa_UserInRole a,SA_EMPLOYEE_VW b Where Upper(a.isActive)=Upper('true') and a.EMPID=b.EMPID ) a Order by  CASE WHEN upper(a.YetAssigned) = upper('false') THEN 1 ELSE 2 END,CASE WHEN a.DESIGNATION='EMA' THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName) p ";



           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleBEL
                   {
                       //EmpID = row["EmpID"].ToString(),
                       //EmpName = row["EmpName"].ToString(),
                       DataOwner = row["DESIGNATION"].ToString(),
                       //YetAssigned = row["YetAssigned"].ToString()

                   }).ToList();
           return item;
       }
       public List<RoleBEL> GetRoleAll()
       {
           string Qry = "Select RoleID,RoleName,IsActive From Sa_Role Where RoleID >='01'";
           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<RoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new RoleBEL
                   {
                       RoleID = row["RoleID"].ToString(),
                       RoleName = row["RoleName"].ToString(),
                       IsActive = Convert.ToBoolean(row["IsActive"].ToString())

                   }).ToList();
           return item;
       }

       public List<UserInRoleAllRelationalData> GetAllRelationalData()
       {
           string Qry = @" Select MPO_CODE, MPO_NAME, MPO_PASS, MPO_DESIGNATION, MPO_MOBILE_NO, MARKET_CODE, MARKET_NAME,  MP_GROUP,  TERRITORY_CODE, TERRITORY_NAME, TSM_ID, TSM_NAME, TSM_PASS, TSM_DESIGNATION, TSM_PHONE, REGION_CODE, RSM_ID, RSM_NAME, RSM_PASS, RSM_DESIGNATION, RSM_PHONE, REGION_NAME, DEPOT_CODE, DEPOT_NAME, DM_ID, DIVISION_CODE, DIVISION_NAME, DSM_ID, DSM_NAME, DSM_PASS, DSM_DESIGNATION, M_ID, M_NAME, M_PASS, M_DESIGNATION, COMPANY_CODE, COMPANY_NAME 
                          from SA_ROLE_CREDENTIAL_ALL_VW";
           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleAllRelationalData> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleAllRelationalData
                   {
                       DivisionCode = row["DIVISION_CODE"].ToString(),
                       DivisionName = row["DIVISION_NAME"].ToString(),               
                       RegionCode = row["REGION_CODE"].ToString(),
                       RegionName = row["REGION_NAME"].ToString(),
                       TerritoryCode = row["TERRITORY_CODE"].ToString(),
                       TerritoryName = row["TERRITORY_NAME"].ToString(),
                       MarketCode = row["MARKET_CODE"].ToString(),
                       MarketName = row["MARKET_NAME"].ToString(),

                       DsmId = row["DSM_ID"].ToString(),
                       DsmName = row["DSM_NAME"].ToString(),
                       DsmPassword = encryption.Decrypt(row["DSM_PASS"].ToString()),

                       RsmId = row["RSM_ID"].ToString(),
                       RsmName = row["RSM_NAME"].ToString(),
                       RsmPassword = encryption.Decrypt(row["RSM_PASS"].ToString()),
                       TmId = row["TSM_ID"].ToString(),
                       TmName = row["TSM_NAME"].ToString(),
                       TmPassword = encryption.Decrypt(row["TSM_PASS"].ToString()),
                       MpoCode = row["MPO_Code"].ToString(),
                       MpoName = row["MPO_Name"].ToString(),
                      
                       MpoPassword = encryption.Decrypt(row["MPO_PASS"].ToString()),
                       MPGroup = row["MP_GROUP"].ToString(),                          

                   }).ToList();
           return item;
       }

      public List<UserInRoleBEL> GetUserInRoleAll(string UserType)
       {
           string Qry = " Select a.EmpID,a.EmpName||' | '||a.EmpID||' | '||a.DESIGNATION EmpName,a.DESIGNATION,a.YetAssigned from " +
                        " (Select b.EmpID,b.EmpName ,b.DESIGNATION,'false' YetAssigned from(Select EmpID,EmpName,DESIGNATION From SA_EMPLOYEE_VW) b where b.EmpID Not in (Select EmpID from Sa_UserInRole Where Upper(isActive)=Upper('true')  and EmpID is not null) " +
                        " Union all Select a.EmpID,b.EmpName,b.DESIGNATION, 'true' YetAssigned From Sa_UserInRole a,SA_EMPLOYEE_VW b Where Upper(a.isActive)=Upper('true') and a.EMPID=b.EMPID ) a  ";

      
           if (UserType != "" && UserType != null)
           {
               Qry = Qry + " Where a.DESIGNATION='" + UserType + "' and upper(a.YetAssigned) = upper('false')";
           }

           Qry = Qry + " Order by  CASE WHEN upper(a.YetAssigned) = upper('false') THEN 1 ELSE 2 END,CASE WHEN a.DESIGNATION='EMA' THEN 1 ELSE 2 END, a.YetAssigned,a.EmpName";
           DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
           List<UserInRoleBEL> item;

           item = (from DataRow row in dt.Rows
                   select new UserInRoleBEL
                   {
                       EmpID = row["EmpID"].ToString(),
                       EmpName = row["EmpName"].ToString(),
                       DataOwner = row["DESIGNATION"].ToString(),
                       YetAssigned = row["YetAssigned"].ToString()

                   }).ToList();
           return item;
       }

      public bool SaveUpdateAll(UserInRoleBELDetail master)
      {
          try
          {
              bool isTrue=false;
              string conn = dbConn.SAConnStrReader();
              foreach (UserInRoleBEL Detail in master.ListAll)
              {
                  string Qry = "Select MAX(UserID) ID from Sa_UserInRole";
                  var tuple = saHelper.ProcedureExecuteTFn5(dbConn.SAConnStrReader(), Qry, "Sa_UserInRole_SSP", "p_RoleID", "p_UserID", "p_EmpID", "p_NewPassword", "p_IsActive", Detail.RoleID, encryption.Encrypt(Detail.UserID.Trim()), Detail.EmpID, encryption.Encrypt(Detail.Password.Trim()), Detail.IsActive.ToString());

                  if (tuple.Item1)
                  {
                      MaxID = tuple.Item2;
                      IUMode = tuple.Item3;
                      //if (master.BuyerID != "" && master.BuyerID != null && master.BuyerID != "null")
                      //{
                      //    EmpBuyerMapping(master);
                      //}

                      isTrue= true;
                  }
                  else
                  {
                      isTrue= false;
                  }
              }
              return isTrue;
          
          }
          catch (Exception errorException)
          {
              throw errorException;
          }
      }














      public List<UserInRoleBEL> UIDPWD()
      {
          string Qry = "SELECT ur.UserID,ur.RoleID,r.RoleName,ur.EmpID,E.EMPNAME,e.MP_GROUP,e.DESIGNATION,u.NewPassword,u.OldPassword,ur.IsActive FROM Sa_UserInRole ur, Sa_UserCredential u,Sa_Role r,SA_EMPLOYEE_VW e Where ur.UserID=u.UserID and ur.RoleID=r.RoleID and ur.EmpID=E.EMPID and ur.RoleID>='10'";
          Qry = Qry + " Order by ur.RoleID,ur.EmpID";
          DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
          List<UserInRoleBEL> item;

          item = (from DataRow row in dt.Rows
                  select new UserInRoleBEL
                  {
                      RoleID = row["RoleID"].ToString(),
                      RoleName = row["RoleName"].ToString(),
                      UserID = encryption.Decrypt(row["UserID"].ToString()),
                      EmpID = row["EmpID"].ToString(),
                      EmpName = row["EmpName"].ToString(),
                      MPGroup = row["MP_GROUP"].ToString(),
                      Designation = row["DESIGNATION"].ToString(),
                      Password = encryption.Decrypt(row["NewPassword"].ToString()),
                      IsActive = Convert.ToBoolean(row["IsActive"].ToString())

                  }).ToList();
          return item;
      }
      public void PostData()
      {
          var data = UIDPWD();
          DataTable dt = DataObjectToDataTable.CreateDataTable(data);
          string Qry = "";
          if(dt.Rows.Count>0)
          {
              for (int i = 0; i < dt.Rows.Count;i++ )
              {
                  string QryIsExists = "Select USERID from SA_UID_PWD Where USERID='" + dt.Rows[i]["UserID"] + "' ";
                  var tuple1 = saHelper.IsExistsWithGetSL(dbConn.SAConnStrReader(), QryIsExists);
                if (tuple1.Item1)
                {
                     Qry = "Update SA_UID_PWD set NEWPASSWORD='" + dt.Rows[i]["Password"] + "', ROLENAME='" + dt.Rows[i]["RoleName"] + "',EMPNAME='" + dt.Rows[i]["EmpName"] + "',MP_Group='" + dt.Rows[i]["MPGroup"] + "', Designation= '" + dt.Rows[i]["Designation"] + "' Where USERID='" + dt.Rows[i]["UserID"] + "'";
                }
                else
                {
                    Qry = "Insert into SA_UID_PWD (USERID, NEWPASSWORD, ROLENAME, EMPNAME,MP_Group,Designation) Values ('" + dt.Rows[i]["UserID"] + "','" + dt.Rows[i]["Password"] + "','" + dt.Rows[i]["RoleName"] + "','" + dt.Rows[i]["EmpName"] + "','" + dt.Rows[i]["MPGroup"] + "','" + dt.Rows[i]["Designation"] + "')";

                }              
                   saHelper.CmdExecute(Qry);
                  //dbHCmdExecute(string ConnString,string Qry)

              }
          }

      }

      
    }
}
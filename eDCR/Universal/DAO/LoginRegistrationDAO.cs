using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.DAL.Gateway;
using eDCR.Models;
using Systems.Universal;
using System.Data.OracleClient;
using eDCR.Universal.Common;
using System.Globalization;

namespace eDCR.DAL.DAO
{

    public class LoginRegistrationDAO
    {
        
        DBConnection dbConn = new DBConnection();      
        DBHelper dbHelper = new DBHelper();
        Terminal terminal = new Terminal();

        public List<LoginRegistrationModel> CheckUserCredential()
        {

            HttpContext.Current.Session["Conn"] = dbConn.SAConnStrReader();


            string conn = HttpContext.Current.Session["Conn"].ToString();

            string Qry = " SELECT ur.UserID,ur.RoleID,r.RoleName,ur.EmpID,"+
                         " v.EMPNAME,v.DESIGNATION,v.LOC_NAME,LOC_CODE," +
                         " u.NewPassword,u.OldPassword,ur.IsActive FROM Sa_UserInRole ur, Sa_UserCredential u,Sa_Role r,SA_EMPLOYEE_VW v "+
                         " Where ur.UserID=u.UserID and ur.RoleID=r.RoleID and ur.EMPID=v.EMPID(+)  and upper(ur.IsActive)=upper('true') ";
            
            DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
            List<LoginRegistrationModel> item;

            item = (from DataRow row in dt.Rows
                    select new LoginRegistrationModel
                    {
                        UserID = row["UserID"].ToString(),
                        Password = row["NewPassword"].ToString(),
                        RoleID = row["RoleID"].ToString(),
                        RoleName = row["RoleName"].ToString(),
                        EmpID = row["EmpID"].ToString(),
                        EmpName = row["EmpName"].ToString(),                    
                        Designation = row["Designation"].ToString() ,
                        LocName =row["LOC_NAME"].ToString(),
                        LocCode = row["LOC_CODE"].ToString(),                     
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString())

                    }).ToList();
            return item;



        }
   

        public bool MenuPopulate(string UserID)
        {
            if (dbHelper.ProcedureExecuteFn1(dbConn.SAConnStrReader(), "", "Sa_Menu_SP", "pUserID", UserID))
            {                    
                return true;
            }
            return false;
        }

     



        public void GetSubordinateFrmUser(string EmpID,string LocCode)
        {
         
            string Qry = "";
            
                if (HttpContext.Current.Session["EmpType"].ToString() == "MPO" || HttpContext.Current.Session["EmpType"].ToString() == "SMPO")
                {
                    Qry = Qry + " Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL Where MPO_CODE ='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO')";
                }
                 else if (HttpContext.Current.Session["EmpType"].ToString() == "TM")
                {
                    Qry = Qry + " Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL Where TERRITORY_CODE ='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO')";
                }
                else if(HttpContext.Current.Session["EmpType"].ToString() == "RSM" || HttpContext.Current.Session["EmpType"].ToString() == "Sr. Executive")
                {
                    Qry = Qry + " Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL  Where REGION_CODE ='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO','TM')";
                }
                else if(HttpContext.Current.Session["EmpType"].ToString() == "DSM" || HttpContext.Current.Session["EmpType"].ToString() == "Executive")
                {
                    Qry = Qry + "Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL  Where DIVISION_CODE ='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO','TM','RSM')";
                }
                else if (HttpContext.Current.Session["EmpType"].ToString() == "Manager" || HttpContext.Current.Session["EmpType"].ToString() == "Sr. Manager")
                {
                    Qry = Qry + "Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL  Where M_ID ='" + LocCode + "' AND DESIGNATION IN ('MPO','SMPO','TM','RSM','DSM')";
                }
                else if(HttpContext.Current.Session["EmpType"].ToString() == "EMA")
                {
                    Qry = Qry + "Select  LOC_CODE from VW_HR_LOC_MAPPING_ALL ";
                }
              

                if (Qry != "")
                {

                    DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);
                    if (dt.Rows.Count > 0)
                    {                  
                        DataTable dtLocCode = new DataView(dt).ToTable(true, "LOC_CODE");
                        string strLocCode = "";                      
                        if (dtLocCode.Rows.Count > 0)
                        {
                            strLocCode = string.Join("','", dbHelper.GetListString(dtLocCode));
                            strLocCode = "'" + strLocCode + "'";
                        }
                        HttpContext.Current.Session["LocCodeForQry"] = strLocCode;

                       
                    }
                   
                }
            
          
        }

      
        public bool UserLoginLOG(string EmpID,string enUserID,string enPassword, string RoleID,string OpMode)
        {
            string CntDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            string ip = terminal.GetLanIPAddress();
            string Qry = "INSERT INTO SA_USERCREDENTIALLOG(EMPID,USERID,NEWPASSWORD,ROLEID,TERMINAL,OPMode,SET_DATE) Values ('" + EmpID + "','" + enUserID + "','" + enPassword.Replace("'", "''") + "','" + RoleID + "','" + ip + "','" + OpMode + "',TO_Date('" + CntDate + "','dd-mm-yyyy HH24:MI:SS') )";
            if (dbHelper.CmdExecute(dbConn.SAConnStrReader(), Qry))
            {
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using eDCR.Areas.SA.Models.BEL;
using eDCR.DAL.Gateway;
using eDCR.Universal.Gateway;
using Systems.Universal;

namespace eDCR.Areas.SA.Models.DAL.DAO
{
    public class UserInFormDAO:ReturnData
    {
        DBConnection dbConn=new DBConnection();
        // SaHelper saHelper = new SaHelper();
        DBHelper saHelper = new DBHelper();
        public List<UserInFormBEL> GetUserInFormPermissionList(string UserID)
        {
            UserID = (UserID == ""|| UserID == null)  ? HttpContext.Current.Session["UserID"].ToString() : UserID;

            string RoleID = saHelper.GetValueFn(dbConn.SAConnStrReader(), "Select distinct RoleID from Sa_UserInRole Where UserID='" + UserID + "'");
            string Qry = "Select distinct  e.UserID,p.RoleID,p.RoleName,p.SoftwareID,p.SoftwareName, p.ModuleID,p.ModuleName, p.FormID,p.FormName,p.FormURL, "+
                        " NVL(e.ViewPermission,'false')  ViewPermission,NVL(e.SavePermission,'false')  SavePermission,NVL(e.EditPermission,'false')  EditPermission,NVL(e.DeletePermission,'false')  DeletePermission,NVL(e.PrintPermission,'false')  PrintPermission,e.SetOn  " + 
                        " from ((Select  a.RoleID,e.RoleName,a.SoftwareID,d.SoftwareName, a.ModuleID,c.ModuleName, b.FormID,b.FormName,b.FormURL  "+
                        " from Sa_RoleInSM  a,Sa_Form  b,Sa_Module c,Sa_Software d,Sa_Role e "+
                        " Where upper(a.IsActive)=upper('true') and a.RoleID||a.SoftwareID||a.ModuleID||b.FormID=e.RoleID||d.SoftwareID||c.ModuleID||b.FormID and e.RoleID='" + RoleID + "' and  d.SoftwareID='02'and a.ModuleID='01' and b.FormID  Between '100' and '199') Union all " +
                        " (Select  a.RoleID,e.RoleName,a.SoftwareID,d.SoftwareName, a.ModuleID,c.ModuleName, b.FormID,b.FormName,b.FormURL from Sa_RoleInSM a,Sa_Form  b,Sa_Module  c, Sa_Software d , Sa_Role e  "+
                        " Where upper(a.IsActive)=upper('true')  and a.RoleID||a.SoftwareID||a.ModuleID||b.FormID=e.RoleID||d.SoftwareID||c.ModuleID||b.FormID and a.RoleID='" + RoleID + "' and a.SoftwareID||a.ModuleID||b.FormID in ('0101001','0101002','0101003','0101004','0101005','0101006','0102009','0102010')) )  p " +
                        " LEFT JOIN Sa_UserInFormP  e ON p.RoleID||p.SoftwareID||p.ModuleID||p.FormID=e.RoleID||e.SoftwareID||e.ModuleID||e.FormID "+
                        " Order by  p.SoftwareID";

            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<UserInFormBEL> item;
            //using lamdaexpression
            item = (from DataRow row in dt.Rows
                    select new UserInFormBEL
                    {
                        RoleID = row["RoleID"].ToString(),
                        RoleName = row["RoleName"].ToString(),
                        SoftwareID = row["SoftwareID"].ToString(),
                        SoftwareName = row["SoftwareName"].ToString(),
                        ModuleID = row["ModuleID"].ToString(),
                        ModuleName = row["ModuleName"].ToString(),
                        FormID = row["FormID"].ToString(),
                        FormName = row["FormName"].ToString(),
                        FormURL = row["FormURL"].ToString(),
                        ViewPermission = Convert.ToBoolean(row["ViewPermission"].ToString()),
                        SavePermission = Convert.ToBoolean(row["SavePermission"].ToString()),
                        EditPermission = Convert.ToBoolean(row["EditPermission"].ToString()),
                        DeletePermission = Convert.ToBoolean(row["DeletePermission"].ToString()),
                        PrintPermission = Convert.ToBoolean(row["PrintPermission"].ToString()),

                    }).ToList();

            return item;
        }

        public bool SaveUpdate(UserInFormBEL master)
        {
            bool IsTrue = false;
            if (master != null)
            {
                if (master.detailsList != null)
                {
                    foreach (UserInFormBEL details in master.detailsList)
                    {
                        IsTrue = false;
                        string VSEDP = Convert.ToString(details.ViewPermission == true ? 1 : 0) + Convert.ToString(details.SavePermission == true ? 1 : 0) + Convert.ToString(details.EditPermission == true ? 1 : 0) + Convert.ToString(details.DeletePermission == true ? 1 : 0) + Convert.ToString(details.PrintPermission == true ? 1 : 0);
                                           

                        string Qry = "Select MAX(RoleID) ID from Sa_RoleInFormP";
                        var tuple = saHelper.ProcedureExecuteTFn3(dbConn.SAConnStrReader(), Qry, "Sa_RoleIn_SMP_SP", "P_RSMFormID", "p_VSEDP", "p_SetOn", details.RoleID + details.SoftwareID + details.ModuleID + details.FormID, VSEDP, master.UserID);
                        if (tuple.Item1)
                        {                       
                            MaxID = tuple.Item2;
                            IUMode = tuple.Item3;
                            DataTable dt = saHelper.DataTableRefCursorFn1(dbConn.SAConnStrReader(), "FNC_SHOW_MENU", "pUserID", master.UserID);
                            if (dt.Rows.Count>0)
                            {
                                IsTrue = true;
                            }                            
                        }
                        else
                        {
                            IsTrue = false;
                        }
                    }
                }
            }
            return IsTrue;
        }
    }
}
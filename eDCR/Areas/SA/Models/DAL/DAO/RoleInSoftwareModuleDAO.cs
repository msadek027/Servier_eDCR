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
    public class RoleInSoftwareModuleDAO:ReturnData
    {
        DBConnection dbConn = new DBConnection();
        // SaHelper saHelper = new SaHelper();
        DBHelper saHelper = new DBHelper();
        public List<RoleInSoftwareModuleBEL> GetRoleInSoftwareModuleMappingList(string RoleID)
        {
            RoleID = (RoleID == "" || RoleID == null) ? HttpContext.Current.Session["RoleID"].ToString() : RoleID;
            string Qry = "Select c.SoftwareID,c.SoftwareName,c.ModuleID,c.ModuleName,NVL(d.IsActive,'false') IsActive  from  " +
                        " (Select a.SoftwareID,a.SoftwareName,b.ModuleID,b.ModuleName from Sa_Software  a,Sa_Module  b Where upper(a.IsActive)=upper('true') and upper(b.IsActive)=upper('true') /* and a.SoftwareID='02' */)  c " +
                        " LEFT JOIN Sa_RoleInSM  d ON c.SoftwareID||c.ModuleID=d.SoftwareID||d.ModuleID and d.RoleID='"+RoleID+"' Order By c.SoftwareID,c.ModuleID";

            DataTable dt = saHelper.DataTableFn(dbConn.SAConnStrReader(), Qry);
            List<RoleInSoftwareModuleBEL> item;
            //using lamdaexpression
            item = (from DataRow row in dt.Rows
                    select new RoleInSoftwareModuleBEL
                    {
                       
                        SoftwareID = row["SoftwareID"].ToString(),
                        SoftwareName = row["SoftwareName"].ToString(),
                        ModuleID = row["ModuleID"].ToString(),
                        ModuleName = row["ModuleName"].ToString(),
                        IsActive = Convert.ToBoolean(row["IsActive"].ToString())



                    }).ToList();

            return item;
        }


        public Boolean SaveUpdate(RoleInSoftwareModuleBEL master)
        {
            bool IsTrue = false;
            if (master != null)
            {
                if (master.detailsList != null)
                {
                    foreach (RoleInSoftwareModuleBEL details in master.detailsList)
                    {
                        IsTrue = false;
                        string Qry = "Select MAX(RoleID) ID from Sa_RoleInSM";
                        var tuple = saHelper.ProcedureExecuteTFn2(dbConn.SAConnStrReader(), Qry, "Sa_RoleIn_SM_SP", "p_RoleSMID", "p_IsActive", details.RoleID + details.SoftwareID + details.ModuleID, details.IsActive.ToString());
                        if (tuple.Item1)
                        {
                            MaxID = tuple.Item2;
                            IUMode = tuple.Item3;
                            IsTrue= true;
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
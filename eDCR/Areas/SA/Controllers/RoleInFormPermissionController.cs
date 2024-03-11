using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.SA.Models.BEL;
using eDCR.Areas.SA.Models.DAL.DAO;
using System.Security.Cryptography;
using System.Text;
using Systems.Universal;
using Systems.ActionFilter;

namespace eDCR.Areas.SA.Controllers
{
    public class RoleInFormPermissionController : Controller
    {
        RoleInFormDAO primaryDAO = new RoleInFormDAO();
    
        // GET: /SA/RoleInSoftwareModuleFormMapping/

          [ActionAuth]
        public ActionResult frmRoleInFormPermission()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetRoleInFormPermissionList(string RoleID)
        {
            var model = primaryDAO.GetRoleInFormPermissionList(RoleID);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
    
        public ActionResult OperationsMode(RoleInFormBEL master)
        {
            try
            {
               
                if (primaryDAO.SaveUpdate(master))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmRole");
            }
            catch (Exception e)
            {
                if (e.Message.Substring(0, 9) == "ORA-00001")
                    return Json(new { Status = "Error:ORA-00001,Data already exists!" });//Unique Identifier.
                else if (e.Message.Substring(0, 9) == "ORA-02292")
                    return Json(new { Status = "Error:ORA-02292,Data already exists!" });//Child Record Found.
                else if (e.Message.Substring(0, 9) == "ORA-12899")
                    return Json(new { Status = "Error:ORA-12899,Data Value Too Large!" });//Value Too Large.
                else
                    return Json(new { Status = "! Error : Error Code:" + e.Message.Substring(0, 9) });//Other Wise Error Found

            }

        }
	}
}
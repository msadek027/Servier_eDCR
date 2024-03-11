using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.SA.Models.BEL;
using eDCR.Areas.SA.Models.DAL.DAO;
using Systems.ActionFilter;
using eDCR.Universal.Common;
using System.Data;
using System.Threading.Tasks;

namespace eDCR.Areas.SA.Controllers
{
    public class UserInRoleController : Controller
    {
        UserInRoleDAO primaryDAO = new UserInRoleDAO();
      
       // [ActionAuth]
        public ActionResult frmUserInRole()
        {
            string EmpTypeSession = Session["EmpType"].ToString();

            if (Session["UserID"] != null)
            {

                if (EmpTypeSession == "Manager" || EmpTypeSession == "DSM" || EmpTypeSession == "EMA")
                {
                    return View();
                }
           
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
           // return RedirectToAction("Index", "LoginRegistration");
        }

        [HttpGet]
        public ActionResult GetEmployee()
        {

            var listData = primaryDAO.GetEmployeeList();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;

            return data;   

          
        }

        [HttpGet]
        public async Task<dynamic> GetEmployeeNotYetAssigned()
        {
           

            var listData = primaryDAO.GetEmployeeNotYetAssignedList();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;

            return data;
          
        }

        [HttpGet]
        public ActionResult GetBuyer()
        {
            var data = primaryDAO.GetBuyerList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetBuyerYetAssigned(string EmpID)
        {
            var data = primaryDAO.GetBuyerYetAssignedList(EmpID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    
        [HttpGet]
        public ActionResult GetUser()
        {
            var data = primaryDAO.GetUserList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        

        [HttpGet]
        public ActionResult GetUserInRole()
        {
           

            var listData = primaryDAO.GetUserInRoleList();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;           
        }


        [HttpPost]
        public ActionResult OperationsMode(UserInRoleBEL master)
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
        [HttpPost]
        public ActionResult OperationsModeDelete(UserInRoleBEL master)
        {
            try
            {
                if (primaryDAO.Delete(master))
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
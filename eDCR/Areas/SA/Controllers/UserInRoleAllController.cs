using eDCR.Areas.SA.Models.BEL;
using eDCR.Areas.SA.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.SA.Controllers
{
    public class UserInRoleAllController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        UserInRoleDAO primaryDAO = new UserInRoleDAO();
        // GET: /SA/frmUserInRoleAll/
        public ActionResult frmUserInRoleAll()
        {
            return View();
        }


        [HttpGet]
        public ActionResult GetEmployeeAll()
        {
            var data = primaryDAO.GetEmployeeAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserType()
        {
            var data = primaryDAO.GetDistinctUserType();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetRoleAll()
        {
            var data = primaryDAO.GetRoleAll();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

         [HttpGet]
        public ActionResult GetAllRelationalData()
        {
            var listData = primaryDAO.GetAllRelationalData();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }

        


        [HttpPost]
        public ActionResult GetUserInRoleAll(string UserType)
        {
            var data = primaryDAO.GetUserInRoleAll(UserType);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OperationsMode(UserInRoleBELDetail master)
        {
            try
            {
                if (primaryDAO.SaveUpdateAll(master))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmRole");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult SendEmail(UserInRoleBELDetail master)
        {
            try
            {
                // Gmail Address from where you send the mail
                var fromAddress = "engr.msadek027@gmail.com";
                // any address where the email will be sending
             //   var toAddress = "msadek_027@yahoo.com";
                var toAddress = "sadequr@squaregroup.com";
                //Password of your gmail address
                const string fromPassword = "b224060m";
                // Passing the values and make a email formate to display
                string subject = "Test";
                string body = "Dear....";
                // smtp settings
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;//  465;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;               
                }
                // Passing values to smtp object
                smtp.Send(fromAddress, toAddress, subject, body);
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
            }
            

            return View("frmRole");
        }

    }
}
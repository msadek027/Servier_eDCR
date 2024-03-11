using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportUserInfoExceptPasswordController : Controller
    {
        // GET: DCR/ReportUserInfoExceptPassword
        ReportUserInLogDAO primaryDAO = new ReportUserInLogDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        public ActionResult frmReportUserInfoExceptPassword()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }


        [HttpPost]
        public ActionResult GetUser(string RoleID)
        {
            var data = primaryDAO.GetUser();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetUserInfo(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetUserInfo(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
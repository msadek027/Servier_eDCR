using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportUserInfoController : Controller
    {
        ReportUserInLogDAO primaryDAO = new ReportUserInLogDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        public ActionResult frmReportUserInfo()
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
            var listData = primaryDAO.GetUserInfo(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

         
        }
    }
}
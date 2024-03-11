using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportItemBalanceController : Controller
    {
        ReportItemBalanceDAO primaryDAO = new ReportItemBalanceDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        public ActionResult frmReportItemBalance()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

      
        [HttpPost]
        public ActionResult GetGridData(ReportItemBalanceBEL model)
        {
            var data = primaryDAO.GetGridData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

	}
}
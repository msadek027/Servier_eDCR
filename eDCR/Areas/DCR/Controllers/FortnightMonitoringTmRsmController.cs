using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class FortnightMonitoringTmRsmController : Controller
    {
        ReportFortnightMonitoringRemarksSupDAO primaryDAO = new ReportFortnightMonitoringRemarksSupDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        public ActionResult frmFortnightMonitoringTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult OperationsMode(ReportFortnightMonitoringRemarksSupBEO model)
        {
            try
            {
                if (primaryDAO.SaveUpdate(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });

                }
                else
                    return View("frmImportExcelToGrid");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        } 
	}
}
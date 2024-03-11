using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class TourPlanPartialAllowController : Controller
    {
        TourPlanDAO primaryDAO = new TourPlanDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        public ActionResult frmTourPlanPartialAllow()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }



        [HttpPost]
        public ActionResult TourPartialAllow()
        {
            var data = primaryDAO.TourPartialAllow();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult OperationsModeTourPlanPartialAllow(DefaultBEL model)
        {
            try
            {
                if (primaryDAO.SaveUpdateTourPlanPartialAllow(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanNew");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
	}
}
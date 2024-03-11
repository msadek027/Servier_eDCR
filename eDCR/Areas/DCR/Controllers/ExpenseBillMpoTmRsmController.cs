using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ExpenseBillMpoTmRsmController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        ExpenseBillMpoTmRsmDAO primaryDAO = new ExpenseBillMpoTmRsmDAO();
        ExportToAnother export = new ExportToAnother();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();

        DefaultExpenseBillMpoTmRsmDAO expenseBillMpoTmRsmDAO = new DefaultExpenseBillMpoTmRsmDAO();
        public ActionResult frmExpenseBillMpoTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmExpenseBillMpoTmRsmDetail()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetPopupView(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetPopupView(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSummaryData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetSummaryExpenseBillMpoTmRsm(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetDetailData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDetailExpenseBillMpoTmRsm(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult OperationsModeSummary(ExpenseBillMpoTmRsmInsertSummaryDetail model)
        {
            try
            {
                if (primaryDAO.SaveUpdateSummary(model))
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

        [HttpPost]
        public ActionResult OperationsModeDetail(ExpenseBillMpoTmRsmInsertSummaryDetail model)
        {
            try
            {
                if (primaryDAO.SaveUpdateDetail(model))
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
        [HttpPost]
        public ActionResult OperationsModeDateWiseRecommend(ExpenseBillMpoTmRsmInsertSummaryDetail model)
        {
            try
            {
                if (primaryDAO.SaveUpdateDetail(model))
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




        [HttpPost]
        public ActionResult OperationsModeDaTa(ExpenseBillMpoTmRsmDaTa model)
        {
            try
            {
                if (primaryDAO.SaveUpdateDaTa(model))
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
using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportProductGiftItemController : Controller
    {
        ItemDataUploadDAO itemDataUploadDAO = new ItemDataUploadDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Object data = null;
        public ActionResult frmReportProductGiftItem()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetGridData(DefaultParameterBEO model)
        {
            data = itemDataUploadDAO.GetReportProductGiftData(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

    }
}
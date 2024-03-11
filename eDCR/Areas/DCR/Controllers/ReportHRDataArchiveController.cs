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
    public class ReportHRDataArchiveController : Controller
    {
        // GET: DCR/ReportHRDataArchive
        HRDataUploadDAO hRDataUploadDAO = new HRDataUploadDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Object data = null;



        public ActionResult frmReportHRDataArchive()
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
            var listData = hRDataUploadDAO.GetMPOGridDataArchive(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

           
        }

    }
}
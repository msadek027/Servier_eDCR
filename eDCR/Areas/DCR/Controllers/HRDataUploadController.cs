using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.IO;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class HRDataUploadController : Controller
    {
        HRDataUploadDAO hRDataUploadDAO = new HRDataUploadDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Object data = null;



        public ActionResult frmHRDataUpload()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult TemplateDownload()
        {
            string FileName = "HR.xlsx";
            string fileLocation = Server.MapPath("~/TempleteFiles/");
            fileLocation = fileLocation + FileName;
            var model = fileLocation;
            return Json(new { FileName = FileName, FilePath = fileLocation });
        }
        [HttpGet]
        public ActionResult Download(string fileName)
        {
            fileName = "HR.xlsx";
            //Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/TempleteFiles"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            //System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        [HttpGet]
        public ActionResult GetGridData()
        {
            var listData = hRDataUploadDAO.GetMPOGridData();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }


        [HttpPost]
        public ActionResult OperationsMode(HRDataUploadBEL model)
        {
            try
            {
                if (hRDataUploadDAO.SaveUpdate(model))
                {
                    return Json(new { ID = hRDataUploadDAO.MaxID, Mode = hRDataUploadDAO.IUMode, Status = "Yes" });
                }

                else
                    return View("frmHRDataUpload");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }

    }
}
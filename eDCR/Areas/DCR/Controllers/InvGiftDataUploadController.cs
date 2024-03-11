using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class InvGiftDataUploadController : Controller
    {
        ItemDataUploadDAO itemDataUploadDAO = new ItemDataUploadDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Object data = null;
        public ActionResult frmInvGiftDataUpload()
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
            string FileName = "InvGift.xlsx";
            string fileLocation = Server.MapPath("~/TempleteFiles/");
            fileLocation = fileLocation + FileName;
            var model = fileLocation;
            return Json(new { FileName = FileName, FilePath = fileLocation });
        }
        [HttpGet]
        public ActionResult Download(string fileName)
        {
            fileName = "InvGift.xlsx";
            //Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/TempleteFiles"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            //System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        [HttpPost]
        public ActionResult OperationsMode(ItemDataUploadBEL model)
        {
            try
            {
                if (itemDataUploadDAO.SaveUpdateGift(model))
                {
                    var jsonResult = Json(new { MaxJsonLength = 86753090, ID = itemDataUploadDAO.MaxID, Mode = itemDataUploadDAO.IUMode, Status = "Yes" }, JsonRequestBehavior.AllowGet);

                    return jsonResult;
                    //return Json(new { ID = itemDataUploadDAO.MaxID, Mode = itemDataUploadDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmItemDataUpload");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult OperationsDeleteMode(ItemDataUploadBEL model)
        {
            try
            {
                if (itemDataUploadDAO.DeleteGift(model))
                {

                    return Json(new { ID = itemDataUploadDAO.MaxID, Mode = itemDataUploadDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmItemDataUpload");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult GetGridData(ItemDataUploadBEL model)
        {

            data = itemDataUploadDAO.GetInvGiftData();
            return Json(data, JsonRequestBehavior.AllowGet);
           

        }

    }
}
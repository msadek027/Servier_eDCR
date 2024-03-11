﻿using eDCR.Areas.DCR.Models.BEL;
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
    public class PromotionalItemUploadSlSmGtWiseController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        PromotionalItemUploadDAO IteamsUploadDAO = new PromotionalItemUploadDAO();
   
  
        public ActionResult frmPromotionalItemUploadSlSmGtWise()
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
            string FileName = "PromotionalItemUploadSlSmGt.xlsx";
            string fileLocation = Server.MapPath("~/TempleteFiles/");
            fileLocation = fileLocation + FileName;
            var model = fileLocation;
            return Json(new { FileName = FileName, FilePath = fileLocation });
        }
        [HttpGet]
        public ActionResult Download(string fileName)
        {
            fileName = "PromotionalItemUploadSlSmGt.xlsx";
            //Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/TempleteFiles"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            //System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }
        [HttpPost]
        public ActionResult GetGridData(string FileType,string MonthNumber,string Year,string ItemFor)
        {

            var listData = IteamsUploadDAO.GetSampleData(MonthNumber, Year, FileType, ItemFor);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;            
        }



        [HttpPost]
        public ActionResult OperationsMode(PromotionalItemUploadBEL model)
        {
            try
            {
                if (IteamsUploadDAO.SaveUpdate(model))
                {                    
                        return Json(new { ID = IteamsUploadDAO.MaxID, Mode = IteamsUploadDAO.IUMode, Status = "Yes" });                    
                }
                else
                    return View("frmImportExcelToGrid");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        } 

        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files, PromotionalItemUploadBEL model)
        {
            ViewBag.FileName = "";
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }
                    file.SaveAs(physicalPath);
                    TempData["file"] = fileName;
                    break;
                }
            }
            // Return an empty string to signify success
            return Content("");
        }


        public ActionResult LoadExcelFile(string fileName, string FileType)
        {
            Object listData = null;
            DataSet dataSet = new DataSet();

            try
            {
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                string connectionString = "";
                string filename = Path.Combine(Server.MapPath("~/Content"), TempData["file"].ToString());
                string[] d = filename.Split('.');
                string fileExtension = "." + d[d.Length - 1].ToString();
                if (d.Length > 0)
                {
                    if (fileExtension == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    }
                }


                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(connectionString);
                excelConnection.Open();
                DataTable dt = new DataTable();

                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }
                OleDbConnection excelConnection1 = new OleDbConnection(connectionString);
                //DataSet dataSet = new DataSet();

                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(dataSet);
                }

                dt = dataSet.Tables[0];
                excelConnection.Close();
                listData = IteamsUploadDAO.GetSampleItemMap(dt);

              
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

          
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
         
        }




     

	}
}
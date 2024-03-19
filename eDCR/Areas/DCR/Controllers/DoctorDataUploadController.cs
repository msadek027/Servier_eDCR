using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.IO;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class DoctorDataUploadController : Controller
    {
        DoctorDataUploadDAO doctorDataUploadDAO = new DoctorDataUploadDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        Object data = null;

        //
        // GET: /DCR/DoctorDataUpload/
        public ActionResult frmDoctorDataUpload()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [HttpPost]
        public ActionResult GetGridData(DoctorDataUploadInfo model)
        {
            var listData = doctorDataUploadDAO.GetDoctorGridData(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

        }
        [HttpPost]
        public ActionResult TemplateDownload()
        {
            string FileName = "DoctorDB.xlsx";
            string fileLocation = Server.MapPath("~/TempleteFiles/");
            fileLocation = fileLocation + FileName;
            var model = fileLocation;
            return Json(new { FileName = FileName, FilePath = fileLocation });
        }
        [HttpGet]
        public ActionResult Download(string fileName)
        {
            fileName = "DoctorDB.xlsx";
            //Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath("~/TempleteFiles"), fileName);
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            //System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }
        [HttpPost]
        public ActionResult OperationsMode(DoctorDataUploadBEL model)
        {
            try
            {
                if (doctorDataUploadDAO.SaveUpdate(model))
                {
                    return Json(new { ID = doctorDataUploadDAO.MaxID, Mode = doctorDataUploadDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmDoctorDataUpload");
            }

            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }

        [HttpGet]
        public ActionResult GetMarkets()
        {
            var listData = doctorDataUploadDAO.GetMarkets();
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }


        //public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files, DoctorDataUploadBEL model)
        //{
        //    ViewBag.FileName = "";
        //    // The Name of the Upload component is "files"
        //    if (files != null)
        //    {
        //        foreach (var file in files)
        //        {
        //            var fileName = Path.GetFileName(file.FileName);
        //            var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
        //            if (System.IO.File.Exists(physicalPath))
        //            {
        //                System.IO.File.Delete(physicalPath);
        //            }
        //            file.SaveAs(physicalPath);
        //            TempData["file"] = fileName;
        //            break;
        //        }
        //    }
        //    // Return an empty string to signify success
        //    return Content("");
        //}


        //public ActionResult LoadExcelFile(string fileName)
        //{
        //    Object data = null;
        //    DataSet dataSet = new DataSet();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(TempData["file"].ToString()))
        //        {
        //            return Json("", JsonRequestBehavior.AllowGet);
        //        }

        //        string connectionString = "";
        //        string filename = Path.Combine(Server.MapPath("~/Content"), TempData["file"].ToString());
        //        string[] d = filename.Split('.');
        //        string fileExtension = "." + d[d.Length - 1].ToString();
        //        if (d.Length > 0)
        //        {
        //            if (fileExtension == ".xls")
        //            {
        //                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
        //            }
        //            //connection String for xlsx file format.
        //            else if (fileExtension == ".xlsx")
        //            {
        //                connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
        //            }
        //        }


        //        //Create Connection to Excel work book and add oledb namespace
        //        OleDbConnection excelConnection = new OleDbConnection(connectionString);
        //        excelConnection.Open();
        //        DataTable dt = new DataTable();

        //        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        if (dt == null)
        //        {
        //            return null;
        //        }

        //        String[] excelSheets = new String[dt.Rows.Count];
        //        int t = 0;
        //        //excel data saves in temp file here.
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            excelSheets[t] = row["TABLE_NAME"].ToString();
        //            t++;
        //        }
        //        OleDbConnection excelConnection1 = new OleDbConnection(connectionString);
        //        //DataSet dataSet = new DataSet();

        //        string query = string.Format("Select * from [{0}]", excelSheets[0]);
        //        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
        //        {
        //            dataAdapter.Fill(dataSet);
        //        }

        //        dt = dataSet.Tables[0];
        //        excelConnection.Close();              
        //       data = doctorDataUploadDAO.GetDoctor(dt);             


        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
    }
}
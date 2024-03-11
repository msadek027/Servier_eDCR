using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportActivityLogController : Controller
    {
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ReportActivityLogDAO primaryDAO=new ReportActivityLogDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        ExportToAnother export = new ExportToAnother();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();


        public ActionResult frmReportActivityLog()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetActivityLog(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetActivityLog(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetExportActivityLog(DefaultParameterBEO model)
        {

            string FileName = "ActivityLog_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Activity Log";

            var tuple = primaryDAO.GetExportActivityLog(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);


            }
            else if (model.ExportType == "PDF" && dt.Rows.Count > 0)
            {

                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                //file name to be created   
                string strPDFFileName = string.Format(FileName + "_Pdf.pdf");
                Document doc = new Document(PageSize.LETTER);
                //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 20, 10);
                //doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 8;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table  
                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                //Add Content to PDF  

                doc.Add(Add_Content_To_PDF(tableLayout, PdfNoOfColumns, dt, dataList, vHeader, vParameter));

                // Closing the document  
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;


                return File(workStream, "application/pdf", strPDFFileName);
            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportActivityLogBEO> item, string vHeaderSubject, string vHeaderParameter)
        {
            float[] headers = { 10, 20, 30, 20, 30, 20, 20, 20}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            //tableLayout Title to the PDF file at the top
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Code", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Operation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date Time", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Set By Name", 0);

            ////Add body 
            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeCode);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Designation);

                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Operation);
                ExportToPDF.AddCellToBody(tableLayout, emp.SetDate);
                ExportToPDF.AddCellToBody(tableLayout, emp.SetByName);        
            }
            if (item.Count == 1)
            {          
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");

                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
            }

            return tableLayout;
        }
    }
}
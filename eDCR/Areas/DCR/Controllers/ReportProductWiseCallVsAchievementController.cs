using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL;
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
    public class ReportProductWiseCallVsAchievementController : Controller
    {
        ReportProductWiseCallSummaryDAO primaryDAO = new ReportProductWiseCallSummaryDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
       
        public ActionResult frmReportProductWiseCallVsAchievement()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Product_Wise_Call_Summary_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Product Wise Call Summary";

            var tuple = primaryDAO.Export(model);
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
                int PdfNoOfColumns = 7;
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

                //AddPageNumbers(byteInfo);

                return File(workStream, "application/pdf", strPDFFileName);
            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }



      protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportProductWiseCallSummaryBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20,10,30, 10, 10, 10,10  }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 3; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);




            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Product Code", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Pack Size", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total Product Call", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Product Call", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "%", 0);

            ////Add body  

            foreach (var emp in item)
            {
             ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
             ExportToPDF.AddCellToBody(tableLayout, emp.ProductCode);
             ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
             ExportToPDF.AddCellToBody(tableLayout, emp.PackSize);
             ExportToPDF.AddCellToBody(tableLayout, emp.TotalProductCall);
             ExportToPDF.AddCellToBody(tableLayout, emp.ProductCall);
             ExportToPDF.AddCellToBody(tableLayout, emp.Achi);
            }

            return tableLayout;
        }

        
	}
}
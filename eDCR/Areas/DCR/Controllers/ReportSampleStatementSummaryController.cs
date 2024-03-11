using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
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
    public class ReportSampleStatementSummaryController : Controller
    {
        ReportSampleStatementSummaryDAO primaryDAO = new ReportSampleStatementSummaryDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportSampleStatementSummary()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetMainGridData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetMainGridData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

       



        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Sample_Statement_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Sample Statement Summary";
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
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 7;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
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



     
   protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportSampleStatementSummaryBEO> item, string vHeaderSubject, string vHeaderParameter)
        {
            float[] headers = { 20, 40, 30, 50, 20, 20, 20 }; //Header Widths         
            
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 3; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            //tableLayout Title to the PDF file at the top 
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header  
            AddCellToHeader(tableLayout, "Market", 0);
            AddCellToHeader(tableLayout, "Employee Name", 0);
            AddCellToHeader(tableLayout, "Product Code", 0);
            AddCellToHeader(tableLayout, "Product Name", 0);      
     
            AddCellToHeader(tableLayout, "Receive Qty", 0);
            AddCellToHeader(tableLayout, "Execute Qty", 0);
            AddCellToHeader(tableLayout, "Balance Qty", 0);
         
            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.EmployeeName);
                AddCellToBody(tableLayout, emp.ProductCode);
                AddCellToBody(tableLayout, emp.ProductName);
             
         
                AddCellToBody(tableLayout, emp.ReceiveQty);
                AddCellToBody(tableLayout, emp.ExecuteQty);
                AddCellToBody(tableLayout, emp.BalanceQty);
              
            }


           

            return tableLayout;
        }
        // Method to add single cell to the Header  

        private static void AddCellToHeaderMain(PdfPTable tableLayout, string cellText, int colSpan)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {

                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 1,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)


            });

        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText, int rDegree)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 6, 0, iTextSharp.text.BaseColor.WHITE)))
            {
                Rotation = rDegree,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)
            });

        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 5, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }


        private static void AddCellToFooterSum(PdfPTable tableLayout, string cellText, int colSpan)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 5, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Areas.DCR.Models.BEL;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportDvrSummaryController : Controller
    {
        ReportDVRArchiveDAO primaryDAO = new ReportDVRArchiveDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDvrSummary()
        {
           if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        public ActionResult GetDvrSummary(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDvrSummary(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }





        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "DVR_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "DVR Summary";

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
                int PdfNoOfColumns = 9;
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

        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            // we create a reader for a certain document
            PdfReader reader = new PdfReader(pdf);
            // we retrieve the total number of pages
            int n = reader.NumberOfPages;
            // we retrieve the size of the first page
            Rectangle psize = reader.GetPageSize(1);

            // step 1: creation of a document-object
            Document document = new Document(psize, 10, 10, 20, 10);

            // step 2: we create a writer that listens to the document
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            // step 3: we open the document

            document.Open();
            // step 4: we add content
            PdfContentByte cb = writer.DirectContent;



            int p = 0;

            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;
                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);

                cb.AddTemplate(importedPage, 0, 0);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, +p + "/" + n, 44, 7, 0);
                cb.EndText();


            }
            // step 5: we close the document

            document.Close();
            return ms.ToArray();
        }



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDVRArchiveBEL> item, string vHeaderSubject, string vHeaderParameter)
        {
        
            float[] headers = { 25, 10, 20, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row

            //tableLayout Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);


            ////Add header  
            AddCellToHeader(tableLayout, "Market Name");
            AddCellToHeader(tableLayout, "MPO Code");
            AddCellToHeader(tableLayout, "MPO Name");
        

            AddCellToHeader(tableLayout, "Date");
            AddCellToHeader(tableLayout, "Day");

            AddCellToHeader(tableLayout, "Morning DOT");
            AddCellToHeader(tableLayout, "Evening DOT");
            AddCellToHeader(tableLayout, "Total DOT");
            AddCellToHeader(tableLayout, "Intern DOT");
            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.MPOCode);
                AddCellToBody(tableLayout, emp.MPOName);
              

                AddCellToBody(tableLayout, emp.Date);
                AddCellToBody(tableLayout, emp.DayName);
                AddCellToBody(tableLayout, emp.MorningDOT);

                AddCellToBody(tableLayout, emp.EveningDOT);
                AddCellToBody(tableLayout, emp.TotalDOT);
                AddCellToBody(tableLayout, emp.InternDOT);

            }


            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)

            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 6, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        } 
    }
}
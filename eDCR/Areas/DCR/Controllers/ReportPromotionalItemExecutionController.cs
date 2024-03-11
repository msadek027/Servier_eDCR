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
    public class ReportPromotionalItemExecutionController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        PromotionalItemViewDAO promotionalItemViewDAO = new PromotionalItemViewDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportPromotionalItemExecution()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult MainGridDataNew(DefaultParameterBEO model)
        {
            var listData = promotionalItemViewDAO.ReportPromotionalItemExecution(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;          
        }
        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "PromotionalItemExecution" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Promotional Item Execution";

            var tuple = promotionalItemViewDAO.Export(model);
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
                int PdfNoOfColumns = 14;
                PdfPTable tableLayout = new PdfPTable(14);
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




         protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportPromotionalItemExecuion> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10, 15, 10, 15, 10, 20, 10, 10,10,30, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 2; // 2 header rows + 1 footer row

            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);




            ////Add header
            AddCellToHeader(tableLayout, "Region Code", 1);
            AddCellToHeader(tableLayout, "Region Name", 1);
            AddCellToHeader(tableLayout, "Territory Code", 1);
            AddCellToHeader(tableLayout, "Territory Name", 1);
            AddCellToHeader(tableLayout, "MPO Code", 1);
            AddCellToHeader(tableLayout, "MPO Name", 1);
            AddCellToHeader(tableLayout, "Market Name", 1);
            AddCellToHeader(tableLayout, "SBU", 1);
            AddCellToHeader(tableLayout, "Product Code", 1);
            AddCellToHeader(tableLayout, "Product Name", 1);         
            AddCellToHeader(tableLayout, "Item Type", 1);
            AddCellToHeader(tableLayout, "Total Qty", 1);
            AddCellToHeader(tableLayout, "Execute Qty", 1);
            AddCellToHeader(tableLayout, "Balance Qty", 1);
           
       
            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.RegionCode);
                AddCellToBody(tableLayout, emp.RegionName);
                AddCellToBody(tableLayout, emp.TerritoryCode);
                AddCellToBody(tableLayout, emp.TerritoryName);
                AddCellToBody(tableLayout, emp.MpoCode);
                AddCellToBody(tableLayout, emp.MpoName);
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.SBU);
                AddCellToBody(tableLayout, emp.ProductCode);
                AddCellToBody(tableLayout, emp.ProductName);
                AddCellToBody(tableLayout, emp.ItemType);
                AddCellToBody(tableLayout, emp.TotalQty);
                AddCellToBody(tableLayout, emp.ExecuteQty);
                AddCellToBody(tableLayout, emp.BalanceQty);
              

            }


            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText, int Span)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
                Colspan = Span,
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
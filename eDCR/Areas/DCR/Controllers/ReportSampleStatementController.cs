
using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Universal.Common;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportSampleStatementController : Controller
    {
        ReportItemStatementDAO primaryDAO = new ReportItemStatementDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();

        public ActionResult frmReportSampleStatement()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        public ActionResult frmReportSampleStatement2()
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
            //var data = primaryDAO.GetMainGridData(model);
            //return Json(data, JsonRequestBehavior.AllowGet);


            var tuple = primaryDAO.GetMainGridData(model);
            DataTable dt = tuple.Item1;
            var dataList = tuple.Item2;

            var DataSum = primaryDAO.GetGrandTotalSum(model, dt);
            dataList = dataList.Concat(DataSum).ToList();


            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDetail(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDetailLink(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        //https://scottstoecker.wordpress.com/2016/12/08/showing-page-numbers-when-an-itextsharp-table-spans-multiple-pages/
        //public class TableRowCounter : IPdfPTableEventSplit
        //{
        //    private int pageCount = 0;
        //    private int totalRowCount = -1;
        //    private double firstPageRowCount = 0;
        //    private bool isTrue = false;
        //    public void SplitTable(PdfPTable table)
        //    {
        //        this.totalRowCount = table.Rows.Count;

        //    }

        //    public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        //    {
              
        //        var writer = canvases[PdfPTable.BASECANVAS].PdfWriter;
        //        var thisRowCount = table.Rows.Count;          

        //        if (isTrue == false)
        //        {
        //            isTrue = true;
        //            firstPageRowCount = table.Rows.Count - 3;
        //        }
        //       // int totalPage = (int)Math.Round(totalRowCount / firstPageRowCount)+1;
        //       int totalPage = (int)Math.Round(totalRowCount / 41.0)+1;     //     


        //        BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
        //        Font font = new Font(baseFont, 9);
        //        //Page 1 of 10
        //        pageCount += 1;

        //        int TotalPageCount = pageCount;
        //        var text = "Page " + pageCount.ToString() + " of " + totalPage;

        //       // ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(text, font), 775, 525, 0);

        //        ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(text, font), 770, 525, 0);
        //    }
        //}
        public ActionResult Export(DefaultParameterBEO model)
        {
            ReportItemStatementBEL model2 = new ReportItemStatementBEL();

            string FileName = "Sample_Statement_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Sample Statement";

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

          



            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {   //Add New Code
                var Data = primaryDAO.GetGrandTotalSum(model, dt);
                dataList= dataList.Concat(Data).ToList();


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
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 52;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF(tableLayout, PdfNoOfColumns, dt, model, dataList, vHeader, vParameter));

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




     
   protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, DefaultParameterBEO model, List<ReportItemStatementBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 70, 30,45, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20,20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 ,20,20}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();



            //tableLayout Title to the PDF file at the top 
           // tableLayout.TableEvent = new TableRowCounter();
            TableRowCounter tableRowCounter = new TableRowCounter();
            tableLayout.TableEvent = tableRowCounter;

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);
      
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);

            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);

            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "First Week Used", 8);          
            AddCellToHeaderMain(tableLayout, "",1);

            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "",1);
            AddCellToHeaderMain(tableLayout, "Second Week Used", 7);        
            AddCellToHeaderMain(tableLayout, "",1 );

            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "Third Week Used", 8);
            AddCellToHeaderMain(tableLayout, "", 1);

            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "Fourth Week Used", 8);
            AddCellToHeaderMain(tableLayout, "",1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);

            ////Add header  
            AddCellToHeader(tableLayout, "Product Name", 0);
            AddCellToHeader(tableLayout, "Type", 0);
            AddCellToHeader(tableLayout, "Market Name",0);

            AddCellToHeader(tableLayout, "Carry In Qty", 90);
            AddCellToHeader(tableLayout, "Receive Qty", 90);
            AddCellToHeader(tableLayout, "Add Defi 01-30", 90);
            AddCellToHeader(tableLayout, "Total Stock Qty", 90);
    
            AddCellToHeader(tableLayout, "Add / Defi 01-08", 90);
            AddCellToHeader(tableLayout, "Rest Qty 01-08", 90);
            AddCellToHeader(tableLayout, "01", 0);
            AddCellToHeader(tableLayout, "02", 0);
            AddCellToHeader(tableLayout, "03", 0);
            AddCellToHeader(tableLayout, "04", 0);
            AddCellToHeader(tableLayout, "05", 0);
            AddCellToHeader(tableLayout, "06", 0);
            AddCellToHeader(tableLayout, "07", 0);
            AddCellToHeader(tableLayout, "08", 0);
            AddCellToHeader(tableLayout, "Used Qty 01-08", 90);

            AddCellToHeader(tableLayout, "Add / Defi 09-15", 90);
            AddCellToHeader(tableLayout, "Rest Qty 09-15", 90);
            AddCellToHeader(tableLayout, "09", 0);
            AddCellToHeader(tableLayout, "10", 0);
            AddCellToHeader(tableLayout, "11", 0);
            AddCellToHeader(tableLayout, "12", 0);
            AddCellToHeader(tableLayout, "13", 0);
            AddCellToHeader(tableLayout, "14", 0);
            AddCellToHeader(tableLayout, "15", 0);
            AddCellToHeader(tableLayout, "Used Qty 09-15", 90);

            AddCellToHeader(tableLayout, "Add / Defi 16-23", 90);
            AddCellToHeader(tableLayout, "Rest Qty 16-23", 90);
            AddCellToHeader(tableLayout, "16", 0);
            AddCellToHeader(tableLayout, "17", 0);
            AddCellToHeader(tableLayout, "18", 0);
            AddCellToHeader(tableLayout, "19", 0);
            AddCellToHeader(tableLayout, "20", 0);
            AddCellToHeader(tableLayout, "21", 0);
            AddCellToHeader(tableLayout, "22", 0);
            AddCellToHeader(tableLayout, "23", 0);
            AddCellToHeader(tableLayout, "Used Qty 16-23", 90);

            AddCellToHeader(tableLayout, "Add / Defi 24-31", 90);
            AddCellToHeader(tableLayout, "Rest Qty 24-31", 90);
            AddCellToHeader(tableLayout, "24", 0);
            AddCellToHeader(tableLayout, "25", 0);
            AddCellToHeader(tableLayout, "26", 0);
            AddCellToHeader(tableLayout, "27", 0);
            AddCellToHeader(tableLayout, "28", 0);
            AddCellToHeader(tableLayout, "29", 0);
            AddCellToHeader(tableLayout, "30", 0);
            AddCellToHeader(tableLayout, "31", 0);
            AddCellToHeader(tableLayout, "Used Qty 24-31", 90);
            AddCellToHeader(tableLayout, "Used Qty 01-31", 90);
            AddCellToHeader(tableLayout, "Closing Stock",90);

            ////Add body  

            foreach (var emp in item)
            {

                AddCellToBody(tableLayout, emp.ProductName);               
                AddCellToBody(tableLayout, emp.ItemType);                
                AddCellToBody(tableLayout, emp.MarketName);                
                AddCellToBody(tableLayout, emp.RestQty);                
                AddCellToBody(tableLayout, emp.CentralQty);                
                AddCellToBody(tableLayout, emp.TotalQtyAddDefi0130);                
                AddCellToBody(tableLayout, emp.TotalQty);

                AddCellToBody(tableLayout, emp.GivenQty0108);
                AddCellToBody(tableLayout, emp.TotalStock0108);
                AddCellToBody(tableLayout, emp.d01);
                AddCellToBody(tableLayout, emp.d02);
                AddCellToBody(tableLayout, emp.d03);
                AddCellToBody(tableLayout, emp.d04);
                AddCellToBody(tableLayout, emp.d05);
                AddCellToBody(tableLayout, emp.d06);
                AddCellToBody(tableLayout, emp.d07);
                AddCellToBody(tableLayout, emp.d08);
                AddCellToBody(tableLayout, emp.TotalDCR0108);

                AddCellToBody(tableLayout, emp.GivenQty0915);
                AddCellToBody(tableLayout, emp.TotalStock0915);
                AddCellToBody(tableLayout, emp.d09);
                AddCellToBody(tableLayout, emp.d10);
                AddCellToBody(tableLayout, emp.d11);
                AddCellToBody(tableLayout, emp.d12);
                AddCellToBody(tableLayout, emp.d13);
                AddCellToBody(tableLayout, emp.d14);
                AddCellToBody(tableLayout, emp.d15);
                AddCellToBody(tableLayout, emp.TotalDCR0915);

                AddCellToBody(tableLayout, emp.GivenQty1623);
                AddCellToBody(tableLayout, emp.TotalStock1623);
                AddCellToBody(tableLayout, emp.d16);
                AddCellToBody(tableLayout, emp.d17);
                AddCellToBody(tableLayout, emp.d18);
                AddCellToBody(tableLayout, emp.d19);
                AddCellToBody(tableLayout, emp.d20);
                AddCellToBody(tableLayout, emp.d21);
                AddCellToBody(tableLayout, emp.d22);
                AddCellToBody(tableLayout, emp.d23);
                AddCellToBody(tableLayout, emp.TotalDCR1623);


                AddCellToBody(tableLayout, emp.GivenQty2431);
                AddCellToBody(tableLayout, emp.TotalStock2431);
                AddCellToBody(tableLayout, emp.d24);
                AddCellToBody(tableLayout, emp.d25);
                AddCellToBody(tableLayout, emp.d26);
                AddCellToBody(tableLayout, emp.d27);
                AddCellToBody(tableLayout, emp.d28);
                AddCellToBody(tableLayout, emp.d29);
                AddCellToBody(tableLayout, emp.d30);
                AddCellToBody(tableLayout, emp.d31);
                AddCellToBody(tableLayout, emp.TotalDCR2431);
                AddCellToBody(tableLayout, emp.TotalDCR0131);

                AddCellToBody(tableLayout, emp.ClosingStock);
            }

            var Data= primaryDAO.GetGrandTotalSum(model,dt);
           foreach (var emp in Data)
            {
                AddCellToFooterSum(tableLayout, "Grand Total:", 3);
                //AddCellToFooterSum(tableLayout, emp.ItemType);
                //AddCellToFooterSum(tableLayout, emp.MPGroup);
                AddCellToFooterSum(tableLayout, emp.RestQty,1);
                AddCellToFooterSum(tableLayout, emp.CentralQty, 1);
                AddCellToFooterSum(tableLayout, emp.TotalQtyAddDefi0130, 1);
                AddCellToFooterSum(tableLayout, emp.TotalQty, 1);

                AddCellToFooterSum(tableLayout, emp.GivenQty0108, 1);
                AddCellToFooterSum(tableLayout, emp.TotalStock0108, 1);
                AddCellToFooterSum(tableLayout, emp.d01, 1);
                AddCellToFooterSum(tableLayout, emp.d02, 1);
                AddCellToFooterSum(tableLayout, emp.d03, 1);
                AddCellToFooterSum(tableLayout, emp.d04, 1);
                AddCellToFooterSum(tableLayout, emp.d05, 1);
                AddCellToFooterSum(tableLayout, emp.d06, 1);
                AddCellToFooterSum(tableLayout, emp.d07, 1);
                AddCellToFooterSum(tableLayout, emp.d08, 1);
                AddCellToFooterSum(tableLayout, emp.TotalDCR0108, 1);

                AddCellToFooterSum(tableLayout, emp.GivenQty0915, 1);
                AddCellToFooterSum(tableLayout, emp.TotalStock0915, 1);
                AddCellToFooterSum(tableLayout, emp.d09, 1);
                AddCellToFooterSum(tableLayout, emp.d10, 1);
                AddCellToFooterSum(tableLayout, emp.d11, 1);
                AddCellToFooterSum(tableLayout, emp.d12, 1);
                AddCellToFooterSum(tableLayout, emp.d13, 1);
                AddCellToFooterSum(tableLayout, emp.d14, 1);
                AddCellToFooterSum(tableLayout, emp.d15, 1);
                AddCellToFooterSum(tableLayout, emp.TotalDCR0915,1);

                AddCellToFooterSum(tableLayout, emp.GivenQty1623, 1);
                AddCellToFooterSum(tableLayout, emp.TotalStock1623, 1);
                AddCellToFooterSum(tableLayout, emp.d16, 1);
                AddCellToFooterSum(tableLayout, emp.d17, 1);
                AddCellToFooterSum(tableLayout, emp.d18, 1);
                AddCellToFooterSum(tableLayout, emp.d19, 1);
                AddCellToFooterSum(tableLayout, emp.d20, 1);
                AddCellToFooterSum(tableLayout, emp.d21, 1);
                AddCellToFooterSum(tableLayout, emp.d22, 1);
                AddCellToFooterSum(tableLayout, emp.d23, 1);
                AddCellToFooterSum(tableLayout, emp.TotalDCR1623, 1);

                AddCellToFooterSum(tableLayout, emp.GivenQty2431, 1);
                AddCellToFooterSum(tableLayout, emp.TotalStock2431, 1);
                AddCellToFooterSum(tableLayout, emp.d24, 1);
                AddCellToFooterSum(tableLayout, emp.d25, 1);
                AddCellToFooterSum(tableLayout, emp.d26, 1);
                AddCellToFooterSum(tableLayout, emp.d27, 1);
                AddCellToFooterSum(tableLayout, emp.d28, 1);
                AddCellToFooterSum(tableLayout, emp.d29, 1);
                AddCellToFooterSum(tableLayout, emp.d30, 1);
                AddCellToFooterSum(tableLayout, emp.d31, 1);
                AddCellToFooterSum(tableLayout, emp.TotalDCR2431, 1);
                AddCellToFooterSum(tableLayout, emp.TotalDCR0131, 1);
                AddCellToFooterSum(tableLayout, emp.ClosingStock, 1);
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


        private static void AddCellToFooterSum(PdfPTable tableLayout, string cellText,int colSpan)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 5, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                Colspan=colSpan,
                HorizontalAlignment =Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }











	}
}
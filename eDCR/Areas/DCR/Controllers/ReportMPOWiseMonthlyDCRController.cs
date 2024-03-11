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
    public class ReportMPOWiseMonthlyDCRController : Controller
    {
        ReportMPOWiseMonthlyDCRDAO primaryDAO = new ReportMPOWiseMonthlyDCRDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        public ActionResult frmReportMPOWiseMonthlyDCR()
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

            string FileName = "MPO_Wise_Monthly_DCR_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "MPO Wise Monthly DCR";

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
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 11 columns 
                int PdfNoOfColumns = 37;
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
            document.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());


            //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width, iTextSharp.text.PageSize.LETTER.Height));



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



     
   protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportMPOWiseMonthlyDCRBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10,30, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10,10,10,10,10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Potential", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "01", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "02", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "03", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "04", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "05", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "06", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "07", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "08", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "09", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "10", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "11", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "12", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "13", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "14", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "15", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "16", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "17", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "18", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "19", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "20", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "21", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "22", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "23", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "24", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "25", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "26", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "27", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "28", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "29", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "30", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "31", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total DCR", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total PLAN", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Achi(%)", 0);

            ////Add body  

            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Potential);
                ExportToPDF.AddCellToBody(tableLayout, emp.d01);
                ExportToPDF.AddCellToBody(tableLayout, emp.d02);
                ExportToPDF.AddCellToBody(tableLayout, emp.d03);
                ExportToPDF.AddCellToBody(tableLayout, emp.d04);
                ExportToPDF.AddCellToBody(tableLayout, emp.d05);
                ExportToPDF.AddCellToBody(tableLayout, emp.d06);
                ExportToPDF.AddCellToBody(tableLayout, emp.d07);
                ExportToPDF.AddCellToBody(tableLayout, emp.d08);
                ExportToPDF.AddCellToBody(tableLayout, emp.d09);
                ExportToPDF.AddCellToBody(tableLayout, emp.d10);
                ExportToPDF.AddCellToBody(tableLayout, emp.d11);
                ExportToPDF.AddCellToBody(tableLayout, emp.d12);
                ExportToPDF.AddCellToBody(tableLayout, emp.d13);
                ExportToPDF.AddCellToBody(tableLayout, emp.d14);
                ExportToPDF.AddCellToBody(tableLayout, emp.d15);
                ExportToPDF.AddCellToBody(tableLayout, emp.d16);
                ExportToPDF.AddCellToBody(tableLayout, emp.d17);
                ExportToPDF.AddCellToBody(tableLayout, emp.d18);
                ExportToPDF.AddCellToBody(tableLayout, emp.d19);
                ExportToPDF.AddCellToBody(tableLayout, emp.d20);
                ExportToPDF.AddCellToBody(tableLayout, emp.d21);
                ExportToPDF.AddCellToBody(tableLayout, emp.d22);
                ExportToPDF.AddCellToBody(tableLayout, emp.d23);
                ExportToPDF.AddCellToBody(tableLayout, emp.d24);
                ExportToPDF.AddCellToBody(tableLayout, emp.d25);
                ExportToPDF.AddCellToBody(tableLayout, emp.d26);
                ExportToPDF.AddCellToBody(tableLayout, emp.d27);
                ExportToPDF.AddCellToBody(tableLayout, emp.d28);
                ExportToPDF.AddCellToBody(tableLayout, emp.d29);
                ExportToPDF.AddCellToBody(tableLayout, emp.d30);
                ExportToPDF.AddCellToBody(tableLayout, emp.d31);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalPLAN);
                ExportToPDF.AddCellToBody(tableLayout, emp.Achi);
            }

            return tableLayout;
        }

        // Method to add single cell to the Header 
     

	}
}
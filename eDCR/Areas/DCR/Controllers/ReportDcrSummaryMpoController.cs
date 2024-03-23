using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Areas.DCR.Models.BEL;
using iTextSharp.text.pdf;
using System.IO;

using System.Text;
using System.Data;
using iTextSharp.text;
using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportDcrSummaryMpoController : Controller
    {

        ReportDCRSummaryDAO primaryDAO = new ReportDCRSummaryDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDcrSummaryMpo()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetDataForDCRSummary(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDataForDCRSummary(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

       

        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "DCR_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "MIO DCR Summary";

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

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns
                int PdfNoOfColumns =21;
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



     protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDCRSummaryBEL> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 10, 15, 15, 15,  10, 10, 10, 10, 10,10,10,10, 10,10,10, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 5;
            //Add Title to the PDF file at the top  

            //List < Employee > employees = _context.employees.ToList < Employee > ();

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

      


            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date",0); 
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Selected Regular Qty", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Sample Regular Qty", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Sample Intern Qty", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Gift Regular Qty", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Gift Intern Qty", 90);

            ExportToPDF.AddCellToHeader(tableLayout, "Regular DOT", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Intern DOT", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Total DOT", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Morning WP", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening WP", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Total DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Morning DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Intern DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Other DCR", 90);

            ExportToPDF.AddCellToHeader(tableLayout, "Morning Absent", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening Absent", 90);
            ////Add body  

            foreach (var emp in item)
            {
                 ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                 ExportToPDF.AddCellToBody(tableLayout, emp.Date);
                 ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
   
                 ExportToPDF.AddCellToBody(tableLayout, emp.Accompany);
        
                 ExportToPDF.AddCellToBody(tableLayout, emp.SelectedRegularQty);
                 ExportToPDF.AddCellToBody(tableLayout, emp.SampleRegularQty);
                 ExportToPDF.AddCellToBody(tableLayout, emp.SampleInternQty);
                 ExportToPDF.AddCellToBody(tableLayout, emp.GiftRegularQty);               
                 ExportToPDF.AddCellToBody(tableLayout, emp.GiftInternQty);

                ExportToPDF.AddCellToBody(tableLayout, emp.RegularDOT);
                ExportToPDF.AddCellToBody(tableLayout, emp.InternDOT);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalDOT);
                ExportToPDF.AddCellToBody(tableLayout, emp.MorningWP);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningWP);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalDCR);             
                ExportToPDF.AddCellToBody(tableLayout, emp.MorningDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.InternDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.OtherDCR);

                ExportToPDF.AddCellToBody(tableLayout, emp.MorningAbsent);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningAbsent);

            }


            return tableLayout;
        }
      
    }
}
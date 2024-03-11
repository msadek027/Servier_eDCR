using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL;
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
    public class ReportRemarksController : Controller
    {
        ReportRemarksDAO primaryDAO = new ReportRemarksDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();

        public ActionResult frmReportRemarks()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" }); ;
        }

        public ActionResult GetMainGrid(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetMainGrid(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Remarks_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Remarks";
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



     protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportRemarksBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10, 15, 15, 15, 15, 15, 10, 30 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top 


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);




            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date",0);
            ExportToPDF.AddCellToHeader(tableLayout, "MPO Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Pack Size", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Item Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Quantity", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Remarks", 0);

            ////Add body  

            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.FromDate);
                ExportToPDF.AddCellToBody(tableLayout, emp.MPOName);
                ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
                ExportToPDF.AddCellToBody(tableLayout, emp.PackSize);
                ExportToPDF.AddCellToBody(tableLayout, emp.ItemType);
                ExportToPDF.AddCellToBody(tableLayout, emp.Quantity);
                ExportToPDF.AddCellToBody(tableLayout, emp.Remark);
           

            }


            return tableLayout;
        }
      
	}
}
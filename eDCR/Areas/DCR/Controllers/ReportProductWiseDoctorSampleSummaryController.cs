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
    public class ReportProductWiseDoctorSampleSummaryController : Controller
    {
        ReportProductWiseDoctorSampleSummaryDAO primaryDAO = new ReportProductWiseDoctorSampleSummaryDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportProductWiseDoctorSampleSummary()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetGridData(DefaultParameterBEO model)
        {
            var dataList = primaryDAO.GetGridData(model);
            var data = Json(dataList, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }



        public ActionResult Export(DefaultParameterBEO model)
        {

            string vHeader = "Product Wise Doctor Sample Summary";
            string FileName = "Product_Wise_Doctor_Sample_Summary_" + DateTime.Now.ToString("yyyyMMdd");

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
                int PdfNoOfColumns = 11;
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




        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportProductWiseDoctorSampleSummaryBEO> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 20, 20, 20,12, 15, 15, 15, 15, 15, 15, 15 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
       
            ExportToPDF.AddCellToHeader(tableLayout, "MPO Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Speciality", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Sample Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Selected Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Gift Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "PPM Qty", 0);
            ////Add body  

            foreach (var emp in item)
            {

               ExportToPDF.AddCellToBody(tableLayout, emp.MPOName);
               ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.FromDate);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);
               ExportToPDF.AddCellToBody(tableLayout, emp.Speciality);
               ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
               ExportToPDF.AddCellToBody(tableLayout, emp.SampleQty);
               ExportToPDF.AddCellToBody(tableLayout, emp.SelectedQty);
               ExportToPDF.AddCellToBody(tableLayout, emp.GiftQty);
               ExportToPDF.AddCellToBody(tableLayout, emp.PPMQty);


            }


            return tableLayout;
        }
       
	}
}
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
    public class ReportDCRDoctorAbsentController : Controller
    {
        ReportDCRDoctorAbsentDAO primaryDAO = new ReportDCRDoctorAbsentDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        // GET: /DCR/ReportDCRDoctorAbsent/
        public ActionResult frmReportDCRDoctorAbsent()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }


        public ActionResult GetAbsentSummary(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetAbsentSummary(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAbsentDetail(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetAbsentDetail(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        public ActionResult Export(DefaultParameterBEO model)
        {
            string vHeader = "Doctor Absent";
            string FileName = "Doctor_Absent_" + DateTime.Now.ToString("yyyyMMdd");

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
                int PdfNoOfColumns = 13;
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



      
      protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDCRDoctorAbsentBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20, 20, 15, 20, 20, 10, 30, 20, 10,10,10,10,10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  

            AddCellToHeader(tableLayout, "Region Name");
            AddCellToHeader(tableLayout, "Territory Name");
            AddCellToHeader(tableLayout, "MPO Code");
            AddCellToHeader(tableLayout, "MPO Name");
            AddCellToHeader(tableLayout, "Market Name");
            AddCellToHeader(tableLayout, "Doctor ID");
            AddCellToHeader(tableLayout, "Doctor Name");
            AddCellToHeader(tableLayout, "Degree");
            AddCellToHeader(tableLayout, "Speciality");
            AddCellToHeader(tableLayout, "Shift");
            AddCellToHeader(tableLayout, "Absent");
            AddCellToHeader(tableLayout, "NotAllowed");
            AddCellToHeader(tableLayout, "Missed");

            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.RegionName);
                AddCellToBody(tableLayout, emp.TerritoryName);
                AddCellToBody(tableLayout, emp.MPOCode);
                AddCellToBody(tableLayout, emp.MPOName);
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.DoctorID);
                AddCellToBody(tableLayout, emp.DoctorName);
                AddCellToBody(tableLayout, emp.Degrees);
                AddCellToBody(tableLayout, emp.Speciality);
                AddCellToBody(tableLayout, emp.ShiftName);
                AddCellToBody(tableLayout, emp.Absent);
                AddCellToBody(tableLayout, emp.NotAllowed);
                AddCellToBody(tableLayout, emp.Missed);

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
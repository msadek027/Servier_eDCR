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
    public class ReportDcrSummaryTmRsmController : Controller
    {
        ReportDCRSummaryDAO primaryDAO = new ReportDCRSummaryDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDcrSummaryTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetDcrSummaryTmRsm(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDcrSummaryTmRsm(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "TmRsm_Dcr_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "TM RSM DCR Summary";

            var tuple = primaryDAO.ExportTmRsm(model);
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
                //Create PDF Table with 14 columns
                int PdfNoOfColumns = 16;
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



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDcrSummaryTmRsm> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 10, 15, 15, 15, 20, 10, 10, 10, 10, 10, 10, 10, 10,10,10,30 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 5;
            //Add Title to the PDF file at the top  

            //List < Employee > employees = _context.employees.ToList < Employee > ();

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);




            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Emp. ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Name", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Territory/Region", 90);   
  

            ExportToPDF.AddCellToHeader(tableLayout, "Total DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Double Call", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Single Call", 90);

            ExportToPDF.AddCellToHeader(tableLayout, "Morning DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "New Doctor DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Intern Doctor DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "No. Of Intern Doctor Visited", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Other DCR", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "No. of Chemist", 90);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 90);
            ////Add body  

            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.Date);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeID);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
           

               
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoubleCall);
                ExportToPDF.AddCellToBody(tableLayout, emp.SingleCall);
                ExportToPDF.AddCellToBody(tableLayout, emp.MorningDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.NewDoctorDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.InternDoctorDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.NoOfInternDoctorVisited);
                ExportToPDF.AddCellToBody(tableLayout, emp.OtherDCR);
                ExportToPDF.AddCellToBody(tableLayout, emp.NoOfChemist);
                ExportToPDF.AddCellToBody(tableLayout, emp.Accompany);


            }


            return tableLayout;
        }

    }
}
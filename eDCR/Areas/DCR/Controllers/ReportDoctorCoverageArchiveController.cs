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
    public class ReportDoctorCoverageArchiveController : Controller
    {
        // GET: DCR/ReportDoctorCoverageArchive
        ReportDoctorCoverageDAO primaryDAO = new ReportDoctorCoverageDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDoctorCoverageArchive()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetDoctorCoverage(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDoctorCoverageArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export(DefaultParameterBEO model)
        {
            string FileName = "Doctor_Coverage_Archive_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Doctor Coverage Archive";
            var tuple = primaryDAO.ExportArchive(model);
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

                int PdfNoOfColumns = 17;
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


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDoctorCoverageBEL> item, string vHeaderSubject, string vHeaderParameter)
        {
            float[] headers = { 15, 10, 15, 10, 10, 10, 10, 10, 10, 10, 10,10,10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row

            //tableLayout Title to the PDF file at the top        
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);


            //int vHeaderLeft = 0; int vHeaderMiddle = 0; int vHeaderRight = 0;
            //if (PdfNoOfHeaders == PdfNoOfColumns)
            //{
            //    vHeaderLeft = (int)Math.Round(1 * PdfNoOfColumns / 3.8);
            //    vHeaderMiddle = (int)Math.Round(1.5 * PdfNoOfColumns / 3.8);
            //    vHeaderRight = (int)Math.Round(1.3 * (PdfNoOfColumns / 3.8));
            //}

            //tableLayout.AddCell(new PdfPCell(new Phrase(primaryDAO.vCompanyName + "\n", new Font(Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = PdfNoOfColumns, Border = 0, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderLeft, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase(vHeader, new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderMiddle, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase(primaryDAO.vPrintDate + "\n", new Font(Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderRight, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });
            //tableLayout.AddCell(new PdfPCell(new Phrase(vParameter, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
            //{
            //    Colspan = PdfNoOfColumns,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //});


            ////Add header  
            AddCellToHeader(tableLayout, "Market Name", 1);
            AddCellToHeader(tableLayout, "MPO ID", 1);
            AddCellToHeader(tableLayout, "MPO Name", 1);
            AddCellToHeader(tableLayout, "Total No. of Doctor", 1);
            AddCellToHeader(tableLayout, "Total No. of DOT Plan Doctor", 1);
            AddCellToHeader(tableLayout, "Doctor Planned(%)", 1);
            AddCellToHeader(tableLayout, "Total No. of Visited Doctor", 1);
            AddCellToHeader(tableLayout, "Doctor Coverage (%)", 1);
            AddCellToHeader(tableLayout, "Total No. of DOT Plan", 1);
            AddCellToHeader(tableLayout, "Total Execution of DOT Plan", 1);
            AddCellToHeader(tableLayout, "Total Execution of DOT Doctor Without DOT Plan", 1);
            AddCellToHeader(tableLayout, "Plan Vs. Execution (%)", 1);
            AddCellToHeader(tableLayout, "Total Execution of Non-DOT Doctor", 1);          
            AddCellToHeader(tableLayout, "Total No. of Non-DOT Doctor Visitied", 1);
            AddCellToHeader(tableLayout, "Total Execution of New Doctor", 1);
            AddCellToHeader(tableLayout, "Total No. of New Doctor Visited", 1);
            AddCellToHeader(tableLayout, "Total Execution of Intern Doctor", 1); 

            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.MPOID);
                AddCellToBody(tableLayout, emp.MPOName);
                AddCellToBody(tableLayout, emp.TotalNoOfDoctor);
                AddCellToBody(tableLayout, emp.TotalNoOfDOTPlanDoctor);

                AddCellToBody(tableLayout, emp.DoctorPlannedPer);
                AddCellToBody(tableLayout, emp.TotalNoOfVisitedDoctor);
                AddCellToBody(tableLayout, emp.DoctorCoveragePer);

                AddCellToBody(tableLayout, emp.TotalNoOfDOTPlan);
                AddCellToBody(tableLayout, emp.TotalExecutionOfDOTPlan);
                AddCellToBody(tableLayout, emp.TotalExecutionOfDOTDoctorWithoutDOTPlan);

                AddCellToBody(tableLayout, emp.PlanVsExecutionPer);
                //AddCellToBody(tableLayout, emp.TotalExecutionOfNonDOTDoctor);
                //AddCellToBody(tableLayout, emp.TotalNoOfNonDOTDoctorVisited);
                AddCellToBody(tableLayout, emp.TotalExecutionOfNewDoctor);

                AddCellToBody(tableLayout, emp.TotalNoOfNewDoctorVisited);
                AddCellToBody(tableLayout, emp.TotalExecutionOfInternDoctor);

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
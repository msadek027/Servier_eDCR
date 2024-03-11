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
    public class ReportDcrTmRsmController : Controller
    {
        ReportSupervisorDCRDAO primaryDAO=new  ReportSupervisorDCRDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
   
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        public ActionResult frmReportDcrTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetViewData(DefaultParameterBEO model)
        {
            //var data = primaryDAO.GetViewData(model);
            //return Json(data, JsonRequestBehavior.AllowGet);

            var listData = primaryDAO.GetViewData(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Supervisor_DCR_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Supervisor DCR";
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

                //file name to be created   
                string strPDFFileName = string.Format(FileName + "_Pdf.pdf");
                Document doc = new Document(PageSize.LETTER);
                //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 20, 10);
                //doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 14;
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


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportSupervisorDCRBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = {20, 10, 15, 25,10, 10, 10, 10, 10,10,10,10,10, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row
            //tableLayout Title to the PDF file at the top 
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);
            //tableLayout.AddCell(new PdfPCell(new Phrase(reportHeader.vCompanyName + "\n", new Font(Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 14, Border = 0, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 4, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase(vHeader, new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 5, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
            //tableLayout.AddCell(new PdfPCell(new Phrase(reportHeader.vPrintDate + "\n", new Font(Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = 5, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });


            //tableLayout.AddCell(new PdfPCell(new Phrase(vParameter, new Font(Font.FontFamily.TIMES_ROMAN, 6, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
            //{
            //    Colspan = 14,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_LEFT
            //});


            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Degree", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Shift", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Call Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Location Followed", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "DCR Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DCR Sub Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address Word", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "No Of Interns", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Remarks", 0);
       

            ////Add body  

            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Date);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Degree);
                ExportToPDF.AddCellToBody(tableLayout, emp.ShiftName);
                ExportToPDF.AddCellToBody(tableLayout, emp.CallType);
                ExportToPDF.AddCellToBody(tableLayout, emp.Accompany);
                ExportToPDF.AddCellToBody(tableLayout, emp.LocationFollowed);

                ExportToPDF.AddCellToBody(tableLayout, emp.DCRType);
                ExportToPDF.AddCellToBody(tableLayout, emp.DCRSubType);
                ExportToPDF.AddCellToBody(tableLayout, emp.AddressWord);
                ExportToPDF.AddCellToBody(tableLayout, emp.NoOfInterns);
                ExportToPDF.AddCellToBody(tableLayout, emp.Remarks);       


            }

            return tableLayout;
        }

       
	}
}
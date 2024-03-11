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
    public class ReportTpLeaveMpoTmRsmController : Controller
    {
        ReportMPOLeaveStatementDAO primaryDAO = new ReportMPOLeaveStatementDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportTpLeaveMpoTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetMainGrid(DefaultParameterBEO model)
        {
            var listData = primaryDAO.GetMainData(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;   

        }

        [HttpPost]
        public ActionResult GetLeaveDetailLink(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetLeaveDetailLink(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "TP_Leave_Monthly_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "TP Leave Statement";
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
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);
                int PdfNoOfColumns = 10;
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



                return File(workStream, "application/pdf", strPDFFileName);
            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }





        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportMPOLeaveStatementBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20, 30, 20, 20, 30, 30, 30, 20, 20, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top 
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);


     

            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Code", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Mobile No", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Territory", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Region", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Morning Leave", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening Leave", 0);        
            ExportToPDF.AddCellToHeader(tableLayout, "Full Leave", 0);
            ////Add body  

            foreach (var emp in item)
            {

                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeCode);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Designation);

                ExportToPDF.AddCellToBody(tableLayout, emp.MobileNo);
                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.TerritoryName);

                ExportToPDF.AddCellToBody(tableLayout, emp.RegionName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MorningLeave);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningLeave);
                ExportToPDF.AddCellToBody(tableLayout, emp.FullLeave);
           


            }

            return tableLayout;
        }



    }
}
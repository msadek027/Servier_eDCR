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
    public class ReportTourPlanMpoTmRsmArchiveController : Controller
    {
        // GET: DCR/ReportTourPlanViewMpoTmRsmArchive
        ExportToAnother export = new ExportToAnother();
        ReportTourPlanSubDAO primaryDAO = new ReportTourPlanSubDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();


        public ActionResult frmReportTourPlanMpoTmRsmArchive()
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
            var data = primaryDAO.GetViewDataArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "TP_Monthly_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "TP Monthly";

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

                Document doc = new Document();
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);
                int PdfNoOfColumns = 11;
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





        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportTourPlanSupBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20, 30, 30, 20, 30, 20, 30, 30, 20, 30, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top 
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header Main 
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "MORNING", 5);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "EVENING", 5);

            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Day", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Location", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "R.Time", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Location", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "R.Time", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);
            ////Add body  

            foreach (var emp in item)
            {

                ExportToPDF.AddCellToBody(tableLayout, emp.DayNumber);

                ExportToPDF.AddCellToBody(tableLayout, emp.MInstName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MMeetingPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.MSetTime);
                ExportToPDF.AddCellToBody(tableLayout, emp.MAccompany);
                ExportToPDF.AddCellToBody(tableLayout, emp.MAllowence);

                ExportToPDF.AddCellToBody(tableLayout, emp.EInstName);
                ExportToPDF.AddCellToBody(tableLayout, emp.EMeetingPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.ESetTime);
                ExportToPDF.AddCellToBody(tableLayout, emp.EAccompany);
                ExportToPDF.AddCellToBody(tableLayout, emp.EAllowence);


            }

            return tableLayout;
        }




    }
}
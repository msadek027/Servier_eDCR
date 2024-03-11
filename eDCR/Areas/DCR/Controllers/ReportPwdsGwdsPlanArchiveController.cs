using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Universal.Common;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
namespace eDCR.Areas.DCR.Controllers
{
    public class ReportPwdsGwdsPlanArchiveController : Controller
    {
        // GET: DCR/ReportSelectedProductPlanArchive
        ReportSelectedItemPlanDAO primaryDAO = new ReportSelectedItemPlanDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();

        public string ReportSubject = "";
        //
        // GET: /DCR/ReportSelectedItemPlan/
        public ActionResult frmReportPwdsGwdsPlanArchive()
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
            var data = primaryDAO.GetGridDataArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(DefaultParameterBEO model)
        {
            ReportSubject = model.ItemType == "SlR" ? "Product Wise Doctor Selection (PWDS)" : "Gift Wise Doctor Selection (GWDS)";

            string vHeader = ReportSubject;
            string FileName = "PWDS_GWDS_Plan" + DateTime.Now.ToString("yyyyMMdd");

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

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 10;
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



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportSelectedItemPlanBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20, 20, 15, 20, 15, 15, 15, 15, 15, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "MPO Name", 0);
          
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "CarryIn Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Receive Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Add/Defi Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total Qty", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Balance Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);

            ////Add body  

            foreach (var emp in item)
            {

                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MPOName);
           
                ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
                ExportToPDF.AddCellToBody(tableLayout, emp.RestQty);

                ExportToPDF.AddCellToBody(tableLayout, emp.CentralQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalQty);

                ExportToPDF.AddCellToBody(tableLayout, emp.VariableQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.BalanceQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);

            }


            return tableLayout;
        }
        // Method to add single cell to the Header  










    }
}
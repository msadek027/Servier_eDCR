using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.DCR.Models.DAL.DAO;
using iTextSharp.text.pdf;
using eDCR.Areas.DCR.Models.BEL;
using System.Data;
using System.IO;
using System.Text;
using iTextSharp.text;
using eDCR.Universal.Common;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportDcrAccompanyMpoTmRsmController : Controller
    {
        ReportAccompanyDAO primaryDAO = new ReportAccompanyDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDcrAccompanyMpoTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetDataMainView(DefaultParameterBEO model)
        {
            var listData = primaryDAO.GetDataMainView(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

           
        }

        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "DCR_Accompany_MpoTmRsm" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "DCR Accompany";
            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3; 

            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcelWithTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);


            }
            else if (model.ExportType == "PDF" && dt.Rows.Count > 0)
            {
                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                //file name to be created   
                string strPDFFileName = string.Format(FileName + ".pdf");
                Document doc = new Document();

          
                //Create PDF Table with 11 columns 
                int PdfNoOfColumns = 12;
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



     protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportAccompanyBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10,15,30,20, 20, 15,15, 15,15,20,20,20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL",0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Code", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Morning", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Evening", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Territory Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Region Name", 0);

            ////Add body  
            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeCode);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Designation);
                ExportToPDF.AddCellToBody(tableLayout, emp.SetDate);
                ExportToPDF.AddCellToBody(tableLayout, emp.Accompany);
                ExportToPDF.AddCellToBody(tableLayout, emp.Morning.ToString());
                ExportToPDF.AddCellToBody(tableLayout, emp.Evening.ToString());
                ExportToPDF.AddCellToBody(tableLayout, emp.Total.ToString());
                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.TerritoryName);
                ExportToPDF.AddCellToBody(tableLayout, emp.RegionName);
            }


            return tableLayout;
        }
        // Method to add single cell to the Header  
    
    }
}
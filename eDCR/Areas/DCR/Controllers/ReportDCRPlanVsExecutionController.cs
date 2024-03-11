using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Areas.DCR.Models.BEL;
using ClosedXML.Excel;
using System.IO;
using System.Data;
using eDCR.Universal.Common;
using iTextSharp.text.pdf;
using System.Text;
using iTextSharp.text;
using System.Globalization;
using iTextSharp.text.pdf.draw;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportDCRPlanVsExecutionController : Controller
    {
        ReportDCRPlanExecutionDAO primaryDAO = new ReportDCRPlanExecutionDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();


        public ActionResult frmReportDCRPlanVsExecution()
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
            var listData = primaryDAO.GetMainGrid(model); 
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;


   
        }


        public ActionResult Export(DefaultParameterBEO model)
        {
           
            string FileName = "Plan_Vs_Execution_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Plan Vs Execution";

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            GC.Collect();



            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
              //  export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                

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
                int PdfNoOfColumns = 15;
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



       protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDCRPlanExecutionBEL> item, string vHeaderSubject, string vHeaderParameter)
        {
            float[] headers = { 10,20, 15,10, 20, 20, 20, 20, 20, 20, 20, 10,15, 15, 15 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            //Add Title to the PDF file at the top 
            //List < Employee > employees = _context.employees.ToList < Employee > ();    
            //tableLayout Title to the PDF file at the top 

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "PLAN", 3);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "DCR", 3);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ////Add header 
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name",0);
            ExportToPDF.AddCellToHeader(tableLayout, "Date",0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Plan Selected", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Plan Sample", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Plan Gift", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DCR Selected", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DCR Sample", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DCR Gift", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Shift", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Accompany", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DCR Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Remarks", 0);
            ////Add body  

            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Date);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);
                ExportToPDF.AddCellToBody(tableLayout, emp.PlanSelected);
                ExportToPDF.AddCellToBody(tableLayout, emp.PlanSample);
                ExportToPDF.AddCellToBody(tableLayout, emp.PlanGift);
                ExportToPDF.AddCellToBody(tableLayout, emp.DCRSelected);
                ExportToPDF.AddCellToBody(tableLayout, emp.DCRSample);
                ExportToPDF.AddCellToBody(tableLayout, emp.DCRGift);
                ExportToPDF.AddCellToBody(tableLayout, emp.ShiftName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Accompany);
                ExportToPDF.AddCellToBody(tableLayout, emp.DcrType);
                ExportToPDF.AddCellToBody(tableLayout, emp.Remarks);

            }


            return tableLayout;
        }
       



   

   
    }
}
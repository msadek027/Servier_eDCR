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
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ExceptionHandler = eDCR.DAL.Gateway.ExceptionHandler;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportExpenseBillMpoTmRsmDepotController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();




        DefaultExpenseBillMpoTmRsmDAO primaryDAO = new DefaultExpenseBillMpoTmRsmDAO();
        public ActionResult frmReportExpenseBillMpoTmRsmDepot()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmReportExpenseBillMpoTmRsmDepotDetail()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult OperationsModeSummary(ExpenseBillMpoTmRsmInsertBEO model)
        {
            try
            {
                if (primaryDAO.SaveUpdateSummary(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });

                }
                else
                    return View("frmImportExcelToGrid");
        }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
}
        [HttpPost]
        public ActionResult GetSummaryData(DefaultParameterBEO model)
        {

            var data = primaryDAO.GetViewExpenseBillSummaryMpoTmRsmDepotCurrentMpoWise2(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDepotDetailData(DefaultParameterBEO model)
        {
           // var data = primaryDAO.GetViewExpenseBillDepotDetailMpoTmRsm(model);
            var data = primaryDAO.GetViewExpenseBillDetailMpoTmRsm(model);            
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Expense_Bill_Summary_MpoTmRsm_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Expense Bill Summary";

            var tuple = primaryDAO.GetExportExpenseBillSummaryMpoTmRsmDepotCurrentMpoWise2(model);
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
                string strPDFFileName = string.Format(FileName + ".pdf");
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


                return File(workStream, "application/pdf", strPDFFileName);
            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }


        public ActionResult AllDepotDataToExcelExport(DefaultParameterBEO model)
        {

            string FileName = "Expense_Bill_Summary_MpoTmRsm_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Expense Bill Summary";

            var tuple = primaryDAO.GetAllDepotDataToExcelExport(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }

            protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ExpenseBillMpoTmRsmBEO> item, string vHeaderSubject, string vHeaderParameter)
        {
            float[] headers = { 10, 15, 30, 18, 20, 20, 20,20, 15, 15, 15, 10, 10,10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 4; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            //tableLayout Title to the PDF file at the top 

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Code", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Employee Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Territory Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Region Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Depot Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Daily Allowance", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Travel Allowance", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Waiting Total", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Approve Total(DA & TA)", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Misc/Manual TA DA Bill", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Grand Total", 0);

            //ExportToPDF.AddCellToHeader(tableLayout, "Link", 0);
            ////Add body 
            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.SL);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeCode);
                ExportToPDF.AddCellToBody(tableLayout, emp.EmployeeName);
                ExportToPDF.AddCellToBody(tableLayout, emp.Designation);

                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.TerritoryName);
                ExportToPDF.AddCellToBody(tableLayout, emp.RegionName);
                ExportToPDF.AddCellToBody(tableLayout, emp.DepotName);
                ExportToPDF.AddCellToBody(tableLayout, emp.DA);
                ExportToPDF.AddCellToBody(tableLayout, emp.TA);
                ExportToPDF.AddCellToBody(tableLayout, emp.WaitingTotal);
                ExportToPDF.AddCellToBody(tableLayout, emp.ApproveTotal);
                ExportToPDF.AddCellToBody(tableLayout, emp.MiscManualTADABill);
                ExportToPDF.AddCellToBody(tableLayout, emp.GrandTotal);



            }
            if (item.Count == 1)
            {
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");

            }

            return tableLayout;
        }
        // Method to add single cell to the Header  

























        public ActionResult ExportDetail(DefaultParameterBEO model)
        {

            string FileName = "ExpenseBillDetailMpoTmRsm" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Expense Bill Detail";

            var tuple = primaryDAO.GetExportExpenseBillDepotDetailMpoTmRsm(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;



            //string vHeaderParameter = vHeader + "\n " + vParameter;
            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                //dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, vHeaderParameter, FileName);

                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }
            else if (model.ExportType == "PDF" && dt.Rows.Count > 0)
            {

                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                //file name to be created   
                string strPDFFileName = string.Format(FileName + ".pdf");
                Document doc = new Document(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);


                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 12;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);


                //Create PDF Table  

                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);




                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                //Add Content to PDF   
                // doc.Add(Add_Content_To_PDF_DTL(tableLayout, dt, dataList, vHeaderParameter, model));
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


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ExpenseBillMpoTmRsmDetail> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 10, 30, 30, 9, 10, 10, 10, 15, 15, 15, 15, 15 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
                                               //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 3; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);


            //tableLayout Title to the PDF file at the top 
            //tableLayout.AddCell(new PdfPCell(new Phrase(vHeaderParameter, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
            //{
            //    Colspan = 14,
            //    Border = 0,
            //    PaddingBottom = 5,
            //    HorizontalAlignment = Element.ALIGN_CENTER,
            //});



            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "MORNING", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "EVENING", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);

            ExportToPDF.AddCellToHeaderMain(tableLayout, "EXPENSE BILL", 3);

            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ////Add header 
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "PLACE OF WORK", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "PLACE OF WORK", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "DA", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "TA", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Total", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "User Remarks", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Is Holiday", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Review Status", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Approve Status", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Supervisor Remarks", 0);


            ////Add body 
            foreach (var emp in item)
            {
                ExportToPDF.AddCellToBody(tableLayout, emp.DayNumber);
                ExportToPDF.AddCellToBody(tableLayout, emp.MorningPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.EveningPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.AllowanceNature);

                ExportToPDF.AddCellToBody(tableLayout, emp.DA);
                ExportToPDF.AddCellToBody(tableLayout, emp.TA);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalAmount);


                ExportToPDF.AddCellToBody(tableLayout, emp.UserRemarks);
                ExportToPDF.AddCellToBody(tableLayout, emp.IsHoliday);
                ExportToPDF.AddCellToBody(tableLayout, emp.ReviewStatus);
                ExportToPDF.AddCellToBody(tableLayout, emp.ApproveStatus);
                ExportToPDF.AddCellToBody(tableLayout, emp.Recommend);

            }
            if (item.Count == 1)
            {
             
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
                ExportToPDF.AddCellToBody(tableLayout, "");
            }

            return tableLayout;
        }
        // Method to add single cell to the Header  


    }
}
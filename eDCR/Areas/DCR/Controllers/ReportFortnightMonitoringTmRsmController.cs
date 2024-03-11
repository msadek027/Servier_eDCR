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
    public class ReportFortnightMonitoringTmRsmController : Controller
    {
        ReportFortnightMonitoringRemarksSupDAO primaryDAO = new ReportFortnightMonitoringRemarksSupDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportFortnightMonitoringTmRsm()
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
            var data = primaryDAO.GetViewData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetViewDataMonitoring(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetViewDataMonitoring(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetViewDataOverall(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetViewDataOverallPer(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetViewDataHistory(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetViewDataHistory(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetViewDataSupervisorComments(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetViewDataSupervisorComments(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Fortnight_" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Fortnight Monthly";

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;


    
            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                DataSet ds = new DataSet();

                dt = ExportToAnother.CreateDataTable(dataList);            
                //Add New Code              

                ds.Tables.Add(dt);               

                DataTable dtHistory = primaryDAO.dtGetViewDataHistory(model);

                if (model.Designation == "TM")
                {
                    var DataMonitoring = primaryDAO.GetViewDataMonitoring(model);
                    var dt2 = ExportToAnother.CreateDataTable(DataMonitoring);
                    var DataOverallPer = primaryDAO.GetViewDataOverallPer(model);
                    var dt3 = ExportToAnother.CreateDataTable(DataOverallPer);

                    var VisitObjectives = primaryDAO.GetViewDataHistory(dtHistory, "VisitObjectives", model.Designation);
                    var dt4 = ExportToAnother.CreateDataTable(VisitObjectives);
                    var RootCause = primaryDAO.GetViewDataHistory(dtHistory, "RootCause", model.Designation);
                    var dt5 = ExportToAnother.CreateDataTable(RootCause);
                    var ImprovementPlan = primaryDAO.GetViewDataHistory(dtHistory, "ImprovementPlan", model.Designation);
                    var dt6 = ExportToAnother.CreateDataTable(ImprovementPlan);
                    var Feedback = primaryDAO.GetViewDataHistory(dtHistory, "Feedback", model.Designation);
                    var dt7 = ExportToAnother.CreateDataTable(Feedback);

                    ds.Tables.Add(dt2);
                    ds.Tables.Add(dt3);
                    ds.Tables.Add(dt4);
                    ds.Tables.Add(dt5);
                    ds.Tables.Add(dt6);
                    ds.Tables.Add(dt7);
                }
                if (model.Designation == "RSM" || model.Designation == "DSM")
                {
                    var VisitObjectives = primaryDAO.GetViewDataHistory(dtHistory, "VisitObjectives", model.Designation);
                    var dt4 = ExportToAnother.CreateDataTable(VisitObjectives);
                    var RootCause = primaryDAO.GetViewDataHistory(dtHistory, "RootCause", model.Designation);
                    var dt5 = ExportToAnother.CreateDataTable(RootCause);
                    var ImprovementPlan = primaryDAO.GetViewDataHistory(dtHistory, "ImprovementPlan", model.Designation);
                    var dt6 = ExportToAnother.CreateDataTable(ImprovementPlan);
                    var Feedback = primaryDAO.GetViewDataHistory(dtHistory, "Feedback", model.Designation);
                    var dt7 = ExportToAnother.CreateDataTable(Feedback);


                    ds.Tables.Add(dt4);
                    ds.Tables.Add(dt5);
                    ds.Tables.Add(dt6);
                    ds.Tables.Add(dt7);
                }
                var DataSupervisorComment = primaryDAO.GetViewDataSupervisorComments(model);
                var dt8 = ExportToAnother.CreateDataTable(DataSupervisorComment);
                ds.Tables.Add(dt8);

                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
               
                export.ExportToExcelDataSetOld(ds, reportHeader.vCompanyName, vHeader, vParameter, FileName);

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
                int PdfNoOfColumns = 12;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table  

                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);




                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF(tableLayout, PdfNoOfColumns, dt, model, dataList, vHeader, vParameter));


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

       



     
   protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, DefaultParameterBEO model, List<ReportFortnightMonitoringRemarksSupBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 3; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);


            ////Add header Main 
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "Worked In", 2);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "Worked With", 2);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "Visited no of Doctors", 5);
            ////Add header  

            ExportToPDF.AddCellToHeader(tableLayout, "Data", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Work Place", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "TP Followed", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Name of Colleague", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Shift", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Double Morning", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Double Evening", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Single Morning", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Single Evening", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Chemist", 0);

            ////Add body  

            foreach (var emp in item)
            {


                ExportToPDF.AddCellToBody(tableLayout, emp.SetDate);
                ExportToPDF.AddCellToBody(tableLayout, emp.LocName);
                ExportToPDF.AddCellToBody(tableLayout, emp.InstName);
                ExportToPDF.AddCellToBody(tableLayout, emp.AllownceNature);

                ExportToPDF.AddCellToBody(tableLayout, emp.Review);
                ExportToPDF.AddCellToBody(tableLayout, emp.ColleagueName);
                ExportToPDF.AddCellToBody(tableLayout, emp.ShiftName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MDouble);
                ExportToPDF.AddCellToBody(tableLayout, emp.EDouble);
                ExportToPDF.AddCellToBody(tableLayout, emp.MSingle);
                ExportToPDF.AddCellToBody(tableLayout, emp.ESingle);
                ExportToPDF.AddCellToBody(tableLayout, emp.ChemistCount);

            }
            AddCellToFooterSum(tableLayout, "Audit as per monitoring tool", 12);

            AddCellToFooterSum(tableLayout, "Sent Date", 1);
            AddCellToFooterSum(tableLayout, "Market", 1);
            AddCellToFooterSum(tableLayout, "Employee Name", 1);
            AddCellToFooterSum(tableLayout, "Designation", 1);
            AddCellToFooterSum(tableLayout, "Paper Audit", 3);
            AddCellToFooterSum(tableLayout, "Problems Identifed", 3);
            AddCellToFooterSum(tableLayout, "Remarks", 2);    
            var DataMonitoring = primaryDAO.GetViewDataMonitoring(model);
            foreach (var emp in DataMonitoring)
            {
                AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                AddCellToFooterSum(tableLayout, emp.LocName, 1);
                AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                AddCellToFooterSum(tableLayout, emp.Designation, 1);
                AddCellToFooterSum(tableLayout, emp.PaperAudit, 3);
                AddCellToFooterSum(tableLayout, emp.ProblemsIdentifed, 3);
                AddCellToFooterSum(tableLayout, emp.Remarks, 2);
            }

            AddCellToFooterSum(tableLayout, "Overall performance", 12);
            AddCellToFooterSum(tableLayout, "Sent Date", 1);
            AddCellToFooterSum(tableLayout, "Market", 1);
            AddCellToFooterSum(tableLayout, "Employee Name", 1);
            AddCellToFooterSum(tableLayout, "Designation", 1);
            AddCellToFooterSum(tableLayout, "Leadership", 2);
            AddCellToFooterSum(tableLayout, "MarketShare", 2);
            AddCellToFooterSum(tableLayout, "SalesAchi", 2);
            AddCellToFooterSum(tableLayout, "Remarks", 2);
            var DataOverAll = primaryDAO.GetViewDataOverallPer(model);
            foreach (var emp in DataOverAll)
            {
                AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                AddCellToFooterSum(tableLayout, emp.LocName, 1);
                AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                AddCellToFooterSum(tableLayout, emp.Designation, 1);
                AddCellToFooterSum(tableLayout, emp.Leadership, 2);
                AddCellToFooterSum(tableLayout, emp.MarketShare, 2);
                AddCellToFooterSum(tableLayout, emp.SalesAchi, 2);
                AddCellToFooterSum(tableLayout, emp.Remarks, 2);
            }
            //-------------------------------------------------
            
            if (model.Designation == "TM")
            {                
              //Visit Objects(Specific), 
             //Root cause of weak performance, 
             //Improvement plan with SMART approach 
             //and Feedback on previous instruction

                AddCellToFooterSum(tableLayout, "Visit Objects(Specific):", 12);
                AddCellToFooterSum(tableLayout, "Sent Date", 1);
                AddCellToFooterSum(tableLayout, "Market", 1);
                AddCellToFooterSum(tableLayout, "Employee Name", 1);
                AddCellToFooterSum(tableLayout, "Designation", 1);
                AddCellToFooterSum(tableLayout, "Remarks", 8);

                DataTable dtHistory = primaryDAO.dtGetViewDataHistory(model);
                var VisitObjectives = primaryDAO.GetViewDataHistory(dtHistory, "VisitObjectives",model.Designation);
                foreach (var emp in VisitObjectives)
                {
                    AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 8);
                }
                AddCellToFooterSum(tableLayout, "Root cause of weak performance:", 12);
                AddCellToFooterSum(tableLayout, "Sent Date", 1);
                AddCellToFooterSum(tableLayout, "Market", 1);
                AddCellToFooterSum(tableLayout, "Employee Name", 1);
                AddCellToFooterSum(tableLayout, "Designation", 1);
                AddCellToFooterSum(tableLayout, "Remarks", 8);
                var RootCause = primaryDAO.GetViewDataHistory(dtHistory, "RootCause", model.Designation);
                foreach (var emp in RootCause)
                {
                    AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 8);
                }
                AddCellToFooterSum(tableLayout, "Improvement plan with SMART approach:", 12);
                AddCellToFooterSum(tableLayout, "Sent Date", 1);
                AddCellToFooterSum(tableLayout, "Market", 1);
                AddCellToFooterSum(tableLayout, "Employee Name", 1);
                AddCellToFooterSum(tableLayout, "Designation", 1);
                AddCellToFooterSum(tableLayout, "Remarks", 8);
                var ImprovementPlan = primaryDAO.GetViewDataHistory(dtHistory, "ImprovementPlan", model.Designation);
                foreach (var emp in ImprovementPlan)
                {
                    AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 8);
                }
                AddCellToFooterSum(tableLayout, "Feedback on previous instruction:", 12);
                AddCellToFooterSum(tableLayout, "Sent Date", 1);
                AddCellToFooterSum(tableLayout, "Market", 1);
                AddCellToFooterSum(tableLayout, "Employee Name", 1);
                AddCellToFooterSum(tableLayout, "Designation", 1);
                AddCellToFooterSum(tableLayout, "Remarks", 8);
                var Feedback = primaryDAO.GetViewDataHistory(dtHistory, "Feedback", model.Designation);
                foreach (var emp in Feedback)
                {
                    AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 8);
                }
            }
            if (model.Designation == "RSM" || model.Designation == "DSM")
            {
                //Problems Identified(Paper and Market Related),
                //Measures taken against identified problems (with time limit), 
                //Follow up of previously identified problems and measures 
                //and Specific comment on market/colleague/competitor/new business opportunity
                AddCellToFooterSum(tableLayout, "Problems Identified(Paper and Market Related):", 12);
                //AddCellToFooterSum(tableLayout, "Sent Date", 1);
                //AddCellToFooterSum(tableLayout, "Market", 1);
                //AddCellToFooterSum(tableLayout, "Employee Name", 1);
                //AddCellToFooterSum(tableLayout, "Designation", 1);
                //AddCellToFooterSum(tableLayout, "Remarks", 12);

                DataTable dtHistory = primaryDAO.dtGetViewDataHistory(model);
                var VisitObjectives = primaryDAO.GetViewDataHistory(dtHistory, "VisitObjectives", model.Designation);
                foreach (var emp in VisitObjectives)
                {
                    //AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    //AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    //AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    //AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 12);
                }
                AddCellToFooterSum(tableLayout, "Measures taken against identified problems (with time limit):", 12);
                //AddCellToFooterSum(tableLayout, "Sent Date", 1);
                //AddCellToFooterSum(tableLayout, "Market", 1);
                //AddCellToFooterSum(tableLayout, "Employee Name", 1);
                //AddCellToFooterSum(tableLayout, "Designation", 1);
                //AddCellToFooterSum(tableLayout, "Remarks", 12);
                var RootCause = primaryDAO.GetViewDataHistory(dtHistory, "RootCause", model.Designation);
                foreach (var emp in RootCause)
                {
                    //AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    //AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    //AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    //AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 12);
                }
                AddCellToFooterSum(tableLayout, "Follow up of previously identified problems and measures:", 12);
                //AddCellToFooterSum(tableLayout, "Sent Date", 1);
                //AddCellToFooterSum(tableLayout, "Market", 1);
                //AddCellToFooterSum(tableLayout, "Employee Name", 1);
                //AddCellToFooterSum(tableLayout, "Designation", 1);
                //AddCellToFooterSum(tableLayout, "Remarks", 12);
                var ImprovementPlan = primaryDAO.GetViewDataHistory(dtHistory, "ImprovementPlan", model.Designation);
                foreach (var emp in ImprovementPlan)
                {
                    //AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    //AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    //AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    //AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 12);
                }

                AddCellToFooterSum(tableLayout, "and Specific comment on market/colleague/competitor/new business opportunity:", 12);
                //AddCellToFooterSum(tableLayout, "Sent Date", 1);
                //AddCellToFooterSum(tableLayout, "Market", 1);
                //AddCellToFooterSum(tableLayout, "Employee Name", 1);
                //AddCellToFooterSum(tableLayout, "Designation", 1);
                //AddCellToFooterSum(tableLayout, "Remarks", 12);
                var Feedback = primaryDAO.GetViewDataHistory(dtHistory, "Feedback", model.Designation);
                foreach (var emp in Feedback)
                {
                    //AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                    //AddCellToFooterSum(tableLayout, emp.LocName, 1);
                    //AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                    //AddCellToFooterSum(tableLayout, emp.Designation, 1);
                    AddCellToFooterSum(tableLayout, emp.Remarks, 12);
                }
            }



          
            //----------------------------------------------

            AddCellToFooterSum(tableLayout, "Manager comments", 12);
            AddCellToFooterSum(tableLayout, "Sent Date", 1);
            AddCellToFooterSum(tableLayout, "Market", 1);
            AddCellToFooterSum(tableLayout, "Employee Name", 1);
            AddCellToFooterSum(tableLayout, "Designation", 1);            
            AddCellToFooterSum(tableLayout, "Remarks", 4);
            AddCellToFooterSum(tableLayout, "Manager Remarks", 4);
            var DataSupervisorComment = primaryDAO.GetViewDataSupervisorComments(model);
            foreach (var emp in DataSupervisorComment)
            {
                AddCellToFooterSum(tableLayout, emp.SetDate, 1);
                AddCellToFooterSum(tableLayout, emp.LocName, 1);
                AddCellToFooterSum(tableLayout, emp.EmployeeName, 1);
                AddCellToFooterSum(tableLayout, emp.Designation, 1);               
                AddCellToFooterSum(tableLayout, emp.Remarks, 4);
                AddCellToFooterSum(tableLayout, emp.ManagerRemarks, 4);
                
            }


            return tableLayout;
        }

   
   
        private static void AddCellToFooterSum(PdfPTable tableLayout, string cellText, int colSpan)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 5, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}
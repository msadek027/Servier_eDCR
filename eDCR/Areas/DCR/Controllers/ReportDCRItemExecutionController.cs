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
    public class ReportDCRItemExecutionController : Controller
    {
        ReportDCRItemExecutionDAO primaryDAO = new ReportDCRItemExecutionDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDCRItemExecution()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

       

       



        [HttpPost]
        public ActionResult GetGridData(ReportDCRItemExecutionBEL model)
        {
            var data = primaryDAO.GetGridData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetItemDetail(ReportDCRItemExecutionBEL model)
        {
            var data = primaryDAO.GetItemDetail(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetExecuteDetail(ReportDCRItemExecutionBEL model)
        {
            var data = primaryDAO.GetExecuteDetail(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }




        public ActionResult Export(DefaultParameterBEO model)
        {
            string vHeader = "DCR Item Execution";
            string FileName = "DCR_Item_Execution" + DateTime.Now.ToString("yyyyMMdd");

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

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 8;
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



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportDCRItemExecutionBEL> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 20, 20, 15, 15, 15, 15, 15, 15 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            AddCellToHeader(tableLayout, "TM");
            AddCellToHeader(tableLayout, "MPO Name");
            AddCellToHeader(tableLayout, "CarryIn Qty");
            AddCellToHeader(tableLayout, "Receive Qty");
            AddCellToHeader(tableLayout, "Add/Defi Qty");
            AddCellToHeader(tableLayout, "Total Qty");
            AddCellToHeader(tableLayout, "Execute Qty");
            AddCellToHeader(tableLayout, "Balance Qty");
  
            ////Add body  

            foreach (var emp in item)
            {

                AddCellToBody(tableLayout, emp.TerritoryManagerName);
                AddCellToBody(tableLayout, emp.MPOName);
                AddCellToBody(tableLayout, emp.RestQty);
                AddCellToBody(tableLayout, emp.CentralQty);
                AddCellToBody(tableLayout, emp.VariableQty);

                AddCellToBody(tableLayout, emp.TotalQty);
                AddCellToBody(tableLayout, emp.ExecuteQty);
                AddCellToBody(tableLayout, emp.BalanceQty);

        

            }


            return tableLayout;
        }
        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))
            {
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
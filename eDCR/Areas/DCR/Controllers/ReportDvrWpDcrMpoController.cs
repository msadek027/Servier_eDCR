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
    public class ReportDvrWpDcrMpoController : Controller
    {
        ReportDVRDAO primaryDAO = new ReportDVRDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportDvrWpDcrMpo()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        [HttpPost]
        public ActionResult GetMainGridData(DefaultParameterBEO model)
        {
            var tuple = primaryDAO.ExportDvrWpDcrSummary(model);
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            var data = Json(dataList, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }
        public ActionResult Export(DefaultParameterBEO model)
        {


            string FileName = "Dvr_Wp_Dcr_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Dvr Wp Dcr Summary";

            var tuple = primaryDAO.ExportDvrWpDcrSummary(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;
            GC.Collect();
            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
             

                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);


            }
            else if (model.ExportType == "PDF" && dt.Rows.Count > 0)
            {

                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                //file name to be created   
                string strPDFFileName = string.Format(FileName + "_Pdf.pdf");
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 11 columns 
                int PdfNoOfColumns = 18;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);


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

       



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, DefaultParameterBEO model, List<ReportDvrWpDcrBEL> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 10, 30, 70, 10, 20, 20, 30, 20, 20, 30, 20, 20, 30, 20, 20, 30, 40, 40 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



         
            ////Add header  
            AddCellToHeader(tableLayout, "SL", 0);
            AddCellToHeader(tableLayout, "Doctor Id", 0);
            AddCellToHeader(tableLayout, "Doctor Name", 0);  
            AddCellToHeader(tableLayout, "Shift", 0);
            AddCellToHeader(tableLayout, "MD", 0);
            AddCellToHeader(tableLayout, "ED", 0);
            AddCellToHeader(tableLayout, "MED", 0);

            AddCellToHeader(tableLayout, "MP", 0);
            AddCellToHeader(tableLayout, "EP", 0);
            AddCellToHeader(tableLayout, "MEP", 0);

            AddCellToHeader(tableLayout, "ME", 0);
            AddCellToHeader(tableLayout, "EE", 0);
            AddCellToHeader(tableLayout, "MEE", 0);

            AddCellToHeader(tableLayout, "MA", 0);
            AddCellToHeader(tableLayout, "EA", 0);
            AddCellToHeader(tableLayout, "MEA", 0);

        
           // AddCellToHeader(tableLayout, "MarketCode", 0);
            AddCellToHeader(tableLayout, "MarketName", 0);
           // AddCellToHeader(tableLayout, "RegionCode", 0);
            AddCellToHeader(tableLayout, "RegionName", 0);


            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.SL);
                AddCellToBody(tableLayout, emp.DoctorID);
                AddCellToBody(tableLayout, emp.DoctorName);        
                AddCellToBody(tableLayout, emp.ShiftName);
                AddCellToBody(tableLayout, emp.MD);
                AddCellToBody(tableLayout, emp.ED);
                AddCellToBody(tableLayout, emp.MED);
                AddCellToBody(tableLayout, emp.MP);
                AddCellToBody(tableLayout, emp.EP);
                AddCellToBody(tableLayout, emp.MEP);
                AddCellToBody(tableLayout, emp.ME);
                AddCellToBody(tableLayout, emp.EE);
                AddCellToBody(tableLayout, emp.MEE);
                AddCellToBody(tableLayout, emp.MA);
                AddCellToBody(tableLayout, emp.EA);
                AddCellToBody(tableLayout, emp.MEA);
            
               // AddCellToBody(tableLayout, emp.MarketCode);
                AddCellToBody(tableLayout, emp.MarketName);
               // AddCellToBody(tableLayout, emp.RegionCode);
                AddCellToBody(tableLayout, emp.RegionName);

            }
            
            return tableLayout;
        }

        // Method to add single cell to the Header 
        private static void AddCellToHeaderMain(PdfPTable tableLayout, string cellText, int colSpan)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 6, 0, iTextSharp.text.BaseColor.WHITE)))
            {

                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 1,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)


            });

        }
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText, int rDegree)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 5, 0, iTextSharp.text.BaseColor.WHITE)))
            {

                Rotation = rDegree,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 1,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)


            });

        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 4, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 1,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
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
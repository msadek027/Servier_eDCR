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
    public class TourPlanTmRsmController : Controller
    {
        TourPlanSubDAO primaryDAO = new TourPlanSubDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmTourPlanTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmTourPlanTmRsm2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetPopupView(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetPopupView(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetPopupReview(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetPopupReview(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetViewData(TourPlanSupReport model)
        {
            var data = primaryDAO.GetViewData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReviewData(TourPlanSupReport model)
        {
            var data = primaryDAO.GetReviewData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PushNotification(TourPlanSupMaster model)
        {
            try
            {
                if (primaryDAO.PushNotification(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanSup");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult OperationsMode(TourPlanSupMaster model)
        {
            try
            {
                if (primaryDAO.SaveUpdateSup(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanSup");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult DetailValueTmRsm(TourPlanSupMaster model)
        {
            var data = primaryDAO.DetailValueTmRsm(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
      

        [HttpPost]
        public ActionResult MorningInst(TourPlanSupMaster model)
        {
            var data = primaryDAO.MorningInst(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EveningInst(TourPlanSupMaster model)
        {
            var data = primaryDAO.EveningInst(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "TP_Monthly_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "TP Monthly";

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
                //Document doc = new Document(PageSize.LETTER);
             

                //doc.SetMargins(10, 10, 20, 10);
                ////Create PDF Table with 11 columns 
                //int PdfNoOfColumns = 11;
                //PdfPTable tableLayout = new PdfPTable(11);
                //doc.SetMargins(10, 10, 20, 10);

                Document doc = new Document();
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);
                int PdfNoOfColumns = 11;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
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

                AddPageNumbers(byteInfo);
                return File(workStream, "application/pdf", strPDFFileName);
            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }

        public static byte[] AddPageNumbers(byte[] pdf)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);
            // we create a reader for a certain document
            PdfReader reader = new PdfReader(pdf);
            // we retrieve the total number of pages
            int n = reader.NumberOfPages;
            // we retrieve the size of the first page
            Rectangle psize = reader.GetPageSize(1);

            // step 1: creation of a document-object
            Document document = new Document(psize, 10, 10, 20, 10);
            document.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());

            //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width, iTextSharp.text.PageSize.LETTER.Height));

            // step 2: we create a writer that listens to the document
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            // step 3: we open the document

            document.Open();
            // step 4: we add content
            PdfContentByte cb = writer.DirectContent;

            int p = 0;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                document.NewPage();
                p++;
                PdfImportedPage importedPage = writer.GetImportedPage(reader, page);
                cb.AddTemplate(importedPage, 0, 0);
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, +p + "/" + n, 44, 7, 0);
                cb.EndText();
            }
            // step 5: we close the document

            document.Close();
            return ms.ToArray();
        }




      protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportTourPlanSupBEO> item, string vHeaderSubject, string vHeaderParameter)
        {
           // float[] headers = { 20, 30, 30, 20, 30, 20, 30, 30, 20, 30, 20 }; //Header Widths  
           // tableLayout.SetWidths(headers); //Set the pdf headers  
           // tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
           // //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
           // tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row

           //// tableLayout.FooterRows = 1;
           // //tableLayout Title to the PDF file at the top  

            float[] headers = { 20, 30, 30, 20, 30, 20, 30, 30, 20, 30, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header Main 
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "MORNING", 5);
            AddCellToHeaderMain(tableLayout, "EVENING", 5);
            ////Add header  
            AddCellToHeader(tableLayout, "Day", 0);

            AddCellToHeader(tableLayout, "Location", 0);
            AddCellToHeader(tableLayout, "Address", 0);
            AddCellToHeader(tableLayout, "R.Time", 0);
            AddCellToHeader(tableLayout, "Accompany", 0);
            AddCellToHeader(tableLayout, "NDA", 0);
            AddCellToHeader(tableLayout, "Location", 0);
            AddCellToHeader(tableLayout, "Address", 0);
            AddCellToHeader(tableLayout, "R.Time", 0);
            AddCellToHeader(tableLayout, "Accompany", 0);
            AddCellToHeader(tableLayout, "NDA", 0);
            ////Add body  

            foreach (var emp in item)
            {

                AddCellToBody(tableLayout, emp.DayNumber);

                AddCellToBody(tableLayout, emp.MInstName);
                AddCellToBody(tableLayout, emp.MMeetingPlace);
                AddCellToBody(tableLayout, emp.MSetTime);
                AddCellToBody(tableLayout, emp.MAccompany);
                AddCellToBody(tableLayout, emp.MAllowence);

                AddCellToBody(tableLayout, emp.EInstName);
                AddCellToBody(tableLayout, emp.EMeetingPlace);
                AddCellToBody(tableLayout, emp.ESetTime);
                AddCellToBody(tableLayout, emp.EAccompany);
                AddCellToBody(tableLayout, emp.EAllowence);

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
  












    }
}
using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAO;
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
    public class TourPlanMpoController : Controller
    {
        TourPlanDAO primaryDAO = new TourPlanDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmTourPlanMpo()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        public ActionResult frmTourPlanMpo2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

       




        [HttpPost]
        public ActionResult OperationsMode(TourPlanningMaster model)
        {
            try
            {
                if (primaryDAO.SaveUpdateNEW(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanNew");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
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
      public ActionResult GetViewData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetViewData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetReviewData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetReviewData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }





        [HttpGet]
        public ActionResult GetAllowanceNature()
        {
            var data = primaryDAO.GetAllowanceNature();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult MorningInst(TourPlanningMaster model)
        {
            var data = primaryDAO.MorningInst(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EveningInst(TourPlanningMaster model)
        {
            var data = primaryDAO.EveningInst(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DetailValue(TourPlanningMaster model)
        {
            var data = primaryDAO.DetailValue(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult OperationsModeEdit(DefaultParameterBEO model)
        {
            try
            {
                if (primaryDAO.SaveUpdateEdit(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanNew");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
     
        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "TP" + DateTime.Now.ToString("yyyyMMdd");
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
                Document doc = new Document(PageSize.A4);
                //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 20, 10);
                //doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                doc.SetMargins(20, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 9;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                doc.SetMargins(20, 10, 20, 10);

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



         protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<TourPlanningReport> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 15, 35, 20, 15, 20, 35, 20, 20, 15}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 3; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header Main 
            ExportToPDF.AddCellToHeaderMain(tableLayout, "", 1);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "MORNING", 4);
            ExportToPDF.AddCellToHeaderMain(tableLayout, "EVENING", 4);
          
            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Day", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Location", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "R.Time", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Location", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "R.Time", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "NDA", 0);
            ////Add body  

            foreach (var emp in item)
            {

                ExportToPDF.AddCellToBody(tableLayout, emp.DayNumber);

                ExportToPDF.AddCellToBody(tableLayout, emp.MInstName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MMeetingPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.MSetTime);
                ExportToPDF.AddCellToBody(tableLayout, emp.MAllowence);

                ExportToPDF.AddCellToBody(tableLayout, emp.EInstName);
                ExportToPDF.AddCellToBody(tableLayout, emp.EMeetingPlace);
                ExportToPDF.AddCellToBody(tableLayout, emp.ESetTime);
                ExportToPDF.AddCellToBody(tableLayout, emp.EAllowence);

              
            }
            
            return tableLayout;
        }

       
       
     

	}
}
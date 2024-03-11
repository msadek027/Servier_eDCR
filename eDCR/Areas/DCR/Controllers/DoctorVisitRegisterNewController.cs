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
    public class DoctorVisitRegisterNewController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        DoctorVisitRegisterNewDAO primaryDAO = new DoctorVisitRegisterNewDAO();
        ExportToAnother export = new ExportToAnother();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        public ActionResult frmDoctorVisitRegisterNew()
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
        public ActionResult frmDoctorVisitRegisterNew2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetMainGridData(DefaultParameterBEO model)
        {
            var tuple = primaryDAO.GetMainGridData(model);
            DataTable dt = tuple.Item1;
            var dataList = tuple.Item2;

            var DataSum = primaryDAO.GetGrandTotalSum(model, dt);
            dataList = dataList.Concat(DataSum).ToList();
            return Json(dataList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDvrWpProductList(DefaultParameterBEO model)
        {  
            var tuple = primaryDAO.GetDvrWpProductList(model);
            var dvr = tuple.Item1;
            var wp = tuple.Item2;
            var productList = tuple.Item3;
            return Json(new { DVR = dvr, WP = wp, ProductList = productList }, JsonRequestBehavior.AllowGet);          
        }

        [HttpPost]
        public ActionResult GetDVR(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDVR(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetWP(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetWP(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProduct(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetProduct(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPopupView(TourPlanningMaster model)
        {
            var data = primaryDAO.GetPopupView(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OperationsModeDVRWP(DefaultParameterBEO model)
        {
            try
            {
                if (primaryDAO.SaveUpdateDVRWP(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmDoctorVisitRegisterNew");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        } 

        [HttpPost]
        public ActionResult OperationsModeWPVoid(WorkPlan model)
        {
            try
            {
                if (primaryDAO.SaveUpdateWPVoid(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = primaryDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }

        [HttpPost]
        public ActionResult OperationsModeWPUpdate(WorkPlan model)
        {
            try
            {
                if (primaryDAO.SaveUpdateWPUpdate(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = primaryDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }


        [HttpPost]
        public ActionResult OperationsModeDVR(WorkPlan model)
        {
            try
            {
                if (primaryDAO.SaveUpdateDVR(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = primaryDAO.MaxID, Mode = "NoTP", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }

        [HttpPost]
        public ActionResult OperationsModeDVRWPDelete(WorkPlan model)
        {
            try
            {
                if (primaryDAO.DeleteDVRWP(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmDoctorVisitRegisterNew");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }


        [HttpPost]
        public ActionResult OperationsModeWP(WorkPlan model)
        {
            try
            {
                if (primaryDAO.SaveUpdateWP(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = primaryDAO.MaxID, Mode = "NoTP", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }



        [HttpPost]
        public ActionResult OperationsModeDeleteWP(WorkPlan model)
        {
            try
            {
                if (primaryDAO.DeleteOnlyWP(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = primaryDAO.MaxID, Mode = "NoTP", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }


        public ActionResult Export(DefaultParameterBEO model)
        {
            string FileName = "Dvr_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "DVR Monthly";

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {
                //Add New Code
                var Data = primaryDAO.GetGrandTotalSum(model, dt);
                dataList = dataList.Concat(Data).ToList();

                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                export.ExportToExcelDvr(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

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
                int PdfNoOfColumns = 73;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);

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




        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, DefaultParameterBEO model, List<ReportDVRBEL> item, string vHeaderSubject, string vHeaderParameter)
        {


            float[] headers = { 30, 70, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  


            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header Main 
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "", 1);
            AddCellToHeaderMain(tableLayout, "MORNING", 32);
            AddCellToHeaderMain(tableLayout, "EVENING", 32);
            ////Add header  
            AddCellToHeader(tableLayout, "SL", 0);
            AddCellToHeader(tableLayout, "Doctor Name", 0);
            AddCellToHeader(tableLayout, "Ca", 0);
            AddCellToHeader(tableLayout, "Shift", 0);
            AddCellToHeader(tableLayout, "MPO", 0);
            AddCellToHeader(tableLayout, "MED", 0);
            AddCellToHeader(tableLayout, "MEP", 0);
            AddCellToHeader(tableLayout, "MEE", 0);
            AddCellToHeader(tableLayout, "MEA", 0);

            AddCellToHeader(tableLayout, "MD", 0);
            AddCellToHeader(tableLayout, "01", 0);
            AddCellToHeader(tableLayout, "02", 0);
            AddCellToHeader(tableLayout, "03", 0);
            AddCellToHeader(tableLayout, "04", 0);
            AddCellToHeader(tableLayout, "05", 0);
            AddCellToHeader(tableLayout, "06", 0);
            AddCellToHeader(tableLayout, "07", 0);
            AddCellToHeader(tableLayout, "08", 0);
            AddCellToHeader(tableLayout, "09", 0);
            AddCellToHeader(tableLayout, "10", 0);
            AddCellToHeader(tableLayout, "11", 0);
            AddCellToHeader(tableLayout, "12", 0);
            AddCellToHeader(tableLayout, "13", 0);
            AddCellToHeader(tableLayout, "14", 0);
            AddCellToHeader(tableLayout, "15", 0);
            AddCellToHeader(tableLayout, "16", 0);
            AddCellToHeader(tableLayout, "17", 0);
            AddCellToHeader(tableLayout, "18", 0);
            AddCellToHeader(tableLayout, "19", 0);
            AddCellToHeader(tableLayout, "20", 0);
            AddCellToHeader(tableLayout, "21", 0);
            AddCellToHeader(tableLayout, "22", 0);
            AddCellToHeader(tableLayout, "23", 0);
            AddCellToHeader(tableLayout, "24", 0);
            AddCellToHeader(tableLayout, "25", 0);
            AddCellToHeader(tableLayout, "26", 0);
            AddCellToHeader(tableLayout, "27", 0);
            AddCellToHeader(tableLayout, "28", 0);
            AddCellToHeader(tableLayout, "29", 0);
            AddCellToHeader(tableLayout, "30", 0);
            AddCellToHeader(tableLayout, "31", 0);

            AddCellToHeader(tableLayout, "ED", 0);
            AddCellToHeader(tableLayout, "01", 0);
            AddCellToHeader(tableLayout, "02", 0);
            AddCellToHeader(tableLayout, "03", 0);
            AddCellToHeader(tableLayout, "04", 0);
            AddCellToHeader(tableLayout, "05", 0);
            AddCellToHeader(tableLayout, "06", 0);
            AddCellToHeader(tableLayout, "07", 0);
            AddCellToHeader(tableLayout, "08", 0);
            AddCellToHeader(tableLayout, "09", 0);
            AddCellToHeader(tableLayout, "10", 0);
            AddCellToHeader(tableLayout, "11", 0);
            AddCellToHeader(tableLayout, "12", 0);
            AddCellToHeader(tableLayout, "13", 0);
            AddCellToHeader(tableLayout, "14", 0);
            AddCellToHeader(tableLayout, "15", 0);
            AddCellToHeader(tableLayout, "16", 0);
            AddCellToHeader(tableLayout, "17", 0);
            AddCellToHeader(tableLayout, "18", 0);
            AddCellToHeader(tableLayout, "19", 0);
            AddCellToHeader(tableLayout, "20", 0);
            AddCellToHeader(tableLayout, "21", 0);
            AddCellToHeader(tableLayout, "22", 0);
            AddCellToHeader(tableLayout, "23", 0);
            AddCellToHeader(tableLayout, "24", 0);
            AddCellToHeader(tableLayout, "25", 0);
            AddCellToHeader(tableLayout, "26", 0);
            AddCellToHeader(tableLayout, "27", 0);
            AddCellToHeader(tableLayout, "28", 0);
            AddCellToHeader(tableLayout, "29", 0);
            AddCellToHeader(tableLayout, "30", 0);
            AddCellToHeader(tableLayout, "31", 0);



            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.SL);
                AddCellToBody(tableLayout, emp.DoctorName);
                AddCellToBody(tableLayout, emp.Potential);
                AddCellToBody(tableLayout, emp.ShiftName);
                AddCellToBody(tableLayout, emp.MPOCode);
                AddCellToBody(tableLayout, emp.MED);
                AddCellToBody(tableLayout, emp.MEP);
                AddCellToBody(tableLayout, emp.MEE);
                AddCellToBody(tableLayout, emp.MEA);
                AddCellToBody(tableLayout, emp.MD);
                AddCellToBody(tableLayout, emp.md01);
                AddCellToBody(tableLayout, emp.md02);
                AddCellToBody(tableLayout, emp.md03);
                AddCellToBody(tableLayout, emp.md04);
                AddCellToBody(tableLayout, emp.md05);
                AddCellToBody(tableLayout, emp.md06);
                AddCellToBody(tableLayout, emp.md07);
                AddCellToBody(tableLayout, emp.md08);
                AddCellToBody(tableLayout, emp.md09);
                AddCellToBody(tableLayout, emp.md10);
                AddCellToBody(tableLayout, emp.md11);
                AddCellToBody(tableLayout, emp.md12);
                AddCellToBody(tableLayout, emp.md13);
                AddCellToBody(tableLayout, emp.md14);
                AddCellToBody(tableLayout, emp.md15);
                AddCellToBody(tableLayout, emp.md16);
                AddCellToBody(tableLayout, emp.md17);
                AddCellToBody(tableLayout, emp.md18);
                AddCellToBody(tableLayout, emp.md19);
                AddCellToBody(tableLayout, emp.md20);
                AddCellToBody(tableLayout, emp.md21);
                AddCellToBody(tableLayout, emp.md22);
                AddCellToBody(tableLayout, emp.md23);
                AddCellToBody(tableLayout, emp.md24);
                AddCellToBody(tableLayout, emp.md25);
                AddCellToBody(tableLayout, emp.md26);
                AddCellToBody(tableLayout, emp.md27);
                AddCellToBody(tableLayout, emp.md28);
                AddCellToBody(tableLayout, emp.md29);
                AddCellToBody(tableLayout, emp.md30);
                AddCellToBody(tableLayout, emp.md31);


                AddCellToBody(tableLayout, emp.ED);
                AddCellToBody(tableLayout, emp.ed01);
                AddCellToBody(tableLayout, emp.ed02);
                AddCellToBody(tableLayout, emp.ed03);
                AddCellToBody(tableLayout, emp.ed04);
                AddCellToBody(tableLayout, emp.ed05);
                AddCellToBody(tableLayout, emp.ed06);
                AddCellToBody(tableLayout, emp.ed07);
                AddCellToBody(tableLayout, emp.ed08);
                AddCellToBody(tableLayout, emp.ed09);
                AddCellToBody(tableLayout, emp.ed10);
                AddCellToBody(tableLayout, emp.ed11);
                AddCellToBody(tableLayout, emp.ed12);
                AddCellToBody(tableLayout, emp.ed13);
                AddCellToBody(tableLayout, emp.ed14);
                AddCellToBody(tableLayout, emp.ed15);
                AddCellToBody(tableLayout, emp.ed16);
                AddCellToBody(tableLayout, emp.ed17);
                AddCellToBody(tableLayout, emp.ed18);
                AddCellToBody(tableLayout, emp.ed19);
                AddCellToBody(tableLayout, emp.ed20);
                AddCellToBody(tableLayout, emp.ed21);
                AddCellToBody(tableLayout, emp.ed22);
                AddCellToBody(tableLayout, emp.ed23);
                AddCellToBody(tableLayout, emp.ed24);
                AddCellToBody(tableLayout, emp.ed25);
                AddCellToBody(tableLayout, emp.ed26);
                AddCellToBody(tableLayout, emp.ed27);
                AddCellToBody(tableLayout, emp.ed28);
                AddCellToBody(tableLayout, emp.ed29);
                AddCellToBody(tableLayout, emp.ed30);
                AddCellToBody(tableLayout, emp.ed31);
            }
            var Data = primaryDAO.GetGrandTotalSum(model, dt);
            foreach (var emp in Data)
            {
                AddCellToFooterSum(tableLayout, "Grand Total", 5);
                AddCellToFooterSum(tableLayout, emp.MED, 1);
                AddCellToFooterSum(tableLayout, emp.MEP, 1);
                AddCellToFooterSum(tableLayout, emp.MEE, 1);
                AddCellToFooterSum(tableLayout, emp.MEA, 1);

                AddCellToFooterSum(tableLayout, emp.MD, 1);
                AddCellToFooterSum(tableLayout, "", 31);

                AddCellToFooterSum(tableLayout, emp.ED, 1);
                AddCellToFooterSum(tableLayout, "", 31);

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
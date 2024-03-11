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
    public class ReportDoctorWiseNoOfCallController : Controller
    {
        ReportMPOWiseDoctorWiseCallDAO primaryDAO = new ReportMPOWiseDoctorWiseCallDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        // GET: /DCR/ReportMPOWiseDoctorWiseCall/
        public ActionResult frmReportDoctorWiseNoOfCall()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetDoctor(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDoctor(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetProduct(string MPGroup)
        {
            var data = primaryDAO.GetProduct(MPGroup);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetGridData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetGridData(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetDetailData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetDetailLink(model);
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //SmR6400002420
        public ActionResult Export(DefaultParameterBEO model)
        {
            string vHeader = "Doctor Wise No Of Call";
            string FileName = "Doctor_Wise_No_Of_Call" + DateTime.Now.ToString("yyyyMMdd");

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (model.ExportType == "Excel" && dt.Rows.Count > 0)
            {//ExportToExcelWithoutTitle     ExportToExcel
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

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                PdfPTable tableLayout = new PdfPTable(9);
                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table  

                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF(tableLayout, dt, vHeader));
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



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, DataTable dt, string vHeader)
        {

            float[] headers = {  20, 20,15, 20, 15,  15, 15,15 ,15}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 5;
            //Add Title to the PDF file at the top  

            //List < Employee > employees = _context.employees.ToList < Employee > ();



            List<ReportDoctorWiseNoOfCallBEO> item;
            item = (from DataRow row in dt.Rows
                    select new ReportDoctorWiseNoOfCallBEO
                    {
                        MarketName = row["MARKET_NAME"].ToString(),
                        MPGroup = row["MP_GROUP"].ToString(),
                        EmployeeID = row["MPO_CODE"].ToString(),
                        EmployeeName = row["MPO_NAME"].ToString(),
                        DoctorID = row["DOCTOR_ID"].ToString(),
                        DoctorName = row["DOCTOR_NAME"].ToString(),
                        Degree = row["DEGREES"].ToString(),
                        Specialization = row["SPECIALIZATION"].ToString(),
              
                        TotalCall = row["VISITED_DOC"].ToString(),

                    }).ToList();


            tableLayout.AddCell(new PdfPCell(new Phrase("MPO Wise Doctor Wise Call  \n" + vHeader, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 9,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });


            ////Add header  
            AddCellToHeader(tableLayout, "Market Name");
            AddCellToHeader(tableLayout, "Employee ID");
            AddCellToHeader(tableLayout, "Employee Name");
            AddCellToHeader(tableLayout, "Doctor ID");
            AddCellToHeader(tableLayout, "Doctor Name");
            AddCellToHeader(tableLayout, "Degree");
            AddCellToHeader(tableLayout, "Specialization");       
            AddCellToHeader(tableLayout, "No Of Call");

            ////Add body  

            foreach (var emp in item)
            {
                AddCellToBody(tableLayout, emp.MarketName);
                AddCellToBody(tableLayout, emp.EmployeeID);
                AddCellToBody(tableLayout, emp.EmployeeName);
                AddCellToBody(tableLayout, emp.DoctorID);
                AddCellToBody(tableLayout, emp.DoctorName);

                AddCellToBody(tableLayout, emp.Degree);
                AddCellToBody(tableLayout, emp.Specialization);           
                AddCellToBody(tableLayout, emp.TotalCall);

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
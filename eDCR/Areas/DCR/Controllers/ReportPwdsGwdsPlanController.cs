
using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Universal.Common;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace eDCR.Areas.DCR.Controllers
{
    public class ReportPwdsGwdsPlanController : Controller
    {
        ReportSelectedItemPlanDAO primaryDAO = new ReportSelectedItemPlanDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();

        public string ReportSubject = "";
        //
        // GET: /DCR/ReportSelectedItemPlan/
        public ActionResult frmReportPwdsGwdsPlan()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetGridData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetGridData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Export(DefaultParameterBEO model)
        {
            ReportSubject = model.ItemType == "SlR" ? "Product Wise Doctor Selection (PWDS)" : "Gift Wise Doctor Selection (GWDS)";

            string vHeader = ReportSubject;
            string FileName = "PWDS_GWDS_Plan" + DateTime.Now.ToString("yyyyMMdd");

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
            else if (model.ExportType == "PDF" && dt.Rows.Count>0)
            {

                MemoryStream workStream = new MemoryStream();
                StringBuilder status = new StringBuilder("");

                //file name to be created   
                string strPDFFileName = string.Format(FileName + "_Pdf.pdf");
                Document doc = new Document();
              
                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 10;
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



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportSelectedItemPlanBEL> item, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 20, 20, 15, 20, 15, 15, 15, 15, 15, 20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 2;
            //Add Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "MPO Name", 0);
      
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "CarryIn Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Receive Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Add/Defi Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Total Qty", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Balance Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);

    
            ////Add body  
         
            foreach (var emp in item)
            {

                ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
                ExportToPDF.AddCellToBody(tableLayout, emp.MPOName);
           
                ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
                ExportToPDF.AddCellToBody(tableLayout, emp.RestQty);

                ExportToPDF.AddCellToBody(tableLayout, emp.CentralQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.TotalQty);

                ExportToPDF.AddCellToBody(tableLayout, emp.VariableQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.BalanceQty);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorID);
                ExportToPDF.AddCellToBody(tableLayout, emp.DoctorName);

            }


            return tableLayout;
        }
        // Method to add single cell to the Header  































//        public FileContentResult ExportPDF(ReportSelectedItemPlanBEL model)
//        {
//            DataTable dt = reportSelectedItemPlanDAO.GetData(model);

//        //https://www.c-sharpcorner.com/article/datatable-to-pdf-in-c-sharp/
//            //https://www.c-sharpcorner.com/blogs/datatable-to-pdf-export
//            //https://www.aspforums.net/Threads/127011/Group-By-multiple-Columns-in-DataTable-using-LINQ-in-C-and-VBNet/
//            DataSet ds = new DataSet();
//            // creating data table and adding dummy data  
//            //DataTable dt = new DataTable();
//            //dt.Columns.Add("Name");
//            //dt.Columns.Add("Branch");
//            //dt.Columns.Add("Officer");
//            //dt.Columns.Add("CustAcct");
//            //dt.Columns.Add("Grade");
//            //dt.Columns.Add("Rate");
//            //dt.Columns.Add("OrigBal");
//            //dt.Columns.Add("BookBal");
//            //dt.Columns.Add("Available");
//            //dt.Columns.Add("Effective");
//            //dt.Columns.Add("Maturity");
//            //dt.Columns.Add("Collateral");
//            //dt.Columns.Add("LoanSource");
//            //dt.Columns.Add("RBCCode");

//            //dt.Rows.Add(new object[] { "James Bond, LLC", 120, "Garrison Neely", "123 3428749020", 35, "6.000", "$24,590", "$13,432",  
//            //"$12,659", "12/13/21", "1/30/27", 55, "ILS", "R"});





//            //var dt = new DataTable();
//            //dt.Columns.Add("NAME", typeof(string));
//            //dt.Columns.Add("PRICE", typeof(float));

//            //dt.Rows.Add("chair", 7.00);
//            //dt.Rows.Add("chair", 6.00);
//            //dt.Rows.Add("chair", 8.00);
//            //dt.Rows.Add("table", 15.00);
//            //dt.Rows.Add("table", 17.00);
//            //dt.Rows.Add("plate", 4.00);
//            //dt.Rows.Add("plate", 4.50);







//            ds.Tables.Add(dt);

//            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

//            byte[] filecontent = exportpdf(dt);
//            string filename = "Sample_PDF_" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".pdf";
//            //return File(filecontent, ExcelExportHelper.ExcelContentType, filename);

//            return File(filecontent, contentType, filename);
//        }


//        private byte[] exportpdf(DataTable dtEmployee)
//        {
//            CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
//            TextInfo text_info = culture_info.TextInfo;
//            //txt = text_info.ToTitleCase(txt);


//            // creating document object  
//            System.IO.MemoryStream ms = new System.IO.MemoryStream();
//            iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(PageSize.A4);
//            rec.BackgroundColor = new BaseColor(System.Drawing.Color.Olive);
//            Document doc = new Document(rec);
//            doc.SetPageSize(iTextSharp.text.PageSize.A4);
//            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
//            doc.Open();

//            string ReportHeader = "Dynamic Report PDF \n Parameter";
//            //Creating paragraph for header  
//            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
//            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 16, 1, iTextSharp.text.BaseColor.BLUE);
//            Paragraph prgHeading = new Paragraph();
//            prgHeading.Alignment = Element.ALIGN_CENTER;
//            prgHeading.Add(new Chunk(ReportHeader, fntHead));
//            doc.Add(prgHeading);

//            //Adding paragraph for report generated by  
//            Paragraph prgGeneratedBY = new Paragraph();
//            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
//            iTextSharp.text.Font fntAuthor = new iTextSharp.text.Font(btnAuthor, 8, 2, iTextSharp.text.BaseColor.BLUE);
//            prgGeneratedBY.Alignment = Element.ALIGN_RIGHT;
//            //prgGeneratedBY.Add(new Chunk("Report Generated by : ASPArticles", fntAuthor));  
//            //prgGeneratedBY.Add(new Chunk("\nGenerated Date : " + DateTime.Now.ToShortDateString(), fntAuthor));  
//            doc.Add(prgGeneratedBY);

//            //Adding a line  
//            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
//            doc.Add(p);

//            //Adding line break  
//            doc.Add(new Chunk("\n", fntHead));

//            //Adding  PdfPTable  
//            PdfPTable table = new PdfPTable(dtEmployee.Columns.Count);

//            for (int i = 0; i < dtEmployee.Columns.Count; i++)
//            {
//                string cellText = Server.HtmlDecode(dtEmployee.Columns[i].ColumnName);
//                PdfPCell cell = new PdfPCell();
//                cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 8, 2, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"))));
//                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
//               // cell.Phrase = new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(grdStudent.HeaderStyle.ForeColor)));  
//                //cell.BackgroundColor = new BaseColor(grdStudent.HeaderStyle.BackColor);  
//                cell.HorizontalAlignment = Element.ALIGN_CENTER;
//                cell.PaddingBottom = 5;
//                table.AddCell(cell);
//            }

//            //writing table Data  
//            for (int i = 0; i < dtEmployee.Rows.Count; i++)
//            {
//                for (int j = 0; j < dtEmployee.Columns.Count; j++)
//                {
//                    table.AddCell(dtEmployee.Rows[i][j].ToString());
//                }
//            }

//            doc.Add(table);
//            doc.Close();

//            byte[] result = ms.ToArray();
//            return result;

//        }

//        public void GenerateInvoicePDF(ReportSelectedItemPlanBEL model)
//        {

//            DataTable dt2 = new DataTable();
//            dt2.Columns.AddRange(new DataColumn[]
//           {
//           new DataColumn("Fruit Name", typeof(string)),
//           new DataColumn("Is Rotten", typeof(string))		
//         });

//            dt2.Rows.Add(new object[] { "Apple", "yes" });
//            dt2.Rows.Add(new object[] { "Apple", "no" });
//            dt2.Rows.Add(new object[] { "Apple", "no" });
//            dt2.Rows.Add(new object[] { "Apple", "no" });
//            dt2.Rows.Add(new object[] { "Orange", "no" });
//            dt2.Rows.Add(new object[] { "Orange", "no" });
//            dt2.Rows.Add(new object[] { "Orange", "no" });
//            dt2.Rows.Add(new object[] { "Orange", "no" });
//            dt2.Rows.Add(new object[] { "Orange", "yes" });
//            dt2.Rows.Add(new object[] { "Orange", "yes" });
//            dt2.Rows.Add(new object[] { "Orange", "no" });

//            var result = dt2.AsEnumerable()
//                .GroupBy(x => x.Field<string>("Fruit Name"))
//                .Select(grp => new
//                {
//                    FruitName = grp.Key,
//                    NoOfRotten = grp.Count(x => x.Field<string>("Is Rotten") == "yes"),
//                    TotalNo = grp.Count()
//                })
//                .ToList();

//            Console.WriteLine("{0}\t|\t{1}\t|\t{2}", "FruitName", "NoOfRotten", "TotalNo");
//            foreach (var r in result)
//            {
//                Console.WriteLine("{0}\t|\t{1}\t|\t{2}", r.FruitName, r.NoOfRotten, r.TotalNo);
//            }

//            //Dummy data for Invoice (Bill).
//            string companyName = "ASPSnippets";
//            int orderNo = 2303;
//            DataTable dt = new DataTable();
//            dt.Columns.AddRange(new DataColumn[5] {
//                            new DataColumn("ProductId", typeof(string)),
//                            new DataColumn("Product", typeof(string)),
//                            new DataColumn("Price", typeof(int)),
//                            new DataColumn("Quantity", typeof(int)),
//                            new DataColumn("Total", typeof(int))});
//            dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
//            dt.Rows.Add(102, "Jeans", 400, 2, 800);
//            dt.Rows.Add(103, "Trousers", 300, 3, 900);
//            dt.Rows.Add(104, "Shirts", 550, 2, 1100);

//            using (StringWriter sw = new StringWriter())
//            {
//                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
//                {
//                    StringBuilder sb = new StringBuilder();

//                    //Generate Invoice (Bill) Header.
//                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
//                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
//                    sb.Append("<tr><td colspan = '2'></td></tr>");
//                    sb.Append("<tr><td><b>Order No: </b>");
//                    sb.Append(orderNo);
//                    sb.Append("</td><td align = 'right'><b>Date: </b>");
//                    sb.Append(DateTime.Now);
//                    sb.Append(" </td></tr>");
//                    sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
//                    sb.Append(companyName);
//                    sb.Append("</td></tr>");
//                    sb.Append("</table>");
//                    sb.Append("<br />");

//                    //Generate Invoice (Bill) Items Grid.
//                    sb.Append("<table border = '1'>");
//                    sb.Append("<tr>");
//                    foreach (DataColumn column in dt.Columns)
//                    {
//                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
//                        sb.Append(column.ColumnName);
//                        sb.Append("</th>");
//                    }
//                    sb.Append("</tr>");
//                    foreach (DataRow row in dt.Rows)
//                    {
//                        sb.Append("<tr>");
//                        foreach (DataColumn column in dt.Columns)
//                        {
//                            sb.Append("<td>");
//                            sb.Append(row[column]);
//                            sb.Append("</td>");
//                        }
//                        sb.Append("</tr>");
//                    }
//                    sb.Append("<tr><td align = 'right' colspan = '");
//                    sb.Append(dt.Columns.Count - 1);
//                    sb.Append("'>Total</td>");
//                    sb.Append("<td>");
//                    sb.Append(dt.Compute("sum(Total)", ""));
//                    sb.Append("</td>");
//                    sb.Append("</tr></table>");

//                    //Export HTML String as PDF.
//                    StringReader sr = new StringReader(sb.ToString());
//                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
//                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
//                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
//                    pdfDoc.Open();
//                    htmlparser.Parse(sr);
//                    pdfDoc.Close();
//                    Response.ContentType = "application/pdf";
//                    Response.AddHeader("content-disposition", "attachment;filename=Invoice_" + orderNo + ".pdf");
//                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
//                    Response.Write(pdfDoc);
//                    Response.End();
//                }
//            }
//        }
     
// public FileResult CreatePdf(ReportSelectedItemPlanBEL model)
//  {
//      //  https://www.c-sharpcorner.com/article/create-a-pdf-file-and-download-using-Asp-Net-mvc/
//    MemoryStream workStream = new MemoryStream();  
//    StringBuilder status = new StringBuilder("");  
//    DateTime dTime = DateTime.Now;  
//    //file name to be created   
//    string strPDFFileName = string.Format("SamplePdf" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");  
//    Document doc = new Document();

//    doc.SetMargins(10, 10, 20, 10);   
//    //Create PDF Table with 5 columns  
//    PdfPTable tableLayout = new PdfPTable(5);
//    doc.SetMargins(10, 10, 20, 10);   
//    //Create PDF Table  
  
//    //file will created in this path  
//    string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);  
  
  
//    PdfWriter.GetInstance(doc, workStream).CloseStream = false;  
//    doc.Open();  
  
//    //Add Content to PDF   
//    doc.Add(Add_Content_To_PDF(tableLayout, model));  
  
//    // Closing the document  
//    doc.Close();  
  
//    byte[] byteInfo = workStream.ToArray();  
//    workStream.Write(byteInfo, 0, byteInfo.Length);  
//    workStream.Position = 0;  
  
  
//    return File(workStream, "application/pdf", strPDFFileName);  
  
//}

// protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, ReportSelectedItemPlanBEL model)  
//{  
  
//        float[] headers = {50,24,45,35,20}; //Header Widths  
//        tableLayout.SetWidths(headers); //Set the pdf headers  
//        tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
//        tableLayout.HeaderRows = 1;  
//        //Add Title to the PDF file at the top  
  
//        //List < Employee > employees = _context.employees.ToList < Employee > ();
    
//        var employees = reportSelectedItemPlanDAO.GetGridData(model);


   
  
//        tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) {  
//            Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER  
//        });  
  
  
//        ////Add header  
//        AddCellToHeader(tableLayout, "MarketName");  
//        AddCellToHeader(tableLayout, "MPOName");  
//        AddCellToHeader(tableLayout, "ProductName");  
//        AddCellToHeader(tableLayout, "TotalDoctor");  
//        AddCellToHeader(tableLayout, "RestQty");  
  
//        ////Add body  
  
//        foreach(var emp in employees)   
//        {

//            AddCellToBody(tableLayout, emp.MarketName);
//            AddCellToBody(tableLayout, emp.MPOName);
//            AddCellToBody(tableLayout, emp.ProductName);
//            AddCellToBody(tableLayout, emp.TotalDoctor);
//            AddCellToBody(tableLayout, emp.RestQty);  
  
//        }  


//        return tableLayout;  
//    }  
//    // Method to add single cell to the Header  
//        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)  
//        {  
  
//            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.WHITE)))  
//            {  
//                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)  
//            });  
//        }  
  
//        // Method to add single cell to the body  
//        private static void AddCellToBody(PdfPTable tableLayout, string cellText)  
//        {  
//            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))  
//             {  
//                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)  
//            });  
//        } 



   






	}
}
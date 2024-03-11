using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Common;
using eDCR.Universal.Common;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
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
    public class ReportMarketWiseDoctorController : Controller
    {
        ReportMarketWiseDoctorDAO primaryDAO = new ReportMarketWiseDoctorDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportMarketWiseDoctor()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult GetMarketWiseDoctor(DefaultParameterBEO model)
        {
            var listData = primaryDAO.GetMarketWiseDoctor(model);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;

     
        }

     



        public ActionResult Export(DefaultParameterBEO model)
        {
             //model.ExportType = "PdfGroupReport";
            // model.ExportType = "PdfManualGroupReport";

            string FileName = "Market_Wise_Doctor_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Market Wise Doctor";

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

                //file name to be created   
                string strPDFFileName = string.Format(FileName + ".pdf");
                Document doc = new Document(PageSize.A4.Rotate());
                //Document doc = new Document(new Rectangle(288f, 144f), 10, 10, 20, 10);
                //doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table with 11 columns  
                int PdfNoOfColumns = 22;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
           
                doc.SetMargins(10, 10, 20, 10);
                
                //Create PDF Table  

                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF(tableLayout, PdfNoOfColumns, dt, vHeader, vParameter));
                // Closing the document  
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return File(workStream, "application/pdf", strPDFFileName);          
            }
            else if (model.ExportType == "PdfGroup_Rowspan" && dt.Rows.Count > 0)           {
               
                MemoryStream workStream = new MemoryStream();
                //file name to be created   
                string strPDFFileName = string.Format(FileName + ".pdf");
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 15 columns  
                int PdfNoOfColumns = 15;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();               

                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF_Group_Rowspan(doc,tableLayout, PdfNoOfColumns, dt, vHeader, vParameter));
                // Closing the document  
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return File(workStream, "application/pdf", strPDFFileName);
            }
            else if (model.ExportType == "PdfGroup_Colspan" && dt.Rows.Count > 0)
            {
                
                MemoryStream workStream = new MemoryStream();
                //file name to be created   
                string strPDFFileName = string.Format(FileName + ".pdf");
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4.Rotate());
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 16 columns  
                int PdfNoOfColumns = 16;
                PdfPTable tableLayout = new PdfPTable(PdfNoOfColumns);
                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);
                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();

                //Add Content to PDF   
                doc.Add(Add_Content_To_PDF_Group_Colspan(doc, tableLayout, PdfNoOfColumns, dt, vHeader, vParameter));
                // Closing the document  
                doc.Close();

                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return File(workStream, "application/pdf", strPDFFileName);
            }
            else if (model.ExportType == "PdfManualGroupReport")
            {

                FileName = string.Format(FileName + ".pdf");

                MemoryStream workStream = new MemoryStream();
                Document doc = new Document(PageSize.A4);
                PdfPTable tableLayout = new PdfPTable(5);
                //tableLayout.HeaderRows = 1;
                doc.SetMargins(10, 10, 20, 10);
                //Create PDF Table  

                //file will created in this path  
                string strAttachment = Server.MapPath("~/Downloadss/" + FileName);

                PdfWriter.GetInstance(doc, workStream).CloseStream = false;
                doc.Open();
                //Add Content to PDF   

                doc.Add(GenerateContent(tableLayout, dt));
                //doc.Add(GenerateContent2(tableLayout, dt));

                // Closing the document  
                doc.Close();
                byte[] byteInfo = workStream.ToArray();
                workStream.Write(byteInfo, 0, byteInfo.Length);
                workStream.Position = 0;

                return File(workStream, "application/pdf", FileName);

            }
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }



        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns,DataTable dt, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = { 10, 30, 15, 30, 20, 20,40, 40, 20, 20, 20, 20, 20, 20, 20, 20, 20,10,10,15,20,20 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 4; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();


            TableRowCounter tableRowCounter = new TableRowCounter();
            tableLayout.TableEvent = tableRowCounter;

            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header  
            ///
            ExportToPDF.AddCellToHeader(tableLayout, "SL", 0);
            
            ExportToPDF.AddCellToHeader(tableLayout, "Market Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Doctor Name", 0);        
            ExportToPDF.AddCellToHeader(tableLayout, "Degrees", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "MorningLocation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "EveningLocation", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Address1", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address2", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address3", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address4", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Specialization", 0);          
            ExportToPDF.AddCellToHeader(tableLayout, "Potential", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Reg No", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Phone", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Email", 0);

            ExportToPDF.AddCellToHeader(tableLayout, "Religion", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Gender", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "MarketCode", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Territory", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Region", 0);
            

            foreach (DataRow row in dt.Rows)
            {
                ExportToPDF.AddCellToBody(tableLayout, row["Col1"].ToString());
          
                ExportToPDF.AddCellToBody(tableLayout, row["MARKET"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_ID"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_NAME"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["DEGREES"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["DESIGNATION"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["Morning_Location"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["Evening_Location"].ToString());

                ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS1"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS2"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS3"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS4"].ToString());
          
        
                ExportToPDF.AddCellToBody(tableLayout, row["SPECIALIZATION"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["POTENTIAL"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["REG_NO"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["PHONE"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["EMAIL"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["RELIGION"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["GENDER"].ToString());

                ExportToPDF.AddCellToBody(tableLayout, row["MARKET_CODE"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["TERRITORY_NAME"].ToString());
                ExportToPDF.AddCellToBody(tableLayout, row["REGION_NAME"].ToString());

            }


            return tableLayout;
        }






       

        protected PdfPTable Add_Content_To_PDF_Group_Rowspan(Document doc, PdfPTable tableLayout, int PdfNoOfColumns,   DataTable dt, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = {20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 40, 40 }; //Header Widths 
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
                                               //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 4; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            TableRowCounter tableRowCounter = new TableRowCounter();
            tableLayout.TableEvent = tableRowCounter;

            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);
        
            ////Add header  
            // ExportToPDF.AddCellToHeader(tableLayout, "MarketCode", 0);     
             ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "DoctorID", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "DoctorName", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Degree", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "MorningLocation", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "EveningLocation", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Address1", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Address2", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Address3", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Address4", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Specialization", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Potential", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Phone", 0);
             ExportToPDF.AddCellToHeader(tableLayout, "Region", 0);

            DataTable dtGroup = new DataView(dt).ToTable(true, "MARKET_CODE");
            if (dtGroup.Rows.Count > 0)
            {
           
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    string paddedString = (i + 1).ToString().Length==1?"0"+ (i + 1).ToString() : (i + 1).ToString();
                    DataTable vdt = new DataView(dt).ToTable(true, "REGION_NAME", "MARKET_CODE", "MARKET", "DOCTOR_ID", "DOCTOR_NAME", "PHONE", "DEGREES", "SPECIALIZATION", "POTENTIAL", "DESIGNATION", "ADDRESS1", "ADDRESS2", "ADDRESS3", "ADDRESS4", "Morning_Location", "Evening_Location");
                    DataRow[] drArray = vdt.Select("MARKET_CODE='" + dtGroup.Rows[i]["MARKET_CODE"].ToString().Trim() + "'");
                    DataTable dtt = new DataTable();
                    dtt = vdt.Clone();
                    foreach (DataRow rowtemp in drArray)
                    {
                        dtt.ImportRow(rowtemp);
                    }
                    Font fontCell = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.BOLD);

                    PdfPCell cellWithRowspan = new PdfPCell(new Phrase(paddedString + ". Market Code: " +dtGroup.Rows[i]["MARKET_CODE"].ToString().Trim()+", Total Doctor: "+ dtt.Rows.Count, fontCell));
                   // cellWithRowspan.Rowspan = dtt.Rows.Count; //
                    cellWithRowspan.Colspan = PdfNoOfColumns;  //
                    tableLayout.AddCell(cellWithRowspan);

                    foreach (DataRow row in dtt.Rows)
                    {                      
                        ExportToPDF.AddCellToBody(tableLayout, row["MARKET"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_ID"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DEGREES"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DESIGNATION"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["Morning_Location"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["Evening_Location"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS1"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS2"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS3"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS4"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["SPECIALIZATION"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["POTENTIAL"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["PHONE"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["REGION_NAME"].ToString());
                    }
                    // tableLayout.KeepTogether = false;
                    // next two properties keep <tr> together if possible
                    // tableLayout.SplitRows = true;                    
                   
                    doc.Add(tableLayout);
                    doc.NewPage();

                    // Create a new table for the next group
                    tableLayout = new PdfPTable(headers);
                    tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage
                    tableLayout.HeaderRows = 4; // 2 header rows + 1 footer row
                    tableLayout.CompleteRow();

                    //TableRowCounter tableRowCounter = new TableRowCounter();
                    tableLayout.TableEvent = tableRowCounter;

                    tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

                    ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "DoctorID", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "DoctorName", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Degree", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "MorningLocation", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "EveningLocation", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Address1", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Address2", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Address3", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Address4", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Specialization", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Potential", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Phone", 0);
                    ExportToPDF.AddCellToHeader(tableLayout, "Region", 0);
                }
            }//if dt count>0 END
            return tableLayout;
        }
        protected PdfPTable Add_Content_To_PDF_Group_Colspan(Document doc, PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, string vHeaderSubject, string vHeaderParameter)
        {

            float[] headers = {20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 40, 40 }; //Header Widths 
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
                                               //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 4; // 2 header rows + 1 footer row
            tableLayout.CompleteRow();

            TableRowCounter tableRowCounter = new TableRowCounter();
            tableLayout.TableEvent = tableRowCounter;

            //tableLayout Title to the PDF file at the top  
            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);

            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "MarketCode", 0);     
            ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DoctorID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "DoctorName", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Degree", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Designation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "MorningLocation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "EveningLocation", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address1", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address2", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address3", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Address4", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Specialization", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Potential", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Phone", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Region", 0);



            DataTable dtGroup = new DataView(dt).ToTable(true, "MARKET_CODE");
            if (dtGroup.Rows.Count > 0)
            {
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    DataTable vdt = new DataView(dt).ToTable(true, "REGION_NAME", "MARKET_CODE", "MARKET", "DOCTOR_ID", "DOCTOR_NAME", "PHONE", "DEGREES", "SPECIALIZATION", "POTENTIAL", "DESIGNATION", "ADDRESS1", "ADDRESS2", "ADDRESS3", "ADDRESS4", "Morning_Location", "Evening_Location");
                    DataRow[] drArray = vdt.Select("MARKET_CODE='" + dtGroup.Rows[i]["MARKET_CODE"].ToString() + "'");
                    DataTable dtt = new DataTable();
                    dtt = vdt.Clone();
                    foreach (DataRow rowtemp in drArray)
                    {
                        dtt.ImportRow(rowtemp);
                    }
                    Font fontCell = new Font(Font.FontFamily.TIMES_ROMAN, 8, Font.NORMAL);

                    PdfPCell cellWithRowspan = new PdfPCell(new Phrase(dtGroup.Rows[i]["MARKET_CODE"].ToString(), fontCell));
                     cellWithRowspan.Rowspan = dtt.Rows.Count; //
                   // cellWithRowspan.Colspan = dtt.Rows.Count;  //
                    tableLayout.AddCell(cellWithRowspan);

                    foreach (DataRow row in dtt.Rows)
                    {

                        ExportToPDF.AddCellToBody(tableLayout, row["MARKET"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_ID"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DOCTOR_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DEGREES"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DESIGNATION"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["Morning_Location"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["Evening_Location"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS1"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS2"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS3"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["ADDRESS4"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["SPECIALIZATION"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["POTENTIAL"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["PHONE"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["REGION_NAME"].ToString());
                    }
                    //tableLayout.KeepTogether = false;
                    // next two properties keep <tr> together if possible
                    //tableLayout.SplitRows = true;

                    doc.NewPage();
                    doc.Add(tableLayout);
                }
            }//if dt count>0 END
            return tableLayout;
        }
        // Method to add single cell to the Header  

        private PdfPTable GenerateContent2(PdfPTable tableLayout, DataTable dt)
        {
            //https://www.c-sharpcorner.com/article/merge-rows-using-itextsharp/
            tableLayout.HeaderRows = 1;
            Font fontCell = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL);
            Font fontHeader = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
            PdfPTable tableWithRowspan = tableLayout;
            tableWithRowspan.SpacingBefore = 10;
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Market Code", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("MARKET", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("DOCTOR_ID", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("DOCTOR_NAME", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("PHONE", fontHeader)));

            DataTable dtGroup = new DataView(dt).ToTable(true, "MARKET_CODE");
            if (dtGroup.Rows.Count > 0)
            {
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    DataTable vdt = new DataView(dt).ToTable(true, "MARKET_CODE", "MARKET", "DOCTOR_ID", "DOCTOR_NAME", "PHONE");
                    DataRow[] drArray = vdt.Select("MARKET_CODE='" + dtGroup.Rows[i]["MARKET_CODE"].ToString() + "'");

                    DataTable dtt = new DataTable();
                    dtt = vdt.Clone();
                    foreach (DataRow rowtemp in drArray)
                    {
                        dtt.ImportRow(rowtemp);
                    }


                PdfPCell cellWithRowspan = new PdfPCell(new Phrase(dtGroup.Rows[i]["MARKET_CODE"].ToString()));
                    // The first cell spans 5 rows  
                    cellWithRowspan.Rowspan = dtt.Rows.Count;
                    tableWithRowspan.AddCell(cellWithRowspan);
                    //tableWithRowspan.AddCell(new PdfPCell(new Phrase("MARKET", fontCell)));
                    //tableWithRowspan.AddCell(new PdfPCell(new Phrase("DOCTOR_ID", fontCell)));
                    //tableWithRowspan.AddCell(new PdfPCell(new Phrase("DOCTOR_NAME", fontCell)));
                    //tableWithRowspan.AddCell(new PdfPCell(new Phrase("PHONE", fontCell)));
                    foreach (DataRow row in dtt.Rows)
                    {
                        // Cell 2,1 does not exist  
                        tableWithRowspan.AddCell(new PdfPCell(new Phrase(row["MARKET"].ToString(), fontCell)));
                        tableWithRowspan.AddCell(new PdfPCell(new Phrase(row["DOCTOR_ID"].ToString(), fontCell)));
                        tableWithRowspan.AddCell(new PdfPCell(new Phrase(row["DOCTOR_NAME"].ToString(), fontCell)));
                        tableWithRowspan.AddCell(new PdfPCell(new Phrase(row["PHONE"].ToString(), fontCell)));                
                    }
                }
            }
            return tableWithRowspan;
        }

        private PdfPTable GenerateContent(PdfPTable tableLayout, DataTable dt)
        { 
            //https://www.c-sharpcorner.com/article/merge-rows-using-itextsharp/
            Font fontCell = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.NORMAL);
            Font fontHeader = new Font(Font.FontFamily.TIMES_ROMAN, 12, Font.BOLD);
            PdfPTable tableWithRowspan = tableLayout;
            tableWithRowspan.SpacingBefore = 10;
            //Global Header
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Header", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Header 1", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Header 2", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Header 3", fontHeader)));
            tableWithRowspan.AddCell(new PdfPCell(new Phrase("Header 4", fontHeader)));
            for (int iRow = 0; iRow < 2; iRow++)
            {
                PdfPCell cellWithRowspan = new PdfPCell(new Phrase("Environment Access Data place"));
                // The first cell spans 5 rows  
                cellWithRowspan.Rowspan = 5;
                tableWithRowspan.AddCell(cellWithRowspan);
                // Sub Header
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iPerform", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iBehaviour", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iTexts", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iMap", fontCell)));
                // Cell 2,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 3,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("158.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("1680.0", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 4,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("158.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("1680.0", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 5,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
            }
           
            tableWithRowspan.SplitLate = false;
            
            for (int iRow = 0; iRow < 2; iRow++)
            {
                PdfPCell cellWithRowspan = new PdfPCell(new Phrase("Environment Access Data place"));
                // The first cell spans 5 rows  
                cellWithRowspan.Rowspan = 5;
                tableWithRowspan.AddCell(cellWithRowspan);
                // Sub Header
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iPerform", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iBehaviour", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iTexts", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("iMap", fontCell)));
                // Cell 2,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 3,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("158.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("1680.0", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 4,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("158.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("1680.0", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("10.5", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("20.5", fontCell)));
                // Cell 5,1 does not exist  
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
                tableWithRowspan.AddCell(new PdfPCell(new Phrase("SubTotal: Sum", fontCell)));
            }
            return tableWithRowspan;
        }

    }
}
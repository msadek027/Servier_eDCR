using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL;
//using eDCR.Areas.DCR.Models.DAL.DAO;
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
    public class ReportItemPhysicalStockCheckSupController : Controller
    {
        ReportItemPhysicalStockCheckSupDAO primaryDAO = new ReportItemPhysicalStockCheckSupDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();

        public ActionResult frmReportItemPhysicalStockCheckSup()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        [HttpPost]
        public ActionResult GetData(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetData(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Export(DefaultParameterBEO model)
        {

            string FileName = "Physical_Stock_Check" + DateTime.Now.ToString("yyyyMMdd");

            string vHeader = "Physical Stock Check";

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

                string strPDFFileName = string.Format(FileName + "_Pdf.pdf");
                Document doc = new Document();
                doc.SetPageSize(PageSize.A4);
                doc.SetMargins(10, 10, 20, 10);

                //Create PDF Table with 11 columns  
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


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, int PdfNoOfColumns, DataTable dt, List<ReportItemPhysicalStockCheckSupBEO> item, string vHeaderSubject, string vHeaderParameter)
        {

         

            float[] headers = {10,20, 25, 10, 25, 10, 10, 10, 10,10,10 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File witdh percentage  
            //tableLayout.HeaderRows = 2;  //1 for First page and 2 for each page Repeat  
            tableLayout.HeaderRows = 5; // 2 header rows + 1 footer row


            //tableLayout Title to the PDF file at the top  

            tableLayout = ExportToPDF.AddCellToPdf(tableLayout, PdfNoOfColumns, headers.Length, reportHeader.vCompanyName, reportHeader.vPrintDate, vHeaderSubject, vHeaderParameter);



            ////Add header  
            ExportToPDF.AddCellToHeader(tableLayout, "Date", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Market", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "MPO Name", 0);
            
            ExportToPDF.AddCellToHeader(tableLayout, "Product Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Pack Size", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Item Type", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "EDCR Qty", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Physical Qty", 0);
            //ExportToPDF.AddCellToHeader(tableLayout, "Remarks", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Checke By ID", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Checked By Name", 0);
            ExportToPDF.AddCellToHeader(tableLayout, "Checked By Designation", 0);
            ////Add body  
            DataTable dtGroup = new DataView(dt).ToTable(true, "MARKET_NAME");
            if (dtGroup.Rows.Count > 0)
            {
                for (int i = 0; i < dtGroup.Rows.Count; i++)
                {
                    
                    DataTable vdt = new DataView(dt).ToTable(true, "SET_DATE", "MPO_NAME","MARKET_NAME","PRODUCT_CODE","PRODUCT_NAME","PACK_SIZE","ITEM_TYPE","ITEM_FOR", "LOGICAL_QTY", "PHYSICAL_QTY",  "SUPERVISOR_ID","SUPERVISOR_NAME","DESIGNATION");
                    DataRow[] drArray = vdt.Select("MARKET_NAME='" + dtGroup.Rows[i]["MARKET_NAME"].ToString() + "'");
                    DataTable dtt = new DataTable();
                    dtt = vdt.Clone();
                    foreach (DataRow rowtemp in drArray)
                    {
                        dtt.ImportRow(rowtemp);
                    }

                    foreach (DataRow row in dtt.Rows)
                    {
                        ExportToPDF.AddCellToBody(tableLayout, row["SET_DATE"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["MARKET_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["MPO_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["PRODUCT_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["PACK_SIZE"].ToString());

                        ExportToPDF.AddCellToBody(tableLayout, row["ITEM_TYPE"].ToString() + row["ITEM_FOR"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["LOGICAL_QTY"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["PHYSICAL_QTY"].ToString());

                       // ExportToPDF.AddCellToBody(tableLayout, row["REMARKS"].ToString());

                        ExportToPDF.AddCellToBody(tableLayout, row["SUPERVISOR_ID"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["SUPERVISOR_NAME"].ToString());
                        ExportToPDF.AddCellToBody(tableLayout, row["DESIGNATION"].ToString());                     
                    }


                    DataTable vdtRemarks = new DataView(dt).ToTable(true, "MARKET_NAME", "REMARKS");
                    DataRow[] drArrayRemarks = vdtRemarks.Select("MARKET_NAME='" + dtGroup.Rows[i]["MARKET_NAME"].ToString() + "'");
                    DataTable dttRemarks = new DataTable();
                    dttRemarks = vdtRemarks.Clone();
                    foreach (DataRow rowtemp in drArrayRemarks)
                    {
                        dttRemarks.ImportRow(rowtemp);
                    }

                    foreach (DataRow rowRemarks in dttRemarks.Rows)
                    {
                        AddCellToFooterSum(tableLayout, "Remarks:", 1);
                        AddCellToFooterSum(tableLayout, rowRemarks["REMARKS"].ToString(), 10);                 

                    }
                }


            
            }



            //        foreach (var emp in item)
            //{
            //    ExportToPDF.AddCellToBody(tableLayout, emp.SetDate);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.MarketName);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.MPOName);

            //    ExportToPDF.AddCellToBody(tableLayout, emp.ProductName);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.PackSize);

            //    ExportToPDF.AddCellToBody(tableLayout, emp.ItemType);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.eDCRQty);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.PhysicalQty);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.Remarks);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.CheckedByID);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.CheckedByName);
            //    ExportToPDF.AddCellToBody(tableLayout, emp.CheckedByDesignation);
            //}

            return tableLayout;
        }

        private static void AddCellToFooterSum(PdfPTable tableLayout, string cellText, int colSpan)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 7, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 2,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });


            
        }
    }
}
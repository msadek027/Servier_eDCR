using eDCR.Areas.DCR.Models.BEL;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace eDCR.Universal.Common
{
    public class ExportToPDF
    {

        // Method to add single cell to the Header 
  

        public static void AddCellToHeaderMain(PdfPTable tableLayout, string cellText, int colSpan)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.WHITE)))
            {

                Colspan = colSpan,
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)


            });

        }
        public static void AddCellToHeader(PdfPTable tableLayout, string cellText, int rDegree)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, iTextSharp.text.BaseColor.WHITE)))
            {

                Rotation = rDegree,
                HorizontalAlignment = Element.ALIGN_JUSTIFIED,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 128, 128)
            });

        }

        // Method to add single cell to the body  
        public static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 7, 0, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 1,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }




        public static PdfPTable AddCellToPdf(PdfPTable tableLayout, int PdfNoOfColumns, int PdfNoOfHeaders, string vCompanyName,string vPrintDate,string vHeaderSubject, string vHeaderParameter)
        {

            int vHeaderLeft = 0; int vHeaderMiddle = 0; int vHeaderRight = 0;
            if (PdfNoOfHeaders == PdfNoOfColumns)
            {
                vHeaderLeft = (int)Math.Round(1 * PdfNoOfColumns / 3.8);
                vHeaderMiddle = (int)Math.Round(1.5 * PdfNoOfColumns / 3.8);
                vHeaderRight = (int)Math.Round(1.3 * (PdfNoOfColumns / 3.8));

                int diff = PdfNoOfColumns - (vHeaderLeft + vHeaderMiddle + vHeaderRight);
                vHeaderRight = vHeaderRight + diff;

                tableLayout.AddCell(new PdfPCell(new Phrase(vCompanyName + "\n", new Font(Font.NORMAL, 15, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = PdfNoOfColumns, Border = 0, PaddingBottom = 0, HorizontalAlignment = Element.ALIGN_CENTER });
                tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderLeft, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_CENTER });
                tableLayout.AddCell(new PdfPCell(new Phrase(vHeaderSubject, new Font(Font.NORMAL, 13, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderMiddle+2, Border = PdfPCell.BOTTOM_BORDER, BorderWidthBottom = 1.5f, PaddingBottom = 10,  HorizontalAlignment = Element.ALIGN_CENTER });
                tableLayout.AddCell(new PdfPCell(new Phrase(vPrintDate + "\n", new Font(Font.NORMAL, 8, 1, new iTextSharp.text.BaseColor(153, 51, 0)))) { Colspan = vHeaderRight, Border = 0, PaddingBottom = 10, HorizontalAlignment = Element.ALIGN_RIGHT });

            }

            tableLayout.AddCell(new PdfPCell(new Phrase(vHeaderParameter, new Font(Font.FontFamily.TIMES_ROMAN, 8, 0, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = PdfNoOfColumns,
                Border = 0,             
                PaddingBottom = 5,              
                HorizontalAlignment = Element.ALIGN_LEFT
            });

            return tableLayout;
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
            document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

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

    }
    //https://scottstoecker.wordpress.com/2016/12/08/showing-page-numbers-when-an-itextsharp-table-spans-multiple-pages/
    public class TableRowCounter : IPdfPTableEventSplit
    {
        private int pageCount = 0;
        private int totalRowCount = -1;
        private double firstPageRowCount = 0;
        private bool isTrue = false;
        public void SplitTable(PdfPTable table)
        {
            this.totalRowCount = table.Rows.Count;

        }

        public void TableLayout(PdfPTable table, float[][] widths, float[] heights, int headerRows, int rowStart, PdfContentByte[] canvases)
        {

            var writer = canvases[PdfPTable.BASECANVAS].PdfWriter;
            var thisRowCount = table.Rows.Count;

            if (isTrue == false)
            {
                isTrue = true;
                firstPageRowCount = table.Rows.Count - 3;
            }
            // int totalPage = (int)Math.Round(totalRowCount / firstPageRowCount)+1;
            int totalPage = (int)Math.Round(totalRowCount / 41.0) + 1;     //     


            BaseFont baseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, false);
            Font font = new Font(baseFont, 9);
            //Page 1 of 10
            pageCount += 1;

        
            //var text = "Page " + pageCount.ToString() + " of " + totalPage;
            var text = "Page: " + pageCount.ToString();

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(text, font), 775, 525, 0);

           // ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(text, font), 770, 525, 0);
        }
    }

}
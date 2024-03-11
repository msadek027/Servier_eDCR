using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace eDCR.Universal.Common
{
    public class ExportToAnother
    {

        public void ExportToExcel(DataTable dt, string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
            try
            {
                var aCode = 65;       
                //Create column and inser rows
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Sheet");

                    var wsReportNameHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                    wsReportNameHeaderRange.Style.Font.Bold = true;
                    wsReportNameHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportNameHeaderRange.Merge();
                    wsReportNameHeaderRange.Value = vComapanyReportHeader;

                    var wsReportDateHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 2, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                    wsReportDateHeaderRange.Merge();
                    wsReportDateHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportDateHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportDateHeaderRange.Value = string.Format(vReportHeader + "{0}", "");

                    var wsReportCreatedByHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 3, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                    wsReportCreatedByHeaderRange.Merge();
                    wsReportCreatedByHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportCreatedByHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportCreatedByHeaderRange.Value = string.Format(vReportParameter + " {0}", "");
                    ws.Row(3).InsertRowsBelow(1);

                    ws.Row(4).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Row(4).Style.Border.RightBorder = XLBorderStyleValues.None;
                    ws.Row(4).Style.Border.LeftBorder = XLBorderStyleValues.None;

                    int rowIndex = 5;
                    int columnIndex = 0;

                    int columnIndexAA = 0;
                    int columnIndexBA = 0;
                    foreach (DataColumn column in dt.Columns)
                    { 
                        if (columnIndex >= 0 && columnIndex <= 25)
                        {
                            ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        }
                        if (columnIndex >= 26 && columnIndex <= 51)
                        {                 
                            ws.Cell(string.Format("{0}{1}", "A"+Char.ConvertFromUtf32(aCode + columnIndexAA), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", "A"+ Char.ConvertFromUtf32(aCode + columnIndexAA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            columnIndexAA++;
                        }
                        if (columnIndex >= 52 && columnIndex <= 77)
                        {
                            ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + columnIndexBA), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + columnIndexBA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            columnIndexBA++;
                        }
                        columnIndex++;

                    }
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    rowIndex++;
                    foreach (DataRow row in dt.Rows)
                    {
                        int valueCount = 0;
                        int valueCountAA = 0;
                        int valueCountBA = 0;
                        foreach (object rowValue in row.ItemArray)
                        {
                            if (valueCount >= 0 && valueCount <= 25)
                            {
                                ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            }
                            if (valueCount >= 26  && valueCount <= 51)
                            {
                                ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + valueCountAA), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + valueCountAA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                valueCountAA++;
                            }
                            if (valueCount >= 52 && valueCount <= 77)
                            {
                                ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + valueCountBA), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + valueCountBA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                valueCountBA++;
                            }
                            valueCount++;
                        }
                        rowIndex++;
                    }



                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {

                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        //Dvr Report New
        public void ExportToExcelDvr(DataTable dt, string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
            try
            {
                var aCode = 65;

                //Create column and inser rows
                using (XLWorkbook wb = new XLWorkbook())
                {
                     var ws = wb.Worksheets.Add("Sheet");
                    var wsReportNameHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(120)));

                    wsReportNameHeaderRange.Style.Font.Bold = true;
                    wsReportNameHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportNameHeaderRange.Merge();
                    wsReportNameHeaderRange.Value = vComapanyReportHeader;

                    var wsReportDateHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 2, Char.ConvertFromUtf32(120)));
                    wsReportDateHeaderRange.Merge();
                    wsReportDateHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportDateHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportDateHeaderRange.Value = string.Format(vReportHeader + "{0}", "");

                    var wsReportCreatedByHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 3, Char.ConvertFromUtf32(120)));
                    wsReportCreatedByHeaderRange.Merge();
                    wsReportCreatedByHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wsReportCreatedByHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    wsReportCreatedByHeaderRange.Value = string.Format(vReportParameter + " {0}", "");
                    ws.Row(3).InsertRowsBelow(1);

                    ws.Row(4).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Row(4).Style.Border.RightBorder = XLBorderStyleValues.None;
                    ws.Row(4).Style.Border.LeftBorder = XLBorderStyleValues.None;

                    int rowIndex = 5;
                    int columnIndex = 0;

                    int columnIndexAA = 0;
                    int columnIndexBA = 0;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (columnIndex >= 0 && columnIndex <= 25)
                        {
                            ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                        }
                        if (columnIndex >= 26 && columnIndex <= 51)
                        {
                            ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + columnIndexAA), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + columnIndexAA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            columnIndexAA++;
                        }
                        if (columnIndex >= 52 && columnIndex <= 77)
                        {
                            ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + columnIndexBA), rowIndex)).Value = column.ColumnName;
                            ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + columnIndexBA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                            columnIndexBA++;
                        }
                        columnIndex++;

                    }
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    rowIndex++;
                    foreach (DataRow row in dt.Rows)
                    {
                        int valueCount = 0;
                        int valueCountAA = 0;
                        int valueCountBA = 0;
                        foreach (object rowValue in row.ItemArray)
                        {
                            if (valueCount >= 0 && valueCount <= 25)
                            {
                                ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                            }
                            if (valueCount >= 26 && valueCount <= 51)
                            {
                                ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + valueCountAA), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", "A" + Char.ConvertFromUtf32(aCode + valueCountAA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                valueCountAA++;
                            }
                            if (valueCount >= 52 && valueCount <= 77)
                            {
                                ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + valueCountBA), rowIndex)).Value = rowValue;
                                ws.Cell(string.Format("{0}{1}", "B" + Char.ConvertFromUtf32(aCode + valueCountBA), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                valueCountBA++;
                            }
                            valueCount++;
                        }
                        rowIndex++;
                    }
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    HttpContext.Current.Response.Charset = "";
                    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {

                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.End();
                    }
                }

            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
        //DVR Old Report
        public void ExportToExcelOld(DataTable table,string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
           // string vHeader = "Header Parameter";


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            // HttpContext.Current.Response.ContentType = "application/text";           
            //HttpContext.Current.Response.ContentType = "application/ms-excel";
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-exce";


            HttpContext.Current.Response.ContentType = "application/x-msexcel";
           // HttpContext.Current.Response.ContentType = "application/ms-word";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");

            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><center><b>" + vComapanyReportHeader + "<BR>");
            HttpContext.Current.Response.Write(vReportHeader);
            HttpContext.Current.Response.Write("<p style='text-align:left;'>" + vReportParameter + " </p>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        //For Fortnight Report
        public void ExportToExcelDataSetOld(DataSet dataset, string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
            // string vHeader = "Header Parameter";


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            // HttpContext.Current.Response.ContentType = "application/text";           
            //HttpContext.Current.Response.ContentType = "application/ms-excel";
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-exce";


            HttpContext.Current.Response.ContentType = "application/x-msexcel";
            // HttpContext.Current.Response.ContentType = "application/ms-word";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName + ".xls");

            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR><center><b>" + vComapanyReportHeader + "<BR>");
            HttpContext.Current.Response.Write(vReportHeader);
            HttpContext.Current.Response.Write("<p style='text-align:left;'>" + vReportParameter + " </p>");
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            //int columnscount = table.Columns.Count;
            foreach (DataTable table in dataset.Tables)
            {
                int columnscount = table.Columns.Count;
                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write("<B>");
                    HttpContext.Current.Response.Write(table.Columns[j].ToString());
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in table.Rows)
                {
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write(row[i].ToString());
                        HttpContext.Current.Response.Write("</Td>");
                    }

                    HttpContext.Current.Response.Write("</TR>");
                }
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        
        public void ExportToExcelWithoutTitle(DataTable table, string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
           
            //Create column and inser rows
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(table, "Sheet1");
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
        }

        public void ExportToExcelWithTitle(DataTable dt, string vComapanyReportHeader, string vReportHeader, string vReportParameter, string FileName)
        {
            var aCode = 65;
            //Create column and inser rows
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Sheet");
                var wsReportNameHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 1, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                wsReportNameHeaderRange.Style.Font.Bold = true;
                wsReportNameHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportNameHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportNameHeaderRange.Merge();
                wsReportNameHeaderRange.Value = vComapanyReportHeader;



                var wsReportDateHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 2, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                wsReportDateHeaderRange.Merge();
                wsReportDateHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportDateHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportDateHeaderRange.Value = string.Format(vReportHeader+"{0}", "");

                var wsReportCreatedByHeaderRange = ws.Range(string.Format("A{0}:{1}{0}", 3, Char.ConvertFromUtf32(aCode + dt.Columns.Count)));
                wsReportCreatedByHeaderRange.Merge();
                wsReportCreatedByHeaderRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wsReportCreatedByHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                wsReportCreatedByHeaderRange.Value = string.Format(vReportParameter+" {0}", "");
                ws.Row(3).InsertRowsBelow(1);

                ws.Row(4).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                ws.Row(4).Style.Border.RightBorder = XLBorderStyleValues.None;
                ws.Row(4).Style.Border.LeftBorder = XLBorderStyleValues.None;

              

                int rowIndex = 5;
                int columnIndex = 0;
                foreach (DataColumn column in dt.Columns)
                {
                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Value = column.ColumnName;
                    ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + columnIndex), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    columnIndex++;

                }
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                rowIndex++;
                foreach (DataRow row in dt.Rows)
                {
                    int valueCount = 0;
                    foreach (object rowValue in row.ItemArray)
                    {
                        ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Value = rowValue;
                        ws.Cell(string.Format("{0}{1}", Char.ConvertFromUtf32(aCode + valueCount), rowIndex)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        valueCount++;
                    }
                    rowIndex++;
                }

           

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.Charset = "";
                HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {

                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.End();
                }
            }
        }
        public void ExportToExcel(DataTable table, string vHeaderParameter, string FileName)
        {
            // string vHeader = "Header Parameter";


            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Buffer = true;
            // HttpContext.Current.Response.ContentType = "application/text";           
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            // HttpContext.Current.Response.ContentType = "application/ms-word";
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
            //text/csv

            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName + "_Excel.xls");

            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.csv");
            // HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.doc");
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
            HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
            HttpContext.Current.Response.Write("<BR>" + vHeaderParameter + "<BR><BR>");       
            HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
            int columnscount = table.Columns.Count;

            for (int j = 0; j < columnscount; j++)
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("<B>");
                HttpContext.Current.Response.Write(table.Columns[j].ToString());
                HttpContext.Current.Response.Write("</B>");
                HttpContext.Current.Response.Write("</Td>");
            }
            HttpContext.Current.Response.Write("</TR>");
            foreach (DataRow row in table.Rows)
            {
                HttpContext.Current.Response.Write("<TR>");
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write(row[i].ToString());
                    HttpContext.Current.Response.Write("</Td>");
                }

                HttpContext.Current.Response.Write("</TR>");
            }
            HttpContext.Current.Response.Write("</Table>");
            HttpContext.Current.Response.Write("</font>");
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        public void ExportToExcelMethod2(DataTable dtSource, string vHeaderParameter)
        {
            StringBuilder sbDocBody = new StringBuilder(); ;
            try
            {

               // string vHeader = "Header Parameter";
                // Declare Styles
                sbDocBody.Append("<style>");
                sbDocBody.Append(".Header {  background-color:Navy; color:#ffffff; font-weight:bold;font-family:Verdana; font-size:12px;}");
                sbDocBody.Append(".SectionHeader { background-color:#8080aa; color:#ffffff; font-family:Verdana; font-size:12px;font-weight:bold;}");
                sbDocBody.Append(".Content { background-color:#ccccff; color:#000000; font-family:Verdana; font-size:12px;text-align:left}");
                sbDocBody.Append(".Label { background-color:#ccccee; color:#000000; font-family:Verdana; font-size:12px; text-align:right;}");
                sbDocBody.Append("</style>");
                //
                StringBuilder sbContent = new StringBuilder(); ;
                sbDocBody.Append("<br><table align=\"center\" cellpadding=1 cellspacing=0 style=\"background-color:#000000;\">");
                sbDocBody.Append("<tr>" + vHeaderParameter + "<td width=\"500\">");
                sbDocBody.Append("<table width=\"100%\" cellpadding=1 cellspacing=2 style=\"background-color:#ffffff;\">");
                //
                if (dtSource.Rows.Count > 0)
                {
                    sbDocBody.Append("<tr><td>");
                    sbDocBody.Append("<table width=\"600\" cellpadding=\"0\" cellspacing=\"2\"><tr><td>");
                    //
                    // Add Column Headers
                    sbDocBody.Append("<tr><td width=\"25\"> </td></tr>");
                    sbDocBody.Append("<tr>");
                    sbDocBody.Append("<td> </td>");
                    for (int i = 0; i < dtSource.Columns.Count; i++)
                    {
                        sbDocBody.Append("<td class=\"Header\" width=\"120\">" + dtSource.Columns[i].ToString().Replace(".", "<br>") + "</td>");
                    }
                    sbDocBody.Append("</tr>");
                    //
                    // Add Data Rows
                    for (int i = 0; i < dtSource.Rows.Count; i++)
                    {
                        sbDocBody.Append("<tr>");
                        sbDocBody.Append("<td> </td>");
                        for (int j = 0; j < dtSource.Columns.Count; j++)
                        {
                            sbDocBody.Append("<td class=\"Content\">" + dtSource.Rows[i][j].ToString() + "</td>");
                        }
                        sbDocBody.Append("</tr>");
                    }
                    sbDocBody.Append("</table>");
                    sbDocBody.Append("</td></tr></table>");
                    sbDocBody.Append("</td></tr></table>");
                }
                //
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                //
                HttpContext.Current.Response.AppendHeader("Content-Type", "application/ms-excel");
                HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment; filename=EmployeeDetails.xls");
                HttpContext.Current.Response.Write(sbDocBody.ToString());
                HttpContext.Current.Response.End();
            }
            catch (Exception ex)
            {
                // Ignore this error as this is caused due to termination of the Response Stream.
            }
        }



        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
      

    }
}
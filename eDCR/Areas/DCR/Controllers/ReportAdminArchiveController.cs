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
    public class ReportAdminArchiveController : Controller
    {
       
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
        ExportToAnother export = new ExportToAnother();
        public ActionResult frmReportAdminArchive()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
       



        public ActionResult Export(DefaultParameterBEO model)
        {
            ReportProductWiseDoctorSampleSummaryDAO ProductWiseDoctorSampleSummaryDAO = new ReportProductWiseDoctorSampleSummaryDAO();
            DataTable dt = new DataTable();
            string vHeader = "Product Wise Doctor Sample Summary";
            string FileName = "Product_Wise_Doctor_Sample_Summary_" + DateTime.Now.ToString("yyyyMMdd");

          
                var tuple = ProductWiseDoctorSampleSummaryDAO.Export(model);
                string vParameter = tuple.Item1;
                dt = tuple.Item2;
                var dataList = tuple.Item3;

                if (dt.Rows.Count > 0)
                {
                    string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                    dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }
            
           
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }

        public ActionResult PromotionalItemExecution(DefaultParameterBEO model)
        {
            ReportAdminDAO reportAdminDAO = new ReportAdminDAO();
          
            string FileName = "Promotional_Item_Execution" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "Promotional Item Execution";

            var tuple = reportAdminDAO.ExportPromotionalItemExecution(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;      

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }


            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }


        public ActionResult DcrSummaryMpo(DefaultParameterBEO model)
        {
            ReportDCRSummaryDAO DCRSummaryDAO = new ReportDCRSummaryDAO();
            string FileName = "DCR_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "MPO DCR Summary";

            var tuple = DCRSummaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }
          
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }
        public ActionResult DvrSummary(DefaultParameterBEO model)
        {
            ReportDVRArchiveDAO primaryDAO = new ReportDVRArchiveDAO();
            string FileName = "DVR_Summary_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "DVR Summary";

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }
          
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }
        public ActionResult DvrMonthly(DefaultParameterBEO model)
        {
            ReportDVRArchiveDAO primaryDAO = new ReportDVRArchiveDAO();
            string FileName = "DVR_Monthly_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "DVR Monthly";

            var tuple = primaryDAO.ExportDvrMonthly(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }

            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }
        public ActionResult DcrDoctorAbsent(DefaultParameterBEO model)
        {
            ReportDCRDoctorAbsentDAO primaryDAO = new ReportDCRDoctorAbsentDAO();
            string vHeader = "Doctor Absent";
            string FileName = "Doctor_Absent_" + DateTime.Now.ToString("yyyyMMdd");

            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);

            }

            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }
        public ActionResult TpLeaveMpoTmRsm(DefaultParameterBEO model)
        {
            ReportMPOLeaveStatementDAO primaryDAO = new ReportMPOLeaveStatementDAO();
            string FileName = "TP_Leave_Monthly_" + DateTime.Now.ToString("yyyyMMdd");
            string vHeader = "TP Leave Statement";
            var tuple = primaryDAO.Export(model);
            string vParameter = tuple.Item1;
            DataTable dt = tuple.Item2;
            var dataList = tuple.Item3;

            if (dt.Rows.Count > 0)
            {
                string vHeaderParameter = vHeader + "\n  " + vParameter + ", " + reportHeader.vPrintDate;
                dt = ExportToAnother.CreateDataTable(dataList);
                //export.ExportToExcel(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
                export.ExportToExcelWithoutTitle(dt, reportHeader.vCompanyName, vHeader, vParameter, FileName);
            }
           
            if (dt.Rows.Count > 0)
            {
                return View();
            }
            else
                return RedirectToAction("MRV", "DCR/MRV");
        }


    }
}
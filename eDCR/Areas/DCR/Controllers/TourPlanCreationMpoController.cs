using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.Areas.DCR.Models.DAO;
using eDCR.DAL.Gateway;
using eDCR.Universal.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class TourPlanCreationMpoController : Controller
    {
        TourPlanCreationMpoDAO primaryDAO = new TourPlanCreationMpoDAO();
        DefaultReportHeaderBEO reportHeader = new DefaultReportHeaderBEO();
       // ExceptionHandler exceptionHandler = new ExceptionHandler();
        ExportToAnother export = new ExportToAnother();

        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();


        public ActionResult frmTourPlanCreationMpo()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetViewData(DefaultParameterBEO model)
        {
            string MarketCode = model.MPGroup.Substring(0, model.MPGroup.Length - 3);
            string Qry = "Select MORNING_LOCATION,EVENING_LOCATION from DOC_DETAIL Where YEAR=" + model.Year + " AND MONTH_NUMBER='" + model.MonthNumber + "' AND MARKET_CODE = '" + MarketCode + "' ";
            DataTable dtLocation = dbHelper.GetDataTable(dbConn.SAConnStrReader(), Qry);

            //DataTable vwMorning = new DataView(dtLocation).ToTable(true, "MORNING_LOCATION");
            //DataTable vwEvening = new DataView(dtLocation).ToTable(true, "EVENING_LOCATION");
            //var mLocation = (from DataRow row in vwMorning.Rows
            //                        select new MorningEveningLocation
            //                        {
            //                            LocationCode = row["MORNING_LOCATION"].ToString(),
            //                            LocationName = row["MORNING_LOCATION"].ToString(),
            //                        }).ToList();

            //var eLocation = (from DataRow row in vwEvening.Rows
            //                        select new MorningEveningLocation
            //                        {
            //                            LocationCode = row["EVENING_LOCATION"].ToString(),
            //                            LocationName = row["EVENING_LOCATION"].ToString(),
            //                        }).ToList();

            DataTable vwMorning = dtLocation.Rows.Count > 0 ? new DataView(dtLocation).ToTable(true, "MORNING_LOCATION") : null;
            DataTable vwEvening = dtLocation.Rows.Count > 0 ? new DataView(dtLocation).ToTable(true, "EVENING_LOCATION") : null;

            var mLocation = vwMorning?.AsEnumerable().Select(row => new MorningEveningLocation
            {
                LocationCode = row["MORNING_LOCATION"].ToString(),
                LocationName = row["MORNING_LOCATION"].ToString(),
            }).ToList();

            var eLocation = vwEvening?.AsEnumerable().Select(row => new MorningEveningLocation
            {
                LocationCode = row["EVENING_LOCATION"].ToString(),
                LocationName = row["EVENING_LOCATION"].ToString(),
            }).ToList();

            var data = primaryDAO.GetViewData(model);      
            return Json(new { data = data, MorningLocations = mLocation, EveningLocations = eLocation }, JsonRequestBehavior.AllowGet);
       
        }

        [HttpPost]
        public ActionResult OperationsMode(TourPlanCreationMpoModelBEO model)
        {
            try
            {
                if (primaryDAO.TpCreation(model))
                {
                    primaryDAO.IUMode = "I";
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmTourPlanNew");
            }
            catch (Exception e)
            {
                return null;// exceptionHandler.ErrorMsg(e);
            }
        }
    }
}
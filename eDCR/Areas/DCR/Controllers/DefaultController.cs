using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class DefaultController : Controller
    {
        

        DefaultDAO primaryDAO = new DefaultDAO(); 
        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

     

        [HttpPost]
        public ActionResult GetMPOPopupList(string TerritoryManagerID)
        {
            var data = primaryDAO.GetMPOPopupList(TerritoryManagerID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


     
        [HttpGet]
        public ActionResult GetUserWiseMPOList()
        {
            var data = primaryDAO.GetUserWiseMPOList();
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
       
        [HttpGet]
        public ActionResult GetAccompanyWith()
        {
            string TerritoryManagerID = ""; 
           var data = primaryDAO.GetAccompanyWith(TerritoryManagerID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


       

        //---------------Cascading
        [HttpGet]
        public ActionResult GetRegion()
        {
            var data = primaryDAO.GetRegion();
            return Json(data, JsonRequestBehavior.AllowGet);
        }  
        [HttpPost]
        public ActionResult GetTerritoryFromRegion(string RegionCode)
        {
            var data = primaryDAO.GetTerritoryFromRegion(RegionCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //--------------------
       

        [HttpPost]
        public ActionResult GetMPOFromTM(string TerritoryManagerID)
        {
            var data = primaryDAO.GetMPOFromTM(TerritoryManagerID);
            return Json(data, JsonRequestBehavior.AllowGet);
        }






        [HttpGet]
        public ActionResult GetEmpForSup()
        {
            var data = primaryDAO.GetEmpForSup();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEmpForOwnSup()
        {
            var data = primaryDAO.GetEmpForOwnSup();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmpForOwnSupfrmDesignation(string Designation)
        {
            var data = primaryDAO.GetEmpForOwnSupfrmDesignation(Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEmpCodeForOwnSupfrmDesignation(string Designation)
        {
            var data = primaryDAO.GetEmpCodeForOwnSupfrmDesignation(Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEmpForOwnSupfrmRegionDesignation(string RegionCode,string Designation)
        {
            var data = primaryDAO.GetEmpForOwnSupfrmRegionDesignation(RegionCode,Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmpForOwnSupfrmRegionDesignationFnIn(string RegionCode, string Designation)
        {
            var data = primaryDAO.GetEmpForOwnSupfrmRegionDesignationFnIn(RegionCode, Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmpForExpenseBillfrmRegionDesignationFnIn(string RegionCode, string Designation)
        {
            var data = primaryDAO.GetEmpForExpenseBillfrmRegionDesignationFnIn(RegionCode, Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmpForDesignationLocCodeSession(string RegionCode, string Designation)
        {
            var data = primaryDAO.GetEmpForDesignationLocCodeSession(RegionCode, Designation);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetGenMonth()
        {
            var data = primaryDAO.GetMonth();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductItem(ProductInfo model)
        {
            var data = primaryDAO.GetProductItem(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductItemMaster(ProductInfo model)
        {
            var data = primaryDAO.GetProductItemMaster(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }





        //For Support Service Where Delete TP/DVR/PWDS/GWDS
        [HttpPost]
        public ActionResult GetEmpForDesignation(string Designation,string OperationMode)
        {
            var data = primaryDAO.GetEmpForDesignation(Designation, OperationMode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        //For Depot Manager
        [HttpGet]
        public ActionResult GetDepot()
        {
            var data = primaryDAO.GetDepot();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //For Depot Manager
        [HttpPost]
        public ActionResult GetEmpForOwnSupfrmDepotDesignation(string DepotCode, string Designation,string SBU)
        {
            var data = primaryDAO.GetEmpForOwnSupfrmDepotDesignation(DepotCode, Designation, SBU);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLeftEmpForDepotDesignation(string DepotCode, string MonthYear)
        {
            var data = primaryDAO.GetLeftEmpForDepotDesignation(DepotCode, MonthYear);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSpecificEmpForDepotDesignation(string DepotCode, string Year, string MonthNumber)
        {
            var data = primaryDAO.GetSpecificEmpForDepotDesignation(DepotCode, Year, MonthNumber);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //For Depot Manager
        [HttpGet]
        public ActionResult GetGenPreviousMonth()
        {
            var data = primaryDAO.GetPreviousMonth();
            return Json(data, JsonRequestBehavior.AllowGet);
        }













        //For Archive Report
        [HttpPost]
        public ActionResult GetRegionForArchive(DefaultBEL model)
        {
            var data = primaryDAO.GetRegionForArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetEmployeeForArchive(DefaultBEL model)
        {
            var data = primaryDAO.GetEmployeeForArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult GetTerritoryForArchive(DefaultBEL model)
        {
            var data = primaryDAO.GetTerritoryForArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetMpoForArchive(DefaultBEL model)
        {
            var data = primaryDAO.GetMpoForArchive(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }









        //Data for HR Data upload
        [HttpGet]
        public ActionResult GetDivision()
        {
            DefaultBEL model=new DefaultBEL();
            var data = primaryDAO.GetDivision(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDepot(DefaultBEL model)
        {
            var data = primaryDAO.GetDepot(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetRegion(DefaultBEL model)
        {
            var data = primaryDAO.GetRegion(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTerritory(DefaultBEL model)
        {
            var data = primaryDAO.GetTerritory(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetMarket(DefaultBEL model)
        {
            var data = primaryDAO.GetMarket(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
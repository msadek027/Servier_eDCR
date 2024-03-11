using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ProductWiseDoctorSelectController : Controller
    {
        ProductWiseDoctorSelectDAO primaryDAO = new ProductWiseDoctorSelectDAO();
        ExceptionHandler exceptionHandler = new ExceptionHandler();
       
        public ActionResult frmProductWiseDoctorSelect()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmProductWiseDoctorSelect2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        

        [HttpPost]
        public ActionResult GetDetailDataForSaveUpdate(ProductWiseDoctorSelectMaster model)
        {
            var data = primaryDAO.GetDetailDataForSaveUpdate(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ProductList(string MPGroup)
        {           
            var data = primaryDAO.GetProductList(MPGroup);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDoctor(ProductWiseDoctorSelectMaster model)
        {
            var data = primaryDAO.GetDoctor(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetPopupView(DefaultParameterBEO model)
        {
            var data = primaryDAO.GetPopupView(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public ActionResult OperationsMode(ProductWiseDoctorSelectBEL master)
        {
            try
            {
                if (primaryDAO.SaveUpdate(master))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmProductWiseDoctorSelect");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }

	}
}
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
    public class GiftWiseDoctorSelectController : Controller
    {
       
        ExceptionHandler exceptionHandler = new ExceptionHandler();

        GiftWiseDoctorSelectDAO primaryDAO = new GiftWiseDoctorSelectDAO();
        public ActionResult frmGiftWiseDoctorSelect()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmGiftWiseDoctorSelect2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
          [HttpPost]
         public ActionResult GetDetailDataForSaveUpdate(GiftWiseDoctorSelectBEL model)
         {
             var data = primaryDAO.GetDetailDataForSaveUpdate(model);
               return Json(data, JsonRequestBehavior.AllowGet);
         }
          [HttpPost]
          public ActionResult GetDoctor(ProductWiseDoctorSelectMaster model)
          {
              var data = primaryDAO.GetDoctor(model);
              return Json(data, JsonRequestBehavior.AllowGet);
          }
          [HttpPost]
          public ActionResult GetGiftItem(string MPGroup)
          {
              var data = primaryDAO.GetGiftItem(MPGroup);
              return Json(data, JsonRequestBehavior.AllowGet);
          }

           [HttpPost]
          public ActionResult GetPopupView(DefaultParameterBEO model)
          {
              var data = primaryDAO.GetPopupView(model);
              return Json(data, JsonRequestBehavior.AllowGet);
          }

          public ActionResult OperationsMode(GiftWiseDoctorSelectBEL master)
          {
              try
              {
                  if (primaryDAO.SaveUpdate(master))
                  {
                      return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                  }
                  else
                      return View("frmGiftWiseDoctorSelect");
              }
              catch (Exception e)
              {
                  return exceptionHandler.ErrorMsg(e);

              }
          } 
    }
}
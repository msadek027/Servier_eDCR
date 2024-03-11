using eDCR.Areas.DCR.Models.BEL;
using eDCR.Areas.DCR.Models.DAL.DAO;
using eDCR.DAL.DAO;
using eDCR.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class PromotionalItemViewEditController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        PromotionalItemViewDAO promotionalItemViewDAO = new PromotionalItemViewDAO();
        Object data = null;
        // GET: /DCR/PromotionalItemView/
        public ActionResult frmPromotionalItemViewEdit()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        public ActionResult frmPromotionalItemViewEdit2()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }



        [HttpPost]
        public ActionResult GetProduct(PromotionalItemViewDetail model)
        {
            var data = promotionalItemViewDAO.GetProduct(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public ActionResult MainGridDataNew(DefaultParameterBEO model)
        {
            data = promotionalItemViewDAO.MainGridDataNew(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OperationsModeSaveSingle(PromotionalItemViewBEL model)
        {
            try
            {
                if (promotionalItemViewDAO.SaveUpdateSingle(model))
                {
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = promotionalItemViewDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }

        [HttpPost]
        public ActionResult OperationsModeSaveNew(PromotionalItemViewBEL model)
        {
            try
            {
                if (promotionalItemViewDAO.SaveUpdateNew(model))
                {
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = promotionalItemViewDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }
        [HttpPost]
        public ActionResult OperationsModeVoidSingle(PromotionalItemViewBEL model)
        {
            try
            {
                if (promotionalItemViewDAO.SaveUpdateVoidSingle(model))
                {
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = promotionalItemViewDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }

        [HttpPost]
        public ActionResult OperationsModeDirtyAllSaveUpdate(PromotionalItemViewBEL model)
        {
            try
            {
                if (promotionalItemViewDAO.DirtyAllSaveUpdate(model))
                {
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = promotionalItemViewDAO.IUMode, Status = "Yes" });
                }
                else
                    return Json(new { ID = promotionalItemViewDAO.MaxID, Mode = "No", Status = "No" });
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);

            }
        }


       
	}
}
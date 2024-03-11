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
    public class DeleteTpDvrPwdsGwdsBillMpoTmRsmController : Controller
    {
        ExceptionHandler exceptionHandler = new ExceptionHandler();
        DeleteTpDvrPwdsGwdsBillMpoTmRsmDAO primaryDAO = new DeleteTpDvrPwdsGwdsBillMpoTmRsmDAO();
        public ActionResult frmDeleteTpDvrPwdsGwdsBillMpoTmRsm()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

        [HttpPost]
        public ActionResult OperationsMode(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            try
            {
                if (primaryDAO.DeleteTpDvrPwdsGwdsBillMpoTmRsm(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmDeleteTpDvrPwdsGwdsBillMpoTmRsm");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }
        [HttpPost]
        public ActionResult OperationsModeApproveWaiting(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            try
            {
                if (primaryDAO.ApproveWaitingTpDvrPwdsGwdsBillMpoTmRsm(model))
                {
                    return Json(new { ID = primaryDAO.MaxID, Mode = primaryDAO.IUMode, Status = "Yes" });
                }
                else
                    return View("frmDeleteTpDvrPwdsGwdsBillMpoTmRsm");
            }
            catch (Exception e)
            {
                return exceptionHandler.ErrorMsg(e);
            }
        }




        object data;


        [HttpPost]
        public ActionResult GetViewDataAsInput(DeleteTpDvrPwdsGwdsBillMpoTmRsmBEO model)
        {
            switch (model.OperationMode)
            {
                case "TP":
                    data = primaryDAO.GetMpoTP(model);
                    break;

                case "DVR":
                    data = primaryDAO.GetMpoDVR(model);
                    break;

                case "PWDS":
                    data = primaryDAO.GetMpoPWDS(model);
                    break;

                case "GWDS":
                    data = primaryDAO.GetMpoGWDS(model);
                    break;

                case "BILL":
                    data = primaryDAO.GetMpoTmRsmBILL(model);
                    break;

                case "TmRsmTP":
                    data = primaryDAO.GetTmRsmTP(model);
                    break;

                case "TmRsmBILL":
                    data = primaryDAO.GetMpoTmRsmBILL(model);
                    break;


                default:
                    data = primaryDAO.GetMpoTP(model);
                    break;

            }

            return Json(new { dt1 = data }, JsonRequestBehavior.AllowGet);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class ExpenseBillMpoTmRsmAdminController : Controller
    {
        // GET: DCR/ExpenseBillMpoTmRsmAdmin
        public ActionResult frmExpenseBillMpoTmRsmAdmin()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }
        public ActionResult frmExpenseBillMpoTmRsmAdminDetail()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" });
        }

    }
}
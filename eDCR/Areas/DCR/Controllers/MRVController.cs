using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eDCR.Areas.DCR.Controllers
{
    public class MRVController : Controller
    {
        //
        // GET: /DCR/MRV/
        public ActionResult MRV()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.Title = "MRV";
                return View();
            }
            return RedirectToAction("Index", "LoginRegistration", new { area = "" }); ;
        }
        public ActionResult LoginMsg()
        {
            return RedirectToAction("Index", "LoginRegistration", new { area = "" }); ;
        }
    }
}
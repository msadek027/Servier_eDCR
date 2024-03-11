using System.Web.Mvc;

namespace eDCR.Areas.DCR
{
    public class DCRAreaRegistration : AreaRegistration 
    {

        public override string AreaName 
        {
            get 
            {
                return "DCR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DCR_default",
                "DCR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }


    }
}
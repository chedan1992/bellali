using System.Web.Mvc;

namespace WebPortalAdmin.Areas.CACAI
{
    public class CACAIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CACAI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CACAI_default",
                "CACAI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

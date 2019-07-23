using System.Web.Mvc;

namespace WebPortalAdmin.Areas.CarWash
{
    public class CarWashreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CarWash";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "CarWash_default",
                "CarWash/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

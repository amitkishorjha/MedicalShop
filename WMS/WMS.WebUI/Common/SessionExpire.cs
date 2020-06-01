
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace WMS.WebUI.Common
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["user"] == null)
            {
                FormsAuthentication.SignOut();
                filterContext.Result =
               new RedirectToRouteResult(new RouteValueDictionary
                 {
             { "action", "Login" },
            { "controller", "UserMasters" },
            { "returnUrl", filterContext.HttpContext.Request.RawUrl}
                  });

                return;
            }
        }

    }
}
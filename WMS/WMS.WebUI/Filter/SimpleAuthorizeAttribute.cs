using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using WMS.Service.Interface;

namespace WMS.WebUI.Filter
{
    public class SimpleAuthorizeAttribute : AuthorizeAttribute
    {
        public IActionMasterService ActionMasterService { get; set; }

        public IUserMasterService UserMasterService { get; set; }

        public IUserRoleMappingService UserRoleMappingService { get; set; }

        public IRoleActionMappingService RoleActionMappingService { get; set; }

        public IRoleMasterService RoleMasterService { get; set; }

        private string routeDataController = "controller";

        public string RouteDataController
        {
            get { return routeDataController; }
            set { routeDataController = value; }
        }

        private string routeDataAction = "action";

        public string RouteDataAction
        {
            get { return routeDataAction; }
            set { routeDataAction = value; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            RequestContext requestContext = httpContext.Request.RequestContext;

            if (HttpContext.Current.Session["RoleId"] == null)
            {
                return false;
            }

            var roleId = new Guid(HttpContext.Current.Session["RoleId"].ToString());

            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
                return false;

            if (!requestContext.RouteData.Values.ContainsKey(RouteDataController))
                throw new ApplicationException("RouteDataKey " + RouteDataController + " does not exist in the current RouteData");

            if (!requestContext.RouteData.Values.ContainsKey(RouteDataAction))
                throw new ApplicationException("RouteDataKey " + RouteDataAction + " does not exist in the current RouteData");

            if (!(user.Identity is ClaimsIdentity))
                return false;

            var username = user.Identity.Name;
            var userInfo = UserMasterService.GetAll().Where(x => x.Username == username).FirstOrDefault();

            if (userInfo == null)
                return false;

            var UserRole = (from ur in UserRoleMappingService.GetAll().ToList().Where(x => x.UserMasterId == userInfo.UniqueId)
                            join ro in RoleMasterService.GetAll().ToList() on ur.RoleMasterId equals ro.UniqueId
                            where ro.UniqueId == roleId && ro.IsActive == true
                            select ro).FirstOrDefault();

            if (UserRole.Name == "Admin")
                return true;

            string activity = requestContext.RouteData.Values[RouteDataController].ToString() + "/" + requestContext.RouteData.Values[RouteDataAction].ToString();

            var roleActions = (from ur in RoleActionMappingService.GetAll().ToList()
                               join ro in ActionMasterService.GetAll().ToList() on ur.ActionMasterId equals ro.UniqueId
                               where ur.RoleMasterId == roleId && ro.IsActive == true
                               select ro.AccessCode).ToList();

            return roleActions.Contains(activity);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
        {
            if (HttpContext.Current.Session["RoleId"] == null)
            {
                FormsAuthentication.SignOut();

                filterContext.Result =
                new RedirectToRouteResult(new RouteValueDictionary
                  {
             { "action", "Login" },
            { "controller", "UserMasters" }
                   });
            }
            else
            {
                if (filterContext.HttpContext.User != null && filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    controller = "UserMasters",
                                    action = "Unauthorized"
                                })
                            );
                }
                else
                {
                    base.HandleUnauthorizedRequest(filterContext);
                }
            }
        }
    }
}
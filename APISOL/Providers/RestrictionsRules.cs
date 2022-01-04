using System.Web.Http;
using System.Web.Http.Controllers;
using System.Linq;
using System;
using System.Security.Claims;
using System.Collections.Generic;

namespace APISOL.Providers
{
    public class RouteAuthorize : AuthorizeAttribute
    {
        protected string[] accesses;

        public RouteAuthorize(params object[] accessRequired)
        {
            if (!(accessRequired.Any(p => p.GetType().BaseType != typeof(Enum))))
            {
                this.accesses = accessRequired.Select(
                    a =>
                    Enum.GetName(a.GetType(), a)
                ).ToArray();
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal as ClaimsPrincipal;

            List<string> userClaims = new List<string>();
            List<string> routeClaims = new List<string>();

            foreach (var claim in user.Claims)
                if (claim.Type == "permissions")
                        userClaims = claim.Value.Split(',').ToList();

            if (this.accesses != null && this.accesses.Count() > 0)
            {
                foreach (string accessCode in this.accesses)
                    if (userClaims.Contains(accessCode))
                        return true;
            }
            else
                return true;

            return false;
        }
    }

    public class Basic : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
                return false;

            return ActiveDirectoryOAuthProvider.isUserAuthenticated();
        }
    }
}
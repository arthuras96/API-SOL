using Application.Authenticate;
using Application.Authenticate.Entities;
using Application.Authenticate.Models;
using Application.General.Models;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APISOL.Providers
{
    public class ActiveDirectoryOAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext c)
        {
            c.Validated();

            return Task.FromResult<object>(c);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var userName = context.UserName;
            var password = context.Password;
            
            AuthenticateService service = new AuthenticateService();

            UserModel user = service.AuthenticateUser(userName, password);

            if (user.iduser > 0)
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                List<string> accounts = new List<string>();
                

                foreach (LabelValueModel account in user.idaccounts)
                    accounts.Add(account.value);

                List<string> permissionsClaims = GetPermissions(user.permissions);

                identity.AddClaim(new Claim(ClaimTypes.Role, user.dsprofile));
                identity.AddClaim(new Claim("email", user.email));
                identity.AddClaim(new Claim("permissions", string.Join(",", permissionsClaims)));
                identity.AddClaim(new Claim("permissionsaccount", JsonConvert.SerializeObject(user.permissions)));
                identity.AddClaim(new Claim("accounts", JsonConvert.SerializeObject(accounts)));
                identity.AddClaim(new Claim("iduser", user.iduser.ToString()));

                List<int> filterPermissions = new List<int>();
                foreach(PermissionModel permission in user.permissions)
                    if (permission.idaccount == Convert.ToInt32(user.idaccounts[0].value))
                        filterPermissions.Add(permission.permission);

                var properties = new AuthenticationProperties(new Dictionary<string, string>
                {
                    {
                        "name", user.name
                    },
                    {
                        "email", user.email
                    },
                    {
                        "permissions", JsonConvert.SerializeObject(filterPermissions)
                    },
                    {
                        "permissionsaccount", JsonConvert.SerializeObject(user.permissions)
                    },
                    {
                        "lastlogin", user.lastlogin.ToString("yyyy-MM-dd HH:mm:ss.fff")
                    },
                    {
                        "idprofile", user.idprofile.ToString()
                    },
                    {
                        "idaccounts", JsonConvert.SerializeObject(user.idaccounts)
                    },
                    {
                        "idselectaccount", user.idaccounts[0].value
                    }
                });

                var ticket = new AuthenticationTicket(identity, properties);
                context.Validated(ticket);
            }

            return Task.FromResult<object>(context);
        }

        public static bool isUserAuthenticated()
        {
            if (System.Web.HttpContext.Current.User != null)
            {
                bool isAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
                if (isAuthenticated)
                    return true;
            }
            return false;
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(property.Key, property.Value);

            return Task.FromResult<object>(context);
        }

        private List<string> GetPermissions(List<PermissionModel> permissions)
        {
            List<string> permissionsClaims = new List<string>();

            foreach(PermissionModel permission in permissions)
            {
                var enumValue = (Permissions.PermissionEnum)permission.permission;
                permissionsClaims.Add(enumValue.ToString());
            }

            return permissionsClaims;
        }
    }
}
using APISOL.Providers;
using Application.Authenticate;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

namespace APISOL
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            HttpConfiguration config = new HttpConfiguration();

            WebApiConfig.Register(config);

            ConfigureOAuth(app);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);
        }

        //public void ConfigureOAuth(IAppBuilder app)
        //{
        //    var myProvider = new AuthenticateService();
        //    OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
        //    {
        //        AllowInsecureHttp = true,
        //        TokenEndpointPath = new PathString("/api/authenticate/token"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
        //        Provider = myProvider
        //    };

        //    // Token Generation
        //    app.UseOAuthAuthorizationServer(options);
        //    app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        //}

        public void ConfigureOAuth(IAppBuilder app)
        {

            string user = System.Web.HttpContext.Current.User?.Identity?.Name;

            // Para utilizar o Header "Authorization" nas requisições
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            // Ativar o método para gerar o OAuth Token
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/api/authenticate/token"),
                Provider = new ActiveDirectoryOAuthProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true
            });
        }
    }
}
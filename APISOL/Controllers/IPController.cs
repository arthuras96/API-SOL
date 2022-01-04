using APISOL.Providers;
using Application.Authenticate.Entities;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;


namespace APISOL.Controllers
{
    /// <summary>API desenvolvida para facilitar a captura do IP de quem acessa, sem depender de API's terceiras.</summary>
    public class IPController : ApiController
    {
        /// <summary>Este método retorna o IP de quem o consumiu.</summary>
        /// <response code="200"></response>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        //[AllowAnonymous]
        [Basic]
        [Authorize(Roles = "user")]
        [RouteAuthorize(Permissions.PermissionEnum.evaluationrecord)]
        [HttpGet]
        public string GetIP()
        {
            return GetClientIp();
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        //[AllowAnonymous]
        [Basic]
        [Authorize(Roles = "admin")]
        [RouteAuthorize(Permissions.PermissionEnum.evaluationcorrect)]
        [HttpGet]
        public IHttpActionResult Teste()
        {
            return Ok();
        }
    }
}
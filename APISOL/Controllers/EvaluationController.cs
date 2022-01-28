using APISOL.Providers;
using Application.Authenticate.Entities;
using Application.Authenticate.Models;
using Application.Evaluation;
using Application.Evaluation.Models;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace APISOL.Controllers
{
    public class EvaluationController : ApiController
    {
        private int iduser;
        private List<string> accounts;
        private List<PermissionModel> permissions;
        private bool unrestricted;
        private int selectedAccount;
        private bool authorized;

        private void VerifyParameters(List<int> permissionsrequired, System.Net.Http.Headers.HttpRequestHeaders headers)
        {
            this.authorized = true;
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;

            if (headers.Contains("Select-Account"))
                selectedAccount = int.Parse(headers.GetValues("Select-Account").First());

            try
            {
                if (identity != null)
                {
                    iduser = int.Parse(identity.FindFirst("iduser").Value);
                    string accountJson = Convert.ToString(identity.FindFirst("accounts").Value);
                    string permissionsAccount = Convert.ToString(identity.FindFirst("permissionsaccount").Value);
                    accounts = JsonConvert.DeserializeObject<List<string>>(accountJson);
                    permissions = JsonConvert.DeserializeObject<List<PermissionModel>>(permissionsAccount);
                }
                else
                    authorized = false;
            }
            catch (Exception e)
            {
                authorized = false;
            }

            if (authorized)
            {
                authorized = false;
                foreach(PermissionModel permission in permissions)
                {
                    if(permission.idaccount == selectedAccount && permissionsrequired.IndexOf(permission.permission) != -1)
                    {
                        unrestricted = permission.unrestricted;
                        authorized = true;
                    }
                }
            }
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [Basic]
        [Authorize(Roles = "admin,academic,broker")]
        [RouteAuthorize(Permissions.PermissionEnum.evaluationcorrect)]
        [HttpGet]
        public IHttpActionResult GetListEvaluations()
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
            VerifyParameters(new List<int>() 
            {
                (int) Permissions.PermissionEnum.evaluationcorrect
            }, 
            headers);

            if (!authorized)
                return Unauthorized();

            EvaluationService service = new EvaluationService();
            var evaluations = service.GetEvaluations(selectedAccount, iduser, unrestricted);
            return Ok(evaluations);
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [Basic]
        [Authorize(Roles = "admin,academic,broker")]
        [RouteAuthorize(Permissions.PermissionEnum.evaluationcorrect)]
        [HttpGet]
        public IHttpActionResult GetEvaluationsStudent(int idevaluation)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
            VerifyParameters(new List<int>()
            {
                (int) Permissions.PermissionEnum.evaluationcorrect
            },
            headers);

            if (!authorized)
                return Unauthorized();

            EvaluationService service = new EvaluationService();
            var evaluations = service.GetEvaluationsStudent(idevaluation, selectedAccount, iduser, unrestricted);
            return Ok(evaluations);
        }

        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(string))]
        [Basic]
        [Authorize(Roles = "admin,academic,broker")]
        [RouteAuthorize(Permissions.PermissionEnum.evaluationcorrect)]
        [HttpPut]
        public IHttpActionResult PutStudentGrade([FromBody]EvaluationStudentModel evaluation, int idevaluation)
        {
            System.Net.Http.Headers.HttpRequestHeaders headers = Request.Headers;
            VerifyParameters(new List<int>()
            {
                (int) Permissions.PermissionEnum.evaluationcorrect
            },
            headers);

            if (!authorized)
                return Unauthorized();

            EvaluationService service = new EvaluationService();
            var evaluations = service.PutStudentGrade(evaluation, selectedAccount, iduser, idevaluation);
            return Ok(evaluations);
        }

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult SendEmails()
        {
            EvaluationService service = new EvaluationService();
            service.SendEmails();
            return Ok();
        }
    }
}
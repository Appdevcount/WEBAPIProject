using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;//Primary WEB API Assembly - Added manually
using System.Net;//Added manually
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace WEBAPIProject.AttributeRepository
{
    public class ModelValidatorAttribute : ActionFilterAttribute
    {//This Attrubute is contructed for Model validation before entering into particular action , instead of doing after getting into particular Action 
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                    actionContext.ModelState);
            }

            //base.OnActionExecuting(actionContext);
        }

    }

    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {//This Attribute is constructed for handling Exception using filter before entering inside action
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string errMessage;
            if (actionExecutedContext.Exception.InnerException.Message == null)
            {
                errMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                errMessage = actionExecutedContext.Exception.InnerException.Message;
            }
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(errMessage),
                ReasonPhrase = errMessage
            };
            actionExecutedContext.Response = response;
            //base.OnException(actionExecutedContext);
        }
    }

    public class CustomGlobalExceptionHandler : ExceptionHandler
    {//This Attribute is constructed for handling excptions inside exception filter ,routing,  Message handler, controller constructor
        public override void Handle(ExceptionHandlerContext context)
        {
            // Access Exception using context.Exception;  
            string errMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errMessage
                });
            response.Headers.Add("X-Error", errMessage);
            context.Result = new ResponseMessageResult(response);
            //base.Handle(context);
        }
    }

    //Automatic Redirection to Https for method calls with custom attribute for redirection
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Found);
                actionContext.Response.Content = new StringContent
                    ("<p>Use https instead of http</p>", Encoding.UTF8, "text/html");

                UriBuilder uriBuilder = new UriBuilder(actionContext.Request.RequestUri);
                uriBuilder.Scheme = Uri.UriSchemeHttps;
                uriBuilder.Port = 44337;

                actionContext.Response.Headers.Location = uriBuilder.Uri;
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
        }
    }

    //Basic Authentication Filter attribute - with Authorization: Basic username:password header , 
    //Note : username:password combination is base64 encoded
    public class UserAuthenticator
    {
        public static bool Login(string username, string password)
        {
            //using (EmployeeDBEntities entities = new EmployeeDBEntities())
            //{
            //    return entities.Users.Any(user =>
            //           user.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            //                              && user.Password == password);
            //}
            return true;// Remove this,it was intentionally added to return some Boolean value for compiler error[This line is just to avoid error]
        }
    }
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers
                                            .Authorization.Parameter;
                string decodedAuthenticationToken = Encoding.UTF8.GetString(
                    Convert.FromBase64String(authenticationToken));
                string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');
                string username = usernamePasswordArray[0];
                string password = usernamePasswordArray[1];

                if (UserAuthenticator.Login(username, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(username), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }

    //For ASP.NET Identity Authentication
    //Need to pass in the Request body with user credentials  to get the Bearer token which can be used for requesting the resource
    //username:    password:    grant_type: 'password' // username=aaa&password=aaa&grant_type=password
    //While requesting the resource , need pass in the bearer token in Authorization:Bearer 24ut3gh919h5139gh header
    //To get logged in User details - can use the User.Identity object 
    //Instead of using User.Identity, we can also use RequestContext.Principal.Identity
    
}
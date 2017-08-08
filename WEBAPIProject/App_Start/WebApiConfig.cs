using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using WEBAPIProject.AttributeRepository;
using System.Web.Http.ExceptionHandling;
using System.Net.Http.Headers;

namespace WEBAPIProject
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                //routeTemplate: "api/{controller}/{id}",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //To Add Custom Model Validation Attribute globally
            config.Filters.Add(new ModelValidatorAttribute());
            //To Add Custom Exception Filter by inheriting Exception filter attribute
            //config.Filters.Add(new CustomExceptionFilterAttribute());
            //To Add Custom Exception handler as above , but by inheriting Exception handler class
            //config.Services.Replace(typeof(IExceptionHandler), new CustomGlobalExceptionHandler());


            //config.Formatters.Remove(config.Formatters.XmlFormatter);//to support only JSON response 
            //config.Formatters.Remove(config.Formatters.JsonFormatter);//to support only XML response
            //config.Formatters.JsonFormatter.SupportedMediaTypes
            //    .Add(new MediaTypeHeaderValue("text/html"));//This tells ASP.NET Web API to use JsonFormatter when a request is made for text/html which is the default for most browsers

            //CORS Issue
            //Install - Package Microsoft.AspNet.WebApi.Cors
            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");//Origin,headers,methods
            //config.EnableCors();
            //or can be on individual controllers or methods as [EnableCorsAttribute("*", "*", "*")]

            //config.Filters.Add(new RequireHttpsAttribute());//Custom Https redirection attribute
            //config.Filters.Add(new BasicAuthenticationAttribute());//Custom Basic Authentication attribute
        }
    }
}

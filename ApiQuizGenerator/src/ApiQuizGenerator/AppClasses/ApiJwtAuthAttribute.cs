using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiQuizGenerator.AppClasses
{
    public class ApiJwtAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        string token = null;
        bool validToken = false;;
            try 
            {
                token = filterContext.HttpContext.Request.Cookies[ApiAuthentication.AUTH_COOKIE_KEY].ToString();
                var tokenProvider = new TokenProvider();
                if (tokenProvider.ValidateToken(token)) 
                {
                    validToken = true;
                }
            }
            catch (Exception ex) 
            {
                validToken = false;
            }
            
            if (!validToken)
            {
                var response = filterContext.HttpContext.Response;
                response.StatusCode = 403;
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}


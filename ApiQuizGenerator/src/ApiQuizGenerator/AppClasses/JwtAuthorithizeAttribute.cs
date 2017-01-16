using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiQuizGenerator.AppClasses
{
    public class JwtAuthorizeAttribute : ActionFilterAttribute
    {
        private ITokenProvider _tokenProvider { get; set; }
            
        public JwtAuthorizeAttribute()
        {
            _tokenProvider = new TokenProvider();
        }
        
        public JwtAuthorizeAttribute(ITokenProvider tokenProvider) 
        {
            _tokenProvider = tokenProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string token = null;
            bool validToken = false;;
            
            try 
            {
                token = filterContext.HttpContext.Request.Cookies[ApiAuthentication.AUTH_COOKIE_KEY].ToString();
                if (_tokenProvider.ValidateToken(token)) 
                {
                    validToken = true;
                }
            }
            catch (Exception) 
            {
                validToken = false;
            }
            
            if (!validToken)
            {
                var response = filterContext.HttpContext.Response;
                response.StatusCode = 404;
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}


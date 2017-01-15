using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiQuizGenerator.AppClasses
{
    public class ApiAuthentication
    {
        public static string AUTH_COOKIE_KEY { get { return ".AspNetCore.Identity.Application"; } }

        private Controller _Controller { get; set;}
        internal string AuthCookie 
        {
            get 
            {
                string _authCookie = string.Empty;
                if (_Controller != null)
                    _authCookie = _Controller.HttpContext.Request.Cookies[AUTH_COOKIE_KEY];                  
            
                return _authCookie;
            } 
        }

        public bool Authenticated 
        {
            get
            {
                bool _authenticated = false;
                if (_Controller != null)
                    _authenticated = _Controller.HttpContext.User.Identity.IsAuthenticated;
                
                return _authenticated;
            }     
        }

        public ApiAuthentication(Controller controller) 
        {
            _Controller = controller;
        }
    }
}
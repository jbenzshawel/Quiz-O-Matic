using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;

namespace ApiQuizGenerator.AppClasses
{
    public class BaseController : Controller
    {
        internal IDataService _DataService { get; set; }

        internal ApiAuthentication _Authenticaiton { get; set; }

        internal ITokenProvider _TokenProvider { get; set; }
        public BaseController(IDataService _dataService, ITokenProvider tokenProvider) 
        {
            _DataService = _dataService;
            _Authenticaiton = new ApiAuthentication(this);
            _TokenProvider = tokenProvider;
        }
    }
}

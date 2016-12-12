using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;

namespace ApiQuizGenerator.Controllers
{
    public class BaseController : Controller
    {
        internal IDataService _DataService { get; set; }

        public BaseController(IDataService _dataService) 
        {
            _DataService = _dataService;
        }
    }
}
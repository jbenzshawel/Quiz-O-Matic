using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

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
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;
using ApiQuizGenerator.AppClasses;
using ApiQuizGenerator.Controllers.Shared;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class ResponsesController : DALController<Response>
    {
        public ResponsesController(IDataService _dataService, ITokenProvider _tokenProvider) :
            base(_dataService, _tokenProvider)
        {
            _DataService = _dataService;
            _TokenProvider = _tokenProvider;
        }

    }
}
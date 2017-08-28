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
    public class QuizesController : DALController<Quiz>
    {
        public QuizesController(IDataService _dataService, ITokenProvider _tokenProvider) :
            base(_dataService, _tokenProvider)
        {
            _DataService = _dataService;
            _TokenProvider = _tokenProvider;
        }

        // GET api/quizes/list/{onlyVisible:bool?}
        [HttpGet("[action]/{onlyVisible:bool?}")]
        public async Task<List<Quiz>> List(bool? onlyVisible = null) 
        {
            if (onlyVisible == null)
                onlyVisible = true;
            
            return await _DataService.Quizes.All(onlyVisible.ToString().ToLower()); 
        }
    }
}

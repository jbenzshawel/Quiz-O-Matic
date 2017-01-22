using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class QuizesController : BaseController
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

        // GET api/quizes/{id}
        [HttpGet("{id}")]
        public async Task<Quiz> Get(Guid id)
        {
            Quiz quiz = await _DataService.Quizes.Get(id.ToString());

            if (quiz == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return quiz;
        }

        // POST api/quizes/
        [HttpPost]        
        public async Task Post([FromBody]Quiz quizModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            bool saveStatus = await _DataService.Quizes.Save(quizModel);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        // PUT api/quizes/{id}
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody]Quiz quizModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            Quiz quizSearch = await _DataService.Quizes.Get(id.ToString());
            if (quizSearch != null)
            {
                quizModel.Id = id;

                bool saveStatus = await _DataService.Quizes.Save(quizModel);    
                if (!saveStatus) 
                {
                    Response.StatusCode = 500; // internal server error
                }         
            }
            else 
            {
                Response.StatusCode = 404; // not found
            }
        }

        // DELETE api/quiz/{id}
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {   
            Quiz quiz = await _DataService.Quizes.Get(id.ToString());

            if (quiz == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _DataService.Quizes.Delete(id.ToString());
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}

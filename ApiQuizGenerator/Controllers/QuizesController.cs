using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;
using System.Linq;

namespace SimpleCMS.Controllers
{
    [Route("api/[controller]/")]
    public class QuizesController : Controller
    {
        private IDataService _DataService { get; set; }

        public QuizesController(IDataService _dataService) 
        {
            this._DataService = _dataService;
        }

        // GET api/quizes/list
        [HttpGet]
        [Route("[action]")]
        public async Task<List<Quiz>> List() 
        {
            return await this._DataService.Quizes.All(); 
        }

        // GET api/quizes/{id}
        [HttpGet("{id}")]
        public async Task<Quiz> Get(Guid id)
        {
            Quiz quiz = await this._DataService.Quizes.Get(id);

            if (quiz == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return quiz;
        }

        // POST api/quizes/post
        [HttpPost]        
        public async Task Post([FromBody]Quiz quizModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            var saveStatus = await this._DataService.Quizes.Save(quizModel);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        // PUT api/quizes/put/{id}
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody]Quiz quizModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            var allQuizes = await this._DataService.Quizes.All();
            if (allQuizes.Any(q => q.Id == id))
            {
                quizModel.Id = id;
                
                var saveStatus = await this._DataService.Quizes.Save(quizModel);    
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

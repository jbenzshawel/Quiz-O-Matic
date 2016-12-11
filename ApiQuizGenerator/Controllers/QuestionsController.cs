using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class QuestionsController : Controller
    {
        private IDataService _DataService { get; set; }

        public QuestionsController(IDataService _dataService) 
        {
            this._DataService = _dataService;
        }

        // GET api/questions/list
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<List<Question>> List(Guid quizId) 
        {
            return await this._DataService.Questions.All(); 
        }

        // GET api/quizes/{id}
        [HttpGet("{id}")]
        public async Task<Question> Get(int id)
        {
            Question question = await this._DataService.Questions.Get(id);

            if (question == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return question;
        }

        // // POST api/quizes/post
        // [HttpPost]        
        // public async Task Post([FromBody]Quiz quizModel)
        // {
        //     if (!ModelState.IsValid)
        //     {   
        //         Response.StatusCode = 400; // bad request 
        //         return;
        //     }

        //     bool saveStatus = await this._DataService.Quizes.Save(quizModel);    
        //     if (!saveStatus) 
        //     {
        //         Response.StatusCode = 500; // internal server error
        //     }
        // }

        // // PUT api/quizes/put/{id}
        // [HttpPut("{id}")]
        // public async Task Put(Guid id, [FromBody]Quiz quizModel)
        // {
        //     if (!ModelState.IsValid)
        //     {   
        //         Response.StatusCode = 400; // bad request 
        //         return;
        //     }

        //     // make sure object exists before trying to update it
        //     Quiz quizSearch = await this._DataService.Quizes.Get(id);
        //     if (quizSearch != null)
        //     {
        //         quizModel.Id = id;

        //         bool saveStatus = await this._DataService.Quizes.Save(quizModel);    
        //         if (!saveStatus) 
        //         {
        //             Response.StatusCode = 500; // internal server error
        //         }         
        //     }
        //     else 
        //     {
        //         Response.StatusCode = 404; // not found
        //     }
        // }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
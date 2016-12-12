using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class QuestionsController : BaseController
    {
        public QuestionsController(IDataService _dataService) : base(_dataService)
        {
            _DataService = _dataService;
        }

        // GET api/questions/list
        [HttpGet("[action]/{id}")]
        public async Task<List<Question>> List(Guid id) 
        {
            return await _DataService.Questions.All(id.ToString()); 
        }

        // GET api/questions/{id}
        [HttpGet("{id}")]
        public async Task<Question> Get(int id)
        {
            Question question = await _DataService.Questions.Get(id.ToString());

            if (question == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return question;
        }

        // POST api/questions/post
        [HttpPost]        
        public async Task Post([FromBody]Question questionModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            bool saveStatus = await _DataService.Questions.Save(questionModel);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        // PUT api/quizes/put/{id}
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Question questionModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            Question questionSearch = await _DataService.Questions.Get(id.ToString());
            if (questionSearch != null)
            {
                questionModel.Id = id;

                bool saveStatus = await _DataService.Questions.Save(questionModel);    
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

        // DELETE api/question/{id}
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {   
            Question question = await _DataService.Questions.Get(id.ToString());

            if (question == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _DataService.Questions.Delete(question);
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}
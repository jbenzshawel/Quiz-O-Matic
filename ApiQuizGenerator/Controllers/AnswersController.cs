using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class AnswersController : BaseController
    {
        public AnswersController(IDataService _dataService) : base(_dataService)
        {
            _DataService = _dataService;
        }

        // GET api/answer/list
        [HttpGet("[action]/{id}")]
        public async Task<List<Answer>> List(Guid id) 
        {
            return await _DataService.Answers.All(id.ToString()); 
        }

        // GET api/answer/{id}
        [HttpGet("{id}")]
        public async Task<Answer> Get(int id)
        {
            Answer answer = await _DataService.Answers.Get(id.ToString());

            if (answer == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return answer;
        }

        // POST api/answer/post
        [HttpPost]        
        public async Task Post([FromBody]Answer answerModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            bool saveStatus = await _DataService.Answers.Save(answerModel);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        // PUT api/answer/put/{id}
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Answer answerModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            Answer answerSearch = await _DataService.Answers.Get(id.ToString());
            if (answerSearch != null)
            {
                answerModel.Id = id;

                bool saveStatus = await _DataService.Answers.Save(answerModel);    
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

        // DELETE api/answer/{id}
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {   
            Answer question = await _DataService.Answers.Get(id.ToString());

            if (question == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _DataService.Answers.Delete(id.ToString());
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}
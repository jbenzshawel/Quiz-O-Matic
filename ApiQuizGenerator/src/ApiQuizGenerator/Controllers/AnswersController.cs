using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class AnswersController : BaseController
    {
        public AnswersController(IDataService _dataService, ITokenProvider _tokenProvider) :
            base(_dataService, _tokenProvider)
        {
            _DataService = _dataService;
            _TokenProvider = _tokenProvider;
        }

        /// <summary>
        /// The way this quiz scoring is desinged causes the attributes of an answer to often
        /// contain infomration about  what result selecting this answer will give. For this 
        /// reason the UI will only need the attributes when scoring a quiz.
        /// </summary>
        /// <param name="id">quiz if for answers</param>
        /// <param name="includeAttributes">
        /// optional flag if set to true includes answer attribute information in returned answer list
        /// </param>
        /// <returns></returns>
        // GET api/List/{id}/{includeAttributes}
        [HttpGet("[action]/{id}/{includeAttributes?}")]
        public async Task<List<Answer>> List(Guid id, bool? includeAttributes = null) 
        {
            List<Answer> answerOptions = await _DataService.Answers.All(id.ToString()); 
           
            if (!(includeAttributes == true)) // funky condition since bool is nullable
            {
                // remove the attributes if includeAttributes false or null                 
                answerOptions = answerOptions.Select(ans => new Answer {
                    Id = ans.Id, 
                    QuestionId = ans.QuestionId, 
                    Content = ans.Content, 
                    Identifier = ans.Identifier
                }).ToList();
            }

            return answerOptions;
        }

        // GET api/answers/{id}
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

        // POST api/answers/post
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

        // PUT api/answers/put/{id}
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

        // DELETE api/answers/{id}
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
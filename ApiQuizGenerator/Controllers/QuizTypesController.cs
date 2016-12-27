using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class QuizTypes : BaseController
    {
        public QuizTypes(IDataService _dataService) : base(_dataService)
        {
            _DataService = _dataService;
        }

        // GET api/quiztypes/list
        [HttpGet("[action]/{id}")]
        public async Task<List<QuizType>> List() 
        {
            return await _DataService.QuizTypes.All(); 
        }

        // GET api/quiztypes/{id}
        [HttpGet("{id}")]
        public async Task<QuizType> Get(int id)
        {
            QuizType quizType = await _DataService.QuizTypes.Get(id.ToString());

            if (quizType == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return quizType;
        }

        // POST api/quiztypes/post
        [HttpPost]        
        public async Task Post([FromBody]Response responseModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            bool saveStatus = await _DataService.Responses.Save(responseModel);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        // PUT api/quiztypes/put/{id}
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]QuizType quizTypeModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            QuizType quizTypeSearch = await _DataService.QuizTypes.Get(id.ToString());
            if (quizTypeSearch != null)
            {
                quizTypeModel.Id = id;

                bool saveStatus = await _DataService.QuizTypes.Save(quizTypeModel);    
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

        // DELETE api/quiztypes/{id}
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {   
            QuizType quizType = await _DataService.QuizTypes.Get(id.ToString());

            if (quizType == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _DataService.QuizTypes.Delete(id.ToString());
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}
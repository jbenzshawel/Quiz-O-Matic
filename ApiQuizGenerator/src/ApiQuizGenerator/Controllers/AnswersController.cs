using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;
using ApiQuizGenerator.AppClasses;
using ApiQuizGenerator.Controllers.Shared;

namespace ApiQuizGenerator.Controllers
{
    [Route("api/[controller]/")]
    public class AnswersController : DALController<Answer>
    {
        public AnswersController(IDataService _dataService, ITokenProvider _tokenProvider) :
            base(_dataService, _tokenProvider)
        {
            _DataService = _dataService;
            _TokenProvider = _tokenProvider;
        }

        /// <summary>
        /// The way this quiz scoring is desinged causes the attributes of an answer will often
        /// contain infomration about what result selecting this answer will give. For this 
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
    }
}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.Models;

namespace SimpleCMS.Controllers
{
    [Route("api/[controller]/")]
    public class QuizesController : Controller
    {
        private Repository<Quiz> _Repository { get; set; }

        public QuizesController() 
        {
            this._Repository = new Repository<Quiz>();
        }

        // GET api/quizes/list
        [HttpGet]
        [Route("[action]")]
        public List<Quiz> List() 
        {
            return this._Repository.All(); 
        }

        // GET api/v
        [HttpGet("[action]/{id}")]
        public Quiz Get(Guid id)
        {
            Quiz quiz = this._Repository.Get(id);

            if (quiz == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return quiz;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

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
    public class ResponsesController : BaseController
    {
        public ResponsesController(IDataService _dataService) : base(_dataService)
        {
            _DataService = _dataService;
        }

        // GET api/responses/list
        [HttpGet("[action]/{id}")]
        public async Task<List<Response>> List(Guid id) 
        {
            return await _DataService.Responses.All(id.ToString()); 
        }

        // GET api/responses/{id}
        [HttpGet("{id}")]
        public async Task<Response> Get(int id)
        {
            Response response = await _DataService.Responses.Get(id.ToString());

            if (response == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return response;
        }

        // POST api/responses/post
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

        // PUT api/responses/put/{id}
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody]Response responseModel)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            Response responseSearch = await _DataService.Responses.Get(id.ToString());
            if (responseSearch != null)
            {
                responseModel.Id = id;

                bool saveStatus = await _DataService.Responses.Save(responseModel);    
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

        // DELETE api/responses/{id}
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {   
            Response response = await _DataService.Responses.Get(id.ToString());

            if (response == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _DataService.Responses.Delete(id.ToString());
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}
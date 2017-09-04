using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ApiQuizGenerator.DAL;
using ApiQuizGenerator.AppClasses;

namespace ApiQuizGenerator.Controllers.Shared
{
    public class DALController<T> : BaseController where T : class, new()
    {
        private IRepository<T> _Repository { get; set; }
        
        public DALController(IDataService _dataService, ITokenProvider _tokenProvider) :
            base(_dataService, _tokenProvider)
        {
            _DataService = _dataService;
            _TokenProvider = _tokenProvider;
            _Repository = new Repository<T>();
        }
        
        [HttpGet("[action]/{id}")]
        public virtual async Task<List<T>> List(Guid? id) 
        {
            return await this._Repository.All(id.ToString());
        }

        [HttpGet("{id}")]
        public virtual async Task<T> Get(string id)
        {
            T modelObj = await this._Repository.Get(id);

            if (modelObj == null) 
            {
                Response.StatusCode = 404; // not found
            }

            return modelObj;
        }

        [HttpPost]
        public virtual async Task Post([FromBody]T modelObj)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            bool saveStatus = await this._Repository.Save(modelObj);    
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }

        [HttpPut("{id}")]
        public virtual async Task Put(int id, [FromBody]T modelObj)
        {
            if (!ModelState.IsValid)
            {   
                Response.StatusCode = 400; // bad request 
                return;
            }

            // make sure object exists before trying to update it
            T searchResult = await this._Repository.Get(id.ToString());
            if (searchResult != null)
            {
                //modelObj.Id = id;

                bool saveStatus = await _Repository.Save(modelObj);    
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

        [HttpDelete("{id}")]
        public virtual async Task Delete(int id)
        {   
            T modelObj = await _Repository.Get(id.ToString());

            if (modelObj == null) 
            {
                Response.StatusCode = 404; // not found
                return;
            }

            bool saveStatus = await _Repository.Delete(id.ToString());
            if (!saveStatus) 
            {
                Response.StatusCode = 500; // internal server error
            }
        }
    }
}
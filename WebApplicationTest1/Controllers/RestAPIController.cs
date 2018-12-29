using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest1.Models;

using Newtonsoft.Json;
using System.Text;

namespace WebApplicationTest1.Controllers
{

    [Produces("application/json")]
    [Route("api/RestAPI")]
    public class RestAPIController : Controller
    {
        IRepository<Mobile> repo = new MobileRepo();

        // GET: api/RestAPI
        [HttpGet]
        public IEnumerable<Mobile> Get()
        {
            var mobiles = repo.GetAll();
            return mobiles;
        }

        //// GET: api/RestAPI/5
        //[HttpGet("{name}", Name = "Get")]
        //public string Get(string name)
        //{
        //    return "value";
        //}

        // POST: api/RestAPI
        [HttpPost]
        public void Post([FromBody] Mobile value)
        {           
            repo.Create(value);
        }
        
        // PUT: api/RestAPI
        [HttpPut]
        public void Put([FromBody] Mobile value)
        {
            
            repo.Update(value);
        }
        
        // DELETE: api/ApiWithActions/nokia
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            List<Mobile> items = Get().ToList();
            var item = items.Where(x => x.Name == name).FirstOrDefault();
            items.Remove(item);
            repo.Delete(items);
        }
    }
}

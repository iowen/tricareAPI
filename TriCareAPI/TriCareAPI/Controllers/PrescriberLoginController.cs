using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Models;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    public class PrescriberLoginController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<LoginModel>(value);
            var util = new PrescriberUtil(new TriCareDataDataContext());
            var outPut = util.ConvertToModel(util.Login(result));
            var json = JsonConvert.SerializeObject(outPut);
            return json;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
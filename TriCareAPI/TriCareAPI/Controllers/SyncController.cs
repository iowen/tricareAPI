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
    [Authorize]
    public class SyncController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return "Invalid Opperation";
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "Invalid Opperation";
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<SyncModel>(value);
            var util = new SyncUtil(new TriCareDataDataContext());
            var outPut = util.GetSyncData(result);
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

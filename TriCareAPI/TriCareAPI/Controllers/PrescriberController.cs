using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    //[Authorize]
    public class PrescriberController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new PrescriberUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetPrescribers());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new PrescriberUtil(new TriCareDataDataContext());
            var result = util.ConvertToModel(util.GetPrescriber(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Prescriber>(value);
            result.PrescriberId = 0;
            var util = new PrescriberUtil(new TriCareDataDataContext());
            var outPut = util.CreatePrescriber(result);
            var json = JsonConvert.SerializeObject(outPut);
            return json;
        }

        // PUT api/values/5
        public string Put([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Prescriber>(value);
            var util = new PrescriberUtil(new TriCareDataDataContext());
             util.UpdatePrescriber(result);
             var outPut = "Success";
            var json = JsonConvert.SerializeObject(outPut);
            return json;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

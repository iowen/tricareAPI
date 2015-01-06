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
    public class InsuranceCarrierController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new InsuranceCarrierUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetInsuranceCarriers());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new InsuranceCarrierUtil(new TriCareDataDataContext());
            var result = util.ConvertToModel(util.GetInsuranceCarrier(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<InsuranceCarrier>(value);
            result.InsuranceCarrierId = 0;
            var util = new InsuranceCarrierUtil(new TriCareDataDataContext());
            var outPut = util.CreateInsuranceCarrier(result);
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

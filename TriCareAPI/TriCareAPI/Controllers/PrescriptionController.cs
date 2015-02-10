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
    [Authorize]
    public class PrescriptionController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetPrescriptions());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var result = util.ConvertToModel(util.GetPrescription(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Prescription>(value);
            result.PrescriptionId = 0;
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var outPut = util.CreatePrescription(result);
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }

        // PUT api/values/5
        public string Put(int id)
        {
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetPrescriptionsByPrescriber(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

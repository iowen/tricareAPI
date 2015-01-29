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
    public class PatientController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new PatientUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetPatients());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new PatientUtil(new TriCareDataDataContext());
            var result = util.ConvertToModel(util.GetPatient(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Patient>(value);
            result.PatientId = 0;
            var util = new PatientUtil(new TriCareDataDataContext());
            var outPut = util.CreatePatient(result);
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }

        // PUT api/values/5
        public string Put(int id)
        {
            var util = new PatientUtil(new TriCareDataDataContext());
            var result = util.ConvertListToModel(util.GetPatientsByPrescriber(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }
        public string Put([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Patient>(value);
            var util = new PatientUtil(new TriCareDataDataContext());
             util.UpdatePatient(result);
             var outPut = "success";
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}

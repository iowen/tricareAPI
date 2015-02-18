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
    public class PatientEditController : ApiController
    {

        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Patient>(value);
            var util = new PatientUtil(new TriCareDataDataContext());
             util.UpdatePatient(result);
             var outPut = "success";
            var json = new JavaScriptSerializer().Serialize(outPut);
            return json;
        }


    }
}

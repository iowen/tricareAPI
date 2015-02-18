using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    [Authorize]
    public class PrescriberEditController : ApiController
    {

        // POST api/values
        public string Post([FromBody]string value)
        {
            var result = new JavaScriptSerializer().Deserialize<Prescriber>(value);
            var util = new PrescriberUtil(new TriCareDataDataContext());
            util.UpdatePrescriber(result);
            var outPut = "Success";
            var json = JsonConvert.SerializeObject(outPut);
            return json;
        }

    }
}

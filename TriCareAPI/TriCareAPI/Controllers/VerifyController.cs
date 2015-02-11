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
    public class VerifyController : ApiController
    {
        public HttpResponseMessage Get(string email)
        {
            var util = new PrescriberUtil(new TriCareDataDataContext());
            var result = util.GetPrescriberByEmail(email);
            if (result.PrescriberId > 0)
            {
                result.Verified = true;
                util.UpdatePrescriber(result);
                var response = Request.CreateResponse(HttpStatusCode.Found);
                response.Headers.Location = new Uri("http://Tricarewellness.com");
                return response;
               }
            return null;
        }
    }
}

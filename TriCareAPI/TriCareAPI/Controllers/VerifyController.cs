using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
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
            var result = util.GetPrescriberByEmail(email.Trim());
            var response = Request.CreateResponse(HttpStatusCode.Found);
            response.Headers.Location = new Uri("http://Tricarewellness.com/verify.html");

            if (result.PrescriberId > 0)
            {
                if (!result.Verified)
                {
                    result.Verified = true;
                    util.verifyPrescriber(result);
                } 
                return response;
            }
            return null;
        }
        //public HttpResponseMessage Post(FormDataCollection verifyForm)
        //{
        //    var util = new PrescriberUtil(new TriCareDataDataContext());
        //    var result = util.GetPrescriberByEmail(verifyForm.First().Value.Trim());
        //    if (result.PrescriberId > 0)
        //    {
        //        result.Verified = true;
        //        util.UpdatePrescriber(result);
        //        var response = Request.CreateResponse(HttpStatusCode.Found);
        //        response.Headers.Location = new Uri("http://Tricarewellness.com/verify.html");
        //        return response;
        //    }
        //    return null;
        //}

    }
}

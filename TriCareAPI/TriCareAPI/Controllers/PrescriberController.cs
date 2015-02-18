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
            var prescExist = util.GetPrescriberByEmail(result.Email.Trim());
            int outPut = 0;
            if(prescExist.PrescriberId > 0)
            {
                var jsonExist = JsonConvert.SerializeObject(outPut);
                return jsonExist;
            }
            outPut = util.CreatePrescriber(result);
            var eu = new EmailUtil();
            var en = new Encrypter();
            try {
                if (outPut > 0)
                {
                    string fromPassword = "10f14lif3";
                    using (MailMessage mail = new MailMessage("admin@tricarewellness.com", result.Email.Trim()))
                    {
                        using (SmtpClient client = new SmtpClient())
                        {
                            client.Port = 80;
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;
                            client.UseDefaultCredentials = false;
                            client.Credentials = new NetworkCredential("admin@tricarewellness.com", fromPassword);
                            client.Host = "smtpout.secureserver.net";
                            mail.Subject = "Welcome To TriCare Wellness App";
                            mail.IsBodyHtml = true;
                            mail.Body = eu.GetWelcomeEmail(result.FirstName.Trim(), result.LastName.Trim(), result.Email.Trim(), en.GetDecryption(result.Password.Trim()));
                            client.Send(mail);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
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

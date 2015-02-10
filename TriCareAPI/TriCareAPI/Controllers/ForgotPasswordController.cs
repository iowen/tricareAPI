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
    public class ForgotPasswordController : ApiController
    {


        // GET api/<controller>/5
        public string Get(string email)
        {
            try
            {
                var util = new PrescriberUtil(new TriCareDataDataContext());
                var result = util.GetPrescriberByEmail(email);
                var encrypter = new Encrypter();
                string returnValue = "";
                var eu = new EmailUtil();
                if (result.PrescriberId > 0)
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
                            mail.Subject = "Account Information For TriCare Wellness App";
                            mail.IsBodyHtml = true;
                            mail.Body = eu.GetPasswordEmail(result.FirstName.Trim(), result.LastName.Trim(), result.Email.Trim(), encrypter.GetDecryption(result.Password.Trim()));
                            client.Send(mail);
                        }
                    }
                    returnValue = "true";
                }
                else
                {
                    returnValue = "false";
                }
                var json = new JavaScriptSerializer().Serialize(returnValue);
                return json;
            }
            catch(Exception ex)
            {
                var returnValue = "false";
                var json = new JavaScriptSerializer().Serialize(returnValue);
                return json;
            }
        }
    }
}

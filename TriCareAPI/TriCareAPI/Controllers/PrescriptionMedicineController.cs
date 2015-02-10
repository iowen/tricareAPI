using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Models;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    [Authorize]
    public class PrescriptionMedicineController : ApiController
    {
        // GET api/values
        public string Get()
        {
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var result = util.ConvertListWithMedicineToModel(util.GetPrescriptions());
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        // GET api/values/5
        public string Get(int id)
        {
            var util = new PrescriptionUtil(new TriCareDataDataContext());
            var result = util.ConvertToModelWithMedicine(util.GetPrescription(id));
            var json = new JavaScriptSerializer().Serialize(result);
            return json;
        }

        private string ReadRequest(HttpPostedFile file, CreatePrescriptionModel model)
        {
           
            var folderName = "Uploads";
            var PATH = HttpContext.Current.Server.MapPath("~/" + folderName);
            var fPath = Path.Combine(PATH,file.FileName);
            string nameFriendly = "test" + DateTime.Now.ToString("hh-mm-ss-dd-MM") + ".pdf";
            var pPath = Path.Combine(PATH,nameFriendly);
            file.SaveAs(fPath);
            try{
                    var util = new PrescriptionUtil(new TriCareDataDataContext());
                    var outPut = util.CreatePrescriptionFromModel(model);
                     string pdfName = pPath;
                     
                    CreatePdf(pdfName, outPut, fPath);
                    util.UpdatePrescriptionLocation("Uploads/"+nameFriendly, outPut.PrescriptionId);
                    var pres = util.GetPrescription(outPut.PrescriptionId);
                    outPut.Location = "Uploads/"+nameFriendly;
                    var json = new JavaScriptSerializer().Serialize(outPut);
                     SendEmailAndFax(pdfName, outPut);
                    return json;
            }
                catch (Exception e )
                {
                    return e.ToString();
                }
            
        }
        // POST api/values
        public string Post()
        {
            string rr = "";
            try
            {
                var file = HttpContext.Current.Request.Files.Get(0);
                dynamic resultFix = JsonConvert.DeserializeObject(HttpContext.Current.Request.Form[0].ToString());
                rr = resultFix.ToString();
                rr = rr.Replace("/n", "").Replace("/r", "");
                var resultItem = JsonConvert.DeserializeObject<CreatePrescriptionModel>(rr);
               return ReadRequest(file,resultItem);
            }
            catch(Exception e)
            {
                return e.ToString()+"-"+rr;
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        private void CreatePdf(string fileName, CreatePrescriptionModel model, string signature)
        {
            var presRepo = new PrescriptionUtil(new TriCareDataDataContext());
            var pres = presRepo.GetPrescription(model.PrescriptionId);
            ///
            string pLoc = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "tricarePrescriptionTemplate.pdf");
            var newFile = fileName;
            PdfReader pRead = new PdfReader(pLoc);
            var fstr = new FileStream(newFile, FileMode.Create, FileAccess.Write, FileShare.None);
            PdfStamper pdfStamper = new PdfStamper(pRead, fstr);
            int fieldCount = 0;
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Created.ToString("d"));
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.FirstName.Trim() + " " + pres.Prescriber.LastName.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.LicenseNumber.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.DeaNumber.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.NpiNumber.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.Address.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.Phone.Trim().Insert(3,"-").Insert(7,"-"));
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.City.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.State.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Prescriber.Zip.ToString());
            //Patient Info
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.FirstName.Trim() + " " + pres.Patient.LastName.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.SSN.ToString());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.BirthDate.ToString("d"));
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Phone.Trim().Insert(3,"-").Insert(7,"-"));
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Gender.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Address.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.PaymentType.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.City.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.State.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Zip.ToString());
            //Insurance Info
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.InsuranceCarrier.Name.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.InsuranceCarrierIdNumber.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.InsurancePhone.Trim().Insert(3,"-").Insert(7,"-"));
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.InsuranceGroupNumber.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.RxBin.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.RxPcn.Trim());

            var mRepo = new MedicineUtil(new TriCareDataDataContext());
            var medItem = mRepo.GetMedicine(model.MedicineId);
            string prq;
            if(pres.PresciptionRefills.First().RefillQuantity.Quantity > 0)
            {
                prq = pres.PresciptionRefills.First().RefillQuantity.Quantity.ToString().Trim();
            }
            else{
                if(pres.PresciptionRefills.First().RefillQuantity.Quantity == 0)
                    prq = "NR";
                else
                    prq = "PRN";
            }

            //Medicine
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, medItem.Name.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Allergies.Trim());
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.PresciptionRefills.First().RefillAmount.Amount.ToString().Trim()+" Grams");
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, pres.Patient.Diagnosis);
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, prq);
            pdfStamper.AcroFields.SetField(pdfStamper.AcroFields.Fields.ElementAt(fieldCount++).Key, medItem.Directions.Trim());
            var sig = iTextSharp.text.Image.GetInstance(signature);
            sig.ScaleAbsolute(30, 30);
            sig.SetAbsolutePosition(60, 85);


            pdfStamper.GetOverContent(1).AddImage(sig);
            pdfStamper.FormFlattening = true;


            pdfStamper.Close();
        }

        private long SendEmailAndFax(string fileName, CreatePrescriptionModel prescription)
        {
            string username = "iowen8";
            string password = "Distime4da4";
            string faxNumbers = "+14802475681";
            string contacts = "Larry Howard";
            string path1 = fileName;
            // read files data
            byte[] file1data = System.IO.File.ReadAllBytes(path1);
            //1st document
            //2nd document
            // combine into a single byte array
            byte[] data = new byte[file1data.Length];
            Array.Copy(file1data, data, file1data.Length);
            string fileTypes = System.IO.Path.GetExtension(path1).TrimStart('.');
            string fileSizes = file1data.Length.ToString();
            var preRepo = new PrescriberUtil(new TriCareDataDataContext());
            var patRepo = new PatientUtil(new TriCareDataDataContext());
            var medRepo = new MedicineUtil(new TriCareDataDataContext());
            var prec = preRepo.GetPrescriber(prescription.PrescriberId);
            var pat = patRepo.GetPatient(prescription.PatientId);
            var med = medRepo.GetMedicine(prescription.MedicineId);
            DateTime postponeTime = DateTime.Now.AddSeconds(10);
            // in two hours. use any PAST time to send ASAP
            int retriesToPerform = 2;
            string csid = "My CSID";
            string pageHeader = "To: Fertility Pharmacy From:"+prec.FirstName.Trim()+" "+prec.LastName.Trim()+" Pages: 1";
            string subject = "Prescription - TCA-"+ prescription.PrescriptionId.ToString();
            string replyAddress = "owat@1of1inc.com";
            string pageSize = "A4";
            string pageorientation = "Portrait";
            bool isHighResolution = false;
            //this will speed up your transmission
            bool isFineRendering = false;
            //fine will fit more graphics, while normal (false) will fit more textual documents

            TriCareAPI.net.interfax.ws.InterFax ifws = new TriCareAPI.net.interfax.ws.InterFax();
            long st = ifws.SendfaxEx_2(username, password, faxNumbers, contacts, data, fileTypes, fileSizes, postponeTime, retriesToPerform, csid,
            pageHeader, "", subject, replyAddress, pageSize, pageorientation, isHighResolution, isFineRendering);
            string fromPassword = "10f14lif3";
            var eu = new EmailUtil();
            try
            {
                using (MailMessage mail = new MailMessage("admin@tricarewellness.com", prec.Email.Trim()))
                {
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Port = 80;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("admin@tricarewellness.com", fromPassword);
                        client.Host = "smtpout.secureserver.net";
                        mail.Subject = "Prescription Completed";
                        mail.IsBodyHtml = true;
                        mail.Body = eu.GetPrescriptionEmail(prec.FirstName.Trim(), prec.LastName.Trim(), pat.FirstName.Trim() + " " + pat.LastName.Trim(), med.Name.Trim());
                        mail.Attachments.Add(new Attachment(fileName));
                        client.Send(mail);
                    }
                }
            }
            catch(Exception ex)
            {

            }
            try
            {
                using (MailMessage mail = new MailMessage("admin@tricarewellness.com", "owen1.watson@gmail.com"))
                {
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Port = 80;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new NetworkCredential("admin@tricarewellness.com", fromPassword);
                        client.Host = "smtpout.secureserver.net";
                        mail.Subject = "Prescription Completed";
                        mail.IsBodyHtml = true;
                        mail.Body = eu.GetPrescriptionEmail(prec.FirstName.Trim(), prec.LastName.Trim(), pat.FirstName.Trim() + " " + pat.LastName.Trim(), med.Name.Trim());
                        mail.Attachments.Add(new Attachment(fileName));
                        client.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return st;
        }
    }
}

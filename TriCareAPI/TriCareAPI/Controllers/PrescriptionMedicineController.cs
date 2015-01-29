using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using TriCareAPI.Models;
using TriCareAPI.Utilities;

namespace TriCareAPI.Controllers
{
    //[Authorize]
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
                    outPut.Location = "Uploads/"+nameFriendly;
                    var json = new JavaScriptSerializer().Serialize(outPut);
                     SendFax(pdfName, outPut.PrescriptionId);
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
            var doc = new Document(new Rectangle(306, 378));
            var output = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            var writer = PdfWriter.GetInstance(doc, output);
            doc.Open();
            doc.SetMargins(0, 0, 0, 0);
            var DrNameFont = FontFactory.GetFont("Times", 10, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Times", 9, Font.NORMAL);
            var dName = new Paragraph(pres.Prescriber.FirstName.Trim() + " " + pres.Prescriber.LastName.Trim(), DrNameFont);
            dName.Alignment = Element.ALIGN_CENTER;
            doc.Add(dName);
            var ldea = new Paragraph("LIC #: " + pres.Prescriber.LicenseNumber.Trim() + " - DEA#: " + pres.Prescriber.DeaNumber.Trim() + "\nNPI #: " + pres.Prescriber.NpiNumber.Trim() + "\n\n", bodyFont);
            ldea.Alignment = Element.ALIGN_CENTER;
            doc.Add(ldea);

            var addr = new Paragraph(pres.Prescriber.Address.Trim() + " " + pres.Prescriber.City.Trim() + " , " + pres.Prescriber.State.Trim() + " " + pres.Prescriber.Zip.ToString().Trim() + "\nTel: " + pres.Prescriber.Phone.Trim() + " - Fax: " + pres.Prescriber.Fax.Trim(), bodyFont);
            addr.Alignment = Element.ALIGN_CENTER;
            doc.Add(addr);
            PdfPTable table = new PdfPTable(3);
            float[] widths = new float[] { 0.6f, 0.75f, 2f };
            table.SetWidths(widths);
            PdfPCell numeroCell = new PdfPCell(new Phrase("               "));
            numeroCell.Border = 0;
            numeroCell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
            numeroCell.BorderWidthBottom = 1f;
            table.AddCell(numeroCell);
            table.AddCell(numeroCell);
            table.AddCell(numeroCell);
            doc.Add(table);
            var mRepo = new MedicineUtil(new TriCareDataDataContext());
            var medItem = mRepo.GetMedicine(model.MedicineId);
            var pat = new Paragraph("Patient : " + pres.Patient.FirstName.Trim() + " " + pres.Patient.LastName.Trim() + "                              DOB:" + pres.Patient.BirthDate.ToShortDateString() + "\nAddress: " + pres.Patient.Address.Trim() + " " + pres.Patient.City.Trim() + " , " + pres.Patient.State.Trim() + " " + pres.Patient.Zip.ToString().Trim() + "\n Date: " + pres.Created.ToShortDateString() + "\nInsurance Carrier: " + pres.Patient.InsuranceCarrier.Name.Trim() + "              Insurance Id: " + pres.Patient.InsuranceCarrierIdNumber.Trim() + "       Insurance Phone: " + pres.Patient.InsurancePhone.Trim(), bodyFont);
            pat.Alignment = Element.ALIGN_CENTER;
            doc.Add(pat);

            var med = new Paragraph("Medicine : " + medItem.Name.Trim(), bodyFont);
            med.Alignment = Element.ALIGN_CENTER;
            doc.Add(med);

            var refl = new Paragraph("Refill Amount (mg): " + pres.PresciptionRefills.First().RefillAmount.Amount.ToString().Trim() + "       Refill Quantity: " + pres.PresciptionRefills.First().RefillQuantity.Quantity.ToString().Trim(), bodyFont);
            refl.Alignment = Element.ALIGN_CENTER;
            doc.Add(refl);
            doc.Add(new Paragraph(" ", DrNameFont));

            var sig = iTextSharp.text.Image.GetInstance(signature);
            sig.ScaleAbsolute(30, 30);
            sig.Alignment = Element.ALIGN_LEFT;
            doc.Add(sig);
            doc.SetPageSize(new Rectangle(306, 378));

            doc.Close();
        }
        private long SendFax(string fileName, int prescriptionId)
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

            DateTime postponeTime = DateTime.Now.AddMinutes(1);
            // in two hours. use any PAST time to send ASAP
            int retriesToPerform = 2;
            string csid = "My CSID";
            string pageHeader = "To: Fertility Pharmacy From: Owen Watson Pages: 1";
            string subject = "Prescription - "+ prescriptionId.ToString();
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

            return st;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class PatientUtil
    {
        private TriCareDataDataContext db;

        public PatientUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Patient> GetPatients()
        {
            try
            {
                return db.Patients.ToList();
            }
            catch (Exception ex)
            {
                return new List<Patient>();
            }
        }

        public List<Patient> GetPatientsByPrescriber(int prescriberId)
        {
            try
            {
                return db.Patients.Where(a => a.PrescriberId == prescriberId).ToList();
            }
            catch (Exception ex)
            {
                return new List<Patient>();
            }
        }

        public List<Patient> GetPatientsByPrescriber(int prescriberId, DateTime lastSync)
        {
            try
            {
                return db.Patients.Where(a => a.PrescriberId == prescriberId && a.LastUpdate > lastSync).ToList();
            }
            catch (Exception ex)
            {
                return new List<Patient>();
            }
        }

        public Patient GetPatient(int id)
        {
            try
            {
                return db.Patients.First(a => a.PatientId == id);
            }
            catch (Exception ex)
            {
                return new Patient();
            }
        }

        public int CreatePatient(Patient item)
        {
            item.LastUpdate = DateTime.Now;
            db.Patients.InsertOnSubmit(item);
            db.SubmitChanges();
            var prescriber = db.Prescribers.First(a => a.PrescriberId == item.PrescriberId);
            prescriber.LastUpdate = item.LastUpdate;
            db.SubmitChanges();
            return item.PatientId;
        }
        public void UpdatePatient(Patient item)
        {
            var pat = db.Patients.First(a => a.PatientId == item.PatientId);
            pat.Address = item.Address;
            pat.Allergies = item.Allergies;
            pat.BirthDate = item.BirthDate;
            pat.City = item.City;
            pat.Diagnosis = item.Diagnosis;
            pat.Email = item.Email;
            pat.FirstName = item.FirstName;
            pat.Gender = item.Gender;
            pat.InsuranceCarrierId = item.InsuranceCarrierId;
            pat.InsuranceCarrierIdNumber = item.InsuranceCarrierIdNumber;
            pat.InsuranceGroupNumber = item.InsuranceGroupNumber;
            pat.InsurancePhone = item.InsurancePhone;
            pat.LastName = item.LastName;
            pat.PatientId = item.PatientId;
            pat.PaymentType = item.PaymentType;
            pat.Phone = item.Phone;
            pat.PrescriberId = item.PrescriberId;
            pat.RxBin = item.RxBin;
            pat.RxPcn = item.RxPcn;
            pat.SSN = item.SSN;
            pat.State = item.State;
            pat.Zip = item.Zip;
            pat.LastUpdate = DateTime.Now;
            db.SubmitChanges();
            var prescriber = db.Prescribers.First(a => a.PrescriberId == item.PrescriberId);
            prescriber.LastUpdate = pat.LastUpdate;
            db.SubmitChanges();
        }

        public PatientModel ConvertToModel(Patient item)
        {
            return new PatientModel() { PatientId = item.PatientId, Address = item.Address, City = item.City, InsuranceCarrierIdNumber = item.InsuranceCarrierIdNumber, Email = item.Email, InsuranceCarrierName = item.InsuranceCarrier.Name, FirstName = item.FirstName, LastName = item.LastName,Phone = item.Phone, PrescriberId = item.PrescriberId, State = item.State, Zip = item.Zip, Allergies = item.Allergies, BirthDate = item.BirthDate, Diagnosis = item.Diagnosis, Gender = item.Gender, InsuranceCarrierId = item.InsuranceCarrierId, InsuranceGroupNumber = item.InsuranceGroupNumber, InsurancePhone = item.InsurancePhone, PaymentType = item.PaymentType, RxBin = item.RxBin, RxPcn = item.RxPcn, SSN = item.SSN , LastUpdate = item.LastUpdate};
        }

        public List<PatientModel> ConvertListToModel(List<Patient> items)
        {
            var result = new List<PatientModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }
}

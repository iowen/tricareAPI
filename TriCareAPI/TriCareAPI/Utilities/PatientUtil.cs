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
            db.Patients.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.PatientId;
        }

        public PatientModel ConvertToModel(Patient item)
        {
            return new PatientModel() { PatientId = item.PatientId, Address = item.Address, City = item.City, InsuranceCarrierIdNumber = item.InsuranceCarrierIdNumber, Email = item.Email, InsuranceCarrierName = item.InsuranceCarrier.Name, FirstName = item.FirstName, LastName = item.LastName,Phone = item.Phone, PrescriberId = item.PrescriberId, State = item.State, Zip = item.Zip, Allergies = item.Allergies, BirthDate = item.BirthDate, Diagnosis = item.Diagnosis, Gender = item.Gender, InsuranceCarrierId = item.InsuranceCarrierId, InsuranceGroupNumber = item.InsuranceGroupNumber, InsurancePhone = item.InsurancePhone, PaymentType = item.PaymentType, RxBin = item.RxBin, RxPcn = item.RxPcn, SSN = item.SSN };
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

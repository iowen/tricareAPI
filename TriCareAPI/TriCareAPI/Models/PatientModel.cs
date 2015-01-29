using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class PatientModel
    {
        public int PatientId;

        public int PrescriberId;

        public string FirstName;

        public string LastName;

        public string Gender;

        public System.DateTime BirthDate;

        public int SSN;

        public string Address;

        public string City;

        public string State;

        public int Zip;

        public string Phone;

        public string Email;

        public string Allergies;

        public string Diagnosis;

        public int InsuranceCarrierId;

        public string InsuranceCarrierName;

        public string InsuranceCarrierIdNumber;

        public string InsuranceGroupNumber;

        public string RxBin;

        public string RxPcn;

        public string InsurancePhone;

        public string PaymentType;

        public DateTime LastUpdate;
		
    }
}

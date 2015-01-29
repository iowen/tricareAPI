using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class PrescriptionModel
    {
        public int PrescriptionId;
        public PrescriberModel Prescriber;
        public PatientModel Patient;
        public DateTime Created;
        public DateTime LastUpdate;
        public string Location;
        public MedicineModelForPrescription Medicine;
        public RefillModel Refill;
    }

}

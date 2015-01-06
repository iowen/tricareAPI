using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class RefillModel
    {
        public int PrescriptionId;
        public RefillAmountModel Amount;
        public RefillQuantityModel Quantity;
    }

    public class RefillAmountModel
    {
        public int RefillAmountId;
        public int Amount;
    }
    public class RefillQuantityModel
    {
        public int RefillQuantityId;
        public int Quantity;
    }
    public class MedicineModelForPrescription
    {
        public int PrescriptionId;
        public string MedicineName;
        public int MedicineId;
        public List<PrescriptionMedicineIngredientModel> Ingredients;
    }

    public class PrescriptionMedicineModel
    {
        public int PrescriptionId;
        public PrescriberModel Prescriber;
        public PatientModel Patient;
        public DateTime Created;
        public string MedicineName;
        public int MedicineId;
        public List<PrescriptionMedicineIngredientModel> Ingredients;
        public RefillModel Refill;
    }

}

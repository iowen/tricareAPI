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
        public int PrescriptionRefillId;

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
        public string Directions;
        public List<PrescriptionMedicineIngredientModel> Ingredients;
    }

    public class PrescriptionMedicineModel
    {
        public int PrescriptionId;
        public PrescriberModel Prescriber;
        public PatientModel Patient;
        public DateTime Created;
        public string MedicineName;
        public string Directions;
        public string Location;
        public DateTime LastUpdate;
        public int MedicineId;
        public List<PrescriptionMedicineIngredientModel> Ingredients;
        public RefillModel Refill;
    }
    public class MedcineIngredientForPrescriptionModel
    {
        public int IngredientId;
        public int PrescriptionMedicineIngredientId;
        public double Percentage;
    }
    public class CreatePrescriptionModel
    {
        public int PrescriptionId;
        public int PrescriberId;
        public int PatientId;
        public int MedicineId;
        public int PrescriptionMedicineId;
        public DateTime Created;
        public List<MedcineIngredientForPrescriptionModel> Ingredients;
        public int RefillAmount;
        public int RefillQuantity;
        public string Location;
        public int PrescriptionRefillId;
    }

}

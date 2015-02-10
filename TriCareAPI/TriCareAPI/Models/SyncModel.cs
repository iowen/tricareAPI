using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class SyncModel
    {
        public char SyncType;

        public int PrescriberId;

        public DateTime LastSync;

        public DateTime LastAppDataSync;
    }

    public class AppSyncDataModel
    {
        public List<InsuranceCarrierModel> InsuranceCarriers;
        public List<MedicineCategoryModel> Categories;
        public List<MedicineModel> Medicines;
        public List<IngredientModel> Ingredients;
        public List<MedicineIngredientModel> MedicineIngredients;
        public List<RefillAmountModel> RefillAmounts;
        public List<RefillQuantityModel> RefillQuantities;
        public DateTime Updated;
        public AppSyncDataModel()
        {
            InsuranceCarriers = new List<InsuranceCarrierModel>();
            Categories = new List<MedicineCategoryModel>();
            Medicines = new List<MedicineModel>();
            Ingredients = new List<IngredientModel>();
            MedicineIngredients = new List<MedicineIngredientModel>();
            RefillAmounts = new List<RefillAmountModel>();
            RefillQuantities = new List<RefillQuantityModel>();
        }

    }

    public class PrescriberSyncDataModel
    {
        public PrescriberModel Prescriber;
        public List<PatientModel> Patients;
        public List<PrescriptionMedicineModel> Prescriptions;
        public DateTime Updated;
        public PrescriberSyncDataModel()
        {
            Prescriber = new PrescriberModel();
            Patients = new List<PatientModel>();
            Prescriptions = new List<PrescriptionMedicineModel>();
        }
    }

    public class SyncResponseModel
    {
        public AppSyncDataModel AppDataUpdates;
        public PrescriberSyncDataModel PrescriberUpdates;
    }
}

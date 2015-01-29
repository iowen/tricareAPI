using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class PrescriptionMedicineUtil
    {
        private TriCareDataDataContext db;

        public PrescriptionMedicineUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<PrescriptionMedicine> GetPrescriptionMedicines()
        {
            try
            {
                return db.PrescriptionMedicines.ToList();
            }
            catch (Exception ex)
            {
                return new List<PrescriptionMedicine>();
            }
        }

        public PrescriptionMedicine GetPrescriptionMedicine(int id)
        {
            try
            {
                return db.PrescriptionMedicines.First(a => a.PrescriptionMedicineId == id);
            }
            catch (Exception ex)
            {
                return new PrescriptionMedicine();
            }
        }

        public int CreatePrescriptionMedicine(PrescriptionMedicine item)
        {
            db.PrescriptionMedicines.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.MedicineId;
        }

        public PrescriptionMedicineModel ConvertToModel(PrescriptionMedicine item)
        {
            var mRepo = new MedicineUtil(new TriCareDataDataContext());
            var paUtil = new PatientUtil(new TriCareDataDataContext());
            var presUtil = new PrescriberUtil(new TriCareDataDataContext());
            var pat = paUtil.ConvertToModel(item.Prescription.Patient);
            var pres = presUtil.ConvertToModel(item.Prescription.Prescriber);
            var med = mRepo.GetMedicine(item.MedicineId);
            var ra = new RefillAmountModel() { RefillAmountId = item.Prescription.PresciptionRefills.First().RefillAmount.RefillAmountId, Amount = item.Prescription.PresciptionRefills.First().RefillAmount.Amount };
            var rq = new RefillQuantityModel() { RefillQuantityId = item.Prescription.PresciptionRefills.First().RefillQuantity.RefillQuantityId, Quantity = item.Prescription.PresciptionRefills.First().RefillQuantity.Quantity };
            var rm = new RefillModel() { Amount = ra, Quantity = rq, PrescriptionId = item.PrescriptionId };
            var ings = new List<PrescriptionMedicineIngredientModel>();
            foreach (var i in item.PrescriptionMedicineIngredients)
            {
                ings.Add(new PrescriptionMedicineIngredientModel() { IngredientId = i.IngredientId, Name = i.Ingredient.Name, Percentage = i.Percentage, PrescriptionMedicineId = i.PrescriptionMedicineId, PrescriptionMedicineIngredientId = i.PrescriptionMedicineIngredientId });
            }
            return new PrescriptionMedicineModel() { MedicineId = item.MedicineId,Created = item.Prescription.Created, Ingredients = ings, MedicineName = med.Name,PrescriptionId = item.PrescriptionId, Patient = pat, Prescriber =pres, Refill = rm};
        }

        public MedicineModelForPrescription ConvertToMedicineModel(PrescriptionMedicine item)
        {
            var mRepo = new MedicineUtil(new TriCareDataDataContext());
            var med = mRepo.GetMedicine(item.MedicineId);
            var ings = new List<PrescriptionMedicineIngredientModel>();
            foreach (var i in item.PrescriptionMedicineIngredients)
            {
                ings.Add(new PrescriptionMedicineIngredientModel() { IngredientId = i.IngredientId, Name = i.Ingredient.Name, Percentage = i.Percentage, PrescriptionMedicineId = i.PrescriptionMedicineId, PrescriptionMedicineIngredientId = i.PrescriptionMedicineIngredientId });
            }
            return new MedicineModelForPrescription() { MedicineId = item.MedicineId,  Ingredients = ings, MedicineName = med.Name, PrescriptionId = item.PrescriptionId };
        }

      

        public List<PrescriptionMedicineModel> ConvertListToModel(List<PrescriptionMedicine> items)
        {
            var result = new List<PrescriptionMedicineModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }

}

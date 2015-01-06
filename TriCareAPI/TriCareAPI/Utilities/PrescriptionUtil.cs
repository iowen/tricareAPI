using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class PrescriptionUtil
    {
        private TriCareDataDataContext db;

        public PrescriptionUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Prescription> GetPrescriptions()
        {
            try
            {
                return db.Prescriptions.ToList();
            }
            catch (Exception ex)
            {
                return new List<Prescription>();
            }
        }

        public List<Prescription> GetPrescriptionsByPrescriber(int prescriberId)
        {
            try
            {
                return db.Prescriptions.Where(a => a.PrescriberId == prescriberId).ToList();
            }
            catch (Exception ex)
            {
                return new List<Prescription>();
            }
        }

        public Prescription GetPrescription(int id)
        {
            try
            {
                return db.Prescriptions.First(a => a.PrescriptionId == id);
            }
            catch (Exception ex)
            {
                return new Prescription();
            }
        }

        public int CreatePrescription(Prescription item)
        {
            db.Prescriptions.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.PrescriptionId;
        }

        public PrescriptionModel CreatePrescriptionFromMedicineModel(PrescriptionMedicineModel item)
        {
            var presciption = new PrescriptionModel();
            var newPres = new Prescription();
            newPres.PatientId = item.Patient.PatientId;
            newPres.PrescriberId = item.Prescriber.PrescriberId;
            newPres.Created = DateTime.Now;

            var presMed = new PrescriptionMedicine();
            presMed.MedicineId = item.MedicineId;
            presMed.PrescriptionId = newPres.PrescriptionId;
            db.PrescriptionMedicines.InsertOnSubmit(presMed);
            db.SubmitChanges();

            db.Prescriptions.InsertOnSubmit(newPres);
            db.SubmitChanges();

            foreach (var ing in item.Ingredients)
            {
                var pIng = new PrescriptionMedicineIngredient();
                pIng.PrescriptionMedicineId = presMed.PrescriptionMedicineId;
                pIng.Percentage = ing.Percentage;
                pIng.IngredientId = ing.IngredientId;
                db.PrescriptionMedicineIngredients.InsertOnSubmit(pIng);
                db.SubmitChanges();
            }
            var refilUtil = new RefillUtil(new TriCareDataDataContext());
            var pRef = new PresciptionRefill();
            pRef.PrescriptionId = newPres.PrescriptionId;
            pRef.RefillAmountId = refilUtil.GetRefillAmount(item.Refill.Amount.Amount).RefillAmountId;
            pRef.RefillQuantityId = refilUtil.GetRefillQuantity(item.Refill.Quantity.Quantity).RefillQuantityId;
            db.PresciptionRefills.InsertOnSubmit(pRef);
            db.SubmitChanges();
            var patUtil = new PatientUtil(new TriCareDataDataContext());
            var prescUtil = new PrescriberUtil(new TriCareDataDataContext());
            presciption.Created = newPres.Created;
            presciption.Patient = patUtil.ConvertToModel(patUtil.GetPatient(newPres.PatientId));
            presciption.Prescriber = prescUtil.ConvertToModel(prescUtil.GetPrescriber(newPres.PrescriberId));
            presciption.PrescriptionId = newPres.PrescriptionId;
            presciption.Medicine = new MedicineModelForPrescription() { Ingredients = item.Ingredients, MedicineId = item.MedicineId, MedicineName = item.MedicineName, PrescriptionId = newPres.PrescriptionId };
            presciption.Refill = item.Refill;
            return presciption;
        }

        public PrescriptionModel ConvertToModel(Prescription item)
        {

            var patUtil = new PatientUtil(new TriCareDataDataContext());
            var presUtil = new PrescriberUtil(new TriCareDataDataContext());
            var pat = patUtil.ConvertToModel(item.Patient);
            var pres = presUtil.ConvertToModel(item.Prescriber);
            return new PrescriptionModel() { PrescriptionId = item.PrescriptionId, Created = item.Created, Patient = pat, Prescriber = pres };
        }

        public PrescriptionMedicineModel ConvertToModelWithMedicine(Prescription item)
        {
            List<PrescriptionMedicineIngredientModel> ing = new List<PrescriptionMedicineIngredientModel>();
            foreach (var it in item.PrescriptionMedicines.First().PrescriptionMedicineIngredients)
            {
                ing.Add(new PrescriptionMedicineIngredientModel() { IngredientId = it.IngredientId, Name = it.Ingredient.Name, Percentage = it.Percentage, PrescriptionMedicineId = it.PrescriptionMedicineId, PrescriptionMedicineIngredientId = it.PrescriptionMedicineIngredientId });
            }
            var patUtil = new PatientUtil(new TriCareDataDataContext());
            var presUtil = new PrescriberUtil(new TriCareDataDataContext());
            var medUtil = new MedicineUtil(new TriCareDataDataContext());
            var refUtil = new RefillUtil(new TriCareDataDataContext());
            var pat = patUtil.ConvertToModel(item.Patient);
            var pres = presUtil.ConvertToModel(item.Prescriber);
            var med = medUtil.GetMedicine(item.PrescriptionMedicines.First().MedicineId);
            var refillInfo = new RefillModel() { Amount = refUtil.ConvertToAmountModel(item.PresciptionRefills.First().RefillAmount), Quantity = refUtil.ConvertToQuantityModel(item.PresciptionRefills.First().RefillQuantity) };
            return new PrescriptionMedicineModel() {Refill= refillInfo, MedicineName =med.Name, Ingredients = ing, PrescriptionId = item.PrescriptionId, Created = item.Created, Patient = pat, Prescriber = pres };
        }
    

        public List<PrescriptionModel> ConvertListToModel(List<Prescription> items)
        {
            var result = new List<PrescriptionModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }

        public List<PrescriptionMedicineModel> ConvertListWithMedicineToModel(List<Prescription> items)
        {
            var result = new List<PrescriptionMedicineModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModelWithMedicine(i));
            }
            return result;
        }
    }

}

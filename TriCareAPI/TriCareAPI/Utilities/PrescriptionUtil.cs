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

        public List<Prescription> GetPrescriptionsByPrescriber(int prescriberId, DateTime lastSync)
        {
            try
            {
                return db.Prescriptions.Where(a => a.PrescriberId == prescriberId && a.LastUpdate > lastSync).ToList();
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
        public void UpdatePrescriptionLocation(string location, int id)
        {
            var presc = db.Prescriptions.First(a => a.PrescriptionId == id);
            presc.Location = location;
            db.SubmitChanges();
        }
        public CreatePrescriptionModel CreatePrescriptionFromModel(CreatePrescriptionModel item)
        {
            var output = item;
            var time = DateTime.Now;
            var p = new Prescription
            {
                PrescriberId = item.PrescriberId,
                PatientId = item.PatientId,
                Created = time,
                LastUpdate = time,
                Location = item.Location
            };
            db.Prescriptions.InsertOnSubmit(p);
            db.SubmitChanges();
            var prescriber = db.Prescribers.First(a => a.PrescriberId == p.PrescriberId);
            prescriber.LastUpdate = p.LastUpdate;
            db.SubmitChanges();
            output.PrescriptionId = p.PrescriptionId;
            output.Created = p.Created;
            var pm = new PrescriptionMedicine
            {
                MedicineId = item.MedicineId,
                PrescriptionId = p.PrescriptionId,
            };
            db.PrescriptionMedicines.InsertOnSubmit(pm);
            db.SubmitChanges();
            output.PrescriptionMedicineId = pm.PrescriptionMedicineId;
            int count = 0;
            foreach (var i in item.Ingredients)
            {
                var mi = new PrescriptionMedicineIngredient
                {
                    IngredientId = i.IngredientId,
                    Percentage = i.Percentage,
                    PrescriptionMedicineId = pm.PrescriptionMedicineId
                };
                db.PrescriptionMedicineIngredients.InsertOnSubmit(mi);
                db.SubmitChanges();
                output.Ingredients[count].PrescriptionMedicineIngredientId = mi.PrescriptionMedicineIngredientId;
                count++;
            }
            var pr = new PresciptionRefill
            {
                PrescriptionId = p.PrescriptionId,
                RefillAmountId = item.RefillAmount,
                RefillQuantityId = item.RefillQuantity
            };
            
            db.PresciptionRefills.InsertOnSubmit(pr);
            db.SubmitChanges();
            output.PrescriptionRefillId = pr.PrescriptionRefillId;
            return output;
        }
        public int CreatePrescription(Prescription item)
        {
            db.Prescriptions.InsertOnSubmit(item);
            db.SubmitChanges();
            var prescriber = db.Prescribers.First(a => a.PrescriberId == item.PrescriberId);
            prescriber.LastUpdate = item.LastUpdate;
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
            var pmUtil = new PrescriptionMedicineUtil(new TriCareDataDataContext());
            var pat = patUtil.ConvertToModel(item.Patient);
            var pres = presUtil.ConvertToModel(item.Prescriber);
            var pml = item.PrescriptionMedicines.First();
            var pm = pmUtil.ConvertToMedicineModel(pml);
            var ra = new RefillAmountModel() { RefillAmountId = item.PresciptionRefills.First().RefillAmount.RefillAmountId, Amount = item.PresciptionRefills.First().RefillAmount.Amount };
            var rq = new RefillQuantityModel() { RefillQuantityId = item.PresciptionRefills.First().RefillQuantity.RefillQuantityId, Quantity = item.PresciptionRefills.First().RefillQuantity.Quantity };
            var rm = new RefillModel() { Amount = ra, Quantity = rq, PrescriptionId = item.PrescriptionId, PrescriptionRefillId = item.PresciptionRefills.First().PrescriptionRefillId };
            return new PrescriptionModel() { PrescriptionId = item.PrescriptionId, Created = item.Created, Patient = pat, Prescriber = pres, Medicine = pm, Refill = rm, LastUpdate = item.LastUpdate, Location = item.Location };
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
         //   AppDataUpdate
            var refillInfo = new RefillModel() { Amount = refUtil.ConvertToAmountModel(item.PresciptionRefills.First().RefillAmount), Quantity = refUtil.ConvertToQuantityModel(item.PresciptionRefills.First().RefillQuantity),PrescriptionId = item.PrescriptionId, PrescriptionRefillId = item.PresciptionRefills.First().PrescriptionRefillId };
            return new PrescriptionMedicineModel() {Refill= refillInfo, MedicineName =med.Name, Ingredients = ing, PrescriptionId = item.PrescriptionId, Created = item.Created, Patient = pat, Prescriber = pres ,LastUpdate = item.LastUpdate, Location = item.Location, MedicineId = item.PrescriptionMedicines.First().MedicineId };
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

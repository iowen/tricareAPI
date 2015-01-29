using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class SyncUtil
    {
        private TriCareDataDataContext db;

        public SyncUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public SyncResponseModel GetSyncData(SyncModel model)
        {
            var appSyncData = new AppSyncDataModel();
            var prescriberSyncData = new PrescriberSyncDataModel();
            var result = new SyncResponseModel();
            if (model.SyncType == 'a' || model.SyncType == 'b')
            {
                //app data sync
                var appSync = db.AppDataUpdates.First(x => x.AppDataUpdateId == 1);
                if (appSync.LastUpdate > model.LastAppDataSync)
                {
                    var iUtil = new InsuranceCarrierUtil(new TriCareDataDataContext());
                    var mUtil = new MedicineUtil(new TriCareDataDataContext());
                    var inUtil = new IngredientUtil(new TriCareDataDataContext());
                    var minUtil = new MedicineIngredientUtil(new TriCareDataDataContext());
                    var rUtil = new RefillUtil(new TriCareDataDataContext());
                    appSyncData.Updated = appSync.LastUpdate;
                    appSyncData.InsuranceCarriers = iUtil.ConvertListToModel(iUtil.GetInsuranceCarriers());
                    appSyncData.Medicines = mUtil.ConvertListToModel(mUtil.GetMedicines());
                    appSyncData.Ingredients = inUtil.ConvertListToModel(inUtil.GetIngredients());
                    appSyncData.MedicineIngredients = minUtil.ConvertListToModel(minUtil.GetMedicineIngredients());
                    appSyncData.RefillAmounts = rUtil.ConvertListToAmountModel(rUtil.GetRefillAmounts());
                    appSyncData.RefillQuantities = rUtil.ConvertListToQuantityModel(rUtil.GetRefillQuantities());
                }
            }
            if (model.SyncType == 'p' || model.SyncType == 'b')
            {
                var pUtil = new PrescriberUtil(new TriCareDataDataContext());
                var paUtil = new PatientUtil(new TriCareDataDataContext());
                var presUtil = new PrescriptionUtil(new TriCareDataDataContext());
                var P = pUtil.GetPrescriber(model.PrescriberId);
                if (P.LastUpdate > model.LastSync)
                {
                    prescriberSyncData.Prescriber = pUtil.ConvertToModel(pUtil.GetPrescriber(model.PrescriberId, model.LastSync));
                    prescriberSyncData.Patients = paUtil.ConvertListToModel(paUtil.GetPatientsByPrescriber(model.PrescriberId, model.LastSync));
                    prescriberSyncData.Prescriptions = presUtil.ConvertListWithMedicineToModel(presUtil.GetPrescriptionsByPrescriber(model.PrescriberId, model.LastSync));
                }
            }
            result.AppDataUpdates = appSyncData;
            result.PrescriberUpdates = prescriberSyncData;
            return result;
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class InsuranceCarrierUtil
    {
        private TriCareDataDataContext db;

        public InsuranceCarrierUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<InsuranceCarrier> GetInsuranceCarriers()
        {
            try
            {
                return db.InsuranceCarriers.ToList();
            }
            catch (Exception ex)
            {
                return new List<InsuranceCarrier>();
            }
        }

        public InsuranceCarrier GetInsuranceCarrier(int id)
        {
            try
            {
                return db.InsuranceCarriers.First(a => a.InsuranceCarrierId == id);
            }
            catch (Exception ex)
            {
                return new InsuranceCarrier();
            }
        }

        public int CreateInsuranceCarrier(InsuranceCarrier item)
        {
            db.InsuranceCarriers.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.InsuranceCarrierId;
        }

        public InsuranceCarrierModel ConvertToModel(InsuranceCarrier item)
        {
            return new InsuranceCarrierModel() { InsuranceCarrierId = item.InsuranceCarrierId, Name = item.Name};
        }

        public List<InsuranceCarrierModel> ConvertListToModel(List<InsuranceCarrier> items)
        {
            var result = new List<InsuranceCarrierModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }

}

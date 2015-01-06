using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class RefillUtil
    {
        private TriCareDataDataContext db;

        public RefillUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<RefillAmount> GetRefillAmounts()
        {
            try
            {
                return db.RefillAmounts.ToList();
            }
            catch (Exception ex)
            {
                return new List<RefillAmount>();
            }
        }

        public List<RefillQuantity> GetRefillQuantities()
        {
            try
            {
                return db.RefillQuantities.ToList();
            }
            catch (Exception ex)
            {
                return new List<RefillQuantity>();
            }
        }

        public RefillAmount GetRefillAmount(int amount)
        {
            try
            {
                return db.RefillAmounts.First(a => a.Amount == amount);
            }
            catch (Exception ex)
            {
                return new RefillAmount();
            }
        }


        public RefillQuantity GetRefillQuantity(int quantity)
        {
            try
            {
                return db.RefillQuantities.First(a => a.Quantity == quantity);
            }
            catch (Exception ex)
            {
                return new RefillQuantity();
            }
        }

        public int CreateRefillAmount(RefillAmount item)
        {
            db.RefillAmounts.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.RefillAmountId;
        }

        public int CreateRefillQuantity(RefillQuantity item)
        {
            db.RefillQuantities.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.RefillQuantityId;
        }
        public RefillAmountModel ConvertToAmountModel(RefillAmount item)
        {
            return new RefillAmountModel() { RefillAmountId = item.RefillAmountId, Amount = item.Amount };
        }
        public RefillQuantityModel ConvertToQuantityModel(RefillQuantity item)
        {
            return new RefillQuantityModel() { RefillQuantityId = item.RefillQuantityId, Quantity = item.Quantity } ;
        }
        public RefillModel ConvertToModel(PresciptionRefill item)
        {
            return new RefillModel() { PrescriptionId = item.PrescriptionId, Amount = new RefillAmountModel() { RefillAmountId = item.RefillAmountId, Amount = item.RefillAmount.Amount }, Quantity = new RefillQuantityModel() { RefillQuantityId = item.RefillQuantityId, Quantity = item.RefillQuantity.Quantity } };
        }

        public List<RefillModel> ConvertListToModel(List<PresciptionRefill> items)
        {
            var result = new List<RefillModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
        public List<RefillAmountModel> ConvertListToAmountModel(List<RefillAmount> items)
        {
            var result = new List<RefillAmountModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToAmountModel(i));
            }
            return result;
        }
        public List<RefillQuantityModel> ConvertListToQuantityModel(List<RefillQuantity> items)
        {
            var result = new List<RefillQuantityModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToQuantityModel(i));
            }
            return result;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class MedicineIngredientUtil
    {
        private TriCareDataDataContext db;

        public MedicineIngredientUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<MedicineIngredient> GetMedicineIngredients()
        {
            try
            {
                return db.MedicineIngredients.ToList();
            }
            catch (Exception ex)
            {
                return new List<MedicineIngredient>();
            }
        }

        public MedicineIngredient GetMedicineIngredient(int id)
        {
            try
            {
                return db.MedicineIngredients.First(a => a.MedicineIngredientId == id);
            }
            catch (Exception ex)
            {
                return new MedicineIngredient();
            }
        }

        public int CreateMedicineIngredient(MedicineIngredient item)
        {
            db.MedicineIngredients.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.MedicineIngredientId;
        }

        public MedicineIngredientModel ConvertToModel(MedicineIngredient item)
        {
 
            return new MedicineIngredientModel() { MedicineId = item.MedicineId, Name = item.Ingredient.Name, IngredientId = item.IngredientId, MedicineIngredientId = item.MedicineIngredientId, Percentage = item.Percentage};
        }

      
        public List<MedicineIngredientModel> ConvertListToModel(List<MedicineIngredient> items)
        {
            var result = new List<MedicineIngredientModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }

}

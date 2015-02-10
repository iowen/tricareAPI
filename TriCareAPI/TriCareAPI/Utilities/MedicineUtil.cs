using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class MedicineUtil
    {
        private TriCareDataDataContext db;

        public MedicineUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Medicine> GetMedicines()
        {
            try
            {
                return db.Medicines.ToList();
            }
            catch (Exception ex)
            {
                return new List<Medicine>();
            }
        }

        public Medicine GetMedicine(int id)
        {
            try
            {
                return db.Medicines.First(a => a.MedicineId == id);
            }
            catch (Exception ex)
            {
                return new Medicine();
            }
        }

        public int CreateMedicine(Medicine item)
        {
            db.Medicines.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.MedicineId;
        }

        public MedicineModel ConvertToModel(Medicine item)
        {
 
            return new MedicineModel() { MedicineId = item.MedicineId, Name = item.Name.Trim(), Directions = item.Directions.Trim(), MedicineCategoryId = item.MedicineCategoryId.Value, MedicineDetail = item.MedicineDetail.Trim()};
        }

        public MedicineWithIngredientsModel ConvertToModelWithIngredients(Medicine item)
        {
            var ingredients = item.MedicineIngredients.ToList();
            var ingredientList = new List<MedicineIngredientModel>();
            foreach (var ingredient in ingredients)
            {
                ingredientList.Add(new MedicineIngredientModel() { MedicineIngredientId = ingredient.MedicineIngredientId, MedicineId = ingredient.MedicineId, IngredientId = ingredient.IngredientId, Name = ingredient.Ingredient.Name, Percentage = ingredient.Percentage });
            }
            return new MedicineWithIngredientsModel() { MedicineId = item.MedicineId, Name = item.Name.Trim(), Ingredients = ingredientList, Directions = item.Directions.Trim(), MedicineCategoryId = item.MedicineCategoryId.Value, MedcineDetail = item.MedicineDetail.Trim()};
        }

        public List<MedicineModel> ConvertListToModel(List<Medicine> items)
        {
            var result = new List<MedicineModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }

        public List<MedicineWithIngredientsModel> ConvertListWithIngredintsToModel(List<Medicine> items)
        {
            var result = new List<MedicineWithIngredientsModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModelWithIngredients(i));
            }
            return result;
        }
    }

}

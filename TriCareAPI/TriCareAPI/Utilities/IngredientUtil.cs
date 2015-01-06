using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class IngredientUtil
    {
        private TriCareDataDataContext db;

        public IngredientUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Ingredient> GetIngredients()
        {
            try
            {
                return db.Ingredients.ToList();
            }
            catch (Exception ex)
            {
                return new List<Ingredient>();
            }
        }

        public Ingredient GetIngredient(int id)
        {
            try
            {
                return db.Ingredients.First(a => a.IngredientId == id);
            }
            catch (Exception ex)
            {
                return new Ingredient();
            }
        }

        public int CreateIngredient(Ingredient item)
        {
            db.Ingredients.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.IngredientId;
        }

        public IngredientModel ConvertToModel(Ingredient item)
        {
            return new IngredientModel() { IngredientId = item.IngredientId, Name = item.Name};
        }

        public List<IngredientModel> ConvertListToModel(List<Ingredient> items)
        {
            var result = new List<IngredientModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }

}

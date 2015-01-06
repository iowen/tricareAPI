using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class MedicineWithIngredientsModel
    {
        public int MedicineId;
        public string Name;
        public List<MedicineIngredientModel> Ingredients;
    }
    public class MedicineModel
    {
        public int MedicineId;
        public string Name;
    }

}

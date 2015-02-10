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
        public int MedicineCategoryId;
        public string Name;
        public string Directions;
        public string MedcineDetail;
        public List<MedicineIngredientModel> Ingredients;
    }
    public class MedicineModel
    {
        public int MedicineId;
        public string MedicineDetail;
        public int MedicineCategoryId;
        public string Name;
        public string Directions;
    }

    public class MedicineCategoryModel
    {
        public int MedicineCategoryId;
        public string Name;
    }

}

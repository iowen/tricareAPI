using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class MedicineIngredientModel
    {
        public int MedicineIngredientId;
        public int MedicineId;
        public int IngredientId;
        public string Name;
        public double Percentage;
    }
}

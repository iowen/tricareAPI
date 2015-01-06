using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriCareAPI.Models
{
    public class PrescriptionMedicineIngredientModel
    {
        public int PrescriptionMedicineIngredientId;
        public int PrescriptionMedicineId;
        public int IngredientId;
        public string Name;
        public double Percentage;
    }
}

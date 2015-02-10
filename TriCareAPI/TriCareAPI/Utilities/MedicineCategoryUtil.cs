using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class MedicineCategoryUtil
    {
        private TriCareDataDataContext db;

        public MedicineCategoryUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<MedicineCategory> GetMedicineCategories()
        {
            try
            {
                return db.MedicineCategories.ToList();
            }
            catch (Exception ex)
            {
                return new List<MedicineCategory>();
            }
        }

        public MedicineCategory GetMedicineCategory(int id)
        {
            try
            {
                return db.MedicineCategories.First(a => a.MedicineCategoryId == id);
            }
            catch (Exception ex)
            {
                return new MedicineCategory();
            }
        }

        public int CreateMedicineCaegory(MedicineCategory item)
        {
            db.MedicineCategories.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.MedicineCategoryId;
        }

        public MedicineCategoryModel ConvertToModel(MedicineCategory item)
        {
 
            return new MedicineCategoryModel() { MedicineCategoryId = item.MedicineCategoryId, Name = item.Name.Trim()};
        }

        public List<MedicineCategoryModel> ConvertListToModel(List<MedicineCategory > items)
        {
            var result = new List<MedicineCategoryModel>();
            foreach (var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriCareAPI.Models;

namespace TriCareAPI.Utilities
{
    class PrescriberUtil
    {
        private TriCareDataDataContext db;

        public PrescriberUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Prescriber> GetPrescribers()
        {
            try
            {
                return db.Prescribers.ToList();
            }
            catch (Exception ex)
            {
                return new List<Prescriber>();
            }
        }

        public Prescriber GetPrescriber(int id)
        {
            try
            {
                return db.Prescribers.First(a => a.PrescriberId == id);
            }
            catch (Exception ex)
            {
                return new Prescriber();
            }
        }

        public int CreatePrescriber(Prescriber item)
        {
            var acc = new Account();
            db.Accounts.InsertOnSubmit(acc);
            db.SubmitChanges();

            item.AccountId = acc.AccountId;
            db.Prescribers.InsertOnSubmit(item);
            db.SubmitChanges();
            return item.PrescriberId;
        }

        public PrescriberModel ConvertToModel(Prescriber item)
        {
            return new PrescriberModel() { AccountId = item.AccountId, Address = item.Address, City = item.City, DeaNumber = item.DeaNumber, Email = item.Email, Fax = item.Fax, FirstName = item.FirstName, LastName = item.LastName, LicenseNumber = item.LicenseNumber, NpiNumber = item.NpiNumber, Password = item.Password, Phone = item.Phone, PrescriberId = item.PrescriberId, State = item.State, Zip = item.Zip };
        }

        public List<PrescriberModel> ConvertListToModel(List<Prescriber> items)
        {
            var result = new List<PrescriberModel>();
            foreach( var i in items)
            {
                result.Add(ConvertToModel(i));
            }
            return result;
        }

        public Prescriber Login(LoginModel model)
        {
            try
            {
                return db.Prescribers.First(a => a.Email == model.Email && a.Password == model.Password);
            }
            catch (Exception ex)
            {
                return new Prescriber();
            }
        }
    }
}

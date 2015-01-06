using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TriCareAPI.Utilities
{
    public class AccountUtil
    {
        private TriCareDataDataContext db;

        public AccountUtil(TriCareDataDataContext context)
        {
            db = context;
        }

        public List<Account> GetAccounts()
        {
            try
            {
                return db.Accounts.ToList();
            }
            catch (Exception ex)
            {
                return new List<Account>();
            }
        }

        public Account GetAccount(int id)
        {
            try
            {
                return db.Accounts.First(a => a.AccountId == id);
            }
            catch (Exception ex)
            {
                return new Account();
            }
        }

        public int CreateAccount()
        {
            var acc = new Account();
            db.Accounts.InsertOnSubmit(acc);
            db.SubmitChanges();
            return acc.AccountId;
        }
    }
}
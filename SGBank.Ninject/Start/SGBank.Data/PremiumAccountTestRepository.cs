using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;

namespace SGBank.Data
{
    public class PremiumAccountTestRepository : IAccountRepository
    {
        private static Account _account = new Account
        {
            Name = "Premium Account",
            Balance = 100.00M,
            AccountNumber = "44444",
            Type = AccountType.Premium
        };

        public Account LoadAccount(string AccountNumber)
        {
            if (AccountNumber != _account.AccountNumber)
            {
                return null;
            }
            return _account;
        }

        public void SaveAccount(Account account)
        {
            _account = account;
        }
    }
}

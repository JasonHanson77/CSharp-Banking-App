using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL.WithdrawRules
{
    class WithdrawRulesFactory
    {
        public static IWithdraw Create(AccountType Type)
        {
            switch (Type)
            {
                case AccountType.Free:
                    return new FreeAccountWithdrawRule();
                case AccountType.Basic:
                    return new BasicAccountWithdrawRule();
                case AccountType.Premium:
                    return new PremiumAccountWithdrawRule();

                default: throw new Exception("Account type is not supported!");
            }
            
        }
    }
}

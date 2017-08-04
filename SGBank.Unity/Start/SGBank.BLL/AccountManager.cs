using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Data;

namespace SGBank.BLL
{
    public class AccountManager
    {
        private IAccountRepository _accountRepository;
        
        public AccountManager(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public AccountLookupResponse LookupAccount(string accountNumber)
        {
            AccountLookupResponse response = new AccountLookupResponse();

            response.Account = _accountRepository.LoadAccount(accountNumber);

            if(response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public AccountDepositResponse Deposit(string accountNumber, decimal amount)
        {
            AccountDepositResponse response = new AccountDepositResponse();

            response.Account = _accountRepository.LoadAccount(accountNumber);

            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
                return response;
            }
            else
            {
                response.Success = true;
            }

            IDeposit depositRule = DIContainer.Container.Resolve<IDeposit>(response.Account.Type.ToString());
            response = depositRule.Deposit(response.Account, amount);

            if(response.Success)
            {
                _accountRepository.SaveAccount(response.Account);
            }

            return response;
        }
        public AccountWithdrawResponse Withdraw(string accountNumber, decimal amount)
        {
            AccountWithdrawResponse response = new AccountWithdrawResponse();

            response.Account = _accountRepository.LoadAccount(accountNumber);

            if (response.Account == null)
            {
                response.Success = false;
                response.Message = $"{accountNumber} is not a valid account.";
                return response;
            }
            else
            {
                response.Success = true;
            }

            IWithdraw withdrawRule = DIContainer.Container.Resolve<IWithdraw>(response.Account.Type.ToString());
            response = withdrawRule.Withdraw(response.Account, amount);

            if (response.Success)
            {
                _accountRepository.SaveAccount(response.Account);
            }
            return response;
        }
    }
}

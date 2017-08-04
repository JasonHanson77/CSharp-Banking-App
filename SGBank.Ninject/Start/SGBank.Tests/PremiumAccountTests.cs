using Ninject;
using NUnit.Framework;
using SGBank.BLL;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Models;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.Tests
{
    [TestFixture]
    public class PremiumAccountTests
    {
        [Test]
        public void CanLoadPremiumAccountTestData()
        {
            IAccountRepository repo = DIContainer.Kernel.Get<IAccountRepository>();
            AccountManager manager = new AccountManager(repo);

            AccountLookupResponse response = manager.LookupAccount("44444");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("44444", response.Account.AccountNumber);
        }

        [TestCase("44444", "Premium Account", 100, AccountType.Free, 1, false)]
        [TestCase("44444", "Premium Account", 100, AccountType.Premium, -1, false)]
        [TestCase("44444", "Premium Account", 100, AccountType.Premium, 2500000, true)]
        public void PremiumAccountDepositRuleTest(string accountNumber, string Name, decimal balance, AccountType accountType,
                                               decimal amount, bool expectedResult)
        {
            IDeposit deposit = new NoLimitDepositRule();

            Account account = new Account();

            account.Name = Name;
            account.AccountNumber = accountNumber;
            account.Balance = balance;
            account.Type = accountType;

            AccountDepositResponse response = deposit.Deposit(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }

        [TestCase("44444", "Premium Account", 15000, AccountType.Basic, -14000, 15000, false)]
        [TestCase("44444", "Premium Account", 1000, AccountType.Premium, -1501, 1000, false)]
        [TestCase("44444", "Premium Account", 10000, AccountType.Premium, 100, 10000, false)]
        [TestCase("44444", "Premium Account", 1, AccountType.Premium, -501, -500, true)]
        [TestCase("44444", "Premium Account", 15000, AccountType.Premium, -5000, 10000, true)]
        public void PremiumAccountWithdrawRuleTest(string accountNumber, string Name, decimal balance, AccountType accountType,
                                              decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdraw = new PremiumAccountWithdrawRule();

            Account account = new Account();

            account.Name = Name;
            account.AccountNumber = accountNumber;
            account.Balance = balance;
            account.Type = accountType;

            AccountWithdrawResponse response = withdraw.Withdraw(account, amount);

            Assert.AreEqual(expectedResult, response.Success);
        }
    }
}

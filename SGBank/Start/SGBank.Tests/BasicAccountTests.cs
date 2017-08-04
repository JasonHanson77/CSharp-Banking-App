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
    public class BasicAccountTests
    {
        [Test]
        public void CanLoadBasicAccountTestData()
        {
            AccountManager manager = AccountManagerFactory.Create();

            AccountLookupResponse response = manager.LookupAccount("33333");

            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("33333", response.Account.AccountNumber);
        }
        [TestCase("33333", "Basic Account", 100, AccountType.Free, 250, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -100, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, 250, true)]
        public void BasicAccountDepositRuleTest(string accountNumber, string Name, decimal balance, AccountType accountType,
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

        [TestCase("33333", "Basic Account", 1500, AccountType.Basic, -1000, 1500, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Free, -100, 100, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, 100, 100, false)]
        [TestCase("33333", "Basic Account", 100, AccountType.Basic, -150, -60, true)]
        [TestCase("33333", "Basic Account", 150, AccountType.Basic, -50, 100, true)]
        public void BasicAccountWithdrawRuleTest(string accountNumber, string Name, decimal balance, AccountType accountType,
                                               decimal amount, decimal newBalance, bool expectedResult)
        {
            IWithdraw withdraw = new BasicAccountWithdrawRule();

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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using SGBank.Data;
using SGBank.Models;
using SGBank.BLL;
using SGBank.Models.Responses;
using SGBank.Models.Interfaces;

namespace SGBank.Tests
{
    [TestFixture]
    public class FileRepositoryTests
    {
        string filePath = Settings.FilePath;

        [SetUp]
        public void Setup()
        {
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            File.Copy(Settings.seedFilePath, filePath );
        }

        [Test]
        public void CanLoadFileData()
        {
            IAccountRepository repo = DIContainer.Kernel.Get<IAccountRepository>();
            AccountManager manager = new AccountManager(repo);

            AccountLookupResponse response = manager.LookupAccount("55555");
           
            Assert.IsNotNull(response.Account);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("55555", response.Account.AccountNumber);
            Assert.AreEqual("Premium Customer", response.Account.Name);
            Assert.AreEqual(AccountType.Premium, response.Account.Type);
            Assert.AreEqual(1000, response.Account.Balance);
        }

        [Test]
        public void CanWriteDepositToFile()
        {
            IAccountRepository repo = DIContainer.Kernel.Get<IAccountRepository>();
            AccountManager manager = new AccountManager(repo);

            AccountDepositResponse response = manager.Deposit("11111", 1.25m);

            Assert.AreEqual(true, response.Success);
            Assert.AreEqual(101.25m, response.Account.Balance);
        }

        [Test]
        public void CanWriteWithdrawToFile()
        {
            IAccountRepository repo = DIContainer.Kernel.Get<IAccountRepository>();
            AccountManager manager = new AccountManager(repo);

            AccountWithdrawResponse response = manager.Withdraw("22222", -499.99m);

            Assert.AreEqual(.01m, response.Account.Balance);
            Assert.AreEqual(true, response.Success);
        }


    }
}

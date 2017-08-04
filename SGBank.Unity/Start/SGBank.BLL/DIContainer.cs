using Microsoft.Practices.Unity;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Data;
using SGBank.Models;
using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.BLL
{
    public static class DIContainer
    {
        public static UnityContainer Container = new UnityContainer();

        static DIContainer()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();
            
            if(mode == "FreeTest")
            {
                Container.RegisterType<IAccountRepository, FreeAccountTestRepository>();
                Container.RegisterType<IWithdraw, FreeAccountWithdrawRule>();
                Container.RegisterType<IDeposit, FreeAccountDepositRule>();
            }
            else if (mode == "BasicTest")
            {
                Container.RegisterType<IAccountRepository, BasicAccountTestRepository>();
                Container.RegisterType<IWithdraw, BasicAccountWithdrawRule>();
                Container.RegisterType<IDeposit, NoLimitDepositRule>();
            }
            else if (mode == "PremiumTest")
            {
                Container.RegisterType<IAccountRepository, PremiumAccountTestRepository>();
                Container.RegisterType<IWithdraw, PremiumAccountWithdrawRule>();
                Container.RegisterType<IDeposit, NoLimitDepositRule>();
            }
            else if (mode == "FileTest")
            {
                Container.RegisterType<IAccountRepository, FileAccountRepository>();
                Container.RegisterInstance(new FileAccountRepository(Settings.FilePath));
                Container.RegisterType<IWithdraw, FreeAccountWithdrawRule>("Free");
                Container.RegisterType<IWithdraw, BasicAccountWithdrawRule>("Basic");
                Container.RegisterType<IWithdraw, PremiumAccountWithdrawRule>("Premium");
                Container.RegisterType<IDeposit, FreeAccountDepositRule>("Free");
                Container.RegisterType<IDeposit, NoLimitDepositRule>("NoLimit");

            }
            else
            {
                throw new Exception("Mode Key in app.config not set properly");
            }
        }
    }
}

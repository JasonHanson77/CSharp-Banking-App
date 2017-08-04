using Ninject;
using SGBank.BLL.DepositRules;
using SGBank.BLL.WithdrawRules;
using SGBank.Data;
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
        // the kernel is the master factory
        public static IKernel Kernel = new StandardKernel();

        // constructor, to configure the bindings
        static DIContainer()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();


            if (mode == "FreeTest")
            {
                Kernel.Bind<IAccountRepository>().To<FreeAccountTestRepository>();
                Kernel.Bind<IWithdraw>().To<FreeAccountWithdrawRule>().Named("Free");
                Kernel.Bind<IDeposit>().To<FreeAccountDepositRule>().Named("Free");
            }
            else if (mode == "BasicTest")
            {
                Kernel.Bind<IAccountRepository>().To<BasicAccountTestRepository>().Named("Basic");
                Kernel.Bind<IWithdraw>().To<BasicAccountWithdrawRule>().Named("Basic");
                Kernel.Bind<IDeposit>().To<NoLimitDepositRule>().Named("Basic");
            }
            else if (mode == "PremiumTest")
            {
                Kernel.Bind<IAccountRepository>().To<PremiumAccountTestRepository>().Named("Premium");
                Kernel.Bind<IWithdraw>().To<PremiumAccountWithdrawRule>().Named("Premium");
                Kernel.Bind<IDeposit>().To<NoLimitDepositRule>().Named("Premium");
            }
            else if (mode == "FileTest")
            {
                Kernel.Bind<IAccountRepository>().To<FileAccountRepository>().WithConstructorArgument("filePath", Settings.FilePath);
                Kernel.Bind<IDeposit>().To<FreeAccountDepositRule>().Named("Free");
                Kernel.Bind<IDeposit>().To<NoLimitDepositRule>().Named("Basic");
                Kernel.Bind<IDeposit>().To<NoLimitDepositRule>().Named("Premium");
                Kernel.Bind<IWithdraw>().To<FreeAccountWithdrawRule>().Named("Free");
                Kernel.Bind<IWithdraw>().To<BasicAccountWithdrawRule>().Named("Basic");
                Kernel.Bind<IWithdraw>().To<PremiumAccountWithdrawRule>().Named("Premium");
            }
            else
                throw new Exception("Chooser key in app.config not set properly!");
        }
    }
}


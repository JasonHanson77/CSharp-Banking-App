using Ninject;
using SGBank.BLL;
using SGBank.Models.Interfaces;
using SGBank.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.UI.Workflows
{
    class WithdrawWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            IAccountRepository repo = DIContainer.Kernel.Get<IAccountRepository>();
            AccountManager manager = new AccountManager(repo);

            Console.Write("Enter an account number: ");
            string accountNumber = Console.ReadLine();

            Console.Write("Enter a withdraw amount: ");
            decimal amount = 0m;
            decimal result;
            string withdrawAmount = Console.ReadLine();
            if (!String.IsNullOrWhiteSpace(withdrawAmount))
            {
                if (withdrawAmount[0] != '-')
                {
                    if (decimal.TryParse("-" + withdrawAmount, out result))
                    {
                        amount = result;
                    }
                    else
                    {
                        Console.WriteLine("You must enter a withdrawal amount in the form of a number in this field!");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        return;
                    }
                }
                else if (decimal.TryParse(withdrawAmount, out result))
                {
                    amount = result;
                }
                else
                {
                    Console.WriteLine("You must enter a withdrawal amount in the form of a number in this field!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return;
                }

            }
            else
            {
                Console.WriteLine("You must enter a withdrawal amount in the form of a number in this field!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return;
            }

            AccountWithdrawResponse response = manager.Withdraw(accountNumber, amount);

            if (response.Success)
            {
                Console.WriteLine("Withdraw completed!");
                Console.WriteLine($"Account Number: {response.Account.AccountNumber}");
                Console.WriteLine($"Old balance: {response.OldBalance:c}");
                Console.WriteLine($"Amount Withdrawn: {response.Amount:c}");
                Console.WriteLine($"New balance: {response.Account.Balance:c}");
            }
            else
            {
                Console.WriteLine("An error occurred: ");
                Console.WriteLine(response.Message);
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

        }

    }
}

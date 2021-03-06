﻿using SGBank.BLL;
using SGBank.Models.Responses;
using System;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGBank.UI.Workflows
{
    public class DepositWorkflow
    {
        public void Execute()
        {
            Console.Clear();
            AccountManager accountManager = DIContainer.Container.Resolve<AccountManager>();

            Console.Write("Enter an account number: ");
            string accountNumber = Console.ReadLine();

            Console.Write("Enter a deposit amount: ");
            string depositAmount = Console.ReadLine();
            decimal amount = 0m;
            decimal result;

            if (!String.IsNullOrWhiteSpace(depositAmount))
            {
                if (decimal.TryParse(depositAmount, out result))
                {
                    amount = result;
                }
                else
                {
                    Console.WriteLine("You must enter a deposit amount in the form of a number in this field!");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.WriteLine("You must enter a deposit amount in the form of a number in this field!");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return;
            }

            AccountDepositResponse response = accountManager.Deposit(accountNumber, amount);

            if (response.Success)
            {
                Console.WriteLine("Deposit completed!");
                Console.WriteLine($"Account Number: {response.Account.AccountNumber}");
                Console.WriteLine($"Old balance: {response.OldBalance:c}");
                Console.WriteLine($"Amount Deposited: {response.Amount:c}");
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

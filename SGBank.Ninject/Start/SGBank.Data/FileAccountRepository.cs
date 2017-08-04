using SGBank.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGBank.Models;
using System.IO;

namespace SGBank.Data
{
    public class FileAccountRepository : IAccountRepository
    {
        private List<Account> _accountsList = new List<Account>();

        private string _filePath;

        public FileAccountRepository(string filePath)
        {
            if (File.Exists(filePath))
            {
                _filePath = filePath;
            }
            else
            {
                File.Copy(Settings.seedFilePath, filePath);

                _filePath = filePath;
            }
        }

        public void createAccountListFromFile()
        {
            using (StreamReader sr = new StreamReader(_filePath))
            {
                sr.ReadLine();
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    Account newAccount = new Account();

                    string[] columns = line.Split(',');
                   
                    newAccount.AccountNumber = columns[0];
                    newAccount.Name = columns[1];
                    newAccount.Balance = decimal.Parse(columns[2]);
                    newAccount.Type = parseAccountType(columns[3]);
                    
                    _accountsList.Add(newAccount);
                }
            }

            
        }
        public Account LoadAccount(string AccountNumber)
        {
            createAccountListFromFile();

            foreach (var account in _accountsList)
            {
                if (AccountNumber.Equals(account.AccountNumber))
                {
                   return account;
                }
            }
            return null;
        }

        public void SaveAccount(Account account)
        {
            saveAccountToList(account);
            CreateAccountFile(_accountsList);
        }

        public AccountType parseAccountType(string accountType)
        {
            if (accountType.Equals("F"))
            {
                return AccountType.Free;
            }
            else if (accountType.Equals("B"))
            {
                return AccountType.Basic;
            }
            else if (accountType.Equals("P"))
            {
                return  AccountType.Premium;
            }

            return AccountType.None;
        }

        public void saveAccountToList(Account account)
        {
            foreach (var bankAccount in _accountsList)
            {

                if (account.AccountNumber.Equals(bankAccount.AccountNumber))
                {
                    int index = _accountsList.IndexOf(bankAccount);
                    _accountsList[index] = account;
                    break;
                }
            }
        }

        private string CreateCsvForAccounts(Account account)
        {
            return string.Format("{0},{1},{2},{3}", account.AccountNumber, account.Name, account.Balance, account.Type.ToString()[0]);
        }

        private void CreateAccountFile(List<Account> accounts)
        {
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
                

            using (StreamWriter sr = new StreamWriter(_filePath))
            {
                sr.WriteLine("AccountNumber,Name,Balance,Type");
                foreach (var account in accounts)
                {
                    sr.WriteLine(CreateCsvForAccounts(account));
                }
            }
        }
    }
}

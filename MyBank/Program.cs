using System;
using static System.Console;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace MyBank
{
    class Program
    {

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Bienvenido a NGB");
            List<BankAccount> BankAccounts = new List<BankAccount>();
            string fileName = "BankAccountData.json";
            await using FileStream createStream = File.Create(fileName);
            try
            {
                Console.WriteLine("Create account (1)");
                Console.WriteLine("Make deposit (2)");
                Console.WriteLine("Withdraw money (3)");
                Console.WriteLine("Show all movements (4)");
                Console.WriteLine("Exit (0)");


                bool check = true;
                while (check)
                {
                    Console.WriteLine("What do you want to do next?");
                    int menu = Convert.ToInt32(Console.ReadLine());

                    switch (menu)
                    {
                        case 1:
                            Console.WriteLine("Insert username:");
                            string username = Console.ReadLine();
                            Validation(username, BankAccounts);
                            Console.WriteLine("Do you want to make your first deposit?");
                            Console.WriteLine("Yes/No");
                            string option = Console.ReadLine();
                            string optionToLower = option.ToLower();
                            if (optionToLower.Equals("yes"))
                            {
                                Console.WriteLine("Please, make your deposit.");
                                decimal InitialBalance = Convert.ToInt32(Console.ReadLine());
                                var UserAccount = new BankAccount(username, InitialBalance);
                                BankAccounts.Add(UserAccount);

                                await JsonSerializer.SerializeAsync(createStream, BankAccounts);
                            }
                            else
                            {
                                var UserAccount = new BankAccount(username, 0);
                                BankAccounts.Add(UserAccount);
                                await JsonSerializer.SerializeAsync(createStream, BankAccounts);
                            }

                            Console.WriteLine("Here's the list of all accounts in our current database.");
                            VisualizeAllAccounts(BankAccounts);
                            break;

                        case 2:
                            Console.WriteLine("In which account do you want to deposit?");
                            string Deposit_Username = Console.ReadLine();
                            Console.WriteLine("How much cash do you want to deposit?");
                            int cash = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Do you want to add a note?");
                            Console.WriteLine("Yes/No");
                            string AddNote_Deposit = Console.ReadLine();
                            string AddNoteToLower_Deposit= AddNote_Deposit.ToLower();
                            DepositInAccount(BankAccounts, Deposit_Username,cash,insertNote(AddNoteToLower_Deposit));
                            break;

                        case 3:
                            Console.WriteLine("From which account do you want to withdrawal?");
                            string WithDrawal_Username = Console.ReadLine();
                            Console.WriteLine("How much cash do you want to deposit?");
                            decimal Withdrawal_Balance = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Do you want to add a note?")
                            string AddNote_Withdrawal = Console.ReadLine();
                            string AddNoteToLower_Withdrawal = AddNote_Withdrawal.ToLower();
                            DepositInAccount(BankAccounts, WithDrawal_Username, Withdrawal_Balance, insertNote(AddNoteToLower_Withdrawal));
                            break;
                        case 4:
                            break;
                        case 0:
                            check = false;
                            break;
                    }
                }
            }
            //    var account = new BankAccount("Alex", 1000);
            //    Console.WriteLine($"Account {account.Number} was created for {account.Owner} with {account.Balance} initial balance.");
            //    account.MakeDeposit(500, DateTime.Now, "Bizum");
            //    account.MakeWithdrawal(100, DateTime.Now, "Gasofa");
            //    Console.WriteLine(account.GetAccountHistory());
            //    Console.WriteLine($"Account {account.Number} was created for {account.Owner} with {account.Balance} actual balance.");
            //    var accountV = new BankAccount("Vanessa", 5000);
            //    Console.WriteLine($"Account {accountV.Number} was created for {accountV.Owner} with {accountV.Balance} initial balance.");
            //}
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("ArgumentOutOfRangeException: " + e.ToString());
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("InvalidOperationException: " + e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

        }

        public static string DepositInAccount(List<BankAccount> Accounts, string username, decimal cash, string note)
        {
            bool notValid = true;
            bool exists = false;
            while (notValid)
            {

                foreach (var item in Accounts)
                {
                    if (item.Owner.Equals(username))
                    {
                       
                        item.MakeDeposit(cash, DateTime.Now, note);
                        exists = true;
                    }
                    else { exists = false; }

                }
                if (exists)
                {

                    Console.WriteLine("Deposit completed succesfully");
                    notValid = false;
                }
                else
                {
                    Console.WriteLine("We couldn't find the provided username");
                    Console.WriteLine("Please, choose another one.");
                    username = Console.ReadLine();
                }

               
            }
            return username;
        }
        public static string WithdrawalFromAccount(List<BankAccount> Accounts, string username, decimal cash, string note)
        {
            bool notValid = true;
            bool exists = false;
            while (notValid)
            {

                foreach (var item in Accounts)
                {
                    if (item.Owner.Equals(username))
                    {
                        item.MakeWithdrawal(cash, DateTime.Now, note);
                        exists = true;
                    }
                    else { exists = false; }

                }
                if (exists)
                {

                    Console.WriteLine("Username is not valid. Please, choose another one.");
                    username = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("We couldn't find the provided username");

                }


            }
            return username;
        }
        public static string Validation(string username, List<BankAccount> Accounts)
        {
            bool notValid = true;
            bool repeats = false;
            while (notValid)
            {

                foreach (var item in Accounts)
                {
                    if (item.Owner.Equals(username))
                    {
                      
                        repeats = true;
                    }
                    else { repeats = false; }

                }
                if (repeats)
                {

                    Console.WriteLine("Username is not valid. Please, choose another one.");
                    username = Console.ReadLine();
                }
                else
                {
                    notValid = false;

                }


            }
            return username;


        }
        public static void VisualizeAllAccounts(List<BankAccount> Accounts)
        {
            foreach (var item in Accounts)
            {
                Console.WriteLine($"{item.Owner}  -  {item.Balance}");
            }
        }
        public static string insertNote(string note)
        {
            const int MaxLength = 50;
            if (note.Equals("yes"))
            {
                Console.WriteLine("Please, add your note.");
                if (note.Length > MaxLength)
                    note = note.Substring(0, MaxLength);
            }
            else
            {
                note = "";
            }
            return note;
        }
    }
}
  

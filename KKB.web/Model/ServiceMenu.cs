﻿using KKB.BLL.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KKB.web.Model
{
    public class ServiceMenu
    {
        private User user = null;
        public void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("1. Registration");
            Console.WriteLine("2. LogOn");
            Console.Write(": ");
            int choice = Int32.Parse(Console.ReadLine());
            if (choice == 1)
            {
                RegistrationMenu();
            }
            else if (choice == 2)
            {
                LogOnMenu();
            }
        }

        private void LogOnMenu()
        {
            string login = "";
            string password = "";
            string message = "";
            Console.Write("Enter Login: ");
            login = Console.ReadLine();
            Console.Write("Enter Password: ");
            password = Console.ReadLine();
            ServiceUser susr = new ServiceUser();
            user = susr.LogOn(login, password, out message); //above user private one
            if (user != null)
            {
                AuthoriseUserMenu();
            }
            else
            {
                Console.WriteLine(message);
                Thread.Sleep(3000);
                MainMenu();
            }
        }

        private void RegistrationMenu()
        {
            User user = new User();
            Console.Write("Enter Login: ");
            user.login = Console.ReadLine();
            Console.Write("Enter Password: ");
            user.password = Console.ReadLine();
            Console.Write("Enter Full name: ");
            user.fullname = Console.ReadLine();
            //Console.Write("Enter date of birth: ");
            //user.dob = Console.ReadLine();
            string message = "";
            ServiceUser susr = new ServiceUser();
            if (susr.Registration(user, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(3000);
                MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void AuthoriseUserMenu()
        {

            Console.WriteLine("Приветствую вас {0}", user.fullname);
            if (user.Accounts.Count > 0)
            {
                Console.WriteLine("1. Вывод баланса");
                Console.WriteLine("2. Пополнение баланса");
                Console.WriteLine("3. Снять деньги со счета");
            }
            else
            {
                Console.WriteLine("5. Создать счет");
            }
            Console.WriteLine("4. Выход");

            int choice = Int32.Parse(Console.ReadLine());

            if (choice == 4)
            {
                user = null;
                MainMenu();

            }
            else if(choice == 5)
            {
                CreateAccountMenu();
            }


        }



        public void CreateAccountMenu()
        {
            ServiceAccount sa = new ServiceAccount();

            var acc = sa.CreateAccount(user.id, Currency.KZT);

            string message = " ";

            bool isCreated = sa.CreateAccountDb(acc, out message);

            if (isCreated)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                user.Accounts = sa.GetAccounts(user.id);

                Thread.Sleep(3000);
                AuthoriseUserMenu();
            }
        }

        private void ShowBalanceMenu()
        {
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                var saac = user.Accounts[i];
                Console.WriteLine(string.Format("{0:20}-{1:0.00}", saac.AccountNumber, saac.Balance));
            }

            Console.ReadKey();
            AuthoriseUserMenu();
        }

        private void AddMoney()
        {
            ShowBalanceMenu();
            Console.Write("Выберите счет:");
            int accid = Int32.Parse(Console.ReadLine());

            Console.Write("Введите сумму");
            double money = Double.Parse(Console.ReadLine());
        }

    }
}

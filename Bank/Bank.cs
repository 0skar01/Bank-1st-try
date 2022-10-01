using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    public class Bank
    {
        private Bank(string name, string bankleitzahl)
        {
            Name = name;
            Bankleitzahl = bankleitzahl;
        }
        public static Bank Erstellen(string name, string bankleitzahl)
        {
            Bank bank = new(name, bankleitzahl);
            string[] inputs = {bank.Name, bank.Bankleitzahl};
            DB.Insert("Bank", inputs);
            return bank;
        }
        public string Name { get; set; }
        public string Bankleitzahl { get; set; }
    }
}
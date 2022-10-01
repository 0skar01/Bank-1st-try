using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bank
{
    
    public class Kunde
    {
        private Kunde(Bank bank, string name, string vorname, string passwort, Konto? konto=null)
        {
            this.Name = name;
            Vorname = vorname;
            Bank = bank;
            Konto = konto;

            // Create the salt value with a cryptographic PRNG
            byte[] salt;
            RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);

            // Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(passwort, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Turn the combined salt+hash into a string for storage
            Passwort = Convert.ToBase64String(hashBytes);
        }
        public static Kunde Erstellen(Bank bank, string name, string vorname, string passwort, Konto? konto = null)
        {
            Kunde kunde = new(bank, name, vorname, passwort, konto);
            string[] inputs = { kunde.Name, kunde.Vorname, kunde.Passwort};
            DB.Insert("Kunde", inputs);
            string[] inputs2 = { "Kname", kunde.Name, "Bname", bank.Name };
            DB.Insert("KundenBank", inputs2);
            if(konto != null)
            {
                string[] inputs3 = { konto.KontoNr, kunde.Name };
                DB.Update("Konto", inputs3);
            }
            return kunde;
        }
        public string Name { get; set; }
        public string Vorname { get; set; }        
        public string Passwort { get; set; }
        public Bank Bank { get; set; }
        public Konto? Konto { get; set; }
    }
}
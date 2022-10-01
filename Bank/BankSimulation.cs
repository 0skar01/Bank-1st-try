using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Bank
{
    public class BankSimulation
    {
        public static void Main(string[] args)
        {
            /**
             * Todo:
             * 
             * update from kunde constructor the konto line in case konto was entered
             */
            Console.WriteLine(GetTablePK("kunden"));
            //Bank bank = Bank.Erstellen("Sparkasse", "DE555");
            //Kunde.Erstellen(bank, "askar", "wajeeh", "helloWorld123");
            
        }
        private static string GetTablePK(string table)
        {
            string connstring = "Data Source = MR10611W10; Initial Catalog = dbBanksimulation; Integrated Security=true";
            SqlConnection con = new(connstring);
            con.Open();
            string query = $"select C.COLUMN_NAME from INFORMATION_SCHEMA.TABLE_CONSTRAINTS T JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE C ON C.CONSTRAINT_NAME = T.CONSTRAINT_NAME where T.CONSTRAINT_TYPE = 'PRIMARY KEY' and T.TABLE_NAME = '{table}'";
            
            SqlCommand cmd = new(query, con);
            SqlDataReader reader = cmd.ExecuteReader();
            string PK = "";
            while (reader.Read())
            {
                PK = (string)reader.GetValue(0);
            }
            
            return PK;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Bank
{
    public class TermingeldKonto : Konto, IZinssatz
    {
        private TermingeldKonto(Bank bank, string kontoNr, float kontoStand, int jahren, double zinssatz, Kunde? kunde = null) : base(bank, kontoNr, kontoStand, kunde)
        {
            Jahren = jahren;
            Zinssatz = zinssatz;
        }
        public int Jahren { get; set; }
        public double Zinssatz { get ; set; }

        public static TermingeldKonto? Erstellen(Bank bank, string kontoNr, float kontoStand, int jahren, double zinssatz, Kunde? kunde = null)
        {
            if(kontoStand < 0.01)
            {
                Console.WriteLine($"TermingeldKonto kann nicht erstellt werden, weil Kontostatd {kontoStand} ist ");
                return null;
            }
            else
            {
                Console.WriteLine($"nach {jahren} Jahren haben Sie {kontoStand * Math.Pow((zinssatz/100)+1, jahren)} Euro");
                TermingeldKonto konto = new (bank, kontoNr, kontoStand, jahren, zinssatz, kunde);
                string connstring = "Data Source = MR10611W10; Initial Catalog = dbBanksimulation; Integrated Security=true";
                SqlConnection con = new(connstring);
                con.Open();
                string query = "Insert Into dbo.Banken( name, Bankleitzahl) Values('hello', 'hello');";
                SqlCommand cmd = new (query, con);
                //cmd.ExecuteNonQuery();
                //SqlDataReader reader = cmd.ExecuteReader();
                Console.WriteLine(cmd.ExecuteNonQuery());
                //while (reader.Read())
                //{
                //    Console.WriteLine(reader.GetString(0)); 
                //}
                return konto;
            }
        }
    }
}
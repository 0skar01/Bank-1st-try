using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    public class SparKonto : KontoMitEAUe, IZinssatz
    {
        private SparKonto(Bank bank, string kontoNr, float kontoStand, double zinssatz, Kunde? kunde = null) : base(bank, kontoNr, kontoStand, kunde)
        {
            Zinssatz = zinssatz;
            Grenze = 0;
        }
        public SparKonto Erstellen(Bank bank, string kontoNr, float kontoStand, double zinssatz, Kunde? kunde = null)
        {
            SparKonto konto = new (bank, kontoNr, kontoStand, zinssatz, kunde);
            string[] inputs = { konto.KontoNr, konto.KontoStand.ToString() };
            DB.Insert("Konto", inputs);
            string[] inputs2 = { "Bank", bank.Name };
            DB.Update("Konto", inputs2);
            return konto;
        }

        public double Zinssatz { get; set; }
    }
}
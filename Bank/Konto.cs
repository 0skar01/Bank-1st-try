using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    public abstract class Konto
    {
        protected Konto(Bank bank, string kontoNr, float kontoStand,Kunde? kunde = null)
        {
            KontoNr = kontoNr;
            KontoStand = kontoStand;
            Bank = bank;
            Kunde = kunde;
        }
        public string KontoNr { get; set; }

        public float KontoStand { get; set; }

        public Bank Bank { get; set; }

        public Kunde? Kunde { get; set; }
    }
}
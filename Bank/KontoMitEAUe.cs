using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    public abstract class KontoMitEAUe : Konto
    {
        protected KontoMitEAUe(Bank bank, string kontoNr, float kontoStand, Kunde? kunde = null) : base(bank, kontoNr, kontoStand, kunde)
        {
        }

        public float Grenze { get; set; }

        public void Einzahlung()
        {

        }
        public void Auszahlung()
        {

        }
        public void Ueberweisung()
        {

        }
    }
}
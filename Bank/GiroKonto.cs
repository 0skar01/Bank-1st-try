using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bank
{
    public class GiroKonto : KontoMitEAUe
    {
        public GiroKonto(Bank bank, string kontoNr, float kontoStand, bool dispo, float dispoHoehe, Kunde? kunde = null) : base(bank, kontoNr, kontoStand, kunde)
        {
            Dispo = dispo;
            DispoHoehe = !dispo ? 0 : dispoHoehe;
            Grenze = !dispo ? 0 : dispoHoehe;
        }

        public bool Dispo { get; set; }
        public float DispoHoehe { get; set; }
    }
}
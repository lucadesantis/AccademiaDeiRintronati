using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccademiaDeiRintronati
{
    public class DataSimply
    { 
        private int giorno=1;
        private int mese=1;
        private int anno = 0;

        public int Giorno
        {
            get { return giorno; }
            set { giorno = value; }
        }

        public int Mese
        {
            get { return mese; }
            set 
            {
                if (value < 1 || value > 12)
                    throw new ArgumentOutOfRangeException();

                mese = value;
            }
        }

        public int Anno
        {
            get { return anno; }
            set{ anno = value; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Data
{
    class MesAyo
    {
        public int Mes { get; set; }
        public int Ayo { get; set; }
        public override string ToString()
        {
            string[] nombre_mes = new string[] {"Invalido","Enero","Febrero","Marzo", "Abril","Mayo","Junio","Julio","Agosto","Septiembre","Octubre","Noviembre","Diciembre" };
            if (Mes <= 12 && Mes >= 1)
            {
                return string.Format("{0} - {1}", Ayo, nombre_mes[Mes]);
            }
            else
            {
                return "Mes invalido";
            }
        }
    }
}

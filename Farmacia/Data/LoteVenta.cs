using Epn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Data
{
    public class LoteVenta
    {
        public int IdLoteVenta { get; set; }
        public int IdLote { get; set; }
        public decimal Cantidad { get; set; }
        public int IdDetalleVenta { get; set; }
        public void Insert()
        {
            IdLoteVenta = Data.Default.Db.dbo.USPLOTEVENTAINSERTAR<int>(Record.FromInstance(this));
        }
    }
}

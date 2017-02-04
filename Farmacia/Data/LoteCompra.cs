using Epn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Data
{
    public class LoteCompra
    {
        public int IdLoteCompra { get; set; }
        public int IdLote { get; set; }
        public int IdDetalleCompra { get; set; }
        public void Insert()
        {
            IdLoteCompra = Data.Default.Db.dbo.USPLOTECOMPRAINSERTAR<int>(Record.FromInstance(this));
        }
    }
}

using Epn.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia.Data
{
    class Venta
    {
        public int Numero { get; set; }
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IVA { get; set; }
        public decimal Total { get; set; }
        public bool Anulado { get; set; }
        public Cliente Cliente { get; set; }
        public BindingList<DetalleVenta> Detalle { get; set; }

        public static Venta Get(int IdVenta)
        {
            return Default.Db.USPVENTASELECCIONAR<Record, Compra>(IdVenta: IdVenta);
        }
        public static BindingList<Venta> Get()
        {
            return new BindingList<Venta>(Default.Db.USPVENTASELECCIONAR<RecordSet, Venta>());
        }

        public Venta()
        {
            Detalle = new BindingList<DetalleVenta>();
        }

        public void Insert()
        {
            IdVenta = Default.Db.dbo.USPVENTAINSERTAR<int>(Record.FromInstance(this));
            foreach (DetalleVenta dv in Detalle)
            {
                dv.IdVenta = IdVenta;
                dv.IdDetalleVenta = Default.Db.dbo.USPDETALLEVENTAINSERTAR<int>(Record.FromInstance(dv));
                foreach (Lote lt in dv.Lote.Detalle)
                {
                    Data.LoteVenta lote_venta = new LoteVenta();
                    lote_venta.IdLote = lt.IdLote;
                    lote_venta.IdDetalleVenta = dv.IdDetalleVenta;
                    lote_venta.Insert();
                }
            }
        }

        public void Annulate()
        {
            Default.Db.dbo.USPVENTAANULAR(IdVenta: IdVenta);
        }
    }
}

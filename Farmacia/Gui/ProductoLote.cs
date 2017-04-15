using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Farmacia.Gui
{
    public partial class ProductoLote : Farmacia.Gui.BaseDialog
    {
        bool m_de_venta = false;
        public ProductoLote(bool deVenta = false)
        {
            InitializeComponent();
            dgvDetalle.AutoGenerateColumns = false;
            dgvProducto.AutoGenerateColumns = false;
            m_de_venta = deVenta;
            if (deVenta)
            {
                dgvProducto.ReadOnly = false;
                colNombre.ReadOnly = true;
                colPrecio.ReadOnly = true;
                colSubtotal.ReadOnly = true;
                colExistencia.Visible = true;
                colAfectacion.Visible = true;
                colSaldo.Visible = true;
                dgvDetalle.ReadOnly = true;
                dgvDetalle.AllowUserToAddRows = false;
                dgvDetalle.AllowUserToDeleteRows = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ProductoLote_Load(object sender, EventArgs e)
        {
            var plote = Data.ProductoLote.Get();
            if (m_de_venta)
            {
                var llote = Data.Lote.Get(0);
                foreach (Data.ProductoLote pl in plote)
                {
                    pl.Detalle = new BindingList<Data.Lote>((from ll in llote where ll.IdProducto == pl.IdProducto select ll).ToList());
                    if(pl.Detalle.Count > 0)
                    {
                        decimal Total = pl.Detalle.Sum(m => m.Precio * m.Existencia);
                        decimal Cantidad = pl.Detalle.Sum(m => m.Existencia);
                        //TODO: Cambiar porcentaje de ganancias
                        pl.Precio = (Total / Cantidad) * 1.2m;
                    }
                    foreach (Data.Lote ll in pl.Detalle)
                    {
                        ll.Afectacion = 0;
                        ll.Saldo = ll.Existencia;
                    }
                }
            }
            dgvProducto.DataSource = plote;

        }

        private void dgvProducto_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) {
                Data.ProductoLote plote = ((BindingList<Data.ProductoLote>) dgvProducto.DataSource)[e.RowIndex];
                dgvDetalle.DataSource = plote.Detalle;
            }
        }

        private void productRowUpdate()
        {
            foreach (Data.ProductoLote producto in (IEnumerable<Data.ProductoLote>) dgvProducto.DataSource)
            {
                decimal cantidad = 0,subtotal = 0;
                foreach(Data.Lote lote in producto.Detalle)
                {
                    lote.IdProducto = producto.IdProducto;
                    cantidad += lote.Cantidad;
                    subtotal += lote.Precio * lote.Cantidad;
                }
                producto.Cantidad = cantidad;
                if(cantidad > 0)
                    producto.Precio = subtotal / cantidad;
            }
        }

        private void dgvDetalle_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(!m_de_venta)
                productRowUpdate();
            dgvDetalle.Update();
        }

        public List<Data.ProductoLote> Lote
        {
            get {
                IEnumerable<Data.ProductoLote> source = ((IEnumerable<Data.ProductoLote>)dgvProducto.DataSource);
                return (from Data.ProductoLote pl in source where pl.Detalle.Count > 0 select pl).ToList();
            }
        }

        private void dgvProducto_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if(m_de_venta)
            { 
                Data.ProductoLote plote = ((BindingList<Data.ProductoLote>)dgvProducto.DataSource)[e.RowIndex];
                decimal Afectacion = plote.Cantidad;
                foreach (Data.Lote llote in plote.Detalle)
                {
                    if (Afectacion > 0)
                    {
                        if (llote.Existencia >= Afectacion)
                        {
                            llote.Saldo = llote.Existencia - Afectacion;
                            llote.Afectacion = Afectacion;
                            Afectacion = 0;
                        }
                        else
                        {
                            llote.Saldo = 0;
                            llote.Afectacion = llote.Existencia;
                            Afectacion = Afectacion - llote.Existencia;
                        }
                    }
                    else
                    {
                        llote.Afectacion = 0;
                        llote.Saldo = llote.Existencia;
                    }
                }

                if (Afectacion > 0)
                {
                    if (MessageBox.Show("No existe suficiente producto en el inventario para satisfacer la venta\n ¿Desea cambiar la cantidad de la venta a la existencia actual?", "Insuficiente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        plote.Cantidad = plote.Cantidad - Afectacion;
                    }
                    else
                    {
                        plote.Cantidad = 0;
                        foreach (Data.Lote llote in plote.Detalle)
                        {
                            llote.Saldo = llote.Existencia;
                            llote.Afectacion = 0;
                        }
                    }
                }

                dgvProducto.Update();
                dgvDetalle.Update();
            }
        }
    }
}

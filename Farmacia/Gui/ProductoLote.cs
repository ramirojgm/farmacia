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
        public ProductoLote()
        {
            InitializeComponent();
            dgvDetalle.AutoGenerateColumns = false;
            dgvProducto.AutoGenerateColumns = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ProductoLote_Load(object sender, EventArgs e)
        {
            dgvProducto.DataSource = Data.ProductoLote.Get();
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
    }
}

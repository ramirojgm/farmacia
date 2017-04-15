using Farmacia.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Farmacia
{
    public partial class VentaDialog : Farmacia.Gui.BaseDialog
    {
        public VentaDialog()
        {
            InitializeComponent();
        }

        private void VentaDialog_Load(object sender, EventArgs e)
        {
            cmbCliente.DataSource = Data.Cliente.Get();
            cmbCliente.ValueMember = "IdCliente";
            cmbCliente.DisplayMember = "Nombre";
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.DataSource = new List<Data.DetalleVenta>();
            colProducto.DataSource = Data.Producto.Get();
            colProducto.DisplayMember = "Nombre";
            colProducto.ValueMember = "IdProducto";
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue != null)
            {
                txtTotal.Enabled = true;
                seleccionarButton.Enabled = true;
                txtIva.Enabled = true;
                chkIva.Enabled = true;
                txtSubtotal.Enabled = true;
                dgvDetalle.Enabled = true;
            }
        }

        private void seleccionarButton_Click(object sender, EventArgs e)
        {
            ProductoLote dialog = new ProductoLote(true);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                /*if (dialog.Lote.Count > 0)
                {
                    List<Data.DetalleCompra> source = new List<Data.DetalleCompra>();
                    foreach (var det in dialog.Lote)
                    {
                        Data.DetalleCompra dc = new Data.DetalleCompra();
                        dc.IdProducto = det.IdProducto;
                        dc.Cantidad = det.Detalle.Sum(m => m.Cantidad);
                        dc.Precio = det.Detalle.Average(m => m.Precio);
                        dc.Importe = dc.Cantidad * dc.Precio;
                        dc.Total = dc.Importe;
                        dc.Lote = det;
                        source.Add(dc);
                    }
                    dgvDetalle.DataSource = source;
                    CalcularIva();
                }
                else
                    dgvDetalle.DataSource = new List<Data.DetalleCompra>();*/
            }
        }
    }
}

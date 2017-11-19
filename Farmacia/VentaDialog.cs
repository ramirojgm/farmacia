using Farmacia.Gui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace Farmacia
{
    public partial class VentaDialog : Farmacia.Gui.BaseDialog
    {
        public VentaDialog()
        {
            InitializeComponent();
        }

        public Data.Venta Get()
        {
            CalcularIva();
            Data.Venta venta = new Data.Venta();
            venta.Numero = Convert.ToInt32(numNumero.Value);
            venta.IdCliente = Convert.ToInt32(cmbCliente.SelectedValue);
            venta.Fecha = dtpFecha.Value;
            venta.IVA = Convert.ToDecimal(txtIva.Text);
            venta.Subtotal = Convert.ToDecimal(txtSubtotal.Text);
            venta.Total = Convert.ToDecimal(txtTotal.Text);
            venta.Detalle = new BindingList<Data.DetalleVenta>((List<Data.DetalleVenta>)dgvDetalle.DataSource);
            return venta;
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

        private void CalcularIva()
        {
            List<Data.DetalleVenta> source = (List<Data.DetalleVenta>)dgvDetalle.DataSource;
            decimal SubTotal = source.Sum(m => m.Importe);
            txtSubtotal.Text = (SubTotal).ToString();
            if (chkIva.Checked)
            {
                decimal Total = SubTotal * 1.15m;
                txtIva.Text = (SubTotal * 0.15m).ToString();
                txtTotal.Text = (Total).ToString();
            }
            else
            {
                txtIva.Text = "0";
                txtTotal.Text = (SubTotal).ToString();
            }

        }

        private void seleccionarButton_Click(object sender, EventArgs e)
        {
            ProductoLote dialog = new ProductoLote(true);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Lote.Count > 0)
                {
                    List<Data.DetalleVenta> source = new List<Data.DetalleVenta>();
                    foreach (var det in dialog.Lote)
                    {
                        Data.DetalleVenta dv = new Data.DetalleVenta();
                        dv.IdProducto = det.IdProducto;
                        dv.Cantidad = det.Cantidad;
                        dv.Precio = det.Precio;
                        dv.Importe = dv.Cantidad * dv.Precio;
                        dv.Lote = det;
                        source.Add(dv);
                    }
                    dgvDetalle.DataSource = source;
                    CalcularIva();
                }
                else
                    dgvDetalle.DataSource = new List<Data.DetalleVenta>();
            }
        }

        private void chkIva_CheckedChanged(object sender, EventArgs e)
        {
            CalcularIva();
        }
    }
}

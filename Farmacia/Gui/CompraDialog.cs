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
    public partial class CompraDialog : Farmacia.Gui.BaseDialog
    {
        public CompraDialog()
        {
            InitializeComponent();
            cmbProveedor.DataSource = Data.Proveedor.Get();
            cmbProveedor.ValueMember = "IdProveedor";
            cmbProveedor.DisplayMember = "Nombre";
            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.DataSource = new List<Data.DetalleCompra>();
            colProducto.DataSource = Data.Producto.Get();
            colProducto.DisplayMember = "Nombre";
            colProducto.ValueMember = "IdProducto";
        }

        public Data.Compra Get()
        {
            CalcularIva();
            Data.Compra compra = new Data.Compra();
            compra.NumeroCompra = Convert.ToInt32(numNumero.Value);
            compra.IdProveedor = Convert.ToInt32(cmbProveedor.SelectedValue);
            compra.Fecha = dtpFecha.Value;
            compra.Descuento = 0;
            compra.IVA = Convert.ToDecimal(txtIva.Text);
            compra.Subtotal = Convert.ToDecimal(txtSubtotal.Text);
            compra.Total = Convert.ToDecimal(txtTotal.Text);
            compra.Detalle = new BindingList<Data.DetalleCompra>((List<Data.DetalleCompra>)dgvDetalle.DataSource); 
            return compra;
        }

        private void CalcularIva()
        {
            List<Data.DetalleCompra> source = (List<Data.DetalleCompra>)dgvDetalle.DataSource;
            decimal SubTotal = source.Sum(m => m.Total);
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
            ProductoLote dialog = new ProductoLote();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.Lote.Count > 0)
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
                    dgvDetalle.DataSource = new List<Data.DetalleCompra>();
            }
        }

        private void cmbProveedor_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue != null)
            {
                txtTotal.Enabled = true;
                seleccionarButton.Enabled = true;
                txtIva.Enabled = true;
                chkIva.Enabled = true;
                txtSubtotal.Enabled = true;
                dgvDetalle.Enabled = true;
            }
        }

        private void chkIva_CheckedChanged(object sender, EventArgs e)
        {
            CalcularIva();
        }
    }
}

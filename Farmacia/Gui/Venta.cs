using Epn.Data;
using Farmacia.Utils;
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
    [Activity("Venta", "price-tag.png")]
    public partial class Venta : Farmacia.Gui.Activity
    {
        private BindingList<Data.Venta> source = null;
        private bool selecting = false;

        public Venta()
        {
            InitializeComponent();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VentaDialog dialog = new VentaDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Data.Venta venta = dialog.Get();
                if (venta.Detalle.Count > 0)
                {
                    venta.Insert();
                    LoadSource();
                }
                else
                {
                    MessageBox.Show("La compra esta vacia no se puede guardar", "Guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void anularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVenta.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Esta seguro que desea anular la venta", "Anular", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Data.Venta venta = source[dgvVenta.SelectedRows[0].Index];
                    if (!venta.Anulado)
                    {
                        venta.Annulate();
                        LoadSource();
                    }
                    else
                    {
                        MessageBox.Show("La venta ya esta anulada", "Anular", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void mes_ayo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!selecting)
            {
                if (mes_ayo.Selected)
                {
                    LoadSource();
                }
            }
        }
        private void LoadSource()
        {
            bool selected = false;
            int selected_index = -1;
            Data.MesAyo selected_value = null;
            if (mes_ayo.SelectedItem != null)
            {
                selected = true;
                selected_value = (Data.MesAyo)mes_ayo.SelectedItem;
            }
            mes_ayo.Items.Clear();
            dynamic min_max = Data.Default.Db.Query<Record>("SELECT MAX(Fecha) AS MX,MIN(Fecha) AS MN FROM Venta");
            if (min_max.MN != null)
            {
                DateTime min = Convert.ToDateTime(min_max.MN);
                DateTime max = Convert.ToDateTime(min_max.MX);
                DateTime from = new DateTime(min.Year, min.Month, 1);
                DateTime at = new DateTime(max.Year, max.Month, 1);
                for (DateTime date = from; date <= at; date = date.AddMonths(1))
                {
                    int index = mes_ayo.Items.Add(new Data.MesAyo
                    {
                        Ayo = date.Year,
                        Mes = date.Month
                    });
                    if (selected)
                    {
                        if (selected_value.Mes == date.Month && selected_value.Ayo == date.Year)
                        {
                            selected_index = index;
                        }
                    }
                }
                if (selected_index != -1)
                {
                    selecting = true;
                    mes_ayo.SelectedIndex = selected_index;
                    selecting = false;
                }
            }
            if (mes_ayo.SelectedItem != null)
            {
                source = Data.Venta.Get();
                IEnumerable<Data.Venta> enumerable = source;
                source = new BindingList<Data.Venta>((from Data.Venta x in enumerable where x.Fecha.Month == selected_value.Mes && x.Fecha.Year == selected_value.Ayo select x).ToList());
                dgvVenta.DataSource = source;
            }
        }

        private void Venta_Load(object sender, EventArgs e)
        {
            dgvVenta.AutoGenerateColumns = false;
            colCliente.DataSource = Data.Cliente.Get();
            colCliente.DisplayMember = "Nombre";
            colCliente.ValueMember = "IdCliente";
            LoadSource();
        }
    }
}

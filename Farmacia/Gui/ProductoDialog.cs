using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Dynamic;
using Epn.Data;

namespace Farmacia.Gui
{
    public partial class ProductoDialog : Farmacia.Gui.BaseDialog
    {
        private Data.Producto entity;
        //Constructor para uno nuevo
        public ProductoDialog()
        {
            InitializeComponent();
            entity = new Data.Producto();
            entity.Categoria = "";
            entity.Marca = "";
            entity.Nombre = "";
            entity.Descripcion = "";
            entity.Especificaciones = "";
            entity.Precio = 0.0m;
            LoadEntity();
        }
        //Constructor para editar
        public ProductoDialog(Data.Producto record)
        {
            InitializeComponent();
            entity = record;
            LoadEntity();
        }

        private void LoadEntity()
        {
            cmbCategoria.DataSource = Data.Default.Db.USPCATEGORIASELECCIONAR<RecordSet>().ToDataTable();
            cmbMarca.DataSource = Data.Default.Db.USPMARCASELECCIONAR<RecordSet>().ToDataTable();
            cmbCategoria.DisplayMember = "Nombre";
            cmbMarca.DisplayMember = "Nombre";
            cmbCategoria.Text = entity.Categoria;
            cmbMarca.Text = entity.Marca;
            txtNombre.Text = entity.Nombre;
            txtDescripcion.Text = entity.Descripcion;
            txtEspecificaciones.Text = entity.Especificaciones;
        }

        public void Save()
        {
            entity.Categoria = cmbCategoria.Text;
            entity.Marca = cmbMarca.Text;
            entity.Nombre = txtNombre.Text;
            entity.Descripcion = txtDescripcion.Text;
            entity.Especificaciones = txtEspecificaciones.Text;
        }

        public Data.Producto Record
        {
            get {
                return entity;
            }
        }

        private void ProductoDialog_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}

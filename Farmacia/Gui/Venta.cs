using Farmacia.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
    }
}

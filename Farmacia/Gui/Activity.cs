using Farmacia.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Farmacia.Gui
{
    public partial class Activity : Form
    {
        public Activity()
        {
            InitializeComponent();
            foreach (var att in this.GetType().GetCustomAttributes(typeof(ActivityAttribute), false))
            {
                var aa = att as ActivityAttribute;
                this.Icon = Icon.FromHandle(new Bitmap(Image.FromFile("Icon/" + aa.IconName),new Size(16,16)).GetHicon());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class Chapeau_Form : Form
    {
        public Chapeau_Form()
        {
            InitializeComponent();

            //Reduce windows form flicker
            this.DoubleBuffered = true;
            //enableDoubleBuff(PanelName);
        }
        public static void enableDoubleBuff(Control cont)
        {
            System.Reflection.PropertyInfo DemoProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            DemoProp.SetValue(cont, true, null);
        }

        private void Chapeau_Form_Load(object sender, EventArgs e)
        {
            Panel order_panel = new Panel();
            order_panel.Size = new Size(3000, 2000);

        }
    }
}

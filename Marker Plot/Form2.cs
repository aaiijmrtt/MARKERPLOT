using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _Marker_Plot
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.Retry;
            button2.DialogResult = DialogResult.Yes;
            button3.DialogResult = DialogResult.Cancel;
            button4.DialogResult = DialogResult.No;
        }
    }
}

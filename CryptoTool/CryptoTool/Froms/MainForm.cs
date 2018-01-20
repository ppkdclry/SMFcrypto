using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoTool.Froms
{
    public partial class MainForm : Form
    {
        private string[] args = null;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string[] args)
        {
            this.args = args;
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}

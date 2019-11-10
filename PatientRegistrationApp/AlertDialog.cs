using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientRegistrationApp
{
    public partial class AlertDialog : Form
    {
        public AlertDialog(string inDialogText)
        {
            InitializeComponent();
            textAlert.Text = inDialogText;
        }

        private void AlertDialog_Load(object sender, EventArgs e)
        {

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

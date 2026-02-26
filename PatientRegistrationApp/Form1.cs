using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;

namespace PatientRegistrationApp
{
    public partial class PatientRegistrationApp : Form
    {

        private string m_FileStr;
        private bool mb_initialized = false;

        public PatientRegistrationApp()
        {
            InitializeComponent();
            m_FileStr = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Patient Registration.xlsx");

            // Check if file exists; if not, ask user to specify file
            if (!File.Exists(m_FileStr))
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files (*.xlsx; *.xls)|*.xlsx;*.xls|All files (*.*)|*.*";
                    openFileDialog.Title = "Please select the missing Excel file";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        m_FileStr = openFileDialog.FileName;
                        mb_initialized = true;
                    } 
                    else
                    {
                        mb_initialized = false;
                        MessageBox.Show("File selection was cancelled. Application cannot proceed.");
                    }

                }
 
            }
            else
            {
                mb_initialized = true;
            }
        }

        public bool IsInitialized() { return mb_initialized; }

        private void ExecuteExcelAction(Action<XLWorkbook> action)
        {
            try
            {
                using (var workbook = new XLWorkbook(m_FileStr))
                {
                    action(workbook);
                    workbook.Save();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("The file is currently open in another program. Please close it and try again.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void btnSendMailmerge_Click(object sender, EventArgs e)
        {
            Form mailMergeDialog = new MailMergeDialog(m_FileStr);
            mailMergeDialog.ShowDialog();
        }

        private void btnOpenXls_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(m_FileStr);
        }


        private void btnInputPat_Click(object sender, EventArgs e)
        {
            Form addPatientDialog = new AddPatientDialog(m_FileStr);
            addPatientDialog.ShowDialog();
        }


        private void btnDeletePat_Click(object sender, EventArgs e)
        {
            Form deletePatientDialog = new DeletePatientDialog(m_FileStr);
            deletePatientDialog.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string dialogText = "Written and designed by Andrew Gorbaty and Anish Boddu." 
                 + "This software is distributed under an 'as-is' license, which allows any developer to modify its source code without permission from its original author.";
            Form prompt = new AlertDialog(dialogText);
            prompt.ShowDialog();
        }

        private void btnEditPat_Click(object sender, EventArgs e)
        {
            Form editPatientDialog = new EditPatientDialog(m_FileStr);
            editPatientDialog.ShowDialog();
        }

        protected override void OnClosed(EventArgs e) {base.OnClosed(e);}

        
        private void PatientRegistrationApp_Load(object sender, EventArgs e)
        {

        }

    }
}

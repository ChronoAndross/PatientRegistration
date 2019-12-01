using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PatientRegistrationApp
{
    public partial class DeletePatientDialog : Form
    {
        public DeletePatientDialog()
        {
            InitializeComponent();
        }

        public DeletePatientDialog(Excel.Workbook inWorkbook)
        {
            InitializeComponent();
            m_existingWkBook = inWorkbook;
        }

        private void DeletePatientDialog_Load(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.Sheets["Patient_Registration MASTER"] ;
                int currRow = 2; // always start on the second row
                while ((currSheet.Cells[currRow, 2]).Text != "")
                {
                    // put new patient into excel doc
                    string firstName = currSheet.Cells[currRow, kFirstNameLoc].Value;
                    string lastName = currSheet.Cells[currRow, kLastNameLoc].Value;
                    comboPatients.Items.Add(firstName + " " + lastName);
                    currRow++;
                }
            }
            comboPatients.SelectedItem = null;
            comboPatients.SelectedText = "--Please select a patient to remove--";
        }

        private Excel.Workbook m_existingWkBook = null; // passed from main dialog

        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (comboPatients.SelectedItem != null && m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.Sheets["Patient_Registration MASTER"] ;
                ((Excel.Range)currSheet.Rows[comboPatients.SelectedIndex+2]).Delete();
                m_existingWkBook.Save();
                this.Close();
            }
            else
            {
                string dialogText = "A patient has not been selected. Please select a patient to remove.";
                Form prompt = new AlertDialog(dialogText);
                prompt.ShowDialog();
            }
        }
    }
}

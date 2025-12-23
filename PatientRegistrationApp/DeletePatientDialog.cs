using ClosedXML.Excel;
using System;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PatientRegistrationApp
{
    public partial class DeletePatientDialog : Form
    {
        private string m_filePath;
        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;

        public DeletePatientDialog(string filePath)
        {
            InitializeComponent();
            m_filePath = filePath;
        }

        private void DeletePatientDialog_Load(object sender, EventArgs e)
        {
            try
            {
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet("Patient_Registration MASTER");

                    // Get all rows that have data (starting from row 2 to skip headers)
                    var rows = worksheet.RowsUsed(r => r.RowNumber() >= 2);

                    comboPatients.Items.Clear();
                    foreach (var row in rows)
                    {
                        string firstName = row.Cell(kFirstNameLoc).GetValue<string>();
                        string lastName = row.Cell(kLastNameLoc).GetValue<string>();
                        comboPatients.Items.Add($"{firstName} {lastName}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load patients, Reason: " + ex.Message);
            }

            comboPatients.Text = "--Please select a patient to remove--";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (comboPatients.SelectedIndex != -1)
            {
                int rowToDelete = comboPatients.SelectedIndex + 2;

                try
                {
                    using (var workbook = new XLWorkbook(m_filePath))
                    {
                        var worksheet = workbook.Worksheet("Patient_Registration MASTER");
                        worksheet.Row(rowToDelete).Delete(); // Removes the row and shifts others up
                        workbook.Save();
                    }
                    this.Close();
                }
                catch (IOException)
                {
                    MessageBox.Show("The file is open in another program. Please close it first.");
                }
            }
            else
            {
                Form prompt = new AlertDialog("A patient has not been selected. Please select a patient to remove.");
                prompt.ShowDialog();
            }
        }
    }
}

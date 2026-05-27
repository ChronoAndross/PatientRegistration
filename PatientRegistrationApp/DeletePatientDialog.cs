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
        private string m_currentUser; // Store the logged-in user
        private string workSheetName = "Patient_Registration MASTER";
        private string m_logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationLog.txt");


        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;

        // Master list to hold all patients loaded from Excel
        private System.Collections.Generic.List<PatientItem> allPatients = new System.Collections.Generic.List<PatientItem>();

        public DeletePatientDialog(string filePath, string currentUser)
        {
            InitializeComponent();
            m_filePath = filePath;
            m_currentUser = currentUser; // Assign from constructor
        }

        private void DeletePatientDialog_Load(object sender, EventArgs e)
        {
            LoadPatientsFromExcel();
        }


        private void LoadPatientsFromExcel()
        {
            try
            {
                allPatients.Clear();
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet(workSheetName);
                    var rows = worksheet.RowsUsed(r => r.RowNumber() >= 2);

                    foreach (var row in rows)
                    {
                        allPatients.Add(new PatientItem
                        {
                            Name = $"{row.Cell(kFirstNameLoc).GetValue<string>()} {row.Cell(kLastNameLoc).GetValue<string>()}",
                            RowIndex = row.RowNumber()
                        });
                    }
                }

                // Sort Alphabetically and populate
                UpdatePatientDisplay(allPatients);
            }
            catch (Exception ex)
            {
                Utils.LogAction("ERROR", $"Failed to load patient list for deletion: {ex.Message}", m_currentUser, m_logPath);
                MessageBox.Show("Unable to load patients: " + ex.Message);
            }
        }

        private void UpdatePatientDisplay(System.Collections.Generic.List<PatientItem> patientsToShow)
        {
            comboPatients.Items.Clear();

            var sorted = System.Linq.Enumerable.ToList(
                System.Linq.Enumerable.OrderBy(patientsToShow, p => p.Name)
            );

            foreach (var p in sorted)
            {
                comboPatients.Items.Add(p);
            }

            if (comboPatients.Items.Count > 0)
            {
                comboPatients.SelectedIndex = 0;
            }
            else
            {
                comboPatients.SelectedIndex = -1;
                comboPatients.Text = "No results found...";
            }
        }

        // --- SEARCH FUNCTION ---
        // In the Designer, add a TextBox named 'textSearch' and double-click it to create this event
        private void textFirstName_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = textSearch.Text.ToLower();

            var filtered = System.Linq.Enumerable.ToList(
                System.Linq.Enumerable.Where(allPatients, p => p.Name.ToLower().Contains(searchTerm))
            );

            UpdatePatientDisplay(filtered);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // Get the selected object
            PatientItem selected = comboPatients.SelectedItem as PatientItem;

            if (selected != null)
            {
                // 1. Confirm deletion with the user
                DialogResult confirm = MessageBox.Show($"Are you sure you want to permanently delete {selected.Name}?",
                    "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.No)
                {
                    Utils.LogAction("DELETE_CANCELLED", $"User declined deletion of: {selected.Name}", m_currentUser, m_logPath);
                    return;
                }

                int rowToDelete = selected.RowIndex;

                try
                {
                    using (var workbook = new XLWorkbook(m_filePath))
                    {
                        var worksheet = workbook.Worksheet(workSheetName);
                        worksheet.Row(rowToDelete).Delete();
                        workbook.Save();
                    }

                    // 2. Log Success
                    Utils.LogAction("RECORD_DELETED", $"Successfully removed patient: {selected.Name} (Original Row: {rowToDelete})", m_currentUser, m_logPath);

                    MessageBox.Show("Patient removed successfully.");
                    this.Close();
                }
                catch (IOException ex)
                {
                    Utils.LogAction("ERROR", $"Failed to delete {selected.Name}: File locked, error: {ex.Message}", m_currentUser, m_logPath);
                    MessageBox.Show("The file is open in another program. Please close it first.");
                }
                catch (Exception ex)
                {
                    Utils.LogAction("ERROR", $"Critical error during deletion of {selected.Name}: {ex.Message}", m_currentUser, m_logPath);
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                Form prompt = new AlertDialog("Please select a patient to remove.");
                prompt.ShowDialog();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }

    public class PatientItem
    {
        public string Name { get; set; }
        public int RowIndex { get; set; }
        // This tells the ComboBox what text to show
        public override string ToString() => Name;
    }

}

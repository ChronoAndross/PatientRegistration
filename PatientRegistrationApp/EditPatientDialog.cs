using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientRegistrationApp
{
    public partial class EditPatientDialog : Form
    {

        private string m_filePath;
        private List<PatientItem> allPatients = new List<PatientItem>();
        private bool m_isFiltering = false; // Prevents UI flicker while searching

        private const int kCurrDateLoc = 1;
        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;
        private const int kAddressLoc = 7;
        private const int kCityLoc = 8;
        private const int kStateLoc = 9;
        private const int kZipLoc = 10;
        private const int kHomePhoneLoc = 11;
        private const int kCellPhoneLoc = 12;
        private const int kInsuranceLoc = 20;
        private const int kReturnDateLoc = 18;
        private const int kNotesLoc = 19;

        enum DialogDataValid
        {
            eNoError,
            eIncompleteForm,
            eBadZipCodeFormat,
            eBadStateFormat,
            eBadDateFormat,
            eNotesTooLong,
            eNoPatientSelected
        }

        public EditPatientDialog(string filePath)
        {
            InitializeComponent();
            m_filePath = filePath;
        }

        private void EditPatientDialog_Load(object sender, EventArgs e)
        {
            LoadPatientList();
        }

        private DialogDataValid IsInputDataValid()
        {

            if (comboPatients.SelectedIndex == -1 || comboPatients.SelectedItem == null)
            {
                return DialogDataValid.eNoPatientSelected;
            }

            DialogDataValid outValidData = DialogDataValid.eNoError;
            string firstNameStr = textFirstName.Text;
            string lastNameStr = textLastName.Text;
            string addrStr = textAddress.Text;
            string cityStr = textCity.Text;
            string stateStr = textState.Text;
            string zipStr = textZip.Text;

            if (firstNameStr.Equals("") || lastNameStr.Equals("") || addrStr.Equals("")
                || cityStr.Equals("") || stateStr.Equals("") || zipStr.Equals(""))
                outValidData = DialogDataValid.eIncompleteForm;

            if (outValidData == DialogDataValid.eNoError)
            {
                if (stateStr.Length != 2)
                    outValidData = DialogDataValid.eBadStateFormat;
            }

            if (outValidData == DialogDataValid.eNoError)
            {
                int actualZip;
                if (zipStr.Length != 5 || !int.TryParse(zipStr, out actualZip))
                    outValidData = DialogDataValid.eBadZipCodeFormat;
            }

            if (outValidData == DialogDataValid.eNoError)
            {
                string textReturn = textReturnDate.Text;
                DateTime res;
                if (!DateTime.TryParse(textReturn, out res))
                    outValidData = DialogDataValid.eBadDateFormat;
            }

            if (textBoxNotes.Text.Length > 1000)
            {
                return DialogDataValid.eNotesTooLong;
            }

            return outValidData;
        }

        private void LoadPatientList()
        {
            try
            {
                allPatients.Clear();
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet("Patient_Registration MASTER");
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
                UpdatePatientDisplay(allPatients);
            }
            catch (Exception ex) { MessageBox.Show("Error loading list: " + ex.Message); }
        }

        private void UpdatePatientDisplay(List<PatientItem> patientsToShow)
        {
            m_isFiltering = true; // Block the IndexChanged event from loading data mid-search
            comboPatients.Items.Clear();

            var sorted = patientsToShow.OrderBy(p => p.Name).ToList();
            foreach (var p in sorted)
            {
                comboPatients.Items.Add(p);
            }

            if (comboPatients.Items.Count > 0)
            {
                comboPatients.SelectedIndex = 0; // Auto-select the first match
            }
            else
            {
                comboPatients.Text = "No results found...";
            }
            m_isFiltering = false;

            // Trigger manual load for the newly selected first item
            if (comboPatients.SelectedIndex != -1) LoadSelectedPatientData();
        }

        private void textSearch_TextChanged(object sender, EventArgs e)
        {
            string term = textSearch.Text.ToLower();
            var filtered = allPatients.Where(p => p.Name.ToLower().Contains(term)).ToList();
            UpdatePatientDisplay(filtered);
        }

        private void comboPatients_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_isFiltering) return;
            LoadSelectedPatientData();
        }

        private void LoadSelectedPatientData()
        {
            PatientItem selected = comboPatients.SelectedItem as PatientItem;
            if (selected == null) return;

            try
            {
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet("Patient_Registration MASTER");
                    var row = worksheet.Row(selected.RowIndex);

                    textFirstName.Text = row.Cell(kFirstNameLoc).GetValue<string>() ?? "";
                    textLastName.Text = row.Cell(kLastNameLoc).GetValue<string>() ?? "";
                    textAddress.Text = row.Cell(kAddressLoc).GetValue<string>() ?? "";
                    textCity.Text = row.Cell(kCityLoc).GetValue<string>() ?? "";
                    textState.Text = row.Cell(kStateLoc).GetValue<string>() ?? "";
                    textZip.Text = row.Cell(kZipLoc).GetValue<string>() ?? "";
                    textHomePhone.Text = row.Cell(kHomePhoneLoc).GetValue<string>() ?? "";
                    textCellPhone.Text = row.Cell(kCellPhoneLoc).GetValue<string>() ?? "";
                    textBoxNotes.Text = row.Cell(kNotesLoc).GetValue<string>() ?? "";
                    textInsurance.Text = row.Cell(kInsuranceLoc).GetValue<string>() ?? "";

                    var dateCell = row.Cell(kReturnDateLoc);
                    textReturnDate.Text = (!dateCell.IsEmpty() && dateCell.DataType == XLDataType.DateTime)
                        ? dateCell.GetValue<DateTime>().ToShortDateString()
                        : dateCell.Value.ToString();
                }
            }
            catch (Exception ex) { /* Handle error */ }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            DialogDataValid dataValid = IsInputDataValid();
            PatientItem selected = comboPatients.SelectedItem as PatientItem;

            if (dataValid == DialogDataValid.eNoError && selected != null)
            {
                int currRow = selected.RowIndex; // Use the row index from our helper class

                try
                {
                    using (var workbook = new XLWorkbook(m_filePath))
                    {
                        var worksheet = workbook.Worksheet("Patient_Registration MASTER");

                        worksheet.Cell(currRow, kFirstNameLoc).Value = textFirstName.Text;
                        worksheet.Cell(currRow, kLastNameLoc).Value = textLastName.Text;
                        worksheet.Cell(currRow, kAddressLoc).Value = textAddress.Text;
                        worksheet.Cell(currRow, kCityLoc).Value = textCity.Text;
                        worksheet.Cell(currRow, kStateLoc).Value = textState.Text;
                        worksheet.Cell(currRow, kZipLoc).Value = textZip.Text;
                        worksheet.Cell(currRow, kHomePhoneLoc).Value = textHomePhone.Text;
                        worksheet.Cell(currRow, kCellPhoneLoc).Value = textCellPhone.Text;
                        worksheet.Cell(currRow, kInsuranceLoc).Value = textInsurance.Text;
                        worksheet.Cell(currRow, kNotesLoc).Value = textBoxNotes.Text;

                        if (DateTime.TryParse(textReturnDate.Text, out DateTime parsedDate))
                            worksheet.Cell(currRow, kReturnDateLoc).Value = parsedDate;

                        workbook.Save();
                        MessageBox.Show("Patient updated successfully!");
                        this.Close();
                    }
                }
                catch (IOException) { MessageBox.Show("Close the Excel file and try again."); }
            }
            else
            {
                string dialogText =
                  dataValid == DialogDataValid.eNoPatientSelected ? "No patient selected. Please select a patient from the list before saving."
                : dataValid == DialogDataValid.eIncompleteForm ? "The information is not complete. Please complete all specified fields."
                : dataValid == DialogDataValid.eBadStateFormat ? "The State format is incorrect. Please make sure the State is abbreviated (2 letters)."
                : dataValid == DialogDataValid.eBadZipCodeFormat ? "The Zip Code format is incorrect. Please make sure the Zip Code is a 5 digit number."
                : dataValid == DialogDataValid.eBadDateFormat ? "The Return Date format is incorrect. Please type in the return date in the MM/DD/YYYY format."
                : dataValid == DialogDataValid.eNotesTooLong ? "Too many characters in notes column. Please shorten the note and try again."
                : "Something else has gone wrong. Please make sure the excel document is open.";

                    Form prompt = new AlertDialog(dialogText);
                    prompt.ShowDialog();
            }
            
        }

        public class PatientItem
        {
            public string Name { get; set; }
            public int RowIndex { get; set; }
            public override string ToString() => Name;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

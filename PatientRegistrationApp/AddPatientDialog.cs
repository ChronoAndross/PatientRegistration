using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PatientRegistrationApp
{
    public partial class AddPatientDialog : Form
    {

        private string mFilePath;
        // determines what sheet to use
        private string workSheetName = "Patient_Registration MASTER";
        private string mCurrentUser; // Store the logged-in user
        private string mLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationLog.txt");

        enum DialogDataValid
        {
            eNoError,
            eIncompleteForm,
            eBadZipCodeFormat,
            eBadStateFormat,
            eBadDateFormat,
            eNotesTooLong
        }

        public AddPatientDialog(string filePath, string currentUser)
        {
            InitializeComponent();
            mFilePath = filePath;
            mCurrentUser = currentUser;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsDuplicatePatient()
        {
            using (var workbook = new XLWorkbook(mFilePath))
            {
                var worksheet = workbook.Worksheet(workSheetName);
                var rows = worksheet.RowsUsed().Skip(1); // Skip header

                foreach (var row in rows)
                {
                    // Compare all 6 fields (Case-Insensitive)
                    bool match = row.Cell(kFirstNameLoc).Value.ToString().Trim().Equals(textFirstName.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                 row.Cell(kLastNameLoc).Value.ToString().Trim().Equals(textLastName.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                 row.Cell(kAddressLoc).Value.ToString().Trim().Equals(textAddress.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                 row.Cell(kCityLoc).Value.ToString().Trim().Equals(textCity.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                 row.Cell(kStateLoc).Value.ToString().Trim().Equals(textState.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                                 row.Cell(kZipLoc).Value.ToString().Trim().Equals(textZip.Text.Trim(), StringComparison.OrdinalIgnoreCase);

                    if (match) return true;
                }
            }
            return false;
        }

        private void SendDialogDataToExcel()
        {
            string patientName = $"{textFirstName.Text} {textLastName.Text}";
            try
            {
                using (var workbook = new XLWorkbook(mFilePath))
                {
                    var worksheet = workbook.Worksheet(workSheetName);

                    int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? 0;
                    int currRow = lastRow + 1;

                    worksheet.Cell(currRow, kCurrDateLoc).Value = DateTime.Today.ToShortDateString();
                    worksheet.Cell(currRow, kFirstNameLoc).Value = textFirstName.Text;
                    worksheet.Cell(currRow, kLastNameLoc).Value = textLastName.Text;
                    worksheet.Cell(currRow, kAddressLoc).Value = textAddress.Text;
                    worksheet.Cell(currRow, kCityLoc).Value = textCity.Text;
                    worksheet.Cell(currRow, kStateLoc).Value = textState.Text;
                    worksheet.Cell(currRow, kZipLoc).Value = textZip.Text;
                    worksheet.Cell(currRow, kHomePhoneLoc).Value = textHomePhone.Text;
                    worksheet.Cell(currRow, kCellPhoneLoc).Value = textCellPhone.Text;
                    worksheet.Cell(currRow, kNotesLoc).Value = textBoxNotes.Text;
                    worksheet.Cell(currRow, kInsuranceLoc).Value = textInsurance.Text;

                    if (DateTime.TryParse(textReturnDate.Text, out DateTime parsedDate))
                    {
                        var dateCell = worksheet.Cell(currRow, kReturnDateLoc);

                        // Clear validation issues
                        dateCell.Clear(XLClearOptions.DataValidation);

                        // Assign the value
                        dateCell.Value = parsedDate;

                        // Optional: Ensure the cell style is set to a Date format
                        dateCell.Style.DateFormat.Format = "mm/dd/yyyy";
                    } else
                    {
                        MessageBox.Show("Cannot parse the specified date for row {x}. Please report an issue to the developers immediately");
                    }

                        workbook.Save(); // Saves the file instantly
                        Utils.LogAction("RECORD_CREATED", $"Successfully added patient: {patientName}", mCurrentUser, mLogPath);

                }
            }
            catch (IOException ioEx)
            {
                Utils.LogAction("ERROR", $"Failed to add {patientName}: File locked by another process, error: {ioEx.Message}", mCurrentUser, mLogPath);
                MessageBox.Show("File is currently open in another program. Close the file and try again.");
            }
            catch (Exception ex)
            {
                Utils.LogAction("ERROR", $"Critical Error adding {patientName}: {ex.Message}", mCurrentUser, mLogPath);
                MessageBox.Show("An unexpected error occurred. Check logs for details.");
            }
        }

        private DialogDataValid IsInputDataValid()
        {
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

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // 1. First, check if basic data formats are valid
            DialogDataValid dataValid = IsInputDataValid();

            if (dataValid == DialogDataValid.eNoError)
            {
                // 2. Data is valid, now check for duplicates in the Excel sheet
                if (IsDuplicatePatient())
                {
                    string warnMsg = $"A patient with the following details already exists:\n\n" +
                                     $"{textFirstName.Text} {textLastName.Text}\n" +
                                     $"{textAddress.Text}, {textCity.Text}\n\n" +
                                     "Are you sure you want to add this duplicate entry?";

                    // Show standard Windows warning with Yes/No buttons
                    DialogResult result = MessageBox.Show(warnMsg, "Duplicate Detected",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        Utils.LogAction("DUPLICATE_CANCELLED", $"User declined to add duplicate: {textFirstName.Text} {textLastName.Text}", mCurrentUser, mLogPath);
                        return;
                    }

                    Utils.LogAction("DUPLICATE_OVERRIDE", $"User approved duplicate entry for: {textFirstName.Text} {textLastName.Text}", mCurrentUser, mLogPath);
                }

                // 3. Either no duplicate was found, or user chose to proceed anyway
                SendDialogDataToExcel();
                this.Close();
            }
            else
            {
                // Handle validation errors using your existing AlertDialog
                string dialogText = dataValid == DialogDataValid.eIncompleteForm ? "The information is not complete. Please complete all specified fields."
                    : dataValid == DialogDataValid.eBadStateFormat ? "The State format is incorrect. Please make sure the State is abbreviated."
                    : dataValid == DialogDataValid.eBadZipCodeFormat ? "The Zip Code format is incorrect. Please make sure the Zip Code is a 5 digit number."
                    : dataValid == DialogDataValid.eBadDateFormat ? "The Return Date format is incorrect. Please type in the return date in the MM/DD/YYYY format."
                    : dataValid == DialogDataValid.eNotesTooLong ? "Too many characters in notes column. Please shorten the note and try again."
                    : "Something else has gone wrong. Please make sure the excel document is writable.";

                Utils.LogAction("VALIDATION_FAILED", $"Form error: {dataValid} for entry: {textFirstName.Text} {textLastName.Text}", mCurrentUser, mLogPath);
                Form prompt = new AlertDialog(dialogText);
                prompt.ShowDialog();
            }
        }


        private const int kCurrDateLoc = 1;
        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;
        private const int kAddressLoc = 7;
        private const int kCityLoc = 8;
        private const int kStateLoc = 9;
        private const int kZipLoc = 10;
        private const int kHomePhoneLoc = 11;
        private const int kCellPhoneLoc = 12;
        private const int kReturnDateLoc = 18;
        private const int kNotesLoc = 19;
        private const int kInsuranceLoc = 20;

        private void AddPatientDialog_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelReturnDate_Click(object sender, EventArgs e)
        {

        }
    }
}

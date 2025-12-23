using ClosedXML.Excel;
using System;
using System.IO;
using System.Windows.Forms;

namespace PatientRegistrationApp
{
    public partial class AddPatientDialog : Form
    {

        private string m_filePath;

        enum DialogDataValid
        {
            eNoError,
            eIncompleteForm,
            eBadZipCodeFormat,
            eBadStateFormat,
            eBadDateFormat
        }

        public AddPatientDialog(string filePath)
        {
            InitializeComponent();
            m_filePath = filePath;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SendDialogDataToExcel()
        {
            try
            {
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet("Patient_Registration MASTER");

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
                    worksheet.Cell(currRow, kReturnDateLoc).Value = textReturnDate.Text;

                    workbook.Save(); // Saves the file instantly
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File is currently open in another program, Close the file and try again");
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
            
            return outValidData;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // TODO: Check if workbook is read-only
            DialogDataValid dataValid = IsInputDataValid();
            if (dataValid == DialogDataValid.eNoError)
            {
                // All data is valid. Send to Excel spreadsheet
                SendDialogDataToExcel();
                this.Close(); // close dialog after sending data to excel
            }
            else
            {
                // TODO: Make better dialog text.
                string dialogText = dataValid == DialogDataValid.eIncompleteForm ? "The information is not complete. Please complete all specified fields."
                    : dataValid == DialogDataValid.eBadStateFormat ? "The State format is incorrect. Please make sure the State is abbreviated."
                    : dataValid == DialogDataValid.eBadZipCodeFormat ? "The Zip Code format is incorrect. Please make sure the Zip Code is a 5 digit number."
                    : dataValid == DialogDataValid.eBadDateFormat ? "The Return Date format is incorrect. Please type in the return date in the MM/DD/YYYY format."
                    : "Something else has gone wrong. Please make sure the excel document is open.";
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

        private void AddPatientDialog_Load(object sender, EventArgs e)
        {

        }
    }
}

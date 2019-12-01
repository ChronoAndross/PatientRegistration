using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PatientRegistrationApp
{
    public partial class AddPatientDialog : Form
    {
        
        enum DialogDataValid
        {
            eNoError,
            eIncompleteForm,
            eBadZipCodeFormat,
            eBadStateFormat,
            eBadDateFormat
        }

        public AddPatientDialog()
        {
            InitializeComponent();
            m_existingWkBook = null;
        }

        public AddPatientDialog(Excel.Workbook inWorkbook)
        {
            InitializeComponent();
            m_existingWkBook = inWorkbook;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SendDialogDataToExcel()
        {
            string currDay = DateTime.Today.Date.ToString();

            Excel.Worksheet currSheet = m_existingWkBook.Sheets["Patient_Registration MASTER"];
            int currRow = 1;
            while ((currSheet.Cells[currRow, 2]).Text != "")
                currRow++;

            // put new patient into excel doc
            currSheet.Cells[currRow, kCurrDateLoc].Value = currDay;
            currSheet.Cells[currRow, kFirstNameLoc].Value = textFirstName.Text;
            currSheet.Cells[currRow, kLastNameLoc].Value = textLastName.Text;
            currSheet.Cells[currRow, kAddressLoc].Value = textAddress.Text;
            currSheet.Cells[currRow , kCityLoc].Value = textCity.Text;
            currSheet.Cells[currRow, kStateLoc].Value = textState.Text;
            currSheet.Cells[currRow, kZipLoc].Value = textZip.Text;
            currSheet.Cells[currRow, kHomePhoneLoc].Value = textHomePhone.Text;
            currSheet.Cells[currRow, kCellPhoneLoc].Value = textCellPhone.Text;
            currSheet.Cells[currRow, kReturnDateLoc].Value = textReturnDate.Text;

            m_existingWkBook.Save(); // save this newly added row
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
            if (dataValid == DialogDataValid.eNoError && m_existingWkBook != null)
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

        private Excel.Workbook m_existingWkBook = null; // passed from main dialog

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
    }
}

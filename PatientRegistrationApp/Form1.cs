using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace PatientRegistrationApp
{
    public partial class PatientRegistrationApp : Form
    {
        public PatientRegistrationApp()
        {
            try
            {
                var app = new Excel.Application();
                app.Visible = false;
                m_existingWkBook = app.Workbooks.Open("Patient Registration.xlsx");
                InitializeComponent(); // initialize app
                mb_initialized = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Form prompt = new AlertDialog("Failed to find Patient Registration excel file. " +
                    "Please make sure that the excel file is in the same folder as the application.");
                prompt.ShowDialog();
                mb_initialized = false;
            }
        }

        public bool IsInitialized() { return mb_initialized; }

        private void btnSendMailmerge_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                /* 
                 * TODO: This command will do the following:
                 * 1. Query all available return dates from the excel file.
                 * 2. After date is selected, we query all patients associated with this return date.
                 * 3. Take each row (assuming one patient per row) and format it in a new Word document using Mailmerge API.
                 * 4. Open newly created document in Word.
                */
            }
        }

        private void btnOpenXls_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                try
                {
                    // just open excel
                    m_existingWkBook.Application.Visible ^= true;
                }
                catch (InvalidCastException /*ex*/)
                {
                    try
                    {
                        var app = new Excel.Application();
                        app.Visible = true;
                        m_existingWkBook = app.Workbooks.Open("Patient Registration.xlsx");
                    }
                    catch (Exception ex)
                    {
                        // TODO: Show some dialog that says that it can't find excel/excel file
                        Console.WriteLine(ex.ToString());
                    }
                    
                }
                catch (Exception ex)
                {
                    // TODO: Show some dialog that says that it can't find excel/excel file
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void btnInputPat_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                // TODO: Spawn a dialog that has name/address/phone number fields.
                // Then add this cell in the workbook
            }
        }

        private void btnDeletePat_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                // TODO: Spawn a dialog that provides a list of names from the Excel document to delete.
                // The "Delete" button will delete them from the Excel document.
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // TODO: Display About dialog, showing developers and other accolades.
        }

        private bool mb_initialized = false;
        private Excel.Workbook m_existingWkBook = null;
    }
}

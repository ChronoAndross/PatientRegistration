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
                InitializeComponent(); // initialize app
                mb_initialized = true;
                m_runningApp = new Excel.Application();
                m_runningApp.Visible = false;
                m_existingWkBook = m_runningApp.Workbooks.Open("Patient Registration.xlsx");
                if (!m_existingWkBook.ReadOnly)
                {
                    InitializeComponent(); // initialize app
                    mb_initialized = true;
                }
                else
                {
                    m_runningApp.Quit();
                    Form prompt = new AlertDialog("The Patient Registration excel file is read-only. " +
                        "Please configure this file correctly and reopen the application again.");
                    prompt.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                m_runningApp.Quit();
                Form prompt = new AlertDialog("Failed to find Patient Registration excel file. " +
                    "Please make sure that the excel file is in the same folder as the application.");
                prompt.ShowDialog();
                mb_initialized = false;
            }
        }

        public bool IsInitialized() { return mb_initialized; }

        private bool OpenExcelFileOnAccessFailure(bool inVisible, bool inShowDialog=true)
        {
            bool bOutSuccess = false;
            try
            {
                m_runningApp.Visible = inVisible;
                bOutSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex is InvalidCastException 
                    || ex is System.Runtime.InteropServices.COMException)
                {
                    m_runningApp = new Excel.Application();
                    m_runningApp.Visible = true;
                    m_existingWkBook = m_runningApp.Workbooks.Open("Patient Registration.xlsx");
                    bOutSuccess = true;
                }
                else // can't open the doc anymore unfortunately. tell the user about this
                {
                    if (inShowDialog)
                    {
                        Form prompt = new AlertDialog("Failed to find Patient Registration excel file. " +
                        "Please make sure that the excel file is in the same folder as the application.");
                        prompt.ShowDialog();
                    }
                }
            }
            return bOutSuccess;
        }

        private void btnSendMailmerge_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null && OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen))
            {
                Form mailMergeDialog = new MailMergeDialog(m_existingWkBook);
                mailMergeDialog.ShowDialog();
            }
        }

        private void btnOpenXls_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null && OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen))
            {
                mb_ExcelDocumentIsOpen ^= true;
                OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen);
            }
        }

        private void btnInputPat_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null && OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen))
            {
                OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen);
                Form addPatientDialog = new AddPatientDialog(m_existingWkBook);
                addPatientDialog.ShowDialog();
            }
        }

        private void btnDeletePat_Click(object sender, EventArgs e)
        {
            if (m_existingWkBook != null && OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen))
            {
                OpenExcelFileOnAccessFailure(mb_ExcelDocumentIsOpen);
                Form deletePatientDialog = new DeletePatientDialog(m_existingWkBook);
                deletePatientDialog.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string dialogText = "Written and designed by Andrew Gorbaty." 
                 + "This software is distributed under an 'as-is' license, which allows any developer to modify its source code without permission from its original author.";
            Form prompt = new AlertDialog(dialogText);
            prompt.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (OpenExcelFileOnAccessFailure(false, false))
            {
                m_existingWkBook.Close();
                m_runningApp.Quit();
            }
        }

        private bool mb_initialized = false;
        private bool mb_ExcelDocumentIsOpen = false;
        private Excel.Workbook m_existingWkBook = null;
        Excel.Application m_runningApp = null;
    }
}

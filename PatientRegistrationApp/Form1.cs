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
        private string m_CurrentUser = "Unknown";
        private string m_LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationLog.txt");

        public PatientRegistrationApp()
        {
            InitializeComponent();

            // User Prompt
            using (Form loginForm = new Form())
            {
                Label lbl = new Label() { Text = "Please enter your name:", Left = 10, Top = 10, Width = 200 };
                TextBox txt = new TextBox() { Left = 10, Top = 35, Width = 250 };
                Button btn = new Button() { Text = "OK", Left = 185, Top = 65, DialogResult = DialogResult.OK };

                loginForm.Text = "Login";
                loginForm.Size = new Size(300, 150);
                loginForm.StartPosition = FormStartPosition.CenterScreen;
                loginForm.Controls.AddRange(new Control[] { lbl, txt, btn });
                loginForm.AcceptButton = btn;

                if (loginForm.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(txt.Text))
                {
                    m_CurrentUser = txt.Text;
                }
                else
                {
                    MessageBox.Show("User identification is required to track changes.");
                    mb_initialized = false;
                    return;
                }
            }
            LogAction("SESSION_START", $"User logged in at {DateTime.Now}");


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
            catch (IOException ioEx)
            {
                LogAction("ERROR", "File Access Denied (File likely open in Excel)");
                MessageBox.Show("The file is currently open in another program.");
            }
            catch (Exception ex)
            {
                LogAction("ERROR", ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void LogAction(string action, string details)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] User: {m_CurrentUser} | Action: {action} | Details: {details}";
                File.AppendAllText(m_LogPath, logEntry + Environment.NewLine);
            }
            catch { /* Avoid crashing if logging fails */ }
        }


        private void btnSendMailmerge_Click(object sender, EventArgs e)
        {
            Form mailMergeDialog = new MailMergeDialog(m_FileStr, m_CurrentUser);
            mailMergeDialog.ShowDialog();
        }

        private void btnOpenXls_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(m_FileStr);
        }


        private void btnInputPat_Click(object sender, EventArgs e)
        {
            Form addPatientDialog = new AddPatientDialog(m_FileStr, m_CurrentUser);
            addPatientDialog.ShowDialog();
            LogAction("NAVIGATE", "Opened Add Patient Dialog");
        }


        private void btnDeletePat_Click(object sender, EventArgs e)
        {
            Form deletePatientDialog = new DeletePatientDialog(m_FileStr, m_CurrentUser);
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
            Form editPatientDialog = new EditPatientDialog(m_FileStr, m_CurrentUser);
            editPatientDialog.ShowDialog();
        }

        protected override void OnClosed(EventArgs e) {base.OnClosed(e);}

        
        private void PatientRegistrationApp_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Logs are saving to: " + m_LogPath);
            // Or show it in a popup once just to be sure
            // MessageBox.Show(m_LogPath);

        }

        private void btnViewLogs_Click(object sender, EventArgs e)
        {
            if (File.Exists(m_LogPath))
            {
                System.Diagnostics.Process.Start("notepad.exe", m_LogPath);
            }
            else
            {
                MessageBox.Show("No log file found yet. Start an action to generate one.");
            }
        }
    }
}

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Generics = System.Collections.Generic;
using OleDbConnection = System.Data.OleDb.OleDbConnection;
using StringBuilder = System.Text.StringBuilder;
using Word = Microsoft.Office.Interop.Word;

/*
* The MailMergeDialog class does the following:
* 1. Query all available return dates from the excel file.
* 2. After date is selected, we query all patients associated with this return date.
* 3. Take each row (assuming one patient per row) and format it in a new Word document using Mailmerge API.
* 4. Open newly created document in Word.
*/

namespace PatientRegistrationApp
{
    public partial class MailMergeDialog : Form
    {
        private string m_filePath;
        private string m_currentUser; // Store the logged-in user
        private string m_logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApplicationLog.txt");
        private Dictionary<string, int> fMonthCounter = new Dictionary<string, int>();
        

        // determines what sheet to use
        private string workSheetName = "Patient_Registration MASTER";

        // Locations constants (keeping your existing mapping)
        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;
        private const int kAddressLoc = 7;
        private const int kCityLoc = 8;
        private const int kStateLoc = 9;
        private const int kZipLoc = 10;
        private const int kReturnDateLoc = 18;
        private const int kHomePhoneLoc = 11;
        private const int kCellPhoneLoc = 12;

        public MailMergeDialog()
        {
            InitializeComponent();
        }

        public MailMergeDialog(string filePath, string currentUser)
        {
            InitializeComponent();
            m_filePath = filePath;
            m_currentUser = currentUser;
        }

        private void LogAction(string action, string details)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:MM/dd/yyyy HH:mm:ss}] User: {m_currentUser} | Action: {action} | Details: {details}";
                File.AppendAllText(m_logPath, logEntry + Environment.NewLine);
            }
            catch { /* Fail silently */ }
        }

        private void MailMergeDialog_Load(object sender, EventArgs e)
        {
            try
            {
                fMonthCounter.Clear();
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet(workSheetName);
                    var rows = worksheet.RowsUsed().Skip(1);

                    foreach (var row in rows)
                    {
                        var cellValue = row.Cell(kReturnDateLoc).Value;
                        if (cellValue.IsDateTime)
                        {
                            string monthYear = cellValue.GetDateTime().ToString("MMMM yyyy");
                            if (!fMonthCounter.ContainsKey(monthYear))
                                fMonthCounter.Add(monthYear, 1);
                            else
                                fMonthCounter[monthYear]++;
                        }
                    }
                }

                var sortedMonths = fMonthCounter.Keys
                    .OrderByDescending(m => DateTime.Parse(m))
                    .ToList();

                comboDateSelection.Items.Clear();
                foreach (var month in sortedMonths)
                {
                    comboDateSelection.Items.Add(month);
                }
            }
            catch (Exception ex)
            {
                LogAction("ERROR", $"Failed to load MailMerge months: {ex.Message}");
                MessageBox.Show("Error loading months: " + ex.Message);
            }
            comboDateSelection.Text = "--Select Month/Year--";
        }

        private void IteratePatientsForPostcards(Word.Application inApp)
        {
            if (comboDateSelection.SelectedIndex == -1) return;

            string selectedMonthYear = comboDateSelection.SelectedItem.ToString();
            Word.Document currDoc = inApp.Documents.Add();

            // Zero out page margins so the table controls the layout
            currDoc.PageSetup.TopMargin = 0;
            currDoc.PageSetup.BottomMargin = 0;
            currDoc.PageSetup.LeftMargin = 0;
            currDoc.PageSetup.RightMargin = 0;

            Word.Table currDocTable = CreateTableForPostcards(ref currDoc, selectedMonthYear, inApp);

            using (var workbook = new XLWorkbook(m_filePath))
            {
                var worksheet = workbook.Worksheet("Patient_Registration MASTER");
                var rows = worksheet.RowsUsed().Skip(1);

                int currWordRow = 1;
                int currWordColumn = 1;

                foreach (var row in rows)
                {
                    var cellValue = row.Cell(kReturnDateLoc).Value;
                    if (cellValue.IsDateTime && cellValue.GetDateTime().ToString("MMMM yyyy") == selectedMonthYear)
                    {
                        string firstName = row.Cell(kFirstNameLoc).Value.ToString();
                        string lastName = row.Cell(kLastNameLoc).Value.ToString();
                        string address = row.Cell(kAddressLoc).Value.ToString();
                        string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";
                        string phone = row.Cell(kHomePhoneLoc).Value.ToString();

                        // Target the specific cell
                        Word.Range cellRange = currDocTable.Cell(currWordRow, currWordColumn).Range;

                        // Build Postcard Content with \v for tight spacing
                        string postcardContent =
                            $"Lenita N. Gorrell, M.D.\v" +
                            $"7845 Oakwood Road, Suite 203\v" +
                            $"Glen Burnie, MD 21061\v" +
                            $"410-768-8214\r\r" +
                            $"Dear {firstName},\r\r" +
                            "We want you back! Our records show that it has been _____________ " +
                            "since your last eye exam. Please call our office to make an " +
                            "appointment at your convenience. We'd love to see you again.";

                        cellRange.Text = postcardContent;
                        cellRange.Font.Name = "Arial";
                        cellRange.Font.Size = 11;

                        // Move to next cell
                        currWordColumn++;
                        if (currWordColumn > 2)
                        {
                            currWordColumn = 1;
                            currWordRow++;
                        }
                    }
                }
            }
        }

        private void SendPatientToMailMerge(string inName, string inAddr, string inCSZ, int inRow, int inCol, Word.Table inTable)
        {
            // Use \v (vertical tab) for a new line without a paragraph break
            inTable.Cell(inRow, inCol).Range.Text = $"{inName}\v{inAddr}\v{inCSZ}";
        }

        private Word.Table CreateTableForPostcards(ref Word.Document inDoc, string selectedMonth, Word.Application inApp)
        {
            int numColumns = 2;
            int patientCount = fMonthCounter[selectedMonth];
            // We need 2 columns, so rows = ceiling(count / 2)
            int numRows = (int)Math.Ceiling(patientCount / 2.0);

            Word.Table outTable = inDoc.Tables.Add(inDoc.Range(), numRows, numColumns);

            // --- Avery 8387 Geometry ---
            outTable.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
            outTable.Rows.Height = inApp.InchesToPoints(5.5f); // Half of 11" sheet

            // Set column widths to 4.25"
            outTable.Columns.Width = inApp.InchesToPoints(4.25f);

            // Remove margins/padding so text doesn't shift
            outTable.TopPadding = inApp.InchesToPoints(0.5f);
            outTable.BottomPadding = 0;
            outTable.LeftPadding = inApp.InchesToPoints(0.5f);
            outTable.RightPadding = inApp.InchesToPoints(0.5f);

            outTable.Range.ParagraphFormat.SpaceAfter = 0;

            return outTable;
        }

        // Create table for newly created word doc representing printing labels based on number of entries found on load
        private Word.Table CreateTableForWord(ref Word.Document inDoc, string selectedMonth, Word.Application inApp)
        {
            int numColumns = 3;
            int patientCount = fMonthCounter[selectedMonth]; // Count for the whole month
            int numRows = (int)Math.Ceiling(patientCount / 3.0);

            Word.Table outTable = inDoc.Tables.Add(inDoc.Range(), numRows, numColumns);

            outTable.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
            outTable.Rows.Height = inApp.InchesToPoints(1.0f); // Exactly 1 inch for 3-col

            outTable.Range.ParagraphFormat.SpaceAfter = 0;
            outTable.TopPadding = 5;

            return outTable;
        }

        private void IteratePatientsForMailMerge(Word.Application inApp)
        {
            if (comboDateSelection.SelectedIndex == -1) return;

            string selectedMonthYear = comboDateSelection.SelectedItem.ToString();
            Word.Document currDoc = inApp.Documents.Add();

            // Setup Page Margins
            currDoc.PageSetup.TopMargin = inApp.InchesToPoints(0.5f);
            currDoc.PageSetup.BottomMargin = inApp.InchesToPoints(0.5f);
            currDoc.PageSetup.LeftMargin = inApp.InchesToPoints(0.19f);
            currDoc.PageSetup.RightMargin = inApp.InchesToPoints(0.19f);

            Word.Table currDocTable = CreateTableForWord(ref currDoc, selectedMonthYear, inApp);

            using (var workbook = new XLWorkbook(m_filePath))
            {
                var worksheet = workbook.Worksheet(workSheetName);
                var rows = worksheet.RowsUsed().Skip(1);

                int currWordRow = 1;
                int currWordColumn = 1;

                foreach (var row in rows)
                {
                    var cellValue = row.Cell(kReturnDateLoc).Value;
                    if (cellValue.IsDateTime)
                    {
                        DateTime rowDate = cellValue.GetDateTime();

                        // Check if this row matches the selected Month and Year
                        if (rowDate.ToString("MMMM yyyy") == selectedMonthYear)
                        {
                            string name = $"{row.Cell(kFirstNameLoc).Value} {row.Cell(kLastNameLoc).Value}";
                            string address = row.Cell(kAddressLoc).Value.ToString();
                            string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";

                            SendPatientToMailMerge(name, address, cityStateZip, currWordRow, currWordColumn, currDocTable);

                            currWordColumn++;
                            if (currWordColumn > 3)
                            {
                                currWordColumn = 1;
                                currWordRow++;
                            }
                        }
                    }
                }
            }
        }

        private void btnSendToMailMerge_Click(object sender, EventArgs e)
        {
            if (comboDateSelection.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a month first.");
                return;
            }

            string selected = comboDateSelection.SelectedItem.ToString();
            LogAction("MAIL_MERGE_START", $"Generating labels for: {selected}");

            try
            {
                var wordApp = new Word.Application();
                IteratePatientsForMailMerge(wordApp);
                wordApp.Visible = true;
                LogAction("MAIL_MERGE_SUCCESS", $"Word labels generated for {fMonthCounter[selected]} patients.");
            }
            catch (Exception ex)
            {
                LogAction("ERROR", $"Mail Merge Failed: {ex.Message}");
                MessageBox.Show("Word Interop Error: " + ex.Message);
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintPostcards_Click(object sender, EventArgs e)
        {
            if (comboDateSelection.SelectedIndex == -1) return;

            string selected = comboDateSelection.SelectedItem.ToString();
            LogAction("POSTCARD_GEN_START", $"Generating postcards for: {selected}");

            try
            {
                var wordApp = new Word.Application();
                IteratePatientsForPostcards(wordApp);
                wordApp.Visible = true;
                LogAction("POSTCARD_GEN_SUCCESS", $"Postcards generated for {fMonthCounter[selected]} patients.");
                this.Close();
            }
            catch (Exception ex)
            {
                LogAction("ERROR", $"Postcard Generation Failed: {ex.Message}");
                MessageBox.Show("Error generating postcards: " + ex.Message);
            }
        }
    }
}

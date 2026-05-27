using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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
        private const int kCurrDateLoc = 1;
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

        private List<string> GetSelectedMonths()
        {
            // Important: Ensure 'using System.Linq;' is at the top of your file
            return comboDateSelection.SelectedItems.Cast<string>().ToList();
        }

        private string GetTimeSinceString(DateTime pastDate)
        {
            TimeSpan span = DateTime.Now - pastDate;

            // Total days is a good baseline
            double days = span.TotalDays;

            if (days < 30)
                return $"{(int)days} days";

            if (days < 365)
            {
                int months = (int)(days / 30.44); // Average month length
                return months == 1 ? "1 month" : $"{months} months";
            }

            int years = (int)(days / 365.25);
            return years == 1 ? "1 year" : $"{years} years";
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
            //comboDateSelection.Text = "--Select Month/Year--";
        }

        private void IteratePatientsForPostcards(Word.Application inApp)
        {
            var selectedMonths = GetSelectedMonths(); // Get the List<string>
            if (selectedMonths.Count == 0) return;

            Word.Document currDoc = inApp.Documents.Add();

            // Zero out page margins so the table handles the 8387 alignment
            currDoc.PageSetup.TopMargin = 0;
            currDoc.PageSetup.BottomMargin = 0;
            currDoc.PageSetup.LeftMargin = 0;
            currDoc.PageSetup.RightMargin = 0;

            // 1. Sum up all patients for all selected months
            int totalPatients = 0;
            foreach (var month in selectedMonths)
            {
                if (fMonthCounter.ContainsKey(month))
                    totalPatients += fMonthCounter[month];
            }

            // 2. Create one big table for all of them
            Word.Table currDocTable = CreateTableForPostcards(ref currDoc, totalPatients, inApp);

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
                        string rowMonthYear = cellValue.GetDateTime().ToString("MMMM yyyy");

                        // --- THE MULTI-MONTH CHECK ---
                        if (selectedMonths.Contains(rowMonthYear))
                        {

                            string timeSinceText = "some time"; // Default fallback
                            var lastExamCellValue = row.Cell(kCurrDateLoc).Value;

                            if (lastExamCellValue.IsDateTime)
                            {
                                timeSinceText = GetTimeSinceString(lastExamCellValue.GetDateTime());
                            }

                            string firstName = row.Cell(kFirstNameLoc).Value.ToString();
                            string lastName = row.Cell(kLastNameLoc).Value.ToString();
                            string address = row.Cell(kAddressLoc).Value.ToString();
                            string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";
                            string phone = row.Cell(kHomePhoneLoc).Value.ToString();

                            Word.Range cellRange = currDocTable.Cell(currWordRow, currWordColumn).Range;

                            string postcardContent =
                                $"Lenita N. Gorrell, M.D.\v" +
                                $"7845 Oakwood Road, Suite 203\v" +
                                $"Glen Burnie, MD 21061\v" +
                                $"410-768-8214\r\r" +
                                $"Dear {firstName},\r\r" +
                                $"We want you back! Our records show that it has been {timeSinceText} " +
                                "since your last eye exam. Please call our office to make an " +
                                "appointment at your convenience. We'd love to see you again.";

                            cellRange.Text = postcardContent;
                            cellRange.Font.Name = "Arial";
                            cellRange.Font.Size = 11;

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
        }

        private void SendPatientToMailMerge(string inName, string inAddr, string inCSZ, int inRow, int inCol, Word.Table inTable)
        {
            // Use \v (vertical tab) for a new line without a paragraph break
            inTable.Cell(inRow, inCol).Range.Text = $"{inName}\v{inAddr}\v{inCSZ}";
        }

        private Word.Table CreateTableForPostcards(ref Word.Document inDoc, int totalPatientCount, Word.Application inApp)
        {
            int numColumns = 2; // Avery 8387 is 2 columns wide
                                // Calculate total rows needed for all selected months combined
            int numRows = (int)Math.Ceiling(totalPatientCount / 2.0);

            Word.Table outTable = inDoc.Tables.Add(inDoc.Range(), numRows, numColumns);

            // --- Avery 8387 Geometry ---
            outTable.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
            outTable.Rows.Height = inApp.InchesToPoints(5.5f); // Exactly half of an 11" sheet

            outTable.Columns.Width = inApp.InchesToPoints(4.25f); // Exactly half of an 8.5" sheet

            // Margins to keep text away from the perforated edges
            outTable.TopPadding = inApp.InchesToPoints(0.5f);
            outTable.BottomPadding = 0;
            outTable.LeftPadding = inApp.InchesToPoints(0.5f);
            outTable.RightPadding = inApp.InchesToPoints(0.5f);

            outTable.Range.ParagraphFormat.SpaceAfter = 0;

            return outTable;
        }

        // Create table for newly created word doc representing printing labels based on number of entries found on load
        // Change 'string selectedMonth' to 'int totalPatients'
        private Word.Table CreateTableForWord(ref Word.Document inDoc, int totalPatients, Word.Application inApp)
        {
            int numColumns = 3;
            int numRows = (int)Math.Ceiling(totalPatients / 3.0);

            Word.Table outTable = inDoc.Tables.Add(inDoc.Range(), numRows, numColumns);

            // Lock dimensions to prevent bleeding
            outTable.Rows.HeightRule = Word.WdRowHeightRule.wdRowHeightExactly;
            outTable.Rows.Height = inApp.InchesToPoints(1.0f);

            return outTable;
        }

        private void IteratePatientsForMailMerge(Word.Application inApp)
        {
            var selectedMonths = GetSelectedMonths();
            Word.Document currDoc = inApp.Documents.Add();

            // 1. Calculate the TOTAL patient count for ALL selected months
            int totalPatients = 0;
            foreach (string m in selectedMonths) totalPatients += fMonthCounter[m];

            // 2. Pass that total to the table creator
            Word.Table currDocTable = CreateTableForWord(ref currDoc, totalPatients, inApp);

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
                        string rowMonth = cellValue.GetDateTime().ToString("MMMM yyyy");

                        // --- MULTI-MONTH FILTER ---
                        if (selectedMonths.Contains(rowMonth))
                        {
                            string name = $"{row.Cell(kFirstNameLoc).Value} {row.Cell(kLastNameLoc).Value}";
                            string address = row.Cell(kAddressLoc).Value.ToString();
                            string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";

                            SendPatientToMailMerge(name, address, cityStateZip, currWordRow, currWordColumn, currDocTable);

                            currWordColumn++;
                            if (currWordColumn > 3) { currWordColumn = 1; currWordRow++; }
                        }
                    }
                }
            }
        }

        private void btnSendToMailMerge_Click(object sender, EventArgs e)
        {

            var selectedMonths = GetSelectedMonths();
            if (selectedMonths.Count == 0)
            {
                MessageBox.Show("Please select at least one month from the list.");
                return;
            }

            // Log all selected months
            string monthDetails = string.Join(", ", selectedMonths);
            LogAction("MAIL_MERGE_START", $"Generating labels for: {monthDetails}");

            try
            {
                var wordApp = new Word.Application();
                IteratePatientsForMailMerge(wordApp); 
                wordApp.Visible = true;
                LogAction("MAIL_MERGE_SUCCESS", $"Labels generated for {selectedMonths.Count} months.");
            }
            catch (Exception ex)
            {
                LogAction("ERROR", $"Multi-month Mail Merge Failed: {ex.Message}");
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintPostcards_Click(object sender, EventArgs e)
        {
            var selectedMonths = GetSelectedMonths();
            if (selectedMonths.Count == 0)
            {
                MessageBox.Show("Please select at least one month from the list.");
                return;
            }

            string logDetails = string.Join(", ", selectedMonths);
            LogAction("POSTCARD_GEN_START", $"Generating postcards for: {logDetails}");

            try
            {
                var wordApp = new Word.Application();
                IteratePatientsForPostcards(wordApp);
                wordApp.Visible = true;

                LogAction("POSTCARD_GEN_SUCCESS", $"Multi-month batch complete.");
                this.Close();
            }
            catch (Exception ex)
            {
                LogAction("ERROR", $"Postcard batch failed: {ex.Message}");
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

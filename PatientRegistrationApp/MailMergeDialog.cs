using ClosedXML.Excel;
using System;
using System.Collections.Generic;
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

        public MailMergeDialog(string filePath)
        {
            InitializeComponent();
            m_filePath = filePath;
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
                            // Group by "MMMM yyyy" (e.g., February 2026)
                            string monthYear = cellValue.GetDateTime().ToString("MMMM yyyy");

                            if (!fMonthCounter.ContainsKey(monthYear))
                                fMonthCounter.Add(monthYear, 1);
                            else
                                fMonthCounter[monthYear]++;
                        }
                    }
                }

                // Sort by Date naturally (descending) then fill the combo box
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
                MessageBox.Show("Error loading months: " + ex.Message);
            }

            comboDateSelection.Text = "--Select Month/Year--";
        }

        private void IteratePatientsForPostcards(Word.Application inApp)
        {
            if (comboDateSelection.SelectedIndex == -1) return;

            string selectedMonthYear = comboDateSelection.SelectedItem.ToString();
            Word.Document currDoc = inApp.Documents.Add();

            // Standard Page Setup for Postcards (or standard letter if printing multiple per sheet)
            currDoc.PageSetup.TopMargin = inApp.InchesToPoints(0.75f);
            currDoc.PageSetup.LeftMargin = inApp.InchesToPoints(0.75f);
            currDoc.PageSetup.RightMargin = inApp.InchesToPoints(0.75f);

            using (var workbook = new XLWorkbook(m_filePath))
            {
                var worksheet = workbook.Worksheet(workSheetName);
                var rows = worksheet.RowsUsed().Skip(1);
                bool firstEntry = true;

                foreach (var row in rows)
                {
                    var cellValue = row.Cell(kReturnDateLoc).Value;
                    if (cellValue.IsDateTime && cellValue.GetDateTime().ToString("MMMM yyyy") == selectedMonthYear)
                    {
                        // Add a page break for every patient except the first
                        if (!firstEntry)
                        {
                            currDoc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                        }

                        string firstName = row.Cell(kFirstNameLoc).Value.ToString();
                        string lastName = row.Cell(kLastNameLoc).Value.ToString();
                        string address = row.Cell(kAddressLoc).Value.ToString();
                        string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";
                        string phone = row.Cell(kHomePhoneLoc).Value.ToString();

                        // Build the Postcard Content
                        Word.Range rng = currDoc.Content;
                        rng.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                        // Heading
                        rng.InsertAfter($"Lenita N. Gorrell, M.D.\r");
                        rng.InsertAfter($"7845 Oakwood Road, Suite 203\r");
                        rng.InsertAfter($"Glen Burnie, MD 21061\r");
                        rng.InsertAfter($"410-768-8214\r\r");

                        // Message Content
                        rng.InsertAfter($"Dear {firstName},\r\r");

                        string bodyText = "We want you back! Our records show that it has been _____________ " +
                                          "since your last eye exam. Please call our office to make an " +
                                          "appointment at your convenience. We'd love to see you again.";

                        rng.InsertAfter(bodyText);

                        // Format the text (Optional: Arial, 11pt)
                        rng.Font.Name = "Arial";
                        rng.Font.Size = 11;

                        firstEntry = false;
                    }
                }
            }
        }

        private void SendPatientToMailMerge(string inName, string inAddr, string inCSZ, int inRow, int inCol, Word.Table inTable)
        {
            // Use \v (vertical tab) for a new line without a paragraph break
            inTable.Cell(inRow, inCol).Range.Text = $"{inName}\v{inAddr}\v{inCSZ}";
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
            var wordApp = new Word.Application();
            IteratePatientsForMailMerge(wordApp);
            wordApp.Visible = true;
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrintPostcards_Click(object sender, EventArgs e)
        {
            var wordApp = new Word.Application();
            IteratePatientsForPostcards(wordApp);
            wordApp.Visible = true;
            this.Close();
        }
    }
}

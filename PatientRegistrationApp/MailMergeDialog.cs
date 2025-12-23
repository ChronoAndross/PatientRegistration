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
        private Dictionary<DateTime, int> fDateCounter = new Dictionary<DateTime, int>();

        // Locations constants (keeping your existing mapping)
        private const int kFirstNameLoc = 2;
        private const int kLastNameLoc = 3;
        private const int kAddressLoc = 7;
        private const int kCityLoc = 8;
        private const int kStateLoc = 9;
        private const int kZipLoc = 10;
        private const int kReturnDateLoc = 18;

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
                using (var workbook = new XLWorkbook(m_filePath))
                {
                    var worksheet = workbook.Worksheet("Patient_Registration MASTER");
                    var rows = worksheet.RowsUsed().Skip(1); // Skip header

                    foreach (var row in rows)
                    {
                        var cellValue = row.Cell(kReturnDateLoc).Value;
                        if (cellValue.IsDateTime)
                        {
                            DateTime returnData = cellValue.GetDateTime();
                            if (!fDateCounter.ContainsKey(returnData))
                            {
                                comboDateSelection.Items.Add(returnData.ToShortDateString());
                                fDateCounter.Add(returnData, 1);
                            }
                            else
                            {
                                fDateCounter[returnData]++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dates: " + ex.Message);
            }

            comboDateSelection.Text = "--Please select a date--";
        }

        private void SendPatientToMailMerge(string inName, string inAddr, string inCSZ, int inRow, int inCol, Word.Table inTable)
        {
            // Word tables use 1-based indexing
            inTable.Cell(inRow, inCol).Range.Text = inName;
            inTable.Cell(inRow + 1, inCol).Range.Text = inAddr;
            inTable.Cell(inRow + 2, inCol).Range.Text = inCSZ;
        }

        // Create table for newly created word doc representing printing labels based on number of entries found on load
        private Word.Table CreateTableForWord(ref Word.Document inDoc, ref DateTime inDate)
        {
            int numColumns = 3;
            int patientCount = fDateCounter[inDate];
            int numRows = ((int)Math.Ceiling(patientCount / 3.0)) * 4;

            Word.Table outTable = inDoc.Tables.Add(inDoc.Range(), numRows, numColumns);
            outTable.Range.ParagraphFormat.SpaceAfter = 4.5f;
            outTable.Range.ParagraphFormat.LineSpacingRule = Word.WdLineSpacing.wdLineSpaceSingle;

            return outTable;
        }

        private void IteratePatientsForMailMerge(Word.Application inApp)
        {
            if (comboDateSelection.SelectedIndex == -1) return;

            DateTime selectedDate = DateTime.Parse(comboDateSelection.SelectedItem.ToString());
            Word.Document currDoc = inApp.Documents.Add();

            // Setup Page (Avery Labels)
            currDoc.PageSetup.LeftMargin = inApp.InchesToPoints(0.25f);
            currDoc.PageSetup.TopMargin = inApp.InchesToPoints(0.8f);
            currDoc.PageSetup.RightMargin = inApp.InchesToPoints(0.125f);
            currDoc.PageSetup.BottomMargin = inApp.InchesToPoints(0.4f);

            Word.Table currDocTable = CreateTableForWord(ref currDoc, ref selectedDate);

            using (var workbook = new XLWorkbook(m_filePath))
            {
                var worksheet = workbook.Worksheet("Patient_Registration MASTER");
                var rows = worksheet.RowsUsed().Skip(1);

                int currWordRow = 1;
                int currWordColumn = 1;

                foreach (var row in rows)
                {
                    var cellValue = row.Cell(kReturnDateLoc).Value;
                    if (cellValue.IsDateTime && cellValue.GetDateTime().Date == selectedDate.Date)
                    {
                        string name = $"{row.Cell(kFirstNameLoc).Value} {row.Cell(kLastNameLoc).Value}";
                        string address = row.Cell(kAddressLoc).Value.ToString();
                        string cityStateZip = $"{row.Cell(kCityLoc).Value}, {row.Cell(kStateLoc).Value} {row.Cell(kZipLoc).Value}";

                        SendPatientToMailMerge(name, address, cityStateZip, currWordRow, currWordColumn, currDocTable);

                        currWordColumn++;
                        if (currWordColumn > 3)
                        {
                            currWordColumn = 1;
                            currWordRow += 4;
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
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

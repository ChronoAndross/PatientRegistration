using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using OleDbConnection = System.Data.OleDb.OleDbConnection;
using StringBuilder = System.Text.StringBuilder;
using Generics = System.Collections.Generic;

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
        public MailMergeDialog()
        {
            InitializeComponent();
        }

        public MailMergeDialog(Excel.Workbook inWorkbook)
        {
            InitializeComponent();
            m_existingWkBook = inWorkbook;
            fDateCounter = new Generics.Dictionary<DateTime, int>();
        }

        private void MailMergeDialog_Load(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.Sheets["Patient_Registration MASTER"] ;
                int currRow = 2; // always start on the second row
                while ((currSheet.Cells[currRow, kLastNameLoc]).Text != "")
                {
                    // put new patient into excel doc
                    if ((currSheet.Cells[currRow, kReturnDateLoc]).Text != "")
                    {
                        DateTime returnData = currSheet.Cells[currRow, kReturnDateLoc].Value;
                        if (!comboDateSelection.Items.Contains(returnData))
                        {
                            comboDateSelection.Items.Add(returnData);
                            fDateCounter.Add(returnData, 1);
                        }
                        else
                            fDateCounter[returnData]++;
                    }
                    currRow++;
                }
            }
            comboDateSelection.SelectedItem = null;
            comboDateSelection.SelectedText = "--Please select a date--";
        }

        private void SendPatientToMailMerge(ref string inFirstLast, ref string inAddress, ref string inCityState, 
            ref int inWordRow, ref int inColumn, ref Word.Table inCurrTable)
        {
            Word.WdParagraphAlignment alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;

            Word.Cell cellName = inCurrTable.Cell(inWordRow, inColumn);
            cellName.Range.Text = inFirstLast;
            cellName.Range.ParagraphFormat.Alignment = alignment;

            Word.Cell cellAddress = inCurrTable.Cell(inWordRow + 1, inColumn);
            cellAddress.Range.Text = inAddress;
            cellAddress.Range.ParagraphFormat.Alignment = alignment;

            Word.Cell cellCityStateZip = inCurrTable.Cell(inWordRow + 2, inColumn);
            cellCityStateZip.Range.Text = inCityState;
            cellCityStateZip.Range.ParagraphFormat.Alignment = alignment;
        }

        // Create table for newly created word doc representing printing labels based on number of entries found on load
        private Word.Table CreateTableForWord(ref Word.Document inCurrentDoc, ref DateTime inSelectedTime)
        {
            object start = 0;
            object end = 0;
            int numColumns = 3;
            int extraRow = fDateCounter[inSelectedTime] % 3 == 0 ? 0 : 1;
            int numRows = ((fDateCounter[inSelectedTime] / 3) + extraRow)*4;
            Word.Range tableLocation = inCurrentDoc.Range(ref start, ref end);
            Word.Table outWordTable = inCurrentDoc.Tables.Add(tableLocation, numRows, numColumns);
            outWordTable.Spacing = 0;
            return outWordTable;
        }

        private void IteratePatientsForMailMerge(Word.Application inApp)
        {
            if (m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.Sheets["Patient_Registration MASTER"] ;
                int currExcelRow = 2; // always start on the second row
                int currWordRow = 1;
                int currWordColumn = 1;
                Word.Document currDoc = inApp.Documents.Add();
                DateTime selectedDate = DateTime.Parse(comboDateSelection.SelectedItem.ToString());
                Word.Table currDocTable = CreateTableForWord(ref currDoc, ref selectedDate);
                while ((currSheet.Cells[currExcelRow, kLastNameLoc]).Text != "")
                {
                    // if the patient's return date matches, send to MailMerge
                    if ((currSheet.Cells[currExcelRow, kReturnDateLoc]).Text != "")
                    {
                        DateTime returnData = currSheet.Cells[currExcelRow, kReturnDateLoc].Value;
                        if (selectedDate.Equals(returnData))
                        {
                            string firstLastNameStr = currSheet.Cells[currExcelRow, kFirstNameLoc].Value + " "
                                + currSheet.Cells[currExcelRow, kLastNameLoc].Value;

                            string addressStr = currSheet.Cells[currExcelRow, kAddressLoc].Value;

                            string cellCityStateZip = currSheet.Cells[currExcelRow, kCityLoc].Value + ", "
                                + currSheet.Cells[currExcelRow, kStateLoc].Value
                                + " " + currSheet.Cells[currExcelRow, kZipLoc].Value;

                            SendPatientToMailMerge(ref firstLastNameStr, ref addressStr, ref cellCityStateZip,
                                ref currWordRow, ref currWordColumn, ref currDocTable);

                            currWordColumn++;

                            if (currWordColumn == 4)
                            {
                                currWordColumn = 1;
                                currWordRow += 4;
                            }
                        }
                    }
                    currExcelRow++;
                }
            }
        }

        private void btnSendToMailMerge_Click(object sender, EventArgs e)
        {            
            var wordApp = new Word.Application();
            IteratePatientsForMailMerge(wordApp);
            wordApp.Visible = true;
            this.Close();
            fDateCounter = null; // collect memory for the allocated dictionary
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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

        Generics.Dictionary<DateTime, int> fDateCounter; 
    }
}

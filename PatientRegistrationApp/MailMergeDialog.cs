using System;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using OleDbConnection = System.Data.OleDb.OleDbConnection;
using StringBuilder = System.Text.StringBuilder;

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
        }

        private void MailMergeDialog_Load(object sender, EventArgs e)
        {
            if (m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.ActiveSheet;
                int currRow = 2; // always start on the second row
                while ((currSheet.Cells[currRow, kLastNameLoc]).Text != "")
                {
                    // put new patient into excel doc
                    if ((currSheet.Cells[currRow, kReturnDateLoc]).Text != "")
                    {
                        DateTime returnData = currSheet.Cells[currRow, kReturnDateLoc].Value;
                        if (!comboDateSelection.Items.Contains(returnData))
                            comboDateSelection.Items.Add(returnData);
                    }
                    currRow++;
                }
            }
            comboDateSelection.SelectedItem = null;
            comboDateSelection.SelectedText = "--Please select a date--";
        }

        private void SendPatientToMailMerge(ref Word.Document inCurrentDoc, 
            ref string inFirstLast, ref string inAddress, ref string inCityState, 
            ref int inWordRow, ref Word.WdParagraphAlignment inAlignment, ref Word.Table inCurrTable)
        {

            if (inAlignment == Word.WdParagraphAlignment.wdAlignParagraphLeft)
            {
                object start = 0;
                object end = 0;
                Word.Range tableLocation = inCurrentDoc.Range(ref start, ref end);
                inCurrTable = inCurrentDoc.Tables.Add(tableLocation, 3, 3);
            }

            int currColumn = 1;
            if (inAlignment == Word.WdParagraphAlignment.wdAlignParagraphCenter)
                currColumn = 2;
            else if (inAlignment == Word.WdParagraphAlignment.wdAlignParagraphRight)
                currColumn = 3;

            Word.Cell cellName = inCurrTable.Cell(inWordRow, currColumn);
            cellName.Range.Text = inFirstLast;
            cellName.Range.ParagraphFormat.Alignment = inAlignment;

            Word.Cell cellAddress = inCurrTable.Cell(inWordRow + 1, currColumn);
            cellAddress.Range.Text = inAddress;
            cellAddress.Range.ParagraphFormat.Alignment = inAlignment;

            Word.Cell cellCityStateZip = inCurrTable.Cell(inWordRow + 2, currColumn);
            cellCityStateZip.Range.Text = inCityState;
            cellCityStateZip.Range.ParagraphFormat.Alignment = inAlignment;

        }

        private void IteratePatientsForMailMerge(Word.Application inApp)
        {
            if (m_existingWkBook != null)
            {
                Excel.Worksheet currSheet = m_existingWkBook.ActiveSheet;
                int currExcelRow = 2; // always start on the second row
                int currWordRow = 1;
                int currAlignmentType = 0;
                Word.WdParagraphAlignment[] alignmentTypes = {
                        Word.WdParagraphAlignment.wdAlignParagraphLeft,
                        Word.WdParagraphAlignment.wdAlignParagraphCenter,
                        Word.WdParagraphAlignment.wdAlignParagraphRight
                };
                Word.Document currDoc = inApp.Documents.Add();
                Word.Table currTable = null;
                while ((currSheet.Cells[currExcelRow, kLastNameLoc]).Text != "")
                {
                    // if the patient's return date matches, send to MailMerge
                    if ((currSheet.Cells[currExcelRow, kReturnDateLoc]).Text != "")
                    {
                        DateTime returnData = currSheet.Cells[currExcelRow, kReturnDateLoc].Value;
                        if (comboDateSelection.SelectedItem.Equals(returnData))
                        {
                            string firstLastNameStr = currSheet.Cells[currExcelRow, kFirstNameLoc].Value + " "
                                + currSheet.Cells[currExcelRow, kLastNameLoc].Value;

                            string addressStr = currSheet.Cells[currExcelRow, kAddressLoc].Value;

                            string cellCityStateZip = currSheet.Cells[currExcelRow, kCityLoc].Value + ", "
                                + currSheet.Cells[currExcelRow, kStateLoc].Value
                                + " " + currSheet.Cells[currExcelRow, kZipLoc].Value;

                            SendPatientToMailMerge(ref currDoc,
                                ref firstLastNameStr, ref addressStr, ref cellCityStateZip,
                                ref currWordRow, ref alignmentTypes[currAlignmentType], ref currTable);

                            currAlignmentType++;

                            if (currAlignmentType == 3)
                            {
                                currAlignmentType = 0;
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
    }
}

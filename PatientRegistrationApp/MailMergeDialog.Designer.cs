namespace PatientRegistrationApp
{
    partial class MailMergeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendToMailMerge = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelDateSelection = new System.Windows.Forms.Label();
            this.comboDateSelection = new System.Windows.Forms.ComboBox();
            this.btnPrintPostcards = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSendToMailMerge
            // 
            this.btnSendToMailMerge.Location = new System.Drawing.Point(28, 163);
            this.btnSendToMailMerge.Margin = new System.Windows.Forms.Padding(2);
            this.btnSendToMailMerge.Name = "btnSendToMailMerge";
            this.btnSendToMailMerge.Size = new System.Drawing.Size(134, 31);
            this.btnSendToMailMerge.TabIndex = 0;
            this.btnSendToMailMerge.Text = "Send To Mailing Labels";
            this.btnSendToMailMerge.UseVisualStyleBackColor = true;
            this.btnSendToMailMerge.Click += new System.EventHandler(this.btnSendToMailMerge_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(295, 163);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelDateSelection
            // 
            this.labelDateSelection.AutoSize = true;
            this.labelDateSelection.Location = new System.Drawing.Point(58, 61);
            this.labelDateSelection.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDateSelection.Name = "labelDateSelection";
            this.labelDateSelection.Size = new System.Drawing.Size(66, 13);
            this.labelDateSelection.TabIndex = 2;
            this.labelDateSelection.Text = "Select Date:";
            // 
            // comboDateSelection
            // 
            this.comboDateSelection.FormattingEnabled = true;
            this.comboDateSelection.Location = new System.Drawing.Point(138, 61);
            this.comboDateSelection.Margin = new System.Windows.Forms.Padding(2);
            this.comboDateSelection.Name = "comboDateSelection";
            this.comboDateSelection.Size = new System.Drawing.Size(140, 21);
            this.comboDateSelection.TabIndex = 3;
            // 
            // btnPrintPostcards
            // 
            this.btnPrintPostcards.Location = new System.Drawing.Point(173, 163);
            this.btnPrintPostcards.Margin = new System.Windows.Forms.Padding(2);
            this.btnPrintPostcards.Name = "btnPrintPostcards";
            this.btnPrintPostcards.Size = new System.Drawing.Size(105, 31);
            this.btnPrintPostcards.TabIndex = 4;
            this.btnPrintPostcards.Text = "Send to Postcard";
            this.btnPrintPostcards.UseVisualStyleBackColor = true;
            this.btnPrintPostcards.Click += new System.EventHandler(this.btnPrintPostcards_Click);
            // 
            // MailMergeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 234);
            this.Controls.Add(this.btnPrintPostcards);
            this.Controls.Add(this.comboDateSelection);
            this.Controls.Add(this.labelDateSelection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSendToMailMerge);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MailMergeDialog";
            this.Text = "Send To Mailing Labels";
            this.Load += new System.EventHandler(this.MailMergeDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendToMailMerge;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelDateSelection;
        private System.Windows.Forms.ComboBox comboDateSelection;
        private System.Windows.Forms.Button btnPrintPostcards;
    }
}
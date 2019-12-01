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
            this.SuspendLayout();
            // 
            // btnSendToMailMerge
            // 
            this.btnSendToMailMerge.Location = new System.Drawing.Point(122, 314);
            this.btnSendToMailMerge.Name = "btnSendToMailMerge";
            this.btnSendToMailMerge.Size = new System.Drawing.Size(267, 59);
            this.btnSendToMailMerge.TabIndex = 0;
            this.btnSendToMailMerge.Text = "Send To Mailing Labels";
            this.btnSendToMailMerge.UseVisualStyleBackColor = true;
            this.btnSendToMailMerge.Click += new System.EventHandler(this.btnSendToMailMerge_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(517, 314);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 59);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelDateSelection
            // 
            this.labelDateSelection.AutoSize = true;
            this.labelDateSelection.Location = new System.Drawing.Point(117, 117);
            this.labelDateSelection.Name = "labelDateSelection";
            this.labelDateSelection.Size = new System.Drawing.Size(129, 25);
            this.labelDateSelection.TabIndex = 2;
            this.labelDateSelection.Text = "Select Date:";
            // 
            // comboDateSelection
            // 
            this.comboDateSelection.FormattingEnabled = true;
            this.comboDateSelection.Location = new System.Drawing.Point(275, 117);
            this.comboDateSelection.Name = "comboDateSelection";
            this.comboDateSelection.Size = new System.Drawing.Size(276, 33);
            this.comboDateSelection.Sorted = true;
            this.comboDateSelection.TabIndex = 3;
            // 
            // MailMergeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.comboDateSelection);
            this.Controls.Add(this.labelDateSelection);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSendToMailMerge);
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
    }
}
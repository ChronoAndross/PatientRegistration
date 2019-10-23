namespace PatientRegistrationApp
{
    partial class PatientRegistrationApp
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
            this.btnInputPat = new System.Windows.Forms.Button();
            this.btnDeletePat = new System.Windows.Forms.Button();
            this.btnOpenXls = new System.Windows.Forms.Button();
            this.btnSendMailmerge = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInputPat
            // 
            this.btnInputPat.Location = new System.Drawing.Point(84, 90);
            this.btnInputPat.Name = "btnInputPat";
            this.btnInputPat.Size = new System.Drawing.Size(209, 78);
            this.btnInputPat.TabIndex = 0;
            this.btnInputPat.Text = "Input Patient";
            this.btnInputPat.UseVisualStyleBackColor = true;
            this.btnInputPat.Click += new System.EventHandler(this.btnInputPat_Click);
            // 
            // btnDeletePat
            // 
            this.btnDeletePat.Location = new System.Drawing.Point(84, 210);
            this.btnDeletePat.Name = "btnDeletePat";
            this.btnDeletePat.Size = new System.Drawing.Size(209, 90);
            this.btnDeletePat.TabIndex = 1;
            this.btnDeletePat.Text = "Delete Patient";
            this.btnDeletePat.UseVisualStyleBackColor = true;
            this.btnDeletePat.Click += new System.EventHandler(this.btnDeletePat_Click);
            // 
            // btnOpenXls
            // 
            this.btnOpenXls.Location = new System.Drawing.Point(459, 90);
            this.btnOpenXls.Name = "btnOpenXls";
            this.btnOpenXls.Size = new System.Drawing.Size(209, 78);
            this.btnOpenXls.TabIndex = 2;
            this.btnOpenXls.Text = "Open Excel File";
            this.btnOpenXls.UseVisualStyleBackColor = true;
            this.btnOpenXls.Click += new System.EventHandler(this.btnOpenXls_Click);
            // 
            // btnSendMailmerge
            // 
            this.btnSendMailmerge.Location = new System.Drawing.Point(459, 210);
            this.btnSendMailmerge.Name = "btnSendMailmerge";
            this.btnSendMailmerge.Size = new System.Drawing.Size(209, 90);
            this.btnSendMailmerge.TabIndex = 3;
            this.btnSendMailmerge.Text = "Send to Mailmerge";
            this.btnSendMailmerge.UseVisualStyleBackColor = true;
            this.btnSendMailmerge.Click += new System.EventHandler(this.btnSendMailmerge_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(296, 369);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(182, 59);
            this.btnAbout.TabIndex = 4;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.button5_Click);
            // 
            // PatientRegistrationApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(903, 515);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnSendMailmerge);
            this.Controls.Add(this.btnOpenXls);
            this.Controls.Add(this.btnDeletePat);
            this.Controls.Add(this.btnInputPat);
            this.Name = "PatientRegistrationApp";
            this.Text = "PatientRegistrationApp";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInputPat;
        private System.Windows.Forms.Button btnDeletePat;
        private System.Windows.Forms.Button btnOpenXls;
        private System.Windows.Forms.Button btnSendMailmerge;
        private System.Windows.Forms.Button btnAbout;
    }
}


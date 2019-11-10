namespace PatientRegistrationApp
{
    partial class AlertDialog
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
            this.btnConfirm = new System.Windows.Forms.Button();
            this.textAlert = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(295, 306);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(128, 55);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "OK";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // textAlert
            // 
            this.textAlert.BackColor = System.Drawing.SystemColors.ControlLight;
            this.textAlert.Location = new System.Drawing.Point(84, 58);
            this.textAlert.Multiline = true;
            this.textAlert.Name = "textAlert";
            this.textAlert.Size = new System.Drawing.Size(549, 153);
            this.textAlert.TabIndex = 1;
            this.textAlert.Text = "Failed to find Patient Registration excel file.Please make sure that the excel fi" +
    "le is in the same folder as the application.";
            this.textAlert.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // AlertDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(722, 445);
            this.Controls.Add(this.textAlert);
            this.Controls.Add(this.btnConfirm);
            this.Name = "AlertDialog";
            this.Text = "PatientRegistration";
            this.Load += new System.EventHandler(this.AlertDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox textAlert;
    }
}
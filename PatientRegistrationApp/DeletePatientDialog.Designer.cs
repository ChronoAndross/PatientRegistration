namespace PatientRegistrationApp
{
    partial class DeletePatientDialog
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
            this.btnAccept = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.comboPatients = new System.Windows.Forms.ComboBox();
            this.labelPatientName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(157, 300);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(134, 69);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(473, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(134, 69);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // comboPatients
            // 
            this.comboPatients.FormattingEnabled = true;
            this.comboPatients.Location = new System.Drawing.Point(286, 141);
            this.comboPatients.Name = "comboPatients";
            this.comboPatients.Size = new System.Drawing.Size(409, 33);
            this.comboPatients.TabIndex = 2;
            // 
            // labelPatientName
            // 
            this.labelPatientName.AutoSize = true;
            this.labelPatientName.Location = new System.Drawing.Point(95, 141);
            this.labelPatientName.Name = "labelPatientName";
            this.labelPatientName.Size = new System.Drawing.Size(147, 25);
            this.labelPatientName.TabIndex = 3;
            this.labelPatientName.Text = "Patient Name:";
            // 
            // DeletePatientDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelPatientName);
            this.Controls.Add(this.comboPatients);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Name = "DeletePatientDialog";
            this.Text = "Remove Patient";
            this.Load += new System.EventHandler(this.DeletePatientDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox comboPatients;
        private System.Windows.Forms.Label labelPatientName;
    }
}
namespace PatientRegistrationApp
{
    partial class AddPatientDialog
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
            this.labelFirstName = new System.Windows.Forms.Label();
            this.labelLastName = new System.Windows.Forms.Label();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelCity = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.labelZip = new System.Windows.Forms.Label();
            this.labelHomePhone = new System.Windows.Forms.Label();
            this.labelCellPhone = new System.Windows.Forms.Label();
            this.labelReturnDate = new System.Windows.Forms.Label();
            this.textFirstName = new System.Windows.Forms.TextBox();
            this.textLastName = new System.Windows.Forms.TextBox();
            this.textHomePhone = new System.Windows.Forms.TextBox();
            this.textCellPhone = new System.Windows.Forms.TextBox();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.textCity = new System.Windows.Forms.TextBox();
            this.textState = new System.Windows.Forms.TextBox();
            this.textZip = new System.Windows.Forms.TextBox();
            this.textReturnDate = new System.Windows.Forms.TextBox();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textInsurance = new System.Windows.Forms.TextBox();
            this.insuranceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(83, 212);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(2);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(80, 34);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "Add Patient";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(228, 212);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(69, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Location = new System.Drawing.Point(26, 33);
            this.labelFirstName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(60, 13);
            this.labelFirstName.TabIndex = 2;
            this.labelFirstName.Text = "First Name:";
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.labelLastName.Location = new System.Drawing.Point(26, 59);
            this.labelLastName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(61, 13);
            this.labelLastName.TabIndex = 3;
            this.labelLastName.Text = "Last Name:";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(202, 33);
            this.labelAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(79, 13);
            this.labelAddress.TabIndex = 4;
            this.labelAddress.Text = "Home Address:";
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(202, 59);
            this.labelCity.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(27, 13);
            this.labelCity.TabIndex = 5;
            this.labelCity.Text = "City:";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(202, 83);
            this.labelState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(35, 13);
            this.labelState.TabIndex = 6;
            this.labelState.Text = "State:";
            // 
            // labelZip
            // 
            this.labelZip.AutoSize = true;
            this.labelZip.Location = new System.Drawing.Point(202, 109);
            this.labelZip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelZip.Name = "labelZip";
            this.labelZip.Size = new System.Drawing.Size(53, 13);
            this.labelZip.TabIndex = 7;
            this.labelZip.Text = "Zip Code:";
            // 
            // labelHomePhone
            // 
            this.labelHomePhone.AutoSize = true;
            this.labelHomePhone.Location = new System.Drawing.Point(26, 83);
            this.labelHomePhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelHomePhone.Name = "labelHomePhone";
            this.labelHomePhone.Size = new System.Drawing.Size(72, 13);
            this.labelHomePhone.TabIndex = 8;
            this.labelHomePhone.Text = "Home Phone:";
            // 
            // labelCellPhone
            // 
            this.labelCellPhone.AutoSize = true;
            this.labelCellPhone.Location = new System.Drawing.Point(26, 109);
            this.labelCellPhone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCellPhone.Name = "labelCellPhone";
            this.labelCellPhone.Size = new System.Drawing.Size(61, 13);
            this.labelCellPhone.TabIndex = 9;
            this.labelCellPhone.Text = "Cell Phone:";
            // 
            // labelReturnDate
            // 
            this.labelReturnDate.AutoSize = true;
            this.labelReturnDate.Location = new System.Drawing.Point(26, 135);
            this.labelReturnDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelReturnDate.Name = "labelReturnDate";
            this.labelReturnDate.Size = new System.Drawing.Size(68, 13);
            this.labelReturnDate.TabIndex = 10;
            this.labelReturnDate.Text = "Return Date:";
            this.labelReturnDate.Click += new System.EventHandler(this.labelReturnDate_Click);
            // 
            // textFirstName
            // 
            this.textFirstName.Location = new System.Drawing.Point(90, 30);
            this.textFirstName.Margin = new System.Windows.Forms.Padding(2);
            this.textFirstName.Name = "textFirstName";
            this.textFirstName.Size = new System.Drawing.Size(88, 20);
            this.textFirstName.TabIndex = 11;
            // 
            // textLastName
            // 
            this.textLastName.Location = new System.Drawing.Point(90, 57);
            this.textLastName.Margin = new System.Windows.Forms.Padding(2);
            this.textLastName.Name = "textLastName";
            this.textLastName.Size = new System.Drawing.Size(88, 20);
            this.textLastName.TabIndex = 12;
            // 
            // textHomePhone
            // 
            this.textHomePhone.Location = new System.Drawing.Point(100, 82);
            this.textHomePhone.Margin = new System.Windows.Forms.Padding(2);
            this.textHomePhone.Name = "textHomePhone";
            this.textHomePhone.Size = new System.Drawing.Size(88, 20);
            this.textHomePhone.TabIndex = 13;
            // 
            // textCellPhone
            // 
            this.textCellPhone.Location = new System.Drawing.Point(91, 109);
            this.textCellPhone.Margin = new System.Windows.Forms.Padding(2);
            this.textCellPhone.Name = "textCellPhone";
            this.textCellPhone.Size = new System.Drawing.Size(88, 20);
            this.textCellPhone.TabIndex = 14;
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(284, 32);
            this.textAddress.Margin = new System.Windows.Forms.Padding(2);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(88, 20);
            this.textAddress.TabIndex = 15;
            // 
            // textCity
            // 
            this.textCity.Location = new System.Drawing.Point(232, 57);
            this.textCity.Margin = new System.Windows.Forms.Padding(2);
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(88, 20);
            this.textCity.TabIndex = 16;
            // 
            // textState
            // 
            this.textState.Location = new System.Drawing.Point(238, 82);
            this.textState.Margin = new System.Windows.Forms.Padding(2);
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(88, 20);
            this.textState.TabIndex = 17;
            // 
            // textZip
            // 
            this.textZip.Location = new System.Drawing.Point(257, 108);
            this.textZip.Margin = new System.Windows.Forms.Padding(2);
            this.textZip.Name = "textZip";
            this.textZip.Size = new System.Drawing.Size(88, 20);
            this.textZip.TabIndex = 18;
            // 
            // textReturnDate
            // 
            this.textReturnDate.Location = new System.Drawing.Point(98, 132);
            this.textReturnDate.Margin = new System.Windows.Forms.Padding(2);
            this.textReturnDate.Name = "textReturnDate";
            this.textReturnDate.Size = new System.Drawing.Size(88, 20);
            this.textReturnDate.TabIndex = 19;
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Location = new System.Drawing.Point(245, 135);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(127, 55);
            this.textBoxNotes.TabIndex = 21;
            this.textBoxNotes.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Location = new System.Drawing.Point(203, 135);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 20;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.Click += new System.EventHandler(this.label1_Click);
            // 
            // textInsurance
            // 
            this.textInsurance.Location = new System.Drawing.Point(91, 156);
            this.textInsurance.Margin = new System.Windows.Forms.Padding(2);
            this.textInsurance.Name = "textInsurance";
            this.textInsurance.Size = new System.Drawing.Size(88, 20);
            this.textInsurance.TabIndex = 23;
            // 
            // insuranceLabel
            // 
            this.insuranceLabel.AutoSize = true;
            this.insuranceLabel.Location = new System.Drawing.Point(27, 158);
            this.insuranceLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.insuranceLabel.Name = "insuranceLabel";
            this.insuranceLabel.Size = new System.Drawing.Size(57, 13);
            this.insuranceLabel.TabIndex = 22;
            this.insuranceLabel.Text = "Insurance:";
            // 
            // AddPatientDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 280);
            this.Controls.Add(this.textInsurance);
            this.Controls.Add(this.insuranceLabel);
            this.Controls.Add(this.textBoxNotes);
            this.Controls.Add(this.labelNotes);
            this.Controls.Add(this.textReturnDate);
            this.Controls.Add(this.textZip);
            this.Controls.Add(this.textState);
            this.Controls.Add(this.textCity);
            this.Controls.Add(this.textAddress);
            this.Controls.Add(this.textCellPhone);
            this.Controls.Add(this.textHomePhone);
            this.Controls.Add(this.textLastName);
            this.Controls.Add(this.textFirstName);
            this.Controls.Add(this.labelReturnDate);
            this.Controls.Add(this.labelCellPhone);
            this.Controls.Add(this.labelHomePhone);
            this.Controls.Add(this.labelZip);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.labelCity);
            this.Controls.Add(this.labelAddress);
            this.Controls.Add(this.labelLastName);
            this.Controls.Add(this.labelFirstName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAccept);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AddPatientDialog";
            this.Text = "New Patient";
            this.Load += new System.EventHandler(this.AddPatientDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelFirstName;
        private System.Windows.Forms.Label labelLastName;
        private System.Windows.Forms.Label labelAddress;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelZip;
        private System.Windows.Forms.Label labelHomePhone;
        private System.Windows.Forms.Label labelCellPhone;
        private System.Windows.Forms.Label labelReturnDate;
        private System.Windows.Forms.TextBox textFirstName;
        private System.Windows.Forms.TextBox textLastName;
        private System.Windows.Forms.TextBox textHomePhone;
        private System.Windows.Forms.TextBox textCellPhone;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.TextBox textCity;
        private System.Windows.Forms.TextBox textState;
        private System.Windows.Forms.TextBox textZip;
        private System.Windows.Forms.TextBox textReturnDate;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textInsurance;
        private System.Windows.Forms.Label insuranceLabel;
    }
}
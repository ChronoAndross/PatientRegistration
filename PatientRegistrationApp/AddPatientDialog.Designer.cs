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
            this.SuspendLayout();
            // 
            // btnAccept
            // 
            this.btnAccept.Location = new System.Drawing.Point(166, 407);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(138, 65);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "OK";
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 407);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(138, 65);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelFirstName
            // 
            this.labelFirstName.AutoSize = true;
            this.labelFirstName.Location = new System.Drawing.Point(52, 64);
            this.labelFirstName.Name = "labelFirstName";
            this.labelFirstName.Size = new System.Drawing.Size(122, 25);
            this.labelFirstName.TabIndex = 2;
            this.labelFirstName.Text = "First Name:";
            // 
            // labelLastName
            // 
            this.labelLastName.AutoSize = true;
            this.labelLastName.Location = new System.Drawing.Point(52, 113);
            this.labelLastName.Name = "labelLastName";
            this.labelLastName.Size = new System.Drawing.Size(121, 25);
            this.labelLastName.TabIndex = 3;
            this.labelLastName.Text = "Last Name:";
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.Location = new System.Drawing.Point(403, 64);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(159, 25);
            this.labelAddress.TabIndex = 4;
            this.labelAddress.Text = "Home Address:";
            // 
            // labelCity
            // 
            this.labelCity.AutoSize = true;
            this.labelCity.Location = new System.Drawing.Point(403, 113);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(55, 25);
            this.labelCity.TabIndex = 5;
            this.labelCity.Text = "City:";
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(403, 160);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(68, 25);
            this.labelState.TabIndex = 6;
            this.labelState.Text = "State:";
            // 
            // labelZip
            // 
            this.labelZip.AutoSize = true;
            this.labelZip.Location = new System.Drawing.Point(403, 210);
            this.labelZip.Name = "labelZip";
            this.labelZip.Size = new System.Drawing.Size(105, 25);
            this.labelZip.TabIndex = 7;
            this.labelZip.Text = "Zip Code:";
            // 
            // labelHomePhone
            // 
            this.labelHomePhone.AutoSize = true;
            this.labelHomePhone.Location = new System.Drawing.Point(53, 160);
            this.labelHomePhone.Name = "labelHomePhone";
            this.labelHomePhone.Size = new System.Drawing.Size(142, 25);
            this.labelHomePhone.TabIndex = 8;
            this.labelHomePhone.Text = "Home Phone:";
            // 
            // labelCellPhone
            // 
            this.labelCellPhone.AutoSize = true;
            this.labelCellPhone.Location = new System.Drawing.Point(53, 210);
            this.labelCellPhone.Name = "labelCellPhone";
            this.labelCellPhone.Size = new System.Drawing.Size(123, 25);
            this.labelCellPhone.TabIndex = 9;
            this.labelCellPhone.Text = "Cell Phone:";
            // 
            // labelReturnDate
            // 
            this.labelReturnDate.AutoSize = true;
            this.labelReturnDate.Location = new System.Drawing.Point(219, 295);
            this.labelReturnDate.Name = "labelReturnDate";
            this.labelReturnDate.Size = new System.Drawing.Size(133, 25);
            this.labelReturnDate.TabIndex = 10;
            this.labelReturnDate.Text = "Return Date:";
            // 
            // textFirstName
            // 
            this.textFirstName.Location = new System.Drawing.Point(181, 57);
            this.textFirstName.Name = "textFirstName";
            this.textFirstName.Size = new System.Drawing.Size(171, 31);
            this.textFirstName.TabIndex = 11;
            // 
            // textLastName
            // 
            this.textLastName.Location = new System.Drawing.Point(179, 110);
            this.textLastName.Name = "textLastName";
            this.textLastName.Size = new System.Drawing.Size(171, 31);
            this.textLastName.TabIndex = 12;
            // 
            // textHomePhone
            // 
            this.textHomePhone.Location = new System.Drawing.Point(201, 157);
            this.textHomePhone.Name = "textHomePhone";
            this.textHomePhone.Size = new System.Drawing.Size(171, 31);
            this.textHomePhone.TabIndex = 13;
            // 
            // textCellPhone
            // 
            this.textCellPhone.Location = new System.Drawing.Point(182, 210);
            this.textCellPhone.Name = "textCellPhone";
            this.textCellPhone.Size = new System.Drawing.Size(171, 31);
            this.textCellPhone.TabIndex = 14;
            // 
            // textAddress
            // 
            this.textAddress.Location = new System.Drawing.Point(568, 61);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(171, 31);
            this.textAddress.TabIndex = 15;
            // 
            // textCity
            // 
            this.textCity.Location = new System.Drawing.Point(464, 110);
            this.textCity.Name = "textCity";
            this.textCity.Size = new System.Drawing.Size(171, 31);
            this.textCity.TabIndex = 16;
            // 
            // textState
            // 
            this.textState.Location = new System.Drawing.Point(477, 157);
            this.textState.Name = "textState";
            this.textState.Size = new System.Drawing.Size(171, 31);
            this.textState.TabIndex = 17;
            // 
            // textZip
            // 
            this.textZip.Location = new System.Drawing.Point(514, 207);
            this.textZip.Name = "textZip";
            this.textZip.Size = new System.Drawing.Size(171, 31);
            this.textZip.TabIndex = 18;
            // 
            // textReturnDate
            // 
            this.textReturnDate.Location = new System.Drawing.Point(358, 292);
            this.textReturnDate.Name = "textReturnDate";
            this.textReturnDate.Size = new System.Drawing.Size(171, 31);
            this.textReturnDate.TabIndex = 19;
            // 
            // AddPatientDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 539);
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
            this.Name = "AddPatientDialog";
            this.Text = "New Patient";
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
    }
}
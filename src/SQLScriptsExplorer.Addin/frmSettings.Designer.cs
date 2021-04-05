
namespace SQLScriptsExplorer.Addin
{
    partial class frmSettings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cboParserVersion = new System.Windows.Forms.ComboBox();
            this.lblGeneralSettings = new System.Windows.Forms.Label();
            this.lblSQLScriptsFolder = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gdvFolderMapping = new System.Windows.Forms.DataGridView();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.chkExpandOnLoad = new System.Windows.Forms.CheckBox();
            this.lblFolderMapping = new System.Windows.Forms.Label();
            this.lblSeparator2 = new System.Windows.Forms.Label();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.lblSQLParserVersion = new System.Windows.Forms.Label();
            this.lblFileTypes = new System.Windows.Forms.Label();
            this.txtAllowedFileTypes = new System.Windows.Forms.TextBox();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFolderMapping)).BeginInit();
            this.SuspendLayout();
            // 
            // cboParserVersion
            // 
            this.cboParserVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParserVersion.FormattingEnabled = true;
            this.cboParserVersion.Items.AddRange(new object[] {
            "SQL Server 2019",
            "SQL Server 2017",
            "SQL Server 2016"});
            this.cboParserVersion.Location = new System.Drawing.Point(148, 294);
            this.cboParserVersion.Name = "cboParserVersion";
            this.cboParserVersion.Size = new System.Drawing.Size(212, 21);
            this.cboParserVersion.TabIndex = 8;
            // 
            // lblGeneralSettings
            // 
            this.lblGeneralSettings.AutoSize = true;
            this.lblGeneralSettings.Location = new System.Drawing.Point(18, 267);
            this.lblGeneralSettings.Name = "lblGeneralSettings";
            this.lblGeneralSettings.Size = new System.Drawing.Size(85, 13);
            this.lblGeneralSettings.TabIndex = 5;
            this.lblGeneralSettings.Text = "General Settings";
            // 
            // lblSQLScriptsFolder
            // 
            this.lblSQLScriptsFolder.AutoSize = true;
            this.lblSQLScriptsFolder.Location = new System.Drawing.Point(43, 44);
            this.lblSQLScriptsFolder.Name = "lblSQLScriptsFolder";
            this.lblSQLScriptsFolder.Size = new System.Drawing.Size(510, 13);
            this.lblSQLScriptsFolder.TabIndex = 2;
            this.lblSQLScriptsFolder.Text = "Map script folders by entering an Alias and its Folder Path. Drag and drop rows t" +
    "o order the mapped folders.";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(515, 362);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(434, 362);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // gdvFolderMapping
            // 
            this.gdvFolderMapping.AllowDrop = true;
            this.gdvFolderMapping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvFolderMapping.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Alias,
            this.FolderPath,
            this.Valid});
            this.gdvFolderMapping.Location = new System.Drawing.Point(46, 65);
            this.gdvFolderMapping.Name = "gdvFolderMapping";
            this.gdvFolderMapping.Size = new System.Drawing.Size(544, 155);
            this.gdvFolderMapping.TabIndex = 3;
            this.gdvFolderMapping.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gdvFolderMapping_CellEndEdit);
            this.gdvFolderMapping.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gdvFolderMapping_CellFormatting);
            this.gdvFolderMapping.DragDrop += new System.Windows.Forms.DragEventHandler(this.gdvFolderMapping_DragDrop);
            this.gdvFolderMapping.DragOver += new System.Windows.Forms.DragEventHandler(this.gdvFolderMapping_DragOver);
            this.gdvFolderMapping.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gdvFolderMapping_MouseDown);
            this.gdvFolderMapping.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gdvFolderMapping_MouseMove);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "IsValid";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.NullValue = "Error";
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewImageColumn1.HeaderText = "Valid";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // chkExpandOnLoad
            // 
            this.chkExpandOnLoad.AutoSize = true;
            this.chkExpandOnLoad.Location = new System.Drawing.Point(46, 228);
            this.chkExpandOnLoad.Name = "chkExpandOnLoad";
            this.chkExpandOnLoad.Size = new System.Drawing.Size(231, 17);
            this.chkExpandOnLoad.TabIndex = 4;
            this.chkExpandOnLoad.Text = "Expand mapped folders after loading scripts";
            this.chkExpandOnLoad.UseVisualStyleBackColor = true;
            // 
            // lblFolderMapping
            // 
            this.lblFolderMapping.AutoSize = true;
            this.lblFolderMapping.Location = new System.Drawing.Point(18, 16);
            this.lblFolderMapping.Name = "lblFolderMapping";
            this.lblFolderMapping.Size = new System.Drawing.Size(80, 13);
            this.lblFolderMapping.TabIndex = 0;
            this.lblFolderMapping.Text = "Folder Mapping";
            // 
            // lblSeparator2
            // 
            this.lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator2.Location = new System.Drawing.Point(120, 275);
            this.lblSeparator2.Name = "lblSeparator2";
            this.lblSeparator2.Size = new System.Drawing.Size(470, 2);
            this.lblSeparator2.TabIndex = 6;
            // 
            // lblSeparator1
            // 
            this.lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator1.Location = new System.Drawing.Point(120, 24);
            this.lblSeparator1.Name = "lblSeparator1";
            this.lblSeparator1.Size = new System.Drawing.Size(470, 2);
            this.lblSeparator1.TabIndex = 1;
            // 
            // lblSQLParserVersion
            // 
            this.lblSQLParserVersion.AutoSize = true;
            this.lblSQLParserVersion.Location = new System.Drawing.Point(43, 297);
            this.lblSQLParserVersion.Name = "lblSQLParserVersion";
            this.lblSQLParserVersion.Size = new System.Drawing.Size(99, 13);
            this.lblSQLParserVersion.TabIndex = 7;
            this.lblSQLParserVersion.Text = "SQL Parser Version";
            // 
            // lblFileTypes
            // 
            this.lblFileTypes.AutoSize = true;
            this.lblFileTypes.Location = new System.Drawing.Point(43, 324);
            this.lblFileTypes.Name = "lblFileTypes";
            this.lblFileTypes.Size = new System.Drawing.Size(95, 13);
            this.lblFileTypes.TabIndex = 9;
            this.lblFileTypes.Text = "Allowed File Types";
            // 
            // txtAllowedFileTypes
            // 
            this.txtAllowedFileTypes.Enabled = false;
            this.txtAllowedFileTypes.Location = new System.Drawing.Point(148, 321);
            this.txtAllowedFileTypes.Name = "txtAllowedFileTypes";
            this.txtAllowedFileTypes.Size = new System.Drawing.Size(212, 20);
            this.txtAllowedFileTypes.TabIndex = 10;
            // 
            // Alias
            // 
            this.Alias.DataPropertyName = "Alias";
            this.Alias.HeaderText = "Alias";
            this.Alias.Name = "Alias";
            this.Alias.Width = 110;
            // 
            // FolderPath
            // 
            this.FolderPath.DataPropertyName = "FolderPath";
            this.FolderPath.HeaderText = "Folder Path";
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Width = 350;
            // 
            // Valid
            // 
            this.Valid.DataPropertyName = "IsValid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.NullValue = "Error";
            this.Valid.DefaultCellStyle = dataGridViewCellStyle6;
            this.Valid.HeaderText = "Valid";
            this.Valid.Name = "Valid";
            this.Valid.Width = 40;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 397);
            this.Controls.Add(this.txtAllowedFileTypes);
            this.Controls.Add(this.lblFileTypes);
            this.Controls.Add(this.lblSQLParserVersion);
            this.Controls.Add(this.lblSeparator1);
            this.Controls.Add(this.lblSeparator2);
            this.Controls.Add(this.lblFolderMapping);
            this.Controls.Add(this.chkExpandOnLoad);
            this.Controls.Add(this.gdvFolderMapping);
            this.Controls.Add(this.cboParserVersion);
            this.Controls.Add(this.lblGeneralSettings);
            this.Controls.Add(this.lblSQLScriptsFolder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Scripts Explorer Settings";
            ((System.ComponentModel.ISupportInitialize)(this.gdvFolderMapping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.ComboBox cboParserVersion;
        internal System.Windows.Forms.Label lblGeneralSettings;
        internal System.Windows.Forms.Label lblSQLScriptsFolder;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView gdvFolderMapping;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        internal System.Windows.Forms.CheckBox chkExpandOnLoad;
        internal System.Windows.Forms.Label lblFolderMapping;
        private System.Windows.Forms.Label lblSeparator2;
        private System.Windows.Forms.Label lblSeparator1;
        internal System.Windows.Forms.Label lblSQLParserVersion;
        internal System.Windows.Forms.Label lblFileTypes;
        private System.Windows.Forms.TextBox txtAllowedFileTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderPath;
        private System.Windows.Forms.DataGridViewImageColumn Valid;
    }
}
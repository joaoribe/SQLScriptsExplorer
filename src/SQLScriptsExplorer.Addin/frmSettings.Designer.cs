
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cboParserVersion = new System.Windows.Forms.ComboBox();
            this.lblGeneralSettings = new System.Windows.Forms.Label();
            this.lblSQLScriptsFolder = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.gdvFolderMapping = new System.Windows.Forms.DataGridView();
            this.Alias = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valid = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.chkExpandOnLoad = new System.Windows.Forms.CheckBox();
            this.lblSeparator2 = new System.Windows.Forms.Label();
            this.lblSQLParserVersion = new System.Windows.Forms.Label();
            this.lblFileTypes = new System.Windows.Forms.Label();
            this.txtAllowedFileTypes = new System.Windows.Forms.TextBox();
            this.lblScriptExecution = new System.Windows.Forms.Label();
            this.cboFileDoubleClick = new System.Windows.Forms.ComboBox();
            this.chkShowExecuteFileButton = new System.Windows.Forms.CheckBox();
            this.chkConfirmScriptExecution = new System.Windows.Forms.CheckBox();
            this.tbcMain = new System.Windows.Forms.TabControl();
            this.tbpFolderMapping = new System.Windows.Forms.TabPage();
            this.lblFolderMapping = new System.Windows.Forms.Label();
            this.lblSeparator1 = new System.Windows.Forms.Label();
            this.tbpFileExplorer = new System.Windows.Forms.TabPage();
            this.lblFileDoubleClick = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFileDoubleClickBehaviour = new System.Windows.Forms.Label();
            this.lblSeparator3 = new System.Windows.Forms.Label();
            this.tbpGeneral = new System.Windows.Forms.TabPage();
            this.cboTheme = new System.Windows.Forms.ComboBox();
            this.lblTheme = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gdvFolderMapping)).BeginInit();
            this.tbcMain.SuspendLayout();
            this.tbpFolderMapping.SuspendLayout();
            this.tbpFileExplorer.SuspendLayout();
            this.tbpGeneral.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboParserVersion
            // 
            this.cboParserVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParserVersion.FormattingEnabled = true;
            this.cboParserVersion.Items.AddRange(new object[] {
            "SQL Server 2025",
            "SQL Server 2022",
            "SQL Server 2019",
            "SQL Server 2017",
            "SQL Server 2016"});
            this.cboParserVersion.Location = new System.Drawing.Point(151, 42);
            this.cboParserVersion.Name = "cboParserVersion";
            this.cboParserVersion.Size = new System.Drawing.Size(212, 21);
            this.cboParserVersion.TabIndex = 3;
            // 
            // lblGeneralSettings
            // 
            this.lblGeneralSettings.AutoSize = true;
            this.lblGeneralSettings.Location = new System.Drawing.Point(21, 15);
            this.lblGeneralSettings.Name = "lblGeneralSettings";
            this.lblGeneralSettings.Size = new System.Drawing.Size(85, 13);
            this.lblGeneralSettings.TabIndex = 0;
            this.lblGeneralSettings.Text = "General Settings";
            // 
            // lblSQLScriptsFolder
            // 
            this.lblSQLScriptsFolder.AutoSize = true;
            this.lblSQLScriptsFolder.Location = new System.Drawing.Point(46, 45);
            this.lblSQLScriptsFolder.Name = "lblSQLScriptsFolder";
            this.lblSQLScriptsFolder.Size = new System.Drawing.Size(510, 13);
            this.lblSQLScriptsFolder.TabIndex = 2;
            this.lblSQLScriptsFolder.Text = "Map script folders by entering an Alias and its Folder Path. Drag and drop rows t" +
    "o order the mapped folders.";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(593, 370);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(512, 370);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
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
            this.gdvFolderMapping.Location = new System.Drawing.Point(49, 66);
            this.gdvFolderMapping.Name = "gdvFolderMapping";
            this.gdvFolderMapping.Size = new System.Drawing.Size(544, 199);
            this.gdvFolderMapping.TabIndex = 3;
            this.gdvFolderMapping.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gdvFolderMapping_CellEndEdit);
            this.gdvFolderMapping.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gdvFolderMapping_CellFormatting);
            this.gdvFolderMapping.DragDrop += new System.Windows.Forms.DragEventHandler(this.gdvFolderMapping_DragDrop);
            this.gdvFolderMapping.DragOver += new System.Windows.Forms.DragEventHandler(this.gdvFolderMapping_DragOver);
            this.gdvFolderMapping.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gdvFolderMapping_MouseDown);
            this.gdvFolderMapping.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gdvFolderMapping_MouseMove);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = "Error";
            this.Valid.DefaultCellStyle = dataGridViewCellStyle1;
            this.Valid.HeaderText = "Valid";
            this.Valid.Name = "Valid";
            this.Valid.Width = 40;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "IsValid";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.NullValue = "Error";
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewImageColumn1.HeaderText = "Valid";
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 40;
            // 
            // chkExpandOnLoad
            // 
            this.chkExpandOnLoad.AutoSize = true;
            this.chkExpandOnLoad.Location = new System.Drawing.Point(49, 271);
            this.chkExpandOnLoad.Name = "chkExpandOnLoad";
            this.chkExpandOnLoad.Size = new System.Drawing.Size(198, 17);
            this.chkExpandOnLoad.TabIndex = 4;
            this.chkExpandOnLoad.Text = "Expand mapped folders after loading";
            this.chkExpandOnLoad.UseVisualStyleBackColor = true;
            // 
            // lblSeparator2
            // 
            this.lblSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator2.Location = new System.Drawing.Point(123, 23);
            this.lblSeparator2.Name = "lblSeparator2";
            this.lblSeparator2.Size = new System.Drawing.Size(470, 2);
            this.lblSeparator2.TabIndex = 1;
            // 
            // lblSQLParserVersion
            // 
            this.lblSQLParserVersion.AutoSize = true;
            this.lblSQLParserVersion.Location = new System.Drawing.Point(46, 45);
            this.lblSQLParserVersion.Name = "lblSQLParserVersion";
            this.lblSQLParserVersion.Size = new System.Drawing.Size(99, 13);
            this.lblSQLParserVersion.TabIndex = 2;
            this.lblSQLParserVersion.Text = "SQL Parser Version";
            // 
            // lblFileTypes
            // 
            this.lblFileTypes.AutoSize = true;
            this.lblFileTypes.Location = new System.Drawing.Point(46, 72);
            this.lblFileTypes.Name = "lblFileTypes";
            this.lblFileTypes.Size = new System.Drawing.Size(95, 13);
            this.lblFileTypes.TabIndex = 4;
            this.lblFileTypes.Text = "Allowed File Types";
            // 
            // txtAllowedFileTypes
            // 
            this.txtAllowedFileTypes.Enabled = false;
            this.txtAllowedFileTypes.Location = new System.Drawing.Point(151, 69);
            this.txtAllowedFileTypes.Name = "txtAllowedFileTypes";
            this.txtAllowedFileTypes.Size = new System.Drawing.Size(212, 20);
            this.txtAllowedFileTypes.TabIndex = 5;
            // 
            // lblScriptExecution
            // 
            this.lblScriptExecution.AutoSize = true;
            this.lblScriptExecution.Location = new System.Drawing.Point(46, 45);
            this.lblScriptExecution.Name = "lblScriptExecution";
            this.lblScriptExecution.Size = new System.Drawing.Size(92, 13);
            this.lblScriptExecution.TabIndex = 4;
            this.lblScriptExecution.Text = "Default Behaviour";
            // 
            // cboFileDoubleClick
            // 
            this.cboFileDoubleClick.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileDoubleClick.FormattingEnabled = true;
            this.cboFileDoubleClick.Items.AddRange(new object[] {
            "Open New Instance",
            "Edit"});
            this.cboFileDoubleClick.Location = new System.Drawing.Point(151, 42);
            this.cboFileDoubleClick.Name = "cboFileDoubleClick";
            this.cboFileDoubleClick.Size = new System.Drawing.Size(212, 21);
            this.cboFileDoubleClick.TabIndex = 5;
            // 
            // chkShowExecuteFileButton
            // 
            this.chkShowExecuteFileButton.AutoSize = true;
            this.chkShowExecuteFileButton.Location = new System.Drawing.Point(49, 105);
            this.chkShowExecuteFileButton.Name = "chkShowExecuteFileButton";
            this.chkShowExecuteFileButton.Size = new System.Drawing.Size(259, 17);
            this.chkShowExecuteFileButton.TabIndex = 2;
            this.chkShowExecuteFileButton.Text = "Show \'Execute\' menu option in File Context Menu";
            this.chkShowExecuteFileButton.UseVisualStyleBackColor = true;
            this.chkShowExecuteFileButton.CheckedChanged += new System.EventHandler(this.chkShowExecuteFileButton_CheckedChanged);
            // 
            // chkConfirmScriptExecution
            // 
            this.chkConfirmScriptExecution.AutoSize = true;
            this.chkConfirmScriptExecution.Location = new System.Drawing.Point(49, 128);
            this.chkConfirmScriptExecution.Name = "chkConfirmScriptExecution";
            this.chkConfirmScriptExecution.Size = new System.Drawing.Size(257, 17);
            this.chkConfirmScriptExecution.TabIndex = 3;
            this.chkConfirmScriptExecution.Text = "Display confirmation dialog when executing script";
            this.chkConfirmScriptExecution.UseVisualStyleBackColor = true;
            // 
            // tbcMain
            // 
            this.tbcMain.Controls.Add(this.tbpFolderMapping);
            this.tbcMain.Controls.Add(this.tbpFileExplorer);
            this.tbcMain.Controls.Add(this.tbpGeneral);
            this.tbcMain.Location = new System.Drawing.Point(16, 17);
            this.tbcMain.Name = "tbcMain";
            this.tbcMain.SelectedIndex = 0;
            this.tbcMain.Size = new System.Drawing.Size(656, 336);
            this.tbcMain.TabIndex = 0;
            // 
            // tbpFolderMapping
            // 
            this.tbpFolderMapping.Controls.Add(this.gdvFolderMapping);
            this.tbpFolderMapping.Controls.Add(this.lblSQLScriptsFolder);
            this.tbpFolderMapping.Controls.Add(this.chkExpandOnLoad);
            this.tbpFolderMapping.Controls.Add(this.lblFolderMapping);
            this.tbpFolderMapping.Controls.Add(this.lblSeparator1);
            this.tbpFolderMapping.Location = new System.Drawing.Point(4, 22);
            this.tbpFolderMapping.Name = "tbpFolderMapping";
            this.tbpFolderMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tbpFolderMapping.Size = new System.Drawing.Size(648, 310);
            this.tbpFolderMapping.TabIndex = 0;
            this.tbpFolderMapping.Text = "Folder Mapping";
            this.tbpFolderMapping.UseVisualStyleBackColor = true;
            // 
            // lblFolderMapping
            // 
            this.lblFolderMapping.AutoSize = true;
            this.lblFolderMapping.Location = new System.Drawing.Point(21, 15);
            this.lblFolderMapping.Name = "lblFolderMapping";
            this.lblFolderMapping.Size = new System.Drawing.Size(80, 13);
            this.lblFolderMapping.TabIndex = 0;
            this.lblFolderMapping.Text = "Folder Mapping";
            // 
            // lblSeparator1
            // 
            this.lblSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator1.Location = new System.Drawing.Point(123, 23);
            this.lblSeparator1.Name = "lblSeparator1";
            this.lblSeparator1.Size = new System.Drawing.Size(470, 2);
            this.lblSeparator1.TabIndex = 1;
            // 
            // tbpFileExplorer
            // 
            this.tbpFileExplorer.Controls.Add(this.lblFileDoubleClick);
            this.tbpFileExplorer.Controls.Add(this.label2);
            this.tbpFileExplorer.Controls.Add(this.lblFileDoubleClickBehaviour);
            this.tbpFileExplorer.Controls.Add(this.chkConfirmScriptExecution);
            this.tbpFileExplorer.Controls.Add(this.lblSeparator3);
            this.tbpFileExplorer.Controls.Add(this.chkShowExecuteFileButton);
            this.tbpFileExplorer.Controls.Add(this.cboFileDoubleClick);
            this.tbpFileExplorer.Controls.Add(this.lblScriptExecution);
            this.tbpFileExplorer.Location = new System.Drawing.Point(4, 22);
            this.tbpFileExplorer.Name = "tbpFileExplorer";
            this.tbpFileExplorer.Padding = new System.Windows.Forms.Padding(3);
            this.tbpFileExplorer.Size = new System.Drawing.Size(648, 310);
            this.tbpFileExplorer.TabIndex = 1;
            this.tbpFileExplorer.Text = "File Explorer";
            this.tbpFileExplorer.UseVisualStyleBackColor = true;
            // 
            // lblFileDoubleClick
            // 
            this.lblFileDoubleClick.AutoSize = true;
            this.lblFileDoubleClick.Location = new System.Drawing.Point(21, 78);
            this.lblFileDoubleClick.Name = "lblFileDoubleClick";
            this.lblFileDoubleClick.Size = new System.Drawing.Size(84, 13);
            this.lblFileDoubleClick.TabIndex = 6;
            this.lblFileDoubleClick.Text = "Script Execution";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(143, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(450, 2);
            this.label2.TabIndex = 7;
            // 
            // lblFileDoubleClickBehaviour
            // 
            this.lblFileDoubleClickBehaviour.AutoSize = true;
            this.lblFileDoubleClickBehaviour.Location = new System.Drawing.Point(21, 15);
            this.lblFileDoubleClickBehaviour.Name = "lblFileDoubleClickBehaviour";
            this.lblFileDoubleClickBehaviour.Size = new System.Drawing.Size(116, 13);
            this.lblFileDoubleClickBehaviour.TabIndex = 0;
            this.lblFileDoubleClickBehaviour.Text = "Script File Double-Click";
            // 
            // lblSeparator3
            // 
            this.lblSeparator3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSeparator3.Location = new System.Drawing.Point(143, 23);
            this.lblSeparator3.Name = "lblSeparator3";
            this.lblSeparator3.Size = new System.Drawing.Size(450, 2);
            this.lblSeparator3.TabIndex = 1;
            // 
            // tbpGeneral
            // 
            this.tbpGeneral.Controls.Add(this.cboTheme);
            this.tbpGeneral.Controls.Add(this.lblTheme);
            this.tbpGeneral.Controls.Add(this.lblGeneralSettings);
            this.tbpGeneral.Controls.Add(this.txtAllowedFileTypes);
            this.tbpGeneral.Controls.Add(this.cboParserVersion);
            this.tbpGeneral.Controls.Add(this.lblFileTypes);
            this.tbpGeneral.Controls.Add(this.lblSeparator2);
            this.tbpGeneral.Controls.Add(this.lblSQLParserVersion);
            this.tbpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tbpGeneral.Name = "tbpGeneral";
            this.tbpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tbpGeneral.Size = new System.Drawing.Size(648, 310);
            this.tbpGeneral.TabIndex = 2;
            this.tbpGeneral.Text = "General";
            this.tbpGeneral.UseVisualStyleBackColor = true;
            // 
            // cboTheme
            // 
            this.cboTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTheme.FormattingEnabled = true;
            this.cboTheme.Items.AddRange(new object[] {
            "Light",
            "Dark"});
            this.cboTheme.Location = new System.Drawing.Point(151, 97);
            this.cboTheme.Name = "cboTheme";
            this.cboTheme.Size = new System.Drawing.Size(212, 21);
            this.cboTheme.TabIndex = 7;
            // 
            // lblTheme
            // 
            this.lblTheme.AutoSize = true;
            this.lblTheme.Location = new System.Drawing.Point(46, 100);
            this.lblTheme.Name = "lblTheme";
            this.lblTheme.Size = new System.Drawing.Size(40, 13);
            this.lblTheme.TabIndex = 6;
            this.lblTheme.Text = "Theme";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 412);
            this.Controls.Add(this.tbcMain);
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
            this.tbcMain.ResumeLayout(false);
            this.tbpFolderMapping.ResumeLayout(false);
            this.tbpFolderMapping.PerformLayout();
            this.tbpFileExplorer.ResumeLayout(false);
            this.tbpFileExplorer.PerformLayout();
            this.tbpGeneral.ResumeLayout(false);
            this.tbpGeneral.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Label lblSeparator2;
        internal System.Windows.Forms.Label lblSQLParserVersion;
        internal System.Windows.Forms.Label lblFileTypes;
        private System.Windows.Forms.TextBox txtAllowedFileTypes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Alias;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderPath;
        private System.Windows.Forms.DataGridViewImageColumn Valid;
        internal System.Windows.Forms.Label lblScriptExecution;
        internal System.Windows.Forms.ComboBox cboFileDoubleClick;
        private System.Windows.Forms.CheckBox chkShowExecuteFileButton;
        private System.Windows.Forms.CheckBox chkConfirmScriptExecution;
        private System.Windows.Forms.TabControl tbcMain;
        private System.Windows.Forms.TabPage tbpFolderMapping;
        private System.Windows.Forms.TabPage tbpFileExplorer;
        private System.Windows.Forms.TabPage tbpGeneral;
        internal System.Windows.Forms.Label lblFolderMapping;
        private System.Windows.Forms.Label lblSeparator1;
        internal System.Windows.Forms.Label lblFileDoubleClickBehaviour;
        private System.Windows.Forms.Label lblSeparator3;
        internal System.Windows.Forms.Label lblFileDoubleClick;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.ComboBox cboTheme;
        internal System.Windows.Forms.Label lblTheme;
    }
}
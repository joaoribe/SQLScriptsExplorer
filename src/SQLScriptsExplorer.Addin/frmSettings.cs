using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Repository;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SQLScriptsExplorer.Addin
{
    public partial class frmSettings : Form
    {
        private ISettingsRepository settingsRepository = null;
        private BindingList<FolderMapping> folderMappingBindingList = null;
        private string jsonFolderMappingPrevious = null;

        public frmSettings()
        {
            InitializeComponent();

            settingsRepository = new SettingsRepository();

            // Folder Mapping
            jsonFolderMappingPrevious = Newtonsoft.Json.JsonConvert.SerializeObject(settingsRepository.FolderMapping);
            folderMappingBindingList = new BindingList<FolderMapping>(settingsRepository.FolderMapping);

            gdvFolderMapping.AutoGenerateColumns = false;
            gdvFolderMapping.DataSource = folderMappingBindingList;

            chkExpandOnLoad.Checked = settingsRepository.ExpandMappedFoldersOnLoad;

            // User Interface
            chkShowExecuteFileButton.Checked = settingsRepository.ShowExecuteFileButton;
            chkConfirmScriptExecution.Checked = settingsRepository.ConfirmScriptExecution;

            chkShowExecuteFileButton_CheckedChanged(null, System.EventArgs.Empty);

            // General
            cboParserVersion.SelectedItem = settingsRepository.SQLParserVersion;
            txtAllowedFileTypes.Text = settingsRepository.AllowedFileTypes;
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            var jsonFolderMappingCurrent = Newtonsoft.Json.JsonConvert.SerializeObject(folderMappingBindingList.ToList());

            // Folder Mapping
            settingsRepository.FolderMapping = folderMappingBindingList.ToList();
            settingsRepository.ExpandMappedFoldersOnLoad = chkExpandOnLoad.Checked;

            // User Interface
            settingsRepository.ShowExecuteFileButton = chkShowExecuteFileButton.Checked;
            settingsRepository.ConfirmScriptExecution = chkConfirmScriptExecution.Checked;

            // General
            settingsRepository.SQLParserVersion = cboParserVersion.SelectedItem.ToString();
            settingsRepository.AllowedFileTypes = txtAllowedFileTypes.Text;

            settingsRepository.Save();

            // Refresh Explorer if mapping has changed
            if (jsonFolderMappingPrevious != jsonFolderMappingCurrent)
                DialogResult = DialogResult.OK;

            Close();
        }

        private void chkShowExecuteFileButton_CheckedChanged(object sender, System.EventArgs e)
        {
            chkConfirmScriptExecution.Enabled = chkShowExecuteFileButton.Checked;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                btnCancel_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region UI Behaviour

        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private void gdvFolderMapping_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = gdvFolderMapping.DoDragDrop(
                    gdvFolderMapping.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void gdvFolderMapping_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = gdvFolderMapping.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1 && rowIndexFromMouseDown != gdvFolderMapping.RowCount - 1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void gdvFolderMapping_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void gdvFolderMapping_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = gdvFolderMapping.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                gdvFolderMapping.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;

                if (rowIndexOfItemUnderMouseToDrop == gdvFolderMapping.RowCount - 1)
                    rowIndexOfItemUnderMouseToDrop--;

                if (rowIndexOfItemUnderMouseToDrop == -1)
                    rowIndexOfItemUnderMouseToDrop++;

                FolderMapping item = folderMappingBindingList[rowIndexFromMouseDown];
                folderMappingBindingList.RemoveAt(rowIndexFromMouseDown);
                folderMappingBindingList.Insert(rowIndexOfItemUnderMouseToDrop, item);
            }
        }

        private void gdvFolderMapping_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Force Valid icon to be refreshed
            gdvFolderMapping.Refresh();
        }

        private void gdvFolderMapping_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Prevent Empty Cell from having a red X box
            if (gdvFolderMapping.Rows[e.RowIndex].IsNewRow && e.ColumnIndex == 2)
            {
                e.Value = new Bitmap(1, 1);
            }
        }

        #endregion
    }
}

using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Infrastructure.Extensions;
using SQLScriptsExplorer.Addin.Infrastructure.Helpers;
using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Models.Enums;
using SQLScriptsExplorer.Addin.Repository;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SQLScriptsExplorer.Addin.Controls
{
    /// <summary>
    /// Interaction logic for FileExplorerTreeView.xaml
    /// </summary>
    public partial class FileExplorerTreeView : UserControl
    {
        public List<TreeNode> DataSourceList = new List<TreeNode>();
        public Dictionary<string, TreeNode> DataSourceDictionary = new Dictionary<string, TreeNode>();

        public event EventHandler TreeNodeAdded;
        public event EventHandler TreeNodeDeleted;
        public event EventHandler TreeNodeRenamed;

        private bool isEditMode = false;
        private string renamingNodeId;
        private string renamingNodeFileName;
        private TextBlock lblRename;
        private TextBox txtRename;

        private TreeNode currentTreeNode;
        private TreeViewItem currentTreeViewItem;

        private TreeNode potentialCurrentTreeNode;
        private TreeViewItem potentialCurrentTreeViewItem;

        public string Filter { get; set; }

        // ----------------------------------------------------------
        // DRAG-AND-DROP: Fields to handle drag logic & mouse offset
        // ----------------------------------------------------------
        private TreeNode _draggedItem = null;
        private Point _mouseDownPosition;

        public FileExplorerTreeView()
        {
            InitializeComponent();
        }

        #region DRAG-AND-DROP Implementation

        /// <summary>
        /// Fired when user first presses left mouse button on TreeView
        /// </summary>
        private void TreeViewMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Identify which TreeViewItem was clicked
            potentialCurrentTreeViewItem = VisualUpwardSearchTreeViewItem(e.OriginalSource as DependencyObject);
            potentialCurrentTreeNode = potentialCurrentTreeViewItem != null
                ? potentialCurrentTreeViewItem.DataContext as TreeNode
                : null;

            // If we were in the middle of a rename, close that out
            if (currentTreeNode != null && isEditMode && currentTreeNode.Id != renamingNodeId)
            {
                txtRename_LostFocus(null, null);
            }

            // Store which TreeNode might be dragged
            _draggedItem = potentialCurrentTreeNode;

            // Record the mouse-down position for drag threshold checks
            _mouseDownPosition = e.GetPosition(this);
        }

        /// <summary>
        /// Fired when user moves the mouse while holding left button
        /// </summary>
        private void TreeViewMain_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            // Only run drag logic if left button is pressed AND we have a valid item
            if (e.LeftButton == MouseButtonState.Pressed && _draggedItem != null)
            {
                // Check how far the mouse has moved
                Point currentPosition = e.GetPosition(this);

                // If movement is less than drag threshold, do not start drag
                if (Math.Abs(currentPosition.X - _mouseDownPosition.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - _mouseDownPosition.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    // Use a typed DataObject to avoid SSMS misinterpreting the data
                    var dataObject = new DataObject(typeof(TreeNode), _draggedItem);

                    DragDrop.DoDragDrop(TreeViewMain, dataObject, DragDropEffects.Move);
                }
            }
        }

        /// <summary>
        /// Fired repeatedly while dragging over the TreeView
        /// </summary>
        private void TreeViewMain_DragOver(object sender, DragEventArgs e)
        {
            // Only proceed if the data is our known type
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }

            e.Effects = DragDropEffects.Move;
            e.Handled = true;
        }

        /// <summary>
        /// Fired when user releases mouse button to drop
        /// 
        /// Includes code to physically move the file/folder on disk.
        /// </summary>
        private void TreeViewMain_Drop(object sender, DragEventArgs e)
        {
            e.Handled = true;

            // 1) Check if the data is our known type
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
            {
                return;
            }

            var droppedNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            if (droppedNode == null) return;

            // 2) Identify the target node by the mouse pointer position
            var dropPosition = e.GetPosition(TreeViewMain);
            var targetItem = TreeViewMain.InputHitTest(dropPosition) as DependencyObject;
            var targetTreeViewItem = VisualUpwardSearchTreeViewItem(targetItem);

            if (targetTreeViewItem == null)
            {
                _draggedItem = null;
                return;
            }

            var targetNode = targetTreeViewItem.DataContext as TreeNode;
            if (targetNode == null) return;

            // 3) Prevent moving a node into its own descendant
            if (IsDescendant(droppedNode, targetNode))
            {
                MessageBox.Show("Cannot move a folder/file into its own child folder.");
                _draggedItem = null;
                return;
            }

            // --------------------------------------------------
            // PHYSICAL MOVE on disk (file or folder)
            // --------------------------------------------------
            string oldPath = droppedNode.FileFullPath;
            string newPath = Path.Combine(targetNode.FileFullPath, droppedNode.FileName);

            try
            {
                // If droppedNode is a folder
                if (droppedNode.Type == TreeNodeType.Folder || droppedNode.Type == TreeNodeType.RootFolder)
                {
                    if (Directory.Exists(oldPath))
                    {
                        // Check collision
                        if (Directory.Exists(newPath) || File.Exists(newPath))
                        {
                            MessageBox.Show($"A folder or file named '{droppedNode.FileName}' already exists in '{targetNode.FileName}'.");
                            return;
                        }
                        Directory.Move(oldPath, newPath);
                    }
                }
                // If droppedNode is a file
                else if (droppedNode.Type == TreeNodeType.File)
                {
                    if (File.Exists(oldPath))
                    {
                        // Check collision
                        if (Directory.Exists(newPath) || File.Exists(newPath))
                        {
                            MessageBox.Show($"A folder or file named '{droppedNode.FileName}' already exists in '{targetNode.FileName}'.");
                            return;
                        }
                        File.Move(oldPath, newPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error moving '{droppedNode.FileName}': {ex.Message}");
                return;
            }

            // 4) Remove from old parent in the in-memory tree
            if (droppedNode.Parent != null)
            {
                droppedNode.Parent.Children.Remove(droppedNode);
            }

            // 5) Add to new parent
            targetNode.Children.Add(droppedNode);
            droppedNode.Parent = targetNode;

            // 6) Update the node’s FileFullPath to its new location
            droppedNode.FileFullPath = newPath;

            // 7) Sort children of the new parent
            targetNode.Children = targetNode.Children
                .OrderBy(p => p.Type)
                .ThenBy(p => p.FileName)
                .ToList();

            // 8) Update dictionary references if needed
            if (!DataSourceDictionary.ContainsKey(droppedNode.Id))
            {
                DataSourceDictionary.Add(droppedNode.Id, droppedNode);
            }

            // 9) Refresh the UI
            RefreshTreeView();

            // 10) Clear the drag reference
            _draggedItem = null;
        }

        /// <summary>
        /// Checks if 'target' is a descendant of 'node'
        /// </summary>
        private bool IsDescendant(TreeNode node, TreeNode target)
        {
            if (node == target) return true;
            foreach (var child in node.Children)
            {
                if (IsDescendant(child, target))
                    return true;
            }
            return false;
        }

        #endregion

        #region Core Methods

        public void RefreshTreeView()
        {
            TreeViewMain.ItemsSource = null;
            TreeViewMain.ItemsSource = DataSourceList;
        }

        #endregion

        #region Existing Mouse/Selection Handlers

        private void TreeViewMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var doubleClickedTreeViewItem = VisualUpwardSearchTreeViewItem(e.OriginalSource as DependencyObject);
                var doubleClickedTreeNode = doubleClickedTreeViewItem?.DataContext as TreeNode;

                if (doubleClickedTreeNode != null
                    && doubleClickedTreeNode.Type == TreeNodeType.File
                    && currentTreeNode != null
                    && currentTreeNode.Id.Equals(doubleClickedTreeNode.Id))
                {
                    ISettingsRepository settingsRepository = new SettingsRepository();

                    if (settingsRepository.ScriptFileDoubleClickBehaviour == ScriptFileDoubleClickBehaviour.OpenNewInstance)
                        mnuOpenNewInstance_Click(sender, null);
                    else
                        mnuEditFile_Click(sender, null);
                }
            }

            e.Handled = true;
        }

        private void TreeViewMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as System.Windows.Controls.TreeView;
            var selectedTreeNode = treeView.SelectedItem as TreeNode;

            if (selectedTreeNode != null)
            {
                if (potentialCurrentTreeNode != null && selectedTreeNode.Id.Equals(potentialCurrentTreeNode.Id))
                {
                    currentTreeNode = potentialCurrentTreeNode;
                    currentTreeViewItem = potentialCurrentTreeViewItem;
                    potentialCurrentTreeNode = null;
                    potentialCurrentTreeViewItem = null;
                }

                switch (selectedTreeNode.Type)
                {
                    case TreeNodeType.File:
                        treeView.ContextMenu = treeView.Resources["FileContext"] as System.Windows.Controls.ContextMenu;
                        break;
                    case TreeNodeType.Folder:
                        treeView.ContextMenu = treeView.Resources["FolderContext"] as System.Windows.Controls.ContextMenu;
                        break;
                    case TreeNodeType.RootFolder:
                        treeView.ContextMenu = treeView.Resources["RootFolderContext"] as System.Windows.Controls.ContextMenu;
                        break;
                }
            }
        }

        private void TreeViewMain_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (currentTreeNode?.Type == TreeNodeType.File)
            {
                ISettingsRepository settingsRepository = new SettingsRepository();
                var fileContextMenu = TreeViewMain.Resources["FileContext"] as System.Windows.Controls.ContextMenu;

                if (fileContextMenu != null)
                {
                    var executeMenuItem = fileContextMenu.Items[2] as MenuItem;
                    if (executeMenuItem != null)
                    {
                        executeMenuItem.Visibility =
                            settingsRepository.ShowExecuteFileButton ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
        }

        private void TreeViewMain_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentTreeViewItem = VisualUpwardSearchTreeViewItem(e.OriginalSource as DependencyObject);
            currentTreeNode = currentTreeViewItem != null
                ? currentTreeViewItem.DataContext as TreeNode
                : null;

            if (currentTreeViewItem != null)
            {
                currentTreeViewItem.Focus();
                e.Handled = true;
            }
        }

        #endregion

        #region Menu Actions (New/Open/Edit/Delete/Rename, etc.)

        private void mnuOpenNewInstance_Click(object sender, RoutedEventArgs e)
        {
            var treeNode = TreeViewMain.SelectedItem as TreeNode;
            if (treeNode != null && treeNode.Type == TreeNodeType.File && !isEditMode)
            {
                try
                {
                    DocumentManager.OpenTemplate(treeNode.FileName, treeNode.FileFullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void mnuOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var treeNode = TreeViewMain.SelectedItem as TreeNode;
            if (treeNode != null)
            {
                if (treeNode.Type == TreeNodeType.Folder || treeNode.Type == TreeNodeType.RootFolder)
                {
                    System.Diagnostics.Process.Start("explorer.exe", treeNode.FileFullPath);
                }
                else if (treeNode.Type == TreeNodeType.File)
                {
                    var directoryPath = Path.GetDirectoryName(treeNode.FileFullPath);
                    System.Diagnostics.Process.Start("explorer.exe", directoryPath);
                }
            }
        }

        private void mnuEditFile_Click(object sender, RoutedEventArgs e)
        {
            var treeNode = TreeViewMain.SelectedItem as TreeNode;
            if (treeNode != null && treeNode.Type == TreeNodeType.File)
            {
                try
                {
                    DocumentManager.EditTemplate(treeNode.FileName, treeNode.FileFullPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void mnuExecuteFile_Click(object sender, RoutedEventArgs e)
        {
            var treeNode = TreeViewMain.SelectedItem as TreeNode;
            if (treeNode != null && treeNode.Type == TreeNodeType.File && !isEditMode)
            {
                try
                {
                    ISettingsRepository settingsRepository = new SettingsRepository();

                    DocumentManager.OpenTemplate(treeNode.FileName, treeNode.FileFullPath);
                    var messageBoxResult = MessageBoxResult.Yes;

                    if (settingsRepository.ConfirmScriptExecution)
                    {
                        messageBoxResult = MessageBox.Show(
                            $"Are sure you want to execute the script '{treeNode.FileName}'?",
                            "Confirmation",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.None,
                            MessageBoxResult.No);
                    }

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        DocumentManager.ExecuteTemplate(
                            treeNode.FileName,
                            treeNode.FileFullPath,
                            settingsRepository.ConfirmScriptExecution);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void mnuRename_Click(object sender, RoutedEventArgs e)
        {
            lblRename = TreeViewItemExtensions.FindVisualChild<TextBlock>(currentTreeViewItem as DependencyObject);
            txtRename = TreeViewItemExtensions.FindVisualChild<TextBox>(currentTreeViewItem as DependencyObject);

            lblRename.Visibility = Visibility.Collapsed;
            txtRename.Visibility = Visibility.Visible;
            txtRename.Focus();

            isEditMode = true;

            renamingNodeId = currentTreeNode.Id;
            renamingNodeFileName = currentTreeNode.FileName;
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            var treeViewMainTreeNode = TreeViewMain.SelectedItem as TreeNode;
            if (treeViewMainTreeNode == null) return;

            var treeNode = DataSourceDictionary[treeViewMainTreeNode.Id];
            if (treeNode == null) return;

            var confirmationMessage =
                $"Are you sure you want to delete this {treeNode.Type.ToString().ToLowerInvariant()}?";
            var messageBoxImage = MessageBoxImage.Question;

            if (PathHelper.IsNetworkPath(treeNode.FileFullPath))
            {
                confirmationMessage =
                    $"WARNING: The {treeNode.Type.ToString().ToLowerInvariant()} you are attempting to delete belongs to a shared folder in the network. " +
                    $"This file might be deleted permanently and other users won't be able to access it.\n\n" +
                    $"{confirmationMessage}";

                messageBoxImage = MessageBoxImage.Warning;
            }

            if (MessageBox.Show(
                confirmationMessage,
                "Confirmation",
                MessageBoxButton.YesNo,
                messageBoxImage,
                MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                try
                {
                    // Remove folder or file
                    if (treeNode.Type == TreeNodeType.Folder && Directory.Exists(treeNode.FileFullPath))
                    {
                        Directory.Delete(treeNode.FileFullPath, true);
                    }
                    if (treeNode.Type == TreeNodeType.File && File.Exists(treeNode.FileFullPath))
                    {
                        File.Delete(treeNode.FileFullPath);
                    }

                    // Remove from DataSource
                    var parentTreeNode = treeNode.Parent;
                    if (parentTreeNode != null)
                    {
                        parentTreeNode.Children.Remove(treeNode);
                    }

                    DataSourceDictionary.Remove(treeNode.Id);
                    RefreshTreeView();

                    if (TreeNodeDeleted != null)
                    {
                        TreeNodeDeleted(treeNode, EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Couldn't delete {treeNode.Type.ToString().ToLowerInvariant()}. Error message: {ex.Message}");
                }
            }
        }

        private void mnuNewFile_Click(object sender, RoutedEventArgs e)
        {
            var newFileName = PathHelper.GetNextNewFileNameAvailable(currentTreeNode.FileFullPath);
            var newFilePath = Path.Combine(currentTreeNode.FileFullPath, newFileName);

            if (!File.Exists(newFilePath))
            {
                File.Create(newFilePath).Dispose(); // ensure file handle is closed

                var newTreeNode = new TreeNode(newFileName, newFilePath, TreeNodeType.File)
                {
                    Parent = currentTreeNode,
                    IsSelected = true
                };

                currentTreeNode.Children.Add(newTreeNode);
                currentTreeNode.Children = currentTreeNode.Children
                    .OrderBy(p => p.Type)
                    .ThenBy(p => p.FileName)
                    .ToList();
                currentTreeNode.IsExpanded = true;

                DataSourceDictionary.Add(newTreeNode.Id, newTreeNode);

                RefreshTreeView();

                if (TreeNodeAdded != null)
                {
                    TreeNodeAdded(newTreeNode, EventArgs.Empty);
                }
            }
        }

        private void mnuNewFolder_Click(object sender, RoutedEventArgs e)
        {
            var newFolderName = PathHelper.GetNextNewFolderNameAvailable(currentTreeNode.FileFullPath);
            var newFolderPath = Path.Combine(currentTreeNode.FileFullPath, newFolderName);

            if (!Directory.Exists(newFolderPath))
            {
                Directory.CreateDirectory(newFolderPath);

                var newTreeNode = new TreeNode(newFolderName, newFolderPath, TreeNodeType.Folder)
                {
                    Parent = currentTreeNode,
                    IsSelected = true
                };

                currentTreeNode.Children.Add(newTreeNode);
                currentTreeNode.Children = currentTreeNode.Children
                    .OrderBy(p => p.Type)
                    .ThenBy(p => p.FileName)
                    .ToList();
                currentTreeNode.IsExpanded = true;

                DataSourceDictionary.Add(newTreeNode.Id, newTreeNode);
                RefreshTreeView();

                if (TreeNodeAdded != null)
                {
                    TreeNodeAdded(newTreeNode, EventArgs.Empty);
                }
            }
        }

        #endregion

        #region Rename Logic

        private void TreeViewMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
                mnuRename_Click(e, null);
            else if (e.Key == Key.Delete)
                mnuDelete_Click(e, null);
        }

        private void txtRename_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtRename_LostFocus(sender, null);
            else if (e.Key == Key.Escape)
            {
                txtRename.Text = lblRename.Text = renamingNodeFileName;
                txtRename_LostFocus(sender, null);
            }
        }

        private void txtRename_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                bool hasRenamed = false;

                if (lblRename != null && txtRename != null)
                {
                    txtRename.Text = txtRename.Text.Trim();

                    if (isEditMode)
                    {
                        if (string.IsNullOrWhiteSpace(txtRename.Text))
                        {
                            // If user empties the file name
                            if (currentTreeViewItem != null &&
                                !currentTreeViewItem.DataContext.ToString().Equals("{DisconnectedItem}"))
                            {
                                MessageBox.Show("Please enter a file name.");
                                txtRename.Text = lblRename.Text = renamingNodeFileName;
                                return;
                            }
                        }
                        else if (currentTreeViewItem != null && renamingNodeFileName != txtRename.Text)
                        {
                            var renamingTreeNode = DataSourceDictionary[renamingNodeId];
                            var basePath = Path.GetDirectoryName(renamingTreeNode.FileFullPath);
                            var previousPath = Path.Combine(basePath, renamingNodeFileName);
                            var newPath = Path.Combine(basePath, txtRename.Text);

                            if (renamingTreeNode.Type == TreeNodeType.File && File.Exists(previousPath))
                            {
                                File.Move(previousPath, newPath);
                                hasRenamed = true;
                            }
                            else if (renamingTreeNode.Type == TreeNodeType.Folder && Directory.Exists(previousPath))
                            {
                                // Handle case-only renaming
                                var directory = new DirectoryInfo(previousPath);
                                directory.MoveTo(PathHelper.GetDirectoryTemporaryPath(previousPath));
                                directory.MoveTo(newPath);
                                hasRenamed = true;
                            }

                            if (hasRenamed)
                            {
                                renamingTreeNode.FileName = lblRename.Text = txtRename.Text;
                                renamingTreeNode.FileFullPath = newPath;
                                renamingTreeNode.Parent.Children = renamingTreeNode.Parent.Children
                                    .OrderBy(p => p.Type)
                                    .ThenBy(p => p.FileName)
                                    .ToList();

                                if (renamingNodeId == currentTreeNode.Id)
                                {
                                    currentTreeViewItem.IsSelected = true;
                                }

                                if (TreeNodeRenamed != null)
                                {
                                    TreeNodeRenamed(renamingTreeNode, EventArgs.Empty);
                                }
                            }
                        }
                    }

                    isEditMode = false;

                    if (hasRenamed)
                        RefreshTreeView();
                    else
                        txtRename.Text = lblRename.Text = renamingNodeFileName;

                    lblRename.Visibility = Visibility.Visible;
                    txtRename.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Utility

        /// <summary>
        /// Returns the TreeViewItem for the specified dependency object.
        /// </summary>
        private static TreeViewItem VisualUpwardSearchTreeViewItem(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

        #endregion
    }
}

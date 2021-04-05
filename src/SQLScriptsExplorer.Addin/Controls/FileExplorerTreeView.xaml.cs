using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Infrastructure.Extensions;
using SQLScriptsExplorer.Addin.Infrastructure.Helpers;
using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Models.Enums;
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
        private string previousFileName;
        private TextBlock lblRename;
        private TextBox txtRename;
        private TreeNode currentTreeNode;
        private TreeViewItem currentTreeViewItem;

        public string Filter { get; set; }

        public FileExplorerTreeView()
        {
            InitializeComponent();
        }

        private void TreeViewMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                mnuOpenNewInstance_Click(sender, null);
            }

            e.Handled = true;
        }

        public void RefreshTreeView()
        {
            TreeViewMain.ItemsSource = null;
            TreeViewMain.ItemsSource = DataSourceList;
        }

        private void TreeViewMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as System.Windows.Controls.TreeView;
            var selectedTreeNode = treeView.SelectedItem as TreeNode;

            if (selectedTreeNode != null)
            {
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

            txtRename_LostFocus(sender, null);
        }

        private void TreeViewMain_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentTreeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            currentTreeNode = currentTreeViewItem != null ?
                currentTreeViewItem.DataContext as TreeNode : null;
        }

        private void TreeViewMain_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            currentTreeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);
            currentTreeNode = currentTreeViewItem != null ?
                currentTreeViewItem.DataContext as TreeNode : null;

            if (currentTreeViewItem != null)
            {
                currentTreeViewItem.Focus();
                e.Handled = true;
            }
        }

        private static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }

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

        private void mnuRename_Click(object sender, RoutedEventArgs e)
        {
            // Get the TextBox control as defined in TreeView.ItemTemplate
            lblRename = TreeViewItemExtensions.FindVisualChild<TextBlock>(currentTreeViewItem as DependencyObject);
            txtRename = TreeViewItemExtensions.FindVisualChild<TextBox>(currentTreeViewItem as DependencyObject);

            // Set the Visible property of the TextBox Visibility
            lblRename.Visibility = Visibility.Collapsed;
            txtRename.Visibility = Visibility.Visible;

            txtRename.Focus();

            isEditMode = true;
            previousFileName = txtRename.Text;
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            var treeViewMainTreeNode = TreeViewMain.SelectedItem as TreeNode;
            var treeNode = DataSourceDictionary[treeViewMainTreeNode.Id];

            if (treeNode != null)
            {
                var confirmationMessage = $"Are you sure you want to delete this {treeNode.Type.ToString().ToLowerInvariant()}?";
                var messageBoxImage = MessageBoxImage.Question;

                if (PathHelper.IsNetworkPath(treeNode.FileFullPath))
                {
                    confirmationMessage = $"WARNING: The {treeNode.Type.ToString().ToLowerInvariant()} you are attemping to delete belongs to a shared folder in the network. " +
                        $"This file might be deleted permanently and other users won't be able to access it.\n\n" +
                        $"{confirmationMessage}";

                    messageBoxImage = MessageBoxImage.Warning;
                }

                if (MessageBox.Show(confirmationMessage, "Confirmation",
                    MessageBoxButton.YesNo, messageBoxImage, MessageBoxResult.No) == MessageBoxResult.Yes)
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
                        MessageBox.Show($"Couldn't delete {treeNode.Type.ToString().ToLowerInvariant()}. Error message: {ex.Message}");
                    }
                }
            }
        }

        private void mnuNewFile_Click(object sender, RoutedEventArgs e)
        {
            var newFileName = PathHelper.GetNextNewFileNameAvailable(currentTreeNode.FileFullPath);
            var newFilePath = Path.Combine(currentTreeNode.FileFullPath, newFileName);

            if (!File.Exists(newFilePath))
            {
                File.Create(newFilePath);

                var newTreeNode = new TreeNode(newFileName, newFilePath, TreeNodeType.File);
                newTreeNode.Parent = currentTreeNode;
                newTreeNode.IsSelected = true;

                currentTreeNode.Children.Add(newTreeNode);
                currentTreeNode.Children = currentTreeNode.Children.OrderBy(p => p.Type).ThenBy(p => p.FileName).ToList();
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

                var newTreeNode = new TreeNode(newFolderName, newFolderPath, TreeNodeType.Folder);
                newTreeNode.Parent = currentTreeNode;
                newTreeNode.IsSelected = true;

                currentTreeNode.Children.Add(newTreeNode);
                currentTreeNode.Children = currentTreeNode.Children.OrderBy(p => p.Type).ThenBy(p => p.FileName).ToList();
                currentTreeNode.IsExpanded = true;

                DataSourceDictionary.Add(newTreeNode.Id, newTreeNode);

                RefreshTreeView();

                if (TreeNodeAdded != null)
                {
                    TreeNodeAdded(newTreeNode, EventArgs.Empty);
                }
            }
        }

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
                txtRename.Text = previousFileName;
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

                    if (isEditMode && currentTreeViewItem != null && previousFileName != txtRename.Text)
                    {
                        var basePath = Path.GetDirectoryName(currentTreeNode.FileFullPath);
                        var previousPath = Path.Combine(basePath, previousFileName);
                        var newPath = Path.Combine(basePath, txtRename.Text);

                        if (currentTreeNode.Type == TreeNodeType.File && File.Exists(previousPath))
                        {
                            File.Move(previousPath, newPath);

                            hasRenamed = true;
                        }
                        else if (currentTreeNode.Type == TreeNodeType.Folder && Directory.Exists(previousPath))
                        {
                            // Necessary to move to another temporary path before moving to the new path, as directory renaming is not case-sensitive
                            // i.e. Renaming a folder from "Service" to "ServicE" would throw an exception using Directory.Move
                            var directory = new DirectoryInfo(previousPath);
                            directory.MoveTo(PathHelper.GetDirectoryTemporaryPath(previousPath));
                            directory.MoveTo(newPath);

                            hasRenamed = true;
                        }

                        if (hasRenamed)
                        {
                            currentTreeNode.FileName = txtRename.Text;
                            currentTreeNode.FileFullPath = newPath;

                            currentTreeNode.Parent.Children = currentTreeNode.Parent.Children.OrderBy(p => p.Type).ThenBy(p => p.FileName).ToList();

                            currentTreeViewItem.IsSelected = true;

                            if (TreeNodeRenamed != null)
                            {
                                TreeNodeRenamed(currentTreeNode, EventArgs.Empty);
                            }
                        }
                    }

                    isEditMode = false;

                    if (hasRenamed)
                    {
                        RefreshTreeView();
                    }

                    lblRename.Visibility = Visibility.Visible;
                    txtRename.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Infrastructure.Helpers;
using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Models.Enums;
using SQLScriptsExplorer.Addin.Repository;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace SQLScriptsExplorer.Addin.Commands.ToolWindow
{
    /// <summary>
    /// Interaction logic for MainToolWindowControl.
    /// </summary>
    public partial class MainToolWindowControl : UserControl
    {
        private ISettingsRepository settingsRepository = null;
        private ITreeNodeRepository treeNodeRepository = null;

        // DispatcherTimer to debounce the search
        private DispatcherTimer searchTimer = null;
        // You can adjust how long to wait before performing the search (e.g., 500ms)
        private readonly TimeSpan SearchDelay = TimeSpan.FromMilliseconds(300);

        public bool IsSearchResultsView
        {
            get { return FileExplorerSearchResults.Visibility == Visibility.Visible; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainToolWindowControl"/> class.
        /// </summary>
        public MainToolWindowControl()
        {
            this.InitializeComponent();

            settingsRepository = new SettingsRepository();
            treeNodeRepository = new TreeNodeRepository();

            // Initialize the search debounce timer
            searchTimer = new DispatcherTimer();
            searchTimer.Interval = SearchDelay;
            searchTimer.Tick += SearchTimer_Tick;

            RefreshTreeView();
        }

        private void btnFormatSelection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DocumentManager.FormatSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTreeView();

            if (IsSearchResultsView)
            {
                // If currently in Search view, re-run the search after refresh
                PerformSearch();
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            var frmSettings = new frmSettings();
            var frmSettingsResult = frmSettings.ShowDialog();

            settingsRepository.Refresh();

            if (frmSettingsResult == System.Windows.Forms.DialogResult.OK)
            {
                RefreshTreeView();
            }
        }

        private void btnCollapseAll_Click(object sender, RoutedEventArgs e)
        {
            if (IsSearchResultsView)
            {
                TreeViewHelper.CollapseTreeView(treeNodeRepository.TreeSearchResult);
                FileExplorerSearchResults.RefreshTreeView();
            }
            else
            {
                TreeViewHelper.CollapseTreeView(treeNodeRepository.Tree);
                FileExplorerAll.RefreshTreeView();
            }
        }

        private void btnExpandAll_Click(object sender, RoutedEventArgs e)
        {
            if (IsSearchResultsView)
            {
                TreeViewHelper.ExpandTreeView(treeNodeRepository.TreeSearchResult);
                FileExplorerSearchResults.RefreshTreeView();
            }
            else
            {
                TreeViewHelper.ExpandTreeView(treeNodeRepository.Tree);
                FileExplorerAll.RefreshTreeView();
            }
        }

        public void RefreshTreeViewDataSource()
        {
            try
            {
                treeNodeRepository.Load(
                    settingsRepository.FolderMapping,
                    settingsRepository.ExpandMappedFoldersOnLoad,
                    settingsRepository.AllowedFileTypes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RefreshTreeView()
        {
            RefreshTreeViewDataSource();

            FileExplorerAll.DataSourceList = treeNodeRepository.Tree;
            FileExplorerAll.DataSourceDictionary = treeNodeRepository.TreeDictionary;

            FileExplorerAll.RefreshTreeView();
        }

        /// <summary>
        /// KeyUp event: we reset/restart the timer, so that we only search
        /// after user stops typing for a brief moment.
        /// </summary>
        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            // Stop and restart the timer each time a key is pressed.
            searchTimer.Stop();
            searchTimer.Start();
        }

        /// <summary>
        /// Called when the search timer elapses (no more key presses for the set interval).
        /// </summary>
        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop(); // Prevent multiple triggers
            PerformSearch();
        }

        /// <summary>
        /// Actually performs the search in the repository, updates the UI accordingly.
        /// </summary>
        private void PerformSearch()
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                treeNodeRepository.Search(txtSearch.Text);

                TreeViewHelper.ExpandTreeView(treeNodeRepository.TreeSearchResult);

                FileExplorerSearchResults.DataSourceList = treeNodeRepository.TreeSearchResult;
                FileExplorerSearchResults.DataSourceDictionary = treeNodeRepository.TreeSearchResultsDictionary;

                FileExplorerSearchResults.RefreshTreeView();

                FileExplorerSearchResults.Visibility = Visibility.Visible;
                FileExplorerAll.Visibility = Visibility.Collapsed;
            }
            else
            {
                FileExplorerSearchResults.Visibility = Visibility.Collapsed;
                FileExplorerAll.Visibility = Visibility.Visible;
            }

            // Update Filter property
            FileExplorerSearchResults.Filter = txtSearch.Text;
        }

        private void FileExplorerSearchResults_TreeNodeAdded(object sender, System.EventArgs e)
        {
            var treeNodeSearchResults = sender as TreeNode;

            var treeNodeParent = FileExplorerAll.DataSourceDictionary[treeNodeSearchResults.Parent.Id];
            var treeNode = treeNodeSearchResults.Clone(treeNodeParent);

            treeNodeParent.Children.Add(treeNode);
            treeNodeParent.Children = treeNodeParent.Children
                .OrderBy(p => p.Type)
                .ThenBy(p => p.FileName)
                .ToList();

            FileExplorerAll.DataSourceDictionary.Add(treeNode.Id, treeNode);

            FileExplorerAll.RefreshTreeView();
        }

        private void FileExplorerSearchResults_TreeNodeDeleted(object sender, System.EventArgs e)
        {
            var treeNodeSearchResults = sender as TreeNode;
            var treeNode = FileExplorerAll.DataSourceDictionary[treeNodeSearchResults.Id];

            treeNode.Parent.Children.Remove(treeNode);
            FileExplorerAll.DataSourceDictionary.Remove(treeNode.Id);

            FileExplorerAll.RefreshTreeView();
        }

        private void FileExplorerSearchResults_TreeNodeRenamed(object sender, System.EventArgs e)
        {
            var treeNodeSearchResults = sender as TreeNode;
            var treeNode = FileExplorerAll.DataSourceDictionary[treeNodeSearchResults.Id];

            treeNode.FileName = treeNodeSearchResults.FileName;
            treeNode.FileFullPath = treeNodeSearchResults.FileFullPath;

            treeNode.Parent.Children = treeNode.Parent.Children
                .OrderBy(p => p.Type)
                .ThenBy(p => p.FileName)
                .ToList();

            FileExplorerAll.RefreshTreeView();
        }

        #region UI
        private void ToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;

            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }

            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness(0);
            }
        }

        #endregion
    }
}

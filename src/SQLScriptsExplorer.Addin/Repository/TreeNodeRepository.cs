using SQLScriptsExplorer.Addin.Infrastructure.Extensions;
using SQLScriptsExplorer.Addin.Models;
using SQLScriptsExplorer.Addin.Models.Enums;
using SQLScriptsExplorer.Addin.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace SQLScriptsExplorer.Addin.Repository
{
    public class TreeNodeRepository : ITreeNodeRepository
    {
        public List<TreeNode> Tree { get; internal set; } = new List<TreeNode>();
        public List<TreeNode> TreeSearchResult { get; internal set; } = new List<TreeNode>();
        public Dictionary<string, TreeNode> TreeDictionary { get; internal set; } = new Dictionary<string, TreeNode>();
        public Dictionary<string, TreeNode> TreeSearchResultsDictionary { get; internal set; } = new Dictionary<string, TreeNode>();

        private string previousSearchKeyword = string.Empty;

        #region Load

        public void Load(List<FolderMapping> lstFolderMapping, bool expandMappedFolderOnLoad, string allowedFileTypes)
        {
            Tree.Clear();
            TreeDictionary.Clear();

            foreach (var folderMapping in lstFolderMapping)
            {
                if (!string.IsNullOrEmpty(folderMapping.Alias) && !string.IsNullOrEmpty(folderMapping.FolderPath) && Directory.Exists(folderMapping.FolderPath))
                {
                    var treeNodeAlias = new TreeNode(folderMapping.Alias, folderMapping.FolderPath, TreeNodeType.RootFolder);
                    treeNodeAlias.IsExpanded = expandMappedFolderOnLoad;

                    Tree.Add(treeNodeAlias);
                    TreeDictionary.Add(treeNodeAlias.Id, treeNodeAlias);

                    CreateTree(Tree, TreeDictionary, folderMapping.FolderPath, treeNodeAlias, allowedFileTypes);
                }
            }
        }

        private List<TreeNode> CreateTree(List<TreeNode> lstTreeNode, Dictionary<string, TreeNode> dicTreeNode, string folder, TreeNode parentTreeNode, string allowedFileTypes)
        {
            var lstDirectories = Directory.GetDirectories(folder);

            foreach (var directory in lstDirectories)
            {
                try
                {
                    var directoryInfo = new DirectoryInfo(directory);
                    var treeNodeFolder = new TreeNode(directoryInfo.Name, directoryInfo.FullName, TreeNodeType.Folder);

                    if (parentTreeNode is null)
                    {
                        lstTreeNode.Add(treeNodeFolder);
                    }
                    else
                    {
                        treeNodeFolder.Parent = parentTreeNode;

                        parentTreeNode.Children.Add(treeNodeFolder);
                    }

                    dicTreeNode.Add(treeNodeFolder.Id, treeNodeFolder);

                    CreateTree(lstTreeNode, dicTreeNode, Path.Combine(folder, directory), treeNodeFolder, allowedFileTypes);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            var lstFiles = Directory.GetFiles(folder, allowedFileTypes);

            foreach (var file in lstFiles)
            {
                try
                {
                    var fileInfo = new FileInfo(file);
                    var treeNodeFile = new TreeNode(fileInfo.Name, fileInfo.FullName, TreeNodeType.File);

                    if (parentTreeNode is null)
                    {
                        lstTreeNode.Add(treeNodeFile);
                    }
                    else
                    {
                        treeNodeFile.Parent = parentTreeNode;

                        parentTreeNode.Children.Add(treeNodeFile);
                    }

                    dicTreeNode.Add(treeNodeFile.Id, treeNodeFile);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return lstTreeNode;
        }

        #endregion

        #region Search

        public List<TreeNode> Search(string keyword)
        {
            // Use current search Tree to perform further searches
            if (previousSearchKeyword.StartsWith(keyword))
            {
                TreeSearchResult = TreeSearchResult.Clone();
            }
            // Branch new search
            else
            {
                TreeSearchResult = Tree.Clone();
            }

            Search(TreeSearchResult, TreeSearchResultsDictionary, keyword);

            TreeSearchResultsDictionary.Clear();
            BuildDictionary(TreeSearchResult, TreeSearchResultsDictionary, keyword);

            previousSearchKeyword = keyword;

            return TreeSearchResult;
        }

        private void Search(List<TreeNode> lstTreeNodeSearchResults, Dictionary<string, TreeNode> dicTreeNodeSearchResults, string keyword)
        {
            SearchNodes(lstTreeNodeSearchResults, dicTreeNodeSearchResults, keyword);
        }

        private void SearchNodes(List<TreeNode> lstTreeNodeSearchResults, Dictionary<string, TreeNode> dicTreeNodeSearchResults, string keyword)
        {
            if (lstTreeNodeSearchResults.Count == 0)
                return;

            for (int i = lstTreeNodeSearchResults.Count - 1; i >= 0; i--)
            {
                var treeNode = lstTreeNodeSearchResults[i];
                SearchNodes(treeNode.Children, dicTreeNodeSearchResults, keyword);

                var isNoMatchFile = treeNode.Type == TreeNodeType.File && treeNode.FileName.ToUpperInvariant().IndexOf(keyword.ToUpperInvariant()) == -1;
                var isNoMatchFolder = (treeNode.Type == TreeNodeType.Folder || treeNode.Type == TreeNodeType.RootFolder) &&
                        lstTreeNodeSearchResults.Contains(treeNode) && treeNode.Children.Count == 0 &&
                        treeNode.FileName.ToUpperInvariant().IndexOf(keyword.ToUpperInvariant()) == -1;

                if (isNoMatchFile || isNoMatchFolder)
                {
                    if (treeNode.Parent != null)
                    {
                        treeNode.Parent.Children.Remove(treeNode);
                    }

                    lstTreeNodeSearchResults.Remove(treeNode);
                    dicTreeNodeSearchResults.Remove(treeNode.Id);
                }
            }
        }

        private void BuildDictionary(List<TreeNode> lstTreeNode, Dictionary<string, TreeNode> dicTreeNode, string keyword)
        {
            foreach (var treeNode in lstTreeNode)
            {
                treeNode.HighlightPhrase = keyword;

                dicTreeNode.Add(treeNode.Id, treeNode);

                BuildDictionary(treeNode.Children, dicTreeNode, keyword);
            }
        }

        #endregion
    }
}

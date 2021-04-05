using System.Collections.Generic;
using SQLScriptsExplorer.Addin.Models;

namespace SQLScriptsExplorer.Addin.Repository.Interfaces
{
    public interface ITreeNodeRepository
    {
        List<TreeNode> Tree { get; }
        List<TreeNode> TreeSearchResult { get; }
        Dictionary<string, TreeNode> TreeDictionary { get; }
        Dictionary<string, TreeNode> TreeSearchResultsDictionary { get; }
        void Load(List<FolderMapping> lstFolderMapping, bool expandMappedFolderOnLoad, string allowedFileTypes);
        List<TreeNode> Search(string keyword);
    }
}
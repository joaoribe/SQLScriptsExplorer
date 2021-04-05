using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SQLScriptsExplorer.Addin.Infrastructure;
using SQLScriptsExplorer.Addin.Models.Enums;

namespace SQLScriptsExplorer.Addin.Models
{
    public class TreeNode : ITreeNode<TreeNode>
    {
        public string Id { get; internal set; }
        public string FileName { get; set; }
        public string FileFullPath { get; set; }
        public TreeNodeType Type { get; set; }

        public TreeNode Parent { get; set; }
        public List<TreeNode> Children { get; set; }

        #region UI

        public BitmapImage Icon { get { return Utils.GetResourceImage(Type.ToString()); } }
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public string HighlightPhrase { get; set; }

        #endregion

        public TreeNode()
        {
            Id = Utils.GenerateID(8);
            Children = new List<TreeNode>();
        }

        public TreeNode(string fileName, string fileFullPath, TreeNodeType type) : this()
        {
            FileName = fileName;
            FileFullPath = fileFullPath;
            Type = type;
        }

        public TreeNode Clone(TreeNode parentNode)
        {
            if (this == null)
                return null;

            var clone = new TreeNode(FileName, FileFullPath, Type)
            {
                Id = Id,
                Parent = parentNode
            };

            clone.Children = new List<TreeNode>();

            if (Children.Count > 0)
                Children.ForEach(p => clone.Children.Add(p.Clone(clone)));

            return clone;
        }
    }
}

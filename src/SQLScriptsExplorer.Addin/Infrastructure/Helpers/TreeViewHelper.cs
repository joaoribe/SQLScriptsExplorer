using System.Collections.Generic;
using SQLScriptsExplorer.Addin.Models;

namespace SQLScriptsExplorer.Addin.Infrastructure.Helpers
{
    public static class TreeViewHelper
    {
        public static void ExpandTreeView(List<TreeNode> lstTreeViewItems)
        {
            ExpandCollapseTreeView(lstTreeViewItems, true);
        }

        public static void CollapseTreeView(List<TreeNode> lstTreeViewItems)
        {
            ExpandCollapseTreeView(lstTreeViewItems, false);
        }

        private static void ExpandCollapseTreeView(List<TreeNode> lstTreeViewItems, bool isExpanded)
        {
            foreach (TreeNode item in lstTreeViewItems)
            {
                item.IsExpanded = isExpanded;

                if (item.Children.Count > 0)
                    ExpandCollapseTreeView(item.Children, isExpanded);
            }
        }
    }
}

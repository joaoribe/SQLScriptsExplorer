using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SQLScriptsExplorer.Addin.Infrastructure.Extensions
{
    public static class TreeViewItemExtensions
    {
        public static int GetDepth(this TreeViewItem item)
        {
            TreeViewItem parent;

            while ((parent = GetParent(item)) != null)
            {
                return GetDepth(parent) + 1;
            }

            return 0;
        }

        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);

            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TreeViewItem;
        }

        /// <summary>
        /// Get the various controls inside the ItemTemplate 
        /// </summary>
        public static ChildItem FindVisualChild<ChildItem>(DependencyObject obj) where ChildItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is ChildItem)
                    return (ChildItem)child;
                else
                {
                    ChildItem childOfChild = FindVisualChild<ChildItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }

            return null;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using SQLScriptsExplorer.Addin.Models;

namespace SQLScriptsExplorer.Addin.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static List<T> Clone<T>(this List<T> listToClone) where T : ITreeNode<T>
        {
            return listToClone.Select(item => ((T)item).Clone(default(T))).ToList();
        }
    }
}

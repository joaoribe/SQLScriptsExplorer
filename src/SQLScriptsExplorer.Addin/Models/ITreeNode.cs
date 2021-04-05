namespace SQLScriptsExplorer.Addin.Models
{
    public interface ITreeNode<T>
    {
        T Clone(T parentNode);
    }
}

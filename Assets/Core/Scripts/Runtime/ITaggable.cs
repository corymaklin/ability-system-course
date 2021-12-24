using System.Collections.ObjectModel;

namespace Core
{
    public interface ITaggable
    {
        ReadOnlyCollection<string> tags { get; }
    }
}
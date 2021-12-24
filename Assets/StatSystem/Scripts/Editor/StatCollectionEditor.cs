using Core.Editor;
using UnityEngine.UIElements;

namespace StatSystem.Editor
{
    public class StatCollectionEditor : ScriptableObjectCollectionEditor<StatDefinition>
    {
        public new class UxmlFactory : UxmlFactory<StatCollectionEditor, UxmlTraits> {}
    }
}
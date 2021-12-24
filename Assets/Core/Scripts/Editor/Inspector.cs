using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Core.Editor
{
    public class Inspector : InspectorElement
    {
        public new class UxmlFactory : UxmlFactory<Inspector, UxmlTraits> {}
    }
}
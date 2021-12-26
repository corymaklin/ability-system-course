using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    public class AbilityElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<AbilityElement, UxmlTraits> {}

        public AbilityElement()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UI/Ability");
            visualTree.CloneTree(this);
        }
    }
}
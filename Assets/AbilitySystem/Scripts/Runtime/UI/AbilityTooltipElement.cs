using Core.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    public class AbilityTooltipElement : TooltipElement
    {
        public new class UxmlFactory : UxmlFactory<AbilityTooltipElement, UxmlTraits> {}

        public AbilityTooltipElement()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("UI/AbilityTooltip");
            visualTree.CloneTree(this);
            var styleSheet = Resources.Load<StyleSheet>("UI/AbilityTooltip");
            styleSheets.Add(styleSheet);
        }
    }
}
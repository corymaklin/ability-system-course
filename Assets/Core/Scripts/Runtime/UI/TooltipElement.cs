using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

namespace Core.UI
{
    public class TooltipElement : VisualElement
    {
        private int m_Duration = 1000;
        
        public new class UxmlFactory : UxmlFactory<TooltipElement, UxmlTraits> {}

        public void Show()
        {
            style.display = DisplayStyle.Flex;
            experimental.animation.Start(new StyleValues { opacity = 0f },
                new StyleValues { opacity = 1f }, m_Duration).Ease(Easing.OutQuad);
        }

        public void Hide()
        {
            experimental.animation.Start(new StyleValues { opacity = 1f },
                new StyleValues { opacity = 0f }, m_Duration).Ease(Easing.OutQuad);
            style.display = DisplayStyle.None;
        }
    }
}
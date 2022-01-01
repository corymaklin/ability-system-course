using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    public class AbilityManipulator : Manipulator
    {
        private AbilityTooltipElement m_TooltipElement;
        private Ability m_Ability;

        public AbilityManipulator(Ability ability, AbilityTooltipElement tooltipElement)
        {
            m_Ability = ability;
            m_TooltipElement = tooltipElement;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseEnterEvent>(OnMouseEnter);
            target.UnregisterCallback<MouseLeaveEvent>(OnMouseLeave);
        }
        
        private void OnMouseLeave(MouseLeaveEvent evt)
        {
            HideTooltip();
        }

        private void OnMouseEnter(MouseEnterEvent evt)
        {
            ShowTooltip();
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            m_TooltipElement.style.left = evt.mousePosition.x;
            m_TooltipElement.style.top = evt.mousePosition.y;
        }
        
        private void ShowTooltip()
        {
            Label title = m_TooltipElement.Q<Label>("ability-tooltip__title");
            title.text = m_Ability.definition.name;
            Label description = m_TooltipElement.Q<Label>("ability-tooltip__description");
            description.text = m_Ability.definition.description;
            VisualElement icon = m_TooltipElement.Q("ability-tooltip__icon");
            icon.style.backgroundImage = new StyleBackground(m_Ability.definition.icon);
            Label data = m_TooltipElement.Q<Label>("ability-tooltip__data");
            data.text = m_Ability.ToString();
            m_TooltipElement.Show();
        }
        
        private void HideTooltip()
        {
            m_TooltipElement.Hide();
        }
    }
}
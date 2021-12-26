using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class AbilitiesUI : MonoBehaviour
    {
        [SerializeField] private PlayerAbilityController m_Controller;
        
        private UIDocument m_UIDocument;
        private VisualElement m_Parent;
        private Button m_CloseButton;
        private Label m_AbilityPoints;

        private void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
        }

        private void Start()
        {
            var root = m_UIDocument.rootVisualElement;
            m_Parent = root.Q("abilities__content");
            m_CloseButton = root.Q<Button>("abilities__close-button");
            m_CloseButton.clicked += Hide;

            foreach (Ability ability in m_Controller.abilities.Values)
            {
                AbilityElement abilityElement = new AbilityElement
                {
                    name = ability.definition.name
                };
                Label level = abilityElement.Q<Label>("ability__level");
                Label title = abilityElement.Q<Label>("ability__title");
                Button incrementButton = abilityElement.Q<Button>("ability__increment-button");
                VisualElement icon = abilityElement.Q("ability__icon");
                level.text = ability.level.ToString();
                title.text = ability.definition.name;
                incrementButton.SetEnabled(m_Controller.abilityPoints > 0 && ability.level != ability.definition.maxLevel);
                incrementButton.clicked += () =>
                {
                    ability.level++;
                    level.text = ability.level.ToString();
                    m_Controller.abilityPoints--;
                };
                icon.style.backgroundImage = new StyleBackground(ability.definition.icon);
                m_Parent.Add(abilityElement);
            }

            m_AbilityPoints = root.Q<Label>("abilities__ability-points-value");
            OnAbilityPointsChanged();
            m_Controller.abilityPointsChanged += OnAbilityPointsChanged;
        }

        private void OnAbilityPointsChanged()
        {
            m_AbilityPoints.text = m_Controller.abilityPoints.ToString();
            for (int i = 0; i < m_Parent.childCount; i++)
            {
                Ability ability = m_Controller.abilities[m_Parent[i].name];
                Button incrementButton = m_Parent[i].Q<Button>("ability__increment-button");
                incrementButton.SetEnabled(m_Controller.abilityPoints > 0 && ability.level != ability.definition.maxLevel);
            }
        }

        public void Hide()
        {
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
    }
}
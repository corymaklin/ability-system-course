using LevelSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatSystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HeadsUpDisplayUI : MonoBehaviour
    {
        [SerializeField] private PlayerStatController m_Controller;
        private UIDocument m_UIDocument;
        private ILevelable m_Levelable;
        private ProgressBar m_HealthBar;
        private ProgressBar m_ManaBar;
        private ProgressBar m_ExperienceBar;
        private Label m_Level;

        private void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            m_Levelable = m_Controller.GetComponent<ILevelable>();
        }

        private void OnEnable()
        {
            var root = m_UIDocument.rootVisualElement;
            m_HealthBar = root.Q<ProgressBar>("health");
            m_ManaBar = root.Q<ProgressBar>("mana");
            m_ExperienceBar = root.Q<ProgressBar>("experience");
            m_Level = root.Q<Label>("level");
        }

        private void Start()
        {
            Attribute mana = m_Controller.stats["Mana"] as Attribute;
            Attribute health = m_Controller.stats["Health"] as Attribute;
            OnManaChangedInternal();
            OnHealthChangedInternal();
            OnLevelChanged();
            mana.valueChanged += OnMaxManaChanged;
            mana.currentValueChanged += OnManaChanged;
            health.valueChanged += OnMaxHealthChanged;
            health.currentValueChanged += OnHealthChanged;
            m_Levelable.levelChanged += OnLevelChanged;
            m_Levelable.currentExperienceChanged += OnCurrentExperienceChanged;
        }

        private void OnDestroy()
        {
            Attribute mana = m_Controller.stats["Mana"] as Attribute;
            Attribute health = m_Controller.stats["Health"] as Attribute;
            mana.valueChanged -= OnMaxManaChanged;
            mana.currentValueChanged -= OnManaChanged;
            health.valueChanged -= OnMaxHealthChanged;
            health.currentValueChanged -= OnHealthChanged;
            m_Levelable.levelChanged -= OnLevelChanged;
            m_Levelable.currentExperienceChanged -= OnCurrentExperienceChanged;
        }

        private void OnCurrentExperienceChanged()
        {
            OnExperienceChangedInternal();
        }

        private void OnLevelChanged()
        {
            OnExperienceChangedInternal();
            m_Level.text = m_Levelable.level.ToString();
        }

        private void OnExperienceChangedInternal()
        {
            m_ExperienceBar.value = (float)m_Levelable.currentExperience / m_Levelable.requiredExperience * 100f;
            m_ExperienceBar.title = $"{m_Levelable.currentExperience} / {m_Levelable.requiredExperience}";
        }

        private void OnHealthChanged()
        {
            OnHealthChangedInternal();
        }

        private void OnMaxHealthChanged()
        {
            OnHealthChangedInternal();
        }

        private void OnHealthChangedInternal()
        {
            Attribute health = m_Controller.stats["Health"] as Attribute;
            m_HealthBar.value = (float)health.currentValue / health.value * 100f;
            m_HealthBar.title = $"{health.currentValue} / {health.value}";
        }

        private void OnManaChanged()
        {
            OnManaChangedInternal();
        }

        private void OnMaxManaChanged()
        {
            OnManaChangedInternal();
        }

        private void OnManaChangedInternal()
        {
            Attribute mana = m_Controller.stats["Mana"] as Attribute;
            m_ManaBar.value = (float)mana.currentValue / mana.value * 100f;
            m_ManaBar.title = $"{mana.currentValue} / {mana.value}";
        }
    }
}
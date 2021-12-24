using LevelSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatSystem.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class StatsUI : MonoBehaviour
    {
        private UIDocument m_UIDocument;
        private ILevelable m_Levelable;
        [SerializeField] private PlayerStatController m_Controller;
        
        private void Awake()
        {
            m_UIDocument = GetComponent<UIDocument>();
            m_Levelable = m_Controller.GetComponent<ILevelable>();
        }

        private void Start()
        {
            var root = m_UIDocument.rootVisualElement;

            VisualElement experience = root.Q("experience");
            Label experienceValue = experience.Q<Label>("value");
            experienceValue.text = $"{m_Levelable.currentExperience} / {m_Levelable.requiredExperience}";
            m_Levelable.currentExperienceChanged += () =>
            {
                experienceValue.text = $"{m_Levelable.currentExperience} / {m_Levelable.requiredExperience}";
            };

            VisualElement level = root.Q("level");
            Label levelValue = level.Q<Label>("value");
            levelValue.text = m_Levelable.level.ToString();
            m_Levelable.levelChanged += () =>
            {
                experienceValue.text = $"{m_Levelable.currentExperience} / {m_Levelable.requiredExperience}";
                levelValue.text = m_Levelable.level.ToString();
            };

            VisualElement primaryStats = root.Q("primary-stats");
            for (int i = 0; i < primaryStats.childCount; i++)
            {
                Stat stat = m_Controller.stats[primaryStats[i].name];
                Label label = primaryStats[i].Q<Label>("value");
                label.text = stat.value.ToString();
                stat.valueChanged += () =>
                {
                    label.text = stat.value.ToString();
                };
                Button incrementButton = primaryStats[i].Q<Button>("increment-button");
                incrementButton.SetEnabled(m_Controller.statPoints > 0 && stat.baseValue != stat.definition.cap);
                incrementButton.clicked += () =>
                {
                    (stat as PrimaryStat).Add(1);
                    label.text = stat.value.ToString();
                    incrementButton.SetEnabled(stat.baseValue != stat.definition.cap);
                    m_Controller.statPoints--;
                };
            }

            VisualElement stats = root.Q("stats");
            for (int i = 0; i < stats.childCount; i++)
            {
                Stat stat = m_Controller.stats[stats[i].name.Replace("-", "")];
                Label label = stats[i].Q<Label>("value");
                label.text = stat.value.ToString();
                stat.valueChanged += () =>
                {
                    label.text = stat.value.ToString();
                };
            }

            VisualElement statPoints = root.Q("stat-points");
            Label statPointsValue = statPoints.Q<Label>("value");
            statPointsValue.text = m_Controller.statPoints.ToString();
            m_Controller.statPointsChanged += () =>
            {
                statPointsValue.text = m_Controller.statPoints.ToString();
                for (int i = 0; i < primaryStats.childCount; i++)
                {
                    Button incrementButton = primaryStats[i].Q<Button>("increment-button");
                    incrementButton.SetEnabled(m_Controller.statPoints > 0);
                }
            };
        }

        public void Show()
        {
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            m_UIDocument.rootVisualElement.style.display = DisplayStyle.None;
        }
    }
}
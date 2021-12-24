using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.Scripts.Runtime.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        private Slider m_Slider;
        private IDamageable m_Damageable;
        [SerializeField] private GameObject m_Owner;

        private void Awake()
        {
            m_Slider = GetComponent<Slider>();
            m_Damageable = m_Owner.GetComponent<IDamageable>();
        }

        private void OnEnable()
        {
            m_Damageable.initialized += OnDamageableInitialized;
            m_Damageable.willUninitialize += OnDamageableWillUninitialize;
            if (m_Damageable.isInitialized)
                OnDamageableInitialized();
        }

        private void OnDamageableWillUninitialize()
        {
            UnregisterEvents();
        }

        private void OnDamageableInitialized()
        {
            m_Slider.maxValue = m_Damageable.maxHealth;
            m_Slider.value = m_Damageable.health;
            RegisterEvents();
        }

        private void UnregisterEvents()
        {
            m_Damageable.healthChanged -= OnHealthChanged;
            m_Damageable.maxHealthChanged -= OnMaxHealthChanged;
        }

        private void RegisterEvents()
        {
            m_Damageable.maxHealthChanged += OnMaxHealthChanged;
            m_Damageable.healthChanged += OnHealthChanged;
        }

        private void OnHealthChanged()
        {
            m_Slider.value = m_Damageable.health;
        }

        private void OnMaxHealthChanged()
        {
            m_Slider.maxValue = m_Damageable.maxHealth;
        }

        private void OnValidate()
        {
            if (m_Owner != null)
            {
                if (m_Owner.GetComponent<IDamageable>() == null)
                {
                    Debug.LogWarning("The owner must implement the IDamageable interface!");
                    m_Owner = null;
                }
            }
        }
    }
}
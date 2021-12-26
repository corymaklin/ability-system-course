using System;
using LevelSystem;
using UnityEngine;

namespace AbilitySystem
{
    [RequireComponent(typeof(ILevelable))]
    public class PlayerAbilityController : AbilityController
    {
        protected ILevelable m_Levelable;
        protected int m_AbilityPoints;
        public event Action abilityPointsChanged;

        public int abilityPoints
        {
            get => m_AbilityPoints;
            internal set
            {
                m_AbilityPoints = value;
                abilityPointsChanged?.Invoke();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_Levelable = GetComponent<ILevelable>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Levelable.initialized += OnLevelableInitialized;
            m_Levelable.willUninitialize += UnregisterEvents;
            if (m_Levelable.isInitialized)
            {
                OnLevelableInitialized();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_Levelable.initialized -= OnLevelableInitialized;
            m_Levelable.willUninitialize -= UnregisterEvents;
            if (m_Levelable.isInitialized)
            {
                UnregisterEvents();
            }
        }

        private void OnLevelableInitialized()
        {
            RegisterEvents();
        }
        
        private void UnregisterEvents()
        {
            m_Levelable.levelChanged -= OnLevelChanged;
        }

        private void RegisterEvents()
        {
            m_Levelable.levelChanged += OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            abilityPoints += 3;
        }
    }
}
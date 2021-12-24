using System;
using System.Collections.Generic;
using LevelSystem;
using LevelSystem.Nodes;
using UnityEngine;

namespace StatSystem
{
    [RequireComponent(typeof(ILevelable))]
    public class PlayerStatController : StatController
    {
        protected ILevelable m_Levelable;

        protected int m_StatPoints = 5;
        public event Action statPointsChanged;

        public int statPoints
        {
            get => m_StatPoints;
            internal set
            {
                m_StatPoints = value;
                statPointsChanged?.Invoke();
            }
        }

        protected override void Awake()
        {
            m_Levelable = GetComponent<ILevelable>();
        }

        private void OnEnable()
        {
            m_Levelable.initialized += OnLevelableInitialized;
            m_Levelable.willUninitialize += UnregisterEvents;
            if (m_Levelable.isInitialized)
            {
                OnLevelableInitialized();
            }
        }

        private void OnDisable()
        {
            m_Levelable.initialized -= OnLevelableInitialized;
            m_Levelable.willUninitialize -= UnregisterEvents;
            if (m_Levelable.isInitialized)
            {
                UnregisterEvents();
            }
        }

        private void OnLevelableInitialized()
        {
            Initialize();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            m_Levelable.levelChanged += OnLevelChanged;
        }
        
        private void UnregisterEvents()
        {
            m_Levelable.levelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged()
        {
            statPoints += 5;
        }

        protected override void InitializeStatFormulas()
        {
            base.InitializeStatFormulas();
            foreach (Stat currentStat in m_Stats.Values)
            {
                if (currentStat.definition.formula != null && currentStat.definition.formula.rootNode != null)
                {
                    List<LevelNode> levelNodes = currentStat.definition.formula.FindNodesOfType<LevelNode>();
                    foreach (LevelNode levelNode in levelNodes)
                    {
                        levelNode.levelable = m_Levelable;
                        m_Levelable.levelChanged += currentStat.CalculateValue;
                    }
                }
            }
        }

        #region Stat System

        public override object data
        {
            get
            {
                return new PlayerStatControllerData(base.data as StatControllerData)
                {
                    statPoints = m_StatPoints
                };
            }
        }

        public override void Load(object data)
        {
            base.Load(data);
            PlayerStatControllerData playerStatControllerData = (PlayerStatControllerData)data;
            m_StatPoints = playerStatControllerData.statPoints;
            statPointsChanged?.Invoke();
        }

        [Serializable]
        protected class PlayerStatControllerData : StatControllerData
        {
            public int statPoints;

            public PlayerStatControllerData(StatControllerData statControllerData)
            {
                stats = statControllerData.stats;
            }
        }

        #endregion
    }
}
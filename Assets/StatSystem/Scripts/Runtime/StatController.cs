using System;
using System.Collections.Generic;
using SaveSystem.Scripts.Runtime;
using StatSystem.Nodes;
using UnityEngine;

namespace StatSystem
{
    public class StatController : MonoBehaviour, ISavable
    {
        [SerializeField] private StatDatabase m_StatDatabase;
        protected Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, Stat> stats => m_Stats;

        private bool m_IsInitialized;
        public bool isInitialized => m_IsInitialized;
        public event Action initialized;
        public event Action willUninitialize;

        protected virtual void Awake()
        {
            if (!m_IsInitialized)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            willUninitialize?.Invoke();
        }

        protected void Initialize()
        {
            foreach (StatDefinition definition in m_StatDatabase.stats)
            {
                m_Stats.Add(definition.name, new Stat(definition, this));
            }

            foreach (StatDefinition definition in m_StatDatabase.attributes)
            {
                if (definition.name.Equals("Health", StringComparison.OrdinalIgnoreCase))
                {
                    m_Stats.Add(definition.name, new Health(definition, this));
                }
                else
                {
                    m_Stats.Add(definition.name, new Attribute(definition, this));   
                }
            }

            foreach (StatDefinition definition in m_StatDatabase.primaryStats)
            {
                m_Stats.Add(definition.name, new PrimaryStat(definition, this));
            }
            
            InitializeStatFormulas();

            foreach (Stat stat in m_Stats.Values)
            {
                stat.Initialize();
            }
            
            m_IsInitialized = true;
            initialized?.Invoke();
        }

        protected virtual void InitializeStatFormulas()
        {
            foreach (Stat currentStat in m_Stats.Values)
            {
                if (currentStat.definition.formula != null && currentStat.definition.formula.rootNode != null)
                {
                    List<StatNode> statNodes = currentStat.definition.formula.FindNodesOfType<StatNode>();

                    foreach (StatNode statNode in statNodes)
                    {
                        if (m_Stats.TryGetValue(statNode.statName.Trim(), out Stat stat))
                        {
                            statNode.stat = stat;
                            stat.valueChanged += currentStat.CalculateValue;
                        }
                        else
                        {
                            Debug.LogWarning($"Stat {statNode.statName.Trim()} does not exist!");
                        }
                    }
                }
            }
        }

        #region Stat System

        public virtual object data
        {
            get
            {
                Dictionary<string, object> stats = new Dictionary<string, object>();
                foreach (Stat stat in m_Stats.Values)
                {
                    if (stat is ISavable savable)
                    {
                        stats.Add(stat.definition.name, savable.data);
                    }
                }

                return new StatControllerData
                {
                    stats = stats
                };
            }
        }
        public virtual void Load(object data)
        {
            StatControllerData statControllerData = (StatControllerData)data;
            foreach (Stat stat in m_Stats.Values)
            {
                if (stat is ISavable savable)
                {
                    savable.Load(statControllerData.stats[stat.definition.name]);
                }
            }
        }

        [Serializable]
        protected class StatControllerData
        {
            public Dictionary<string, object> stats;
        }

        #endregion

        
    }
}
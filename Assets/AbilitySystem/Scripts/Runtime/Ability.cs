using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AbilitySystem.Scripts.Runtime;
using SaveSystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class Ability : ISavable
    {
        protected AbilityDefinition m_Definition;
        public AbilityDefinition definition => m_Definition;
        protected AbilityController m_Controller;

        public event Action levelChanged;
        private int m_Level;

        public int level
        {
            get => m_Level;
            internal set
            {
                int newLevel = Mathf.Min(value, definition.maxLevel);
                if (newLevel != m_Level)
                {
                    m_Level = newLevel;
                    levelChanged?.Invoke();
                }
            }
        }
        
        public Ability(AbilityDefinition definition, AbilityController controller)
        {
            m_Definition = definition;
            m_Controller = controller;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (GameplayEffectDefinition effectDefinition in definition.gameplayEffectDefinitions)
            {
                EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>().FirstOrDefault();
                GameplayEffect effect =
                    Activator.CreateInstance(attribute.type, effectDefinition, this, m_Controller.gameObject) as
                        GameplayEffect;
                stringBuilder.Append(effect).AppendLine();
            }

            return stringBuilder.ToString();
        }

        internal void ApplyEffects(GameObject other)
        {
            ApplyEffectsInternal(m_Definition.gameplayEffectDefinitions, other);
        }

        private void ApplyEffectsInternal(ReadOnlyCollection<GameplayEffectDefinition> effectDefinitions, GameObject other)
        {
            if (other.TryGetComponent(out GameplayEffectController effectController))
            {
                foreach (GameplayEffectDefinition effectDefinition in effectDefinitions)
                {
                    EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                        .OfType<EffectTypeAttribute>().FirstOrDefault();
                    GameplayEffect effect =
                        Activator.CreateInstance(attribute.type, effectDefinition, this, m_Controller.gameObject) as
                            GameplayEffect;
                    effectController.ApplyGameplayEffectToSelf(effect);
                }
            }
        }

        #region Save System

        public object data => new AbilityData
        {
            level = m_Level
        };
        public void Load(object data)
        {
            AbilityData abilityData = (AbilityData)data;
            m_Level = abilityData.level;
            levelChanged?.Invoke();
        }

        [Serializable]
        protected class AbilityData
        {
            public int level;
        }

        #endregion
        
    }
}
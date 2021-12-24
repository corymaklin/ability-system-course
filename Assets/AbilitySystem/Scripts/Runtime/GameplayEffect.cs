using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class GameplayEffect
    {
        protected GameplayEffectDefinition m_Definition;
        public GameplayEffectDefinition definition => m_Definition;
        private object m_Source;
        public object source => m_Source;
        private GameObject m_Instigator;
        public GameObject instigator => m_Instigator;
        private List<StatModifier> m_Modifiers = new List<StatModifier>();
        public ReadOnlyCollection<StatModifier> modifiers => m_Modifiers.AsReadOnly();

        public GameplayEffect(GameplayEffectDefinition definition, object source, GameObject instigator)
        {
            m_Definition = definition;
            m_Source = source;
            m_Instigator = instigator;

            StatController statController = instigator.GetComponent<StatController>();

            foreach (AbstractGameplayEffectStatModifierDefinition modifierDefinition in definition.modifierDefinitions)
            {
                StatModifier statModifier;
                if (modifierDefinition is GameplayEffectDamageDefinition damageDefinition)
                {
                    HealthModifier healthModifier = new HealthModifier
                    {
                        magnitude = Mathf.RoundToInt(modifierDefinition.formula.CalculateValue(instigator)),
                        isCriticalHit = false
                    };

                    if (damageDefinition.canCriticalHit)
                    {
                        if (statController.stats["CriticalHitChance"].value / 100f >= Random.value)
                        {
                            healthModifier.magnitude = Mathf.RoundToInt(healthModifier.magnitude *
                                statController.stats["CriticalHitMultiplier"].value / 100f);
                            healthModifier.isCriticalHit = true;
                        }
                    }

                    statModifier = healthModifier;
                }
                else
                {
                    statModifier = new StatModifier()
                    {
                        magnitude = Mathf.RoundToInt(modifierDefinition.formula.CalculateValue(instigator))
                    };
                }

                statModifier.source = this;
                statModifier.type = modifierDefinition.type;
                m_Modifiers.Add(statModifier);
            }
        }
    }
}
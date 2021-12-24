using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StatSystem;
using UnityEngine;
using Attribute = StatSystem.Attribute;

namespace AbilitySystem.Scripts.Runtime
{
    public class GameplayEffectController : MonoBehaviour
    {
        protected List<GameplayPersistentEffect> m_ActiveEffects = new List<GameplayPersistentEffect>();
        public ReadOnlyCollection<GameplayPersistentEffect> activeEffects => m_ActiveEffects.AsReadOnly();
        protected StatController m_StatController;


        private void Update()
        {
            HandleDuration();
        }

        private void Awake()
        {
            m_StatController = GetComponent<StatController>();
        }

        public void ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
        {
            if (effectToApply is GameplayPersistentEffect persistentEffect)
            {
                AddGameplayEffect(persistentEffect);
            }
            else
            {
                ExecuteGameplayEffect(effectToApply);   
            }
        }

        private void AddGameplayEffect(GameplayPersistentEffect effect)
        {
            m_ActiveEffects.Add(effect);
            AddUninhibitedEffects(effect);
        }

        private void RemoveActiveGameplayEffect(GameplayPersistentEffect effect, bool prematureRemoval)
        {
            m_ActiveEffects.Remove(effect);
            RemoveUninhibitedEffects(effect);
        }

        private void RemoveUninhibitedEffects(GameplayPersistentEffect effect)
        {
            foreach (var modifierDefinition in effect.definition.modifierDefinitions)
            {
                if (m_StatController.stats.TryGetValue(modifierDefinition.statName, out Stat stat))
                {
                    stat.RemoveModifierFromSource(effect);
                }
            }
        }

        private void AddUninhibitedEffects(GameplayPersistentEffect effect)
        {
            for (int i = 0; i < effect.modifiers.Count; i++)
            {
                if (m_StatController.stats.TryGetValue(effect.definition.modifierDefinitions[i].statName, out Stat stat))
                {
                    stat.AddModifier(effect.modifiers[i]);
                }
            }
        }

        private void ExecuteGameplayEffect(GameplayEffect effect)
        {
            for (int i = 0; i < effect.modifiers.Count; i++)
            {
                if (m_StatController.stats.TryGetValue(effect.definition.modifierDefinitions[i].statName,
                    out Stat stat))
                {
                    if (stat is Attribute attribute)
                    {
                        attribute.ApplyModifier(effect.modifiers[i]);
                    }
                }
            }
        }
        
        private void HandleDuration()
        {
            List<GameplayPersistentEffect> effectsToRemove = new List<GameplayPersistentEffect>();
            foreach (GameplayPersistentEffect activeEffect in m_ActiveEffects)
            {
                if (!activeEffect.definition.isInfinite)
                {
                    activeEffect.remainingDuration = Math.Max(activeEffect.remainingDuration - Time.deltaTime, 0f);
                    if (Mathf.Approximately(activeEffect.remainingDuration, 0f))
                    {
                        effectsToRemove.Add(activeEffect);
                    }
                }
            }

            foreach (GameplayPersistentEffect effect in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effect, false);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using Core;
using StatSystem;
using UnityEngine;
using Attribute = StatSystem.Attribute;

namespace AbilitySystem
{
    [RequireComponent(typeof(StatController))]
    [RequireComponent(typeof(TagController))]
    public partial class GameplayEffectController : MonoBehaviour
    {
        protected List<GameplayPersistentEffect> m_ActiveEffects = new List<GameplayPersistentEffect>();
        public ReadOnlyCollection<GameplayPersistentEffect> activeEffects => m_ActiveEffects.AsReadOnly();
        protected StatController m_StatController;
        protected TagController m_TagController;

        [SerializeField] private List<GameplayEffectDefinition> m_StartEffectDefinitions;
        public event Action initialized;
        private bool m_IsInitialized;
        public bool isInitialized => m_IsInitialized;
        private void Update()
        {
            HandleDuration();
        }

        private void Awake()
        {
            m_StatController = GetComponent<StatController>();
            m_TagController = GetComponent<TagController>();
        }

        private void OnEnable()
        {
            m_StatController.initialized += OnStatControllerInitialized;
            if (m_StatController.isInitialized)
            {
                OnStatControllerInitialized();
            }
        }

        private void OnStatControllerInitialized()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (GameplayEffectDefinition effectDefinition in m_StartEffectDefinitions)
            {
                EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>().FirstOrDefault();
                
                GameplayEffect effect = Activator.CreateInstance(attribute.type, effectDefinition, m_StartEffectDefinitions, gameObject) as GameplayEffect;
                ApplyGameplayEffectToSelf(effect);
            }

            m_IsInitialized = true;
            initialized?.Invoke();
        }

        public bool ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
        {
            foreach (GameplayPersistentEffect activeEffect in m_ActiveEffects)
            {
                foreach (string tag in activeEffect.definition.grantedApplicationImmunityTags)
                {
                    if (effectToApply.definition.tags.Contains(tag))
                    {
                        Debug.Log($"Immune to {effectToApply.definition.name}");
                        return false;
                    }
                }
            }

            if (!m_TagController.SatisfiesRequirements(effectToApply.definition.applicationMustBePresentTags,
                effectToApply.definition.applicationMustBeAbsentTags))
            {
                Debug.Log($"Failed to satisfy application requirements for {effectToApply.definition.name}");
                return false;
            }
            
            bool isAdded = true;
            if (effectToApply is GameplayStackableEffect stackableEffect)
            {
                GameplayStackableEffect existingStackableEffect = m_ActiveEffects.Find(activeEffect => activeEffect.definition == effectToApply.definition) as GameplayStackableEffect;

                if (existingStackableEffect != null)
                {
                    isAdded = false;

                    if (existingStackableEffect.stackCount == existingStackableEffect.definition.stackLimitCount)
                    {
                        foreach (GameplayEffectDefinition effectDefinition in existingStackableEffect.definition.overflowEffects)
                        {
                            EffectTypeAttribute attribute = effectDefinition.GetType().GetCustomAttributes(true)
                                .OfType<EffectTypeAttribute>().FirstOrDefault();
                            GameplayEffect overflowEffect = Activator.CreateInstance(attribute.type, effectDefinition, existingStackableEffect, gameObject) as GameplayEffect;
                            ApplyGameplayEffectToSelf(overflowEffect);
                        }

                        if (existingStackableEffect.definition.clearStackOnOverflow)
                        {
                            RemoveActiveGameplayEffect(existingStackableEffect, true);
                            isAdded = true;
                        }

                        if (existingStackableEffect.definition.denyOverflowApplication)
                        {
                            Debug.Log("Denied overflow application!");
                            return false;
                        }
                    }

                    if (!isAdded)
                    {
                        existingStackableEffect.stackCount =
                            Math.Min(existingStackableEffect.stackCount + stackableEffect.stackCount,
                                existingStackableEffect.definition.stackLimitCount);

                        if (existingStackableEffect.definition.stackDurationRefreshPolicy ==
                            GameplayEffectStackingDurationPolicy.RefreshOnSuccessfulApplication)
                        {
                            existingStackableEffect.remainingDuration = existingStackableEffect.duration;
                        }

                        if (existingStackableEffect.definition.stackPeriodResetPolicy ==
                            GameplayEffectStackingPeriodPolicy.ResetOnSuccessfulApplication)
                        {
                            existingStackableEffect.remainingPeriod = existingStackableEffect.definition.period;
                        }
                    }
                }
            }

            foreach (GameplayEffectDefinition conditionalEffectDefinition in effectToApply.definition.conditionalEffects)
            {
                EffectTypeAttribute attribute = conditionalEffectDefinition.GetType().GetCustomAttributes(true)
                    .OfType<EffectTypeAttribute>()
                    .FirstOrDefault();
                GameplayEffect conditionalEffect = Activator.CreateInstance(attribute.type, conditionalEffectDefinition, effectToApply, effectToApply.instigator) as GameplayEffect;
                ApplyGameplayEffectToSelf(conditionalEffect);
            }

            List<GameplayPersistentEffect> effectsToRemove = new List<GameplayPersistentEffect>();

            foreach (GameplayPersistentEffect activeEffect in m_ActiveEffects)
            {
                foreach (string tag in activeEffect.definition.tags)
                {
                    if (effectToApply.definition.removeEffectsWithTags.Contains(tag))
                    {
                        effectsToRemove.Add(activeEffect);
                    }
                }
            }

            foreach (GameplayPersistentEffect effectToRemove in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effectToRemove, true);
            }

            if (effectToApply is GameplayPersistentEffect persistentEffect)
            {
                if (isAdded)
                    AddGameplayEffect(persistentEffect);
            }
            else
            {
                ExecuteGameplayEffect(effectToApply);   
            }
            
            if (effectToApply.definition.specialEffectDefinition != null)
                PlaySpecialEffect(effectToApply);

            return true;
        }

        private void AddGameplayEffect(GameplayPersistentEffect effect)
        {
            m_ActiveEffects.Add(effect);
            AddUninhibitedEffects(effect);

            if (effect.definition.isPeriodic)
            {
                if (effect.definition.executePeriodicEffectOnApplication)
                {
                    ExecuteGameplayEffect(effect);
                }
            }
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

            foreach (string tag in effect.definition.grantedTags)
            {
                m_TagController.RemoveTag(tag);
            }
            
            if (effect.definition.specialPersistentEffectDefinition != null)
                StopSpecialEffect(effect);
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

            foreach (string tag in effect.definition.grantedTags)
            {
                m_TagController.AddTag(tag);
            }
            
            if (effect.definition.specialPersistentEffectDefinition != null)
                PlaySpecialEffect(effect);
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
                if (activeEffect.definition.isPeriodic)
                {
                    activeEffect.remainingPeriod = Math.Max(activeEffect.remainingPeriod - Time.deltaTime, 0f);

                    if (Mathf.Approximately(activeEffect.remainingPeriod, 0f))
                    {
                        ExecuteGameplayEffect(activeEffect);
                        activeEffect.remainingPeriod = activeEffect.definition.period;
                    }
                }
                
                if (!activeEffect.definition.isInfinite)
                {
                    activeEffect.remainingDuration = Math.Max(activeEffect.remainingDuration - Time.deltaTime, 0f);
                    if (Mathf.Approximately(activeEffect.remainingDuration, 0f))
                    {
                        if (activeEffect is GameplayStackableEffect stackableEffect)
                        {
                            switch (stackableEffect.definition.stackExpirationPolicy)
                            {
                                case GameplayEffectStackingExpirationPolicy.RemoveSingleStackAndRefreshDuration:
                                    stackableEffect.stackCount--;
                                    if (stackableEffect.stackCount == 0)
                                        effectsToRemove.Add(stackableEffect);
                                    else
                                        activeEffect.remainingDuration = activeEffect.duration;
                                    break;
                                case GameplayEffectStackingExpirationPolicy.NeverRefresh:
                                    effectsToRemove.Add(stackableEffect);
                                    break;
                            }
                        }
                        else
                        {
                            effectsToRemove.Add(activeEffect);
                        }
                    }
                }
            }

            foreach (GameplayPersistentEffect effect in effectsToRemove)
            {
                RemoveActiveGameplayEffect(effect, false);
            }
        }

        public bool CanApplyAttributeModifiers(GameplayEffectDefinition effectDefinition)
        {
            foreach (var modifierDefinition in effectDefinition.modifierDefinitions)
            {
                if (m_StatController.stats.TryGetValue(modifierDefinition.statName, out Stat stat))
                {
                    if (stat is Attribute attribute)
                    {
                        if (modifierDefinition.type == ModifierOperationType.Additive)
                        {
                            if (attribute.currentValue <
                                Mathf.Abs(modifierDefinition.formula.CalculateValue(gameObject)))
                            {
                                Debug.Log($"{effectDefinition.name} cannot satisfy costs!");
                                return false;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Only addition is supported!");
                            return false;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"{modifierDefinition.statName} is not an attribute!");
                        return false;
                    }
                }
                else
                {
                    Debug.LogWarning($"{modifierDefinition.statName} not found!");
                    return false;
                }
            }
            return true;
        }
    }
}
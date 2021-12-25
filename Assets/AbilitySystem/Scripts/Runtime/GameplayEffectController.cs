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
            
            if (effectToApply.definition.specialEffectDefinition != null)
                PlaySpecialEffect(effectToApply);
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
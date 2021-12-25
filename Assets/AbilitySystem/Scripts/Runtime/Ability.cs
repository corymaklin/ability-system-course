﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class Ability
    {
        protected AbilityDefinition m_Definition;
        public AbilityDefinition definition => m_Definition;
        protected AbilityController m_Controller;
        
        public Ability(AbilityDefinition definition, AbilityController controller)
        {
            m_Definition = definition;
            m_Controller = controller;
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
    }
}
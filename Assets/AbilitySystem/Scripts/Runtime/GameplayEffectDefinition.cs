﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    [EffectType(typeof(GameplayEffect))]
    [CreateAssetMenu(fileName = "GameplayEffect", menuName = "AbilitySystem/Effect/GameplayEffect", order = 0)]
    public class GameplayEffectDefinition : ScriptableObject
    {
        [SerializeField] protected List<AbstractGameplayEffectStatModifierDefinition> m_ModifierDefinitions;

        public ReadOnlyCollection<AbstractGameplayEffectStatModifierDefinition> modifierDefinitions =>
            m_ModifierDefinitions.AsReadOnly();

        [SerializeField] private SpecialEffectDefinition m_SpecialEffectDefinition;
        public SpecialEffectDefinition specialEffectDefinition => m_SpecialEffectDefinition;
    }
}
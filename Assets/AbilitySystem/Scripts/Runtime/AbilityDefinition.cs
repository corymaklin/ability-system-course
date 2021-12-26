using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class AbilityDefinition : ScriptableObject
    {
        [SerializeField] private List<GameplayEffectDefinition> m_GameplayEffectDefinitions;

        public ReadOnlyCollection<GameplayEffectDefinition> gameplayEffectDefinitions =>
            m_GameplayEffectDefinitions.AsReadOnly();

        [SerializeField] private int m_MaxLevel;
        public int maxLevel => m_MaxLevel;
    }
}
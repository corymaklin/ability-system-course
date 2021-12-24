using Core;
using StatSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public abstract class AbstractGameplayEffectStatModifierDefinition : ScriptableObject
    {
        public abstract string statName { get; }
        public abstract ModifierOperationType type { get; }
        [SerializeField] private NodeGraph m_Formula;
        public NodeGraph formula => m_Formula;
    }
}
using StatSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class GameplayEffectStatModifierDefinition : AbstractGameplayEffectStatModifierDefinition
    {
        [SerializeField] private string m_StatName;

        public override string statName => m_StatName;
        [SerializeField] private ModifierOperationType m_Type;

        public override ModifierOperationType type => m_Type;
    }
}
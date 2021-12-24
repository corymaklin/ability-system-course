using StatSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class GameplayEffectDamageDefinition : AbstractGameplayEffectStatModifierDefinition
    {
        public override string statName => "Health";
        public override ModifierOperationType type => ModifierOperationType.Additive;
        [SerializeField] private bool m_CanCriticalHit;
        public bool canCriticalHit => m_CanCriticalHit;
    }
}
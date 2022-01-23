using System.Collections.Generic;
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

        [SerializeField] private List<GameplayEffectDefinition> m_ConditionalEffects;
        public ReadOnlyCollection<GameplayEffectDefinition> conditionalEffects => m_ConditionalEffects.AsReadOnly();

        [SerializeField] private string m_Description;
        public string description => m_Description;

        [SerializeField] private SpecialEffectDefinition m_SpecialEffectDefinition;
        public SpecialEffectDefinition specialEffectDefinition => m_SpecialEffectDefinition;

        [SerializeField] private List<string> m_Tags;
        public ReadOnlyCollection<string> tags => m_Tags.AsReadOnly();

        [SerializeField] private List<string> m_RemoveEffectsWithTags;
        public ReadOnlyCollection<string> removeEffectsWithTags => m_RemoveEffectsWithTags.AsReadOnly();

        [SerializeField] private List<string> m_ApplicationMustBePresentTags;
        public ReadOnlyCollection<string> applicationMustBePresentTags => m_ApplicationMustBePresentTags.AsReadOnly();

        [SerializeField] private List<string> m_ApplicationMustBeAbsentTags;
        public ReadOnlyCollection<string> applicationMustBeAbsentTags => m_ApplicationMustBeAbsentTags.AsReadOnly();

    }
}
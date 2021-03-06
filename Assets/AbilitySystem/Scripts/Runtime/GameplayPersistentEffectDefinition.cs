using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    public enum GameplayEffectPeriodInhibitionRemovedPolicy
    {
        NeverReset,
        ResetPeriod,
        ExecuteAndResetPeriod
    }
    
    [EffectType(typeof(GameplayPersistentEffect))]
    [CreateAssetMenu(fileName = "GameplayPersistentEffect", menuName = "AbilitySystem/Effect/GameplayPersistentEffect", order = 0)]
    public class GameplayPersistentEffectDefinition : GameplayEffectDefinition
    {
        [SerializeField] protected bool m_IsInfinite;
        public bool isInfinite => m_IsInfinite;

        [SerializeField] protected NodeGraph m_DurationFormula;
        public NodeGraph durationFormula => m_DurationFormula;

        [SerializeField] protected bool m_IsPeriodic;
        public bool isPeriodic => m_IsPeriodic;

        [SerializeField] protected float m_Period;
        public float period => m_Period;

        [Tooltip("If true, the effect executes on application then at every interval")]
        [SerializeField] private bool m_ExecutePeriodicEffectOnApplication;
        public bool executePeriodicEffectOnApplication => m_ExecutePeriodicEffectOnApplication;

        [Tooltip("These tags are applied to the actor I am applied to")]
        [SerializeField] protected List<string> m_GrantedTags;
        public ReadOnlyCollection<string> grantedTags => m_GrantedTags.AsReadOnly();

        [SerializeField] private SpecialEffectDefinition m_SpecialPersistentEffectDefinition;
        public SpecialEffectDefinition specialPersistentEffectDefinition => m_SpecialPersistentEffectDefinition;

        [SerializeField] private List<string> m_GrantedApplicationImmunityTags;

        public ReadOnlyCollection<string> grantedApplicationImmunityTags =>
            m_GrantedApplicationImmunityTags.AsReadOnly();


        [SerializeField] private List<string> m_UninhibitedMustBePresentTags;
        public ReadOnlyCollection<string> uninhibitedMustBePresentTags => m_UninhibitedMustBePresentTags.AsReadOnly();

        [SerializeField] private List<string> m_UninhibitedMustBeAbsentTags;
        public ReadOnlyCollection<string> uninhibitedMustBeAbsentTags => m_UninhibitedMustBeAbsentTags.AsReadOnly();


        [SerializeField] private GameplayEffectPeriodInhibitionRemovedPolicy m_PeriodicInhibitionPolicy;
        public GameplayEffectPeriodInhibitionRemovedPolicy periodicInhibitionPolicy => m_PeriodicInhibitionPolicy;

        [SerializeField] private List<string> m_PersistMustBePresentTags;
        public ReadOnlyCollection<string> persistMustBePresentTags => m_PersistMustBePresentTags.AsReadOnly();

        [SerializeField] private List<string> m_PersistMustBeAbsentTags;
        public ReadOnlyCollection<string> persistMustBeAbsentTags => m_PersistMustBeAbsentTags.AsReadOnly();

    }
}
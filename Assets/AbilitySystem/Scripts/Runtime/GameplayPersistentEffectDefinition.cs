using System.Collections.Generic;
using System.Collections.ObjectModel;
using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    [EffectType(typeof(GameplayPersistentEffect))]
    [CreateAssetMenu(fileName = "GameplayPersistentEffect", menuName = "AbilitySystem/Effect/GameplayPersistentEffect", order = 0)]
    public class GameplayPersistentEffectDefinition : GameplayEffectDefinition
    {
        [SerializeField] protected bool m_IsInfinite;
        public bool isInfinite => m_IsInfinite;

        [SerializeField] protected NodeGraph m_DurationFormula;
        public NodeGraph durationFormula => m_DurationFormula;

        [Tooltip("These tags are applied to the actor I am applied to")]
        [SerializeField] protected List<string> m_GrantedTags;
        public ReadOnlyCollection<string> grantedTags => m_GrantedTags.AsReadOnly();
    }
}
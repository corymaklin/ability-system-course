using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    [CreateAssetMenu(fileName = "GameplayPersistentEffect", menuName = "AbilitySystem/Effect/GameplayPersistentEffect", order = 0)]
    public class GameplayPersistentEffectDefinition : GameplayEffectDefinition
    {
        [SerializeField] protected bool m_IsInfinite;
        public bool isInfinite => m_IsInfinite;

        [SerializeField] protected NodeGraph m_DurationFormula;
        public NodeGraph durationFormula => m_DurationFormula;
    }
}
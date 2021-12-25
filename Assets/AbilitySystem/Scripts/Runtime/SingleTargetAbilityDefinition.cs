using UnityEngine;

namespace AbilitySystem
{
    [AbilityType(typeof(SingleTargetAbility))]
    [CreateAssetMenu(fileName = "SingleTargetAbility", menuName = "AbilitySystem/Ability/SingleTargetAbility", order = 0)]
    public class SingleTargetAbilityDefinition : ActiveAbilityDefinition
    {
        
    }
}
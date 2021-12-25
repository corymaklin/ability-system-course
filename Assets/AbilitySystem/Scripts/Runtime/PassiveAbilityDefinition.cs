using UnityEngine;

namespace AbilitySystem
{
    [AbilityType(typeof(PassiveAbility))]
    [CreateAssetMenu(fileName = "PassiveAbility", menuName = "AbilitySystem/Ability/PassiveAbility", order = 0)]
    public class PassiveAbilityDefinition : AbilityDefinition
    {
        
    }
}
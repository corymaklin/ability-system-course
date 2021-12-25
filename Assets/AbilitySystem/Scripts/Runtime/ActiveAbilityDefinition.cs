using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class ActiveAbilityDefinition : AbilityDefinition
    {
        [SerializeField] protected string m_AnimationName;
        public string animationName => m_AnimationName;

        [SerializeField] protected GameplayEffectDefinition m_Cost;
        public GameplayEffectDefinition cost => m_Cost;

        [SerializeField] private GameplayPersistentEffectDefinition m_Cooldown;
        public GameplayPersistentEffectDefinition cooldown => m_Cooldown;
    }
}
using UnityEngine;

namespace AbilitySystem
{
    public abstract class ActiveAbilityDefinition : AbilityDefinition
    {
        [SerializeField] protected string m_AnimationName;
        public string animationName => m_AnimationName;

    }
}
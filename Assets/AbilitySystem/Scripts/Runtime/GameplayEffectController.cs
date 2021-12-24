using StatSystem;
using UnityEngine;

namespace AbilitySystem.Scripts.Runtime
{
    public class GameplayEffectController : MonoBehaviour
    {
        protected StatController m_StatController;
        
        private void Awake()
        {
            m_StatController = GetComponent<StatController>();
        }

        public void ApplyGameplayEffectToSelf(GameplayEffect effectToApply)
        {
            ExecuteGameplayEffect(effectToApply);
        }

        private void ExecuteGameplayEffect(GameplayEffect effect)
        {
            for (int i = 0; i < effect.modifiers.Count; i++)
            {
                if (m_StatController.stats.TryGetValue(effect.definition.modifierDefinitions[i].statName,
                    out Stat stat))
                {
                    if (stat is Attribute attribute)
                    {
                        attribute.ApplyModifier(effect.modifiers[i]);
                    }
                }
            }
        }
    }
}
using System.Collections.Generic;
using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    public partial class GameplayEffectController
    {
        private Dictionary<SpecialEffectDefinition, int> m_SpecialEffectCountMap =
            new Dictionary<SpecialEffectDefinition, int>();

        private Dictionary<SpecialEffectDefinition, VisualEffect> m_SpecialEffectMap =
            new Dictionary<SpecialEffectDefinition, VisualEffect>();

        private void PlaySpecialEffect(GameplayPersistentEffect effect)
        {
            VisualEffect visualEffect =
                Instantiate(effect.definition.specialPersistentEffectDefinition.prefab, transform);
            visualEffect.finished += visualEffect => Destroy(visualEffect.gameObject);

            if (effect.definition.specialPersistentEffectDefinition.location == PlayLocation.Center)
            {
                visualEffect.transform.localPosition = Utils.GetCenterOfCollider(transform);
            }
            else if (effect.definition.specialPersistentEffectDefinition.location == PlayLocation.Above)
            {
                visualEffect.transform.localPosition = Utils.GetComponentHeight(gameObject) * Vector3.up;
            }

            if (visualEffect.isLooping)
            {
                if (m_SpecialEffectCountMap.ContainsKey(effect.definition.specialPersistentEffectDefinition))
                {
                    m_SpecialEffectCountMap[effect.definition.specialPersistentEffectDefinition]++;
                }
                else
                {
                    m_SpecialEffectCountMap.Add(effect.definition.specialPersistentEffectDefinition, 1);
                    m_SpecialEffectMap.Add(effect.definition.specialPersistentEffectDefinition, visualEffect);
                }
            }
            
            visualEffect.Play();
        }

        private void PlaySpecialEffect(GameplayEffect effect)
        {
            VisualEffect visualEffect = Instantiate(effect.definition.specialEffectDefinition.prefab,
                transform.position, transform.rotation);
            visualEffect.finished += visualEffect => Destroy(visualEffect.gameObject);

            if (effect.definition.specialEffectDefinition.location == PlayLocation.Center)
            {
                visualEffect.transform.position += Utils.GetCenterOfCollider(transform);
            }
            else if (effect.definition.specialEffectDefinition.location == PlayLocation.Above)
            {
                visualEffect.transform.position += Utils.GetComponentHeight(gameObject) * Vector3.up;
            }
            visualEffect.Play();
        }

        private void StopSpecialEffect(GameplayPersistentEffect effect)
        {
            if (m_SpecialEffectCountMap.ContainsKey(effect.definition.specialPersistentEffectDefinition))
            {
                m_SpecialEffectCountMap[effect.definition.specialPersistentEffectDefinition]--;
                if (m_SpecialEffectCountMap[effect.definition.specialPersistentEffectDefinition] == 0)
                {
                    m_SpecialEffectCountMap.Remove(effect.definition.specialPersistentEffectDefinition);
                    VisualEffect visualEffect = m_SpecialEffectMap[effect.definition.specialPersistentEffectDefinition];
                    visualEffect.Stop();
                    m_SpecialEffectMap.Remove(effect.definition.specialPersistentEffectDefinition);
                }
            }
            else
            {
                Debug.LogWarning("Attempting to remove a status effect that does not exist!");
            }
        }
    }
}
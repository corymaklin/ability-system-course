using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    public partial class GameplayEffectController
    {
        private List<VisualEffect> m_StatusEffects = new List<VisualEffect>();
        private float m_Period = 1f;
        private int m_Index;
        private float m_RemainingPeriod;

        private Dictionary<SpecialEffectDefinition, int> m_SpecialEffectCountMap =
            new Dictionary<SpecialEffectDefinition, int>();

        private Dictionary<SpecialEffectDefinition, VisualEffect> m_SpecialEffectMap =
            new Dictionary<SpecialEffectDefinition, VisualEffect>();

        private void HandleStatusEffects()
        {
            if (m_StatusEffects.Count > 1)
            {
                m_RemainingPeriod = Mathf.Max(m_RemainingPeriod - Time.deltaTime, 0f);

                if (Mathf.Approximately(m_RemainingPeriod, 0f))
                {
                    m_StatusEffects[m_Index].gameObject.SetActive(false);
                    m_Index = (m_Index + 1) % m_StatusEffects.Count;
                    m_StatusEffects[m_Index].gameObject.SetActive(true);
                    m_RemainingPeriod = m_Period;
                }
            }
        }
        
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
                    if (effect.definition.tags.Any(tag => tag.StartsWith("status")))
                    {
                        m_StatusEffects.Add(visualEffect);
                    }
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
                    if (effect.definition.tags.Any(tag => tag.StartsWith("status")))
                    {
                        m_StatusEffects.Remove(visualEffect);
                    }
                }
            }
            else
            {
                Debug.LogWarning("Attempting to remove a status effect that does not exist!");
            }
        }
    }
}
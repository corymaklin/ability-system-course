using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    [RequireComponent(typeof(GameplayEffectController))]
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private List<AbilityDefinition> m_AbilityDefinitions;
        protected Dictionary<string, Ability> m_Abilities = new Dictionary<string, Ability>();
        public Dictionary<string, Ability> abilities => m_Abilities;

        private GameplayEffectController m_EffectController;

        protected virtual void Awake()
        {
            m_EffectController = GetComponent<GameplayEffectController>();
        }

        private void OnEnable()
        {
            m_EffectController.initialized += OnEffectControllerInitialized;
            if (m_EffectController.isInitialized)
            {
                OnEffectControllerInitialized();
            }
        }

        private void OnDisable()
        {
            m_EffectController.initialized -= OnEffectControllerInitialized;
        }

        private void OnEffectControllerInitialized()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            foreach (AbilityDefinition abilityDefinition in m_AbilityDefinitions)
            {
                AbilityTypeAttribute abilityTypeAttribute = abilityDefinition.GetType().GetCustomAttributes(true)
                    .OfType<AbilityTypeAttribute>().FirstOrDefault();
                Ability ability =
                    Activator.CreateInstance(abilityTypeAttribute.type, abilityDefinition, this) as Ability;
                m_Abilities.Add(abilityDefinition.name, ability);
                if (ability is PassiveAbility passiveAbility)
                {
                    passiveAbility.ApplyEffects(gameObject);
                }
            }
        }
    }
}
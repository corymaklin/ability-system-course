using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using Core;
using UnityEngine;

namespace AbilitySystem
{
    [RequireComponent(typeof(GameplayEffectController))]
    [RequireComponent(typeof(TagController))]
    public class AbilityController : MonoBehaviour
    {
        public event Action<ActiveAbility> activatedAbility; 
        [SerializeField] private List<AbilityDefinition> m_AbilityDefinitions;
        protected Dictionary<string, Ability> m_Abilities = new Dictionary<string, Ability>();
        public Dictionary<string, Ability> abilities => m_Abilities;

        private GameplayEffectController m_EffectController;
        private TagController m_TagController;
        public ActiveAbility currentAbility;
        public GameObject target;

        protected virtual void Awake()
        {
            m_EffectController = GetComponent<GameplayEffectController>();
            m_TagController = GetComponent<TagController>();
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

        public bool TryActivateAbility(string abilityName, GameObject target)
        {
            if (m_Abilities.TryGetValue(abilityName, out Ability ability))
            {
                if (ability is ActiveAbility activeAbility)
                {
                    if (!CanActivateAbility(activeAbility))
                        return false;
                    this.target = target;
                    currentAbility = activeAbility;
                    CommitAbility(activeAbility);
                    activatedAbility?.Invoke(activeAbility);
                    return true;
                }
            }
            Debug.Log($"Ability with name {abilityName} not found!");
            return false;
        }

        public bool CanActivateAbility(ActiveAbility ability)
        {
            if (ability.definition.cooldown != null)
            {
                if (m_TagController.ContainsAny(ability.definition.cooldown.grantedTags))
                {
                    Debug.Log($"{ability.definition.name} is on cooldown!");
                    return false;
                }
            }
            
            if (ability.definition.cost != null)
                return m_EffectController.CanApplyAttributeModifiers(ability.definition.cost);
            
            return true;
        }
        private void CommitAbility(ActiveAbility ability)
        {
            m_EffectController.ApplyGameplayEffectToSelf(new GameplayEffect(ability.definition.cost, ability, gameObject));
            m_EffectController.ApplyGameplayEffectToSelf(new GameplayPersistentEffect(ability.definition.cooldown, ability, gameObject));
        }
    }
}
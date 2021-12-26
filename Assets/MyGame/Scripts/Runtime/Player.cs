using AbilitySystem;
using AbilitySystem.UI;
using CombatSystem.Scripts.Runtime;
using LevelSystem;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AbilityController))]
    public class Player : CombatableCharacter
    {
        private ILevelable m_Levelable;
        [SerializeField] private AbilitiesUI m_AbilitiesUI;
        
        [SerializeField] private GameObject m_Target;
        private NavMeshAgent m_NavMeshAgent;
        private AbilityController m_AbilityController;
        

        protected override void Awake()
        {
            base.Awake();
            m_Levelable = GetComponent<ILevelable>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_AbilityController = GetComponent<AbilityController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_AbilityController.TryActivateAbility("Shock", m_Target);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                m_AbilityController.TryActivateAbility("Heal", m_Target);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                m_AbilityController.TryActivateAbility("Regeneration", m_Target);
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                m_AbilityController.TryActivateAbility("Poison", m_Target);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_AbilityController.TryActivateAbility("Malediction", m_Target);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                m_Levelable.currentExperience += 100;
            }
            
            if (Input.GetKeyDown(KeyCode.D))
            {
                m_AbilitiesUI.Show();
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                m_AbilitiesUI.Hide();
            }
        }
    }
}
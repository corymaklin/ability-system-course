using AbilitySystem;
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
        }
    }
}
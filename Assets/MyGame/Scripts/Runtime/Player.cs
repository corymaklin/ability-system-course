using CombatSystem.Scripts.Runtime;
using LevelSystem;
using StatSystem;
using UnityEngine;
using UnityEngine.AI;

namespace MyGame.Scripts
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Player : CombatableCharacter
    {
        private ILevelable m_Levelable;
        [SerializeField] private Transform m_Target;
        private NavMeshAgent m_NavMeshAgent;
        [SerializeField] private MeleeWeapon m_MeleeWeapon;
        

        protected override void Awake()
        {
            base.Awake();
            m_Levelable = GetComponent<ILevelable>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            if (m_MeleeWeapon != null)
                m_MeleeWeapon.hit += collision => ApplyDamage(m_MeleeWeapon, collision.target);
        }

        private void Start()
        {
            if (m_Target != null)
                m_NavMeshAgent.SetDestination(m_Target.position);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                (m_StatController.stats["Health"] as Attribute).ApplyModifier(new StatModifier
                {
                    magnitude = -10,
                    type = ModifierOperationType.Additive
                });
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                (m_StatController.stats["Mana"] as Attribute).ApplyModifier(new StatModifier
                {
                    magnitude = -10,
                    type = ModifierOperationType.Additive
                });
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_Levelable.currentExperience += 50;
            }
        }
    }
}
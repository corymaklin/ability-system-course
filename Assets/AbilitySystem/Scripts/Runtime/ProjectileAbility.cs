using CombatSystem.Scripts.Runtime;
using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace AbilitySystem
{
    public class ProjectileAbility : ActiveAbility
    {
        public new ProjectileAbilityDefinition definition => m_Definition as ProjectileAbilityDefinition;
        private ObjectPool<Projectile> m_Pool;
        protected CombatController m_CombatController;

        public ProjectileAbility(ProjectileAbilityDefinition definition, AbilityController controller) : base(definition, controller)
        {
            m_Pool = new ObjectPool<Projectile>(OnCreate, OnGet, OnRelease);
            m_CombatController = controller.GetComponent<CombatController>();
        }

        private void OnRelease(Projectile projectile)
        {
            projectile.rigidbody.velocity = Vector3.zero;
            projectile.gameObject.SetActive(false);
        }

        private void OnGet(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private Projectile OnCreate()
        {
            Projectile projectile = GameObject.Instantiate(definition.projectilePrefab);
            projectile.hit += OnHit;
            return projectile;
        }

        private void OnHit(CollisionData data)
        {
            OnRelease(data.source as Projectile);
            ApplyEffects(data.target);
        }

        public void Shoot(GameObject target)
        {
            if (m_CombatController.rangedWeapons.TryGetValue(definition.weaponId, out RangedWeapon rangedWeapon))
            {
                Projectile projectile = m_Pool.Get();
                rangedWeapon.Shoot(
                    projectile,
                    target.transform,
                    definition.projectileSpeed,
                    definition.shotType,
                    definition.isSpinning);
            }
            else
            {
                Debug.LogWarning($"Could not find weapon {definition.weaponId}");
            }
        }
    }
}
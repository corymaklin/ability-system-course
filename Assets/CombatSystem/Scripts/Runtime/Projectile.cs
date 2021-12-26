using System;
using CombatSystem.Scripts.Runtime.Core;
using Core;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        protected Rigidbody m_Rigidbody;
        public Rigidbody rigidbody => m_Rigidbody;

        protected Collider m_Collider;

        public event Action<CollisionData> hit;

        [SerializeField] private VisualEffect m_CollisionVisualEffectPrefab;

        protected void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision other)
        {
            HandleCollision(other.gameObject);
        }

        protected void HandleCollision(GameObject other)
        {
            if (m_CollisionVisualEffectPrefab != null)
            {
                VisualEffect collisionVisualEffect =
                    Instantiate(m_CollisionVisualEffectPrefab, transform.position, transform.rotation);
                collisionVisualEffect.finished += effect => Destroy(effect.gameObject);
                collisionVisualEffect.Play();
            }
            hit?.Invoke(new CollisionData
            {
                source = this,
                target = other
            });
        }
    }
}
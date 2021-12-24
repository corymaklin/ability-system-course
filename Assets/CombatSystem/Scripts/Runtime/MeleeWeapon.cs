using System;
using CombatSystem.Scripts.Runtime.Core;
using UnityEngine;

namespace CombatSystem.Scripts.Runtime
{
    public class MeleeWeapon : MonoBehaviour
    {
        public event Action<CollisionData> hit;

        private void OnTriggerEnter(Collider other)
        {
            hit?.Invoke(new CollisionData
            {
                target = other.gameObject,
                source = this
            });
        }
    }
}
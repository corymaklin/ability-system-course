using CombatSystem.Scripts.Runtime.Core;
using Core;
using UnityEngine;
using UnityEngine.Pool;

namespace CombatSystem.Scripts.Runtime
{
    [RequireComponent(typeof(Collider))]
    public class CombatController : MonoBehaviour
    {
        [SerializeField] private FloatingText m_FloatingTextPrefab;
        private ObjectPool<FloatingText> m_Pool;
        private Collider m_Collider;
        private IDamageable m_Damageable;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Damageable = GetComponent<IDamageable>();
            m_Pool = new ObjectPool<FloatingText>(OnCreate, OnGet, OnRelease);
        }

        private void OnEnable()
        {
            if (!m_Collider.enabled)
                m_Collider.enabled = true;

            m_Damageable.initialized += OnDamageableInitialized;
            m_Damageable.willUninitialize += OnDamageableWillUninitialize;
            if (m_Damageable.isInitialized)
                OnDamageableInitialized();
        }

        private void OnDamageableWillUninitialize()
        {
            m_Damageable.damaged -= DisplayDamage;
            m_Damageable.healed -= DisplayRestorationAmount;
            m_Damageable.defeated -= OnDefeated;
        }


        private void OnDamageableInitialized()
        {
            m_Damageable.damaged += DisplayDamage;
            m_Damageable.healed += DisplayRestorationAmount;
            m_Damageable.defeated += OnDefeated;
        }

        private void OnDefeated()
        {
            m_Collider.enabled = false;
        }

        private void DisplayRestorationAmount(int amount)
        {
            FloatingText floatingText = m_Pool.Get();
            floatingText.Set(amount.ToString(), Color.green);
        }

        private void DisplayDamage(int magnitude, bool isCriticalHit)
        {
            FloatingText damageText = m_Pool.Get();
            damageText.Set(magnitude.ToString(), isCriticalHit ? Color.red : Color.white);
            if (isCriticalHit)
                damageText.transform.localScale *= 2;
        }

        private void OnRelease(FloatingText floatingText)
        {
            floatingText.gameObject.SetActive(false);
        }

        private void OnGet(FloatingText floatingText)
        {
            floatingText.transform.position = transform.position + GetCenterOfCollider();
            floatingText.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            floatingText.gameObject.SetActive(true);
        }

        private Vector3 GetCenterOfCollider()
        {
            Vector3 center;
            switch (m_Collider)
            {
                case CapsuleCollider capsuleCollider:
                    center = capsuleCollider.center;
                    break;
                case CharacterController characterController:
                    center = characterController.center;
                    break;
                default:
                    center = Vector3.zero;
                    Debug.LogWarning("Could not find center");
                    break;
            }

            return center;
        }

        private FloatingText OnCreate()
        {
            FloatingText floatingText = Instantiate(m_FloatingTextPrefab);
            floatingText.finished += m_Pool.Release;
            return floatingText;
        }
    }
}
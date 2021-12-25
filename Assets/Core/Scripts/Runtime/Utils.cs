using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class Utils
    {
        public static Vector3 GetCenterOfCollider(Transform target)
        {
            Vector3 center;
            Collider collider = target.GetComponent<Collider>();
            switch (collider)
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
        
        public static float GetComponentHeight(GameObject target)
        {
            float height;
            if (target.TryGetComponent(out NavMeshAgent navMeshAgent))
            {
                height = navMeshAgent.height;
            }
            else if (target.TryGetComponent(out CharacterController characterController))
            {
                height = characterController.height;
            }
            else
            {
                height = 0f;
                Debug.LogWarning("Could not determine height!");
            }

            return height;
        }
    }
}
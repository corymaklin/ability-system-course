using UnityEngine;

namespace Core
{
    public enum PlayLocation
    {
        Above,
        Center,
        Below
    }
    [CreateAssetMenu(fileName = "SpecialEffect", menuName = "Core/SpecialEffect", order = 0)]
    public class SpecialEffectDefinition : ScriptableObject
    {
        [SerializeField] private PlayLocation m_Location;
        public PlayLocation location => m_Location;

        [SerializeField] private VisualEffect m_Prefab;
        public VisualEffect prefab => m_Prefab;
    }
}
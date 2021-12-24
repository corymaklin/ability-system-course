using UnityEngine;

namespace StatSystem
{
    public enum ModifierOperationType
    {
        Additive,
        Multiplicative,
        Override
    }
    
    public class StatModifier
    {
        public object source { get; set; }
        public int magnitude { get; set; }
        public ModifierOperationType type { get; set; }
    }
}
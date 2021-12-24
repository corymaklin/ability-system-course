using UnityEngine;

namespace Core
{
    public interface IDamage
    {
        bool isCriticalHit { get; }
        int magnitude { get; }
        GameObject instigator { get; }
        Object source { get; }
    }
}
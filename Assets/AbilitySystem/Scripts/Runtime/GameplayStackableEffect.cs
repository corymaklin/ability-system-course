using UnityEngine;

namespace AbilitySystem
{
    public class GameplayStackableEffect : GameplayPersistentEffect
    {
        public new GameplayStackableEffectDefinition definition => m_Definition as GameplayStackableEffectDefinition;
        public int stackCount;
        public GameplayStackableEffect(GameplayStackableEffectDefinition definition, object source, GameObject instigator) : base(definition, source, instigator)
        {
            stackCount = 1;
        }
    }
}
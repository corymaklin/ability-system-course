﻿using AbilitySystem.Scripts.Runtime;
using UnityEngine;

namespace AbilitySystem
{
    public class GameplayPersistentEffect : GameplayEffect
    {
        public new GameplayPersistentEffectDefinition definition => m_Definition as GameplayPersistentEffectDefinition;
        public float remainingDuration;
        private float m_Duration;
        public float duration => m_Duration;
        
        public GameplayPersistentEffect(GameplayPersistentEffectDefinition definition, object source, GameObject instigator) : base(definition, source, instigator)
        {
            if (!definition.isInfinite)
                remainingDuration = m_Duration = definition.durationFormula.CalculateValue(instigator);
        }
    }
}
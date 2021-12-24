using System;

namespace AbilitySystem
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EffectTypeAttribute : Attribute
    {
        public readonly Type type;

        public EffectTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}
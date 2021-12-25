using System;

namespace AbilitySystem
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AbilityAttributeType : Attribute
    {
        public readonly Type type;

        public AbilityAttributeType(Type type)
        {
            this.type = type;
        }
    }
}
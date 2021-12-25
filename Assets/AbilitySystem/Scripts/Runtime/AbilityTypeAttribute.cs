using System;

namespace AbilitySystem
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AbilityTypeAttribute : Attribute
    {
        public readonly Type type;

        public AbilityTypeAttribute(Type type)
        {
            this.type = type;
        }
    }
}
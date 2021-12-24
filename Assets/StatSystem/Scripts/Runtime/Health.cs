using Core;

namespace StatSystem
{
    public class Health : Attribute
    {
        public Health(StatDefinition definition, StatController controller) : base(definition, controller)
        {
        }

        public override void ApplyModifier(StatModifier modifier)
        {
            ITaggable source = modifier.source as ITaggable;

            if (source != null)
            {
                if (source.tags.Contains("physical"))
                {
                    modifier.magnitude += m_Controller.stats["PhysicalDefense"].value;
                }
                else if (source.tags.Contains("magical"))
                {
                    modifier.magnitude += m_Controller.stats["MagicalDefense"].value;
                }
                else if (source.tags.Contains("pure"))
                {
                    // do nothing
                }
            }
            
            base.ApplyModifier(modifier);
        }
    }
}
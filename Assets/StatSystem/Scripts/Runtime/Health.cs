using Core;

namespace StatSystem
{
    public class Health : Attribute
    {
        private TagController m_TagController;
        public Health(StatDefinition definition, StatController statController, TagController tagController) : base(definition, statController)
        {
            m_TagController = tagController;
        }

        public override void ApplyModifier(StatModifier modifier)
        {
            ITaggable source = modifier.source as ITaggable;

            if (m_TagController.Contains("zombify"))
            {
                if (source.tags.Contains("healing"))
                {
                    modifier.magnitude *= -1;
                }
            }

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
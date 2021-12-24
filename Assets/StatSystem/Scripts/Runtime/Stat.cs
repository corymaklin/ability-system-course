using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StatSystem
{
    public class Stat
    {
        protected StatDefinition m_Definition;
        protected StatController m_Controller;
        public StatDefinition definition => m_Definition;
        protected int m_Value;
        public int value => m_Value;
        public virtual int baseValue => m_Definition.baseValue;
        public event Action valueChanged;
        protected List<StatModifier> m_Modifiers = new List<StatModifier>();

        public Stat(StatDefinition definition, StatController controller)
        {
            m_Definition = definition;
            m_Controller = controller;
        }

        public virtual void Initialize()
        {
            CalculateValue();
        }

        public void AddModifier(StatModifier modifier)
        {
            m_Modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(Object source)
        {
            m_Modifiers = m_Modifiers.Where(m => m.source.GetInstanceID() != source.GetInstanceID()).ToList();
            CalculateValue();
        }

        internal void CalculateValue()
        {
            int newValue = baseValue;

            if (m_Definition.formula != null && m_Definition.formula.rootNode != null)
            {
                newValue += Mathf.RoundToInt(m_Definition.formula.rootNode.value);
            }
            
            m_Modifiers.Sort((x, y) => x.type.CompareTo(y.type));

            for (int i = 0; i < m_Modifiers.Count; i++)
            {
                StatModifier modifier = m_Modifiers[i];
                if (modifier.type == ModifierOperationType.Additive)
                {
                    newValue += modifier.magnitude;
                }
                else if (modifier.type == ModifierOperationType.Multiplicative)
                {
                    newValue *= modifier.magnitude;
                }
            }
            
            if (m_Definition.cap >= 0)
            {
                newValue = Mathf.Min(newValue, m_Definition.cap);
            }

            if (m_Value != newValue)
            {
                m_Value = newValue;
                valueChanged?.Invoke();
            }
        }
    }
}

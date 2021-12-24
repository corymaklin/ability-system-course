using UnityEngine;

namespace Core.Nodes
{
    public class ScalarNode : CodeFunctionNode
    {
        [SerializeField] protected float m_Value;
        public override float value => m_Value;
    }
}
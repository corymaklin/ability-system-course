using UnityEngine;

namespace Core.Nodes
{
    public abstract class CodeFunctionNode : AbstractNode
    {
        public abstract float value { get; }
        public abstract float CalculateValue(GameObject source);
    }
}
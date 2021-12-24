using UnityEngine;

namespace Core.Nodes
{
    public class ResultNode : CodeFunctionNode
    {
        [HideInInspector] public CodeFunctionNode child;
        public override float value => child.value;
        public override float CalculateValue(GameObject source)
        {
            return child.CalculateValue(source);
        }
    }
}
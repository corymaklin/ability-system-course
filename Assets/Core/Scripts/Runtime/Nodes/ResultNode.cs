using UnityEngine;

namespace Core.Nodes
{
    public class ResultNode : CodeFunctionNode
    {
        [HideInInspector] public CodeFunctionNode child;
        public override float value => child.value;
    }
}
using Core.Nodes;
using UnityEngine;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(MultiplyNode))]
    [Title("Math", "Multiply")]
    public class MultiplyNodeView : NodeView 
    {
        public MultiplyNodeView()
        {
            title = "Multiply";
            node = ScriptableObject.CreateInstance<MultiplyNode>();
            output = CreateOutputPort();
            inputs.Add(CreateInputPort("A"));
            inputs.Add(CreateInputPort("B"));
        }
    }
}
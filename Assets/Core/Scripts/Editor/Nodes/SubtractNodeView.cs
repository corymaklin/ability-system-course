using Core.Nodes;
using UnityEngine;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(SubtractNode))]
    [Title("Math", "Subtract")]
    public class SubtractNodeView : NodeView
    {
        public SubtractNodeView()
        {
            title = "Subtract";
            node = ScriptableObject.CreateInstance<SubtractNode>();
            output = CreateOutputPort();
            inputs.Add(CreateInputPort("A"));
            inputs.Add(CreateInputPort("B"));
        }
    }
}
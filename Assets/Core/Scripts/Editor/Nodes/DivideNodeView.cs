using Core.Nodes;
using UnityEngine;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(DivideNode))]
    [Title("Math", "Divide")]
    public class DivideNodeView : NodeView
    {
        public DivideNodeView()
        {
            title = "Divide";
            node = ScriptableObject.CreateInstance<DivideNode>();
            output = CreateOutputPort();
            inputs.Add(CreateInputPort("A"));
            inputs.Add(CreateInputPort("B"));
        }
    }
}
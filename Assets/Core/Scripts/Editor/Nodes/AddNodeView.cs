using Core.Nodes;
using UnityEngine;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(AddNode))]
    [Title("Math", "Add")]
    public class AddNodeView : NodeView
    {
        public AddNodeView()
        {
            title = "Add";
            node = ScriptableObject.CreateInstance<AddNode>();
            output = CreateOutputPort();
            inputs.Add(CreateInputPort("A"));
            inputs.Add(CreateInputPort("B"));
        }
    }
}
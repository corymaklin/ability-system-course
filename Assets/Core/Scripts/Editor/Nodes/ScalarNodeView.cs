using Core.Nodes;
using UnityEngine;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(ScalarNode))]
    [Title("Math", "Scalar")]
    public class ScalarNodeView : NodeView
    {
        public ScalarNodeView()
        {
            title = "Scalar";
            node = ScriptableObject.CreateInstance<ScalarNode>();
            output = CreateOutputPort();
        }
    }
}
using Core;
using Core.Editor;
using Core.Editor.Nodes;
using StatSystem.Nodes;
using UnityEngine;

namespace StatSystem.Editor.Nodes
{
    [NodeType(typeof(StatNode))]
    [Title("Stat System", "Stat")]
    public class StatNodeView : NodeView
    {
        public StatNodeView()
        {
            title = "Stat";
            node = ScriptableObject.CreateInstance<StatNode>();
            output = CreateOutputPort();
        }
    }
}
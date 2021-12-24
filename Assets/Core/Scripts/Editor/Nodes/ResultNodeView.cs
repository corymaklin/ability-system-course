using Core.Nodes;

namespace Core.Editor.Nodes
{
    [NodeType(typeof(ResultNode))]
    public class ResultNodeView : NodeView
    {
        public ResultNodeView()
        {
            title = "Result";
            inputs.Add(CreateInputPort());
        }
    }
}
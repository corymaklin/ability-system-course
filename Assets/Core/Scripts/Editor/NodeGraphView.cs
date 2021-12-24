using System;
using System.Collections.Generic;
using System.Linq;
using Core.Editor.Nodes;
using Core.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Core.Editor
{
    public class NodeGraphView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<NodeGraphView, UxmlTraits> {}

        private NodeGraph m_NodeGraph;
        public Action<NodeView> nodeSelected;

        public NodeGraphView()
        {
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            GridBackground gridBackground = new GridBackground();
            Insert(0, gridBackground);
            gridBackground.StretchToParentSize();

            var styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Core/Scripts/Editor/NodeGraphEditorWindow.uss");
            styleSheets.Add(styleSheet);
        }

        internal void PopulateView(NodeGraph nodeGraph)
        {
            m_NodeGraph = nodeGraph;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements.ToList());
            graphViewChanged += OnGraphViewChanged;

            if (m_NodeGraph.rootNode == null)
            {
                m_NodeGraph.rootNode = ScriptableObject.CreateInstance<ResultNode>();
                m_NodeGraph.rootNode.name = nodeGraph.rootNode.GetType().Name;
                m_NodeGraph.rootNode.guid = GUID.Generate().ToString();
                m_NodeGraph.AddNode(m_NodeGraph.rootNode);
            }
            
            m_NodeGraph.nodes.ForEach(n => CreateAndAddNodeView(n));
            
            m_NodeGraph.nodes.ForEach(n =>
            {
                if (n is IntermediateNode intermediateNode)
                {
                    NodeView parentView = FindNodeView(n);
                    for (int i = 0; i < intermediateNode.children.Count; i++)
                    {
                        NodeView childView = FindNodeView(intermediateNode.children[i]);
                        Edge edge = parentView.inputs[i].ConnectTo(childView.output);
                        AddElement(edge);
                    }
                }
                else if (n is ResultNode rootNode)
                {
                    if (rootNode.child != null)
                    {
                        NodeView parentView = FindNodeView(n);
                        NodeView childView = FindNodeView(rootNode.child);
                        Edge edge = parentView.inputs[0].ConnectTo(childView.output);
                        AddElement(edge);
                    }
                }
            });
        }

        private NodeView FindNodeView(CodeFunctionNode node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(element =>
                {
                    if (element is NodeView nodeView)
                    {
                        m_NodeGraph.DeleteNode(nodeView.node);
                    }
                    else if (element is Edge edge)
                    {
                        NodeView parentView = edge.input.node as NodeView;
                        NodeView childView = edge.output.node as NodeView;
                        m_NodeGraph.RemoveChild(parentView.node, childView.node, edge.input.portName);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.input.node as NodeView;
                    NodeView childView = edge.output.node as NodeView;
                    m_NodeGraph.AddChild(parentView.node, childView.node, edge.input.portName);
                });
            }

            return graphViewChange;
        }

        private void CreateAndAddNodeView(CodeFunctionNode node)
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(NodeView).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToArray();

            foreach (Type type in types)
            {
                if (type.GetCustomAttributes(typeof(NodeType), false) is NodeType[] attrs && attrs.Length > 0)
                {
                    if (attrs[0].type == node.GetType())
                    {
                        NodeView nodeView = (NodeView)Activator.CreateInstance(type);
                        nodeView.node = node;
                        nodeView.viewDataKey = node.guid;
                        nodeView.style.left = node.position.x;
                        nodeView.style.top = node.position.y;
                        AddNodeView(nodeView);
                    }
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()
                .Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        internal void AddNodeView(NodeView nodeView)
        {
            nodeView.nodeSelected = nodeSelected;
            AddElement(nodeView);
        }
    }
}
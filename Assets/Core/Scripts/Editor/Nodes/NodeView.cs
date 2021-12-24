using System;
using System.Collections.Generic;
using Core.Nodes;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Editor.Nodes
{
    public class NodeView : Node
    {
        public CodeFunctionNode node;
        public List<Port> inputs = new List<Port>();
        public Port output;
        public Action<NodeView> nodeSelected;

        protected Port CreateOutputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Single)
        {
            Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, capacity, typeof(float));
            outputPort.portName = portName;
            outputContainer.Add(outputPort);
            RefreshPorts();
            return outputPort;
        }
        
        protected Port CreateInputPort(string portName = "", Port.Capacity capacity = Port.Capacity.Single)
        {
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, capacity, typeof(float));
            inputPort.portName = portName;
            inputContainer.Add(inputPort);
            RefreshPorts();
            return inputPort;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            nodeSelected?.Invoke(this);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Core.Nodes
{
    public class SubtractNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode minuend;
        [HideInInspector] public CodeFunctionNode subtrahend;
        public override float value => minuend.value - subtrahend.value;
        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                minuend = null;
            }
            else
            {
                subtrahend = null;
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                minuend = child;
            }
            else
            {
                subtrahend = child;
            }
        }

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new List<CodeFunctionNode>();
                if (minuend != null)
                {
                    nodes.Add(minuend);
                }

                if (subtrahend != null)
                {
                    nodes.Add(subtrahend);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}
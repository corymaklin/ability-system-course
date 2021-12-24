using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Core.Nodes
{
    public class AddNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode addendA;
        [HideInInspector] public CodeFunctionNode addendB;
        public override float value => addendA.value + addendB.value;
        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                addendA = null;
            }
            else
            {
                addendB = null;
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                addendA = child;
            }
            else
            {
                addendB = child;
            }
        }

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new List<CodeFunctionNode>();
                if (addendA != null)
                {
                    nodes.Add(addendA);
                }

                if (addendB != null)
                {
                    nodes.Add(addendB);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}
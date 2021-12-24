using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Core.Nodes
{
    public class MultiplyNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode factorA;
        [HideInInspector] public CodeFunctionNode factorB;
        public override float value => factorA.value * factorB.value;
        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                factorA = null;
            }
            else
            {
                factorB = null;
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                factorA = child;
            }
            else
            {
                factorB = child;
            }
        }

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new List<CodeFunctionNode>();
                if (factorA != null)
                {
                    nodes.Add(factorA);
                }

                if (factorB != null)
                {
                    nodes.Add(factorB);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}
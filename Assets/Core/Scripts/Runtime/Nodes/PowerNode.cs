using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Core.Nodes
{
    public class PowerNode : IntermediateNode
    {
        [HideInInspector] public CodeFunctionNode exponent;
        [HideInInspector] public CodeFunctionNode @base;
        public override float value => (float)Math.Pow(@base.value, exponent.value);
        public override float CalculateValue(GameObject source)
        {
            return (float)Math.Pow(@base.CalculateValue(source), exponent.CalculateValue(source));
        }

        public override void RemoveChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                @base = null;
            }
            else
            {
                exponent = null;
            }
        }

        public override void AddChild(CodeFunctionNode child, string portName)
        {
            if (portName.Equals("A"))
            {
                @base = child;
            }
            else
            {
                exponent = child;
            }
        }

        public override ReadOnlyCollection<CodeFunctionNode> children
        {
            get
            {
                List<CodeFunctionNode> nodes = new List<CodeFunctionNode>();
                if (@base != null)
                {
                    nodes.Add(@base);
                }

                if (exponent != null)
                {
                    nodes.Add(exponent);
                }

                return nodes.AsReadOnly();
            }
        }
    }
}
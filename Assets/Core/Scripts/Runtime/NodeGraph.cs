using System.Collections.Generic;
using Core.Nodes;
using UnityEditor;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "NodeGraph", menuName = "Core/NodeGraph", order = 0)]
    public class NodeGraph : ScriptableObject
    {
        public CodeFunctionNode rootNode;
        public List<CodeFunctionNode> nodes = new List<CodeFunctionNode>();

        public List<T> FindNodesOfType<T>()
        {
            List<T> nodesOfType = new List<T>();
            foreach (CodeFunctionNode node in nodes)
            {
                if (node is T nodeOfType)
                {
                    nodesOfType.Add(nodeOfType);
                }
            }

            return nodesOfType;
        }

        public void AddNode(CodeFunctionNode node)
        {
            nodes.Add(node);
            AssetDatabase.AddObjectToAsset(node, this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void DeleteNode(CodeFunctionNode node)
        {
            nodes.Remove(node);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public void RemoveChild(CodeFunctionNode parent, CodeFunctionNode child, string portName)
        {
            if (parent is IntermediateNode intermediateNode)
            {
                intermediateNode.RemoveChild(child, portName);
                EditorUtility.SetDirty(intermediateNode);
            }
            else if (parent is ResultNode resultNode)
            {
                resultNode.child = null;
                EditorUtility.SetDirty(resultNode);
            }
        }

        public void AddChild(CodeFunctionNode parent, CodeFunctionNode child, string portName)
        {
            if (parent is IntermediateNode intermediateNode)
            {
                intermediateNode.AddChild(child, portName);
                EditorUtility.SetDirty(intermediateNode);
            }
            else if (parent is ResultNode resultNode)
            {
                resultNode.child = child;
                EditorUtility.SetDirty(resultNode);
            }
        }
    }
}
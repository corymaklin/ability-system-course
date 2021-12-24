using Core.Nodes;
using UnityEngine;

namespace StatSystem.Nodes
{
    public class StatNode : CodeFunctionNode
    {
        [SerializeField] private string m_StatName;
        public string statName => m_StatName;
        public Stat stat;
        public override float value => stat.value;
    }
}
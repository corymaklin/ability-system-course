using Core.Nodes;
using UnityEngine;

namespace LevelSystem.Nodes
{
    public class LevelNode : CodeFunctionNode
    {
        public ILevelable levelable;
        public override float value => levelable.level;
        public override float CalculateValue(GameObject source)
        {
            ILevelable levelable = source.GetComponent<ILevelable>();
            return levelable.level;
        }
    }
}
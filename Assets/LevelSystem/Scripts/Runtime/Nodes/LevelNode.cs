using Core.Nodes;

namespace LevelSystem.Nodes
{
    public class LevelNode : CodeFunctionNode
    {
        public ILevelable levelable;
        public override float value => levelable.level;
    }
}
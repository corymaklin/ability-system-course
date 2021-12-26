using Core.Nodes;
using UnityEngine;

namespace AbilitySystem
{
    public class AbilityLevelNode : CodeFunctionNode
    {
        [SerializeField] private string m_AbilityName;
        public string abilityName => m_AbilityName;
        public Ability ability;
        public override float value => ability.level;
        public override float CalculateValue(GameObject source)
        {
            AbilityController abilityController = source.GetComponent<AbilityController>();
            return abilityController.abilities[m_AbilityName].level;
        }
    }
}
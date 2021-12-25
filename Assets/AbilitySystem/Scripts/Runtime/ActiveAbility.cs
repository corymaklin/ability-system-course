namespace AbilitySystem
{
    public class ActiveAbility : Ability
    {
        public new ActiveAbilityDefinition definition => m_Definition as ActiveAbilityDefinition;
        public ActiveAbility(ActiveAbilityDefinition definition, AbilityController controller) : base(definition, controller)
        {
        }
    }
}
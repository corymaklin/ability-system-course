using System.Text;

namespace AbilitySystem
{
    public class ActiveAbility : Ability
    {
        public new ActiveAbilityDefinition definition => m_Definition as ActiveAbilityDefinition;
        public ActiveAbility(ActiveAbilityDefinition definition, AbilityController controller) : base(definition, controller)
        {
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(base.ToString());

            if (definition.cost != null)
            {
                GameplayEffect cost = new GameplayEffect(definition.cost, this, m_Controller.gameObject);
                stringBuilder.Append(cost).AppendLine();
            }

            if (definition.cooldown != null)
            {
                GameplayPersistentEffect cooldown =
                    new GameplayPersistentEffect(definition.cooldown, this, m_Controller.gameObject);
                stringBuilder.Append(cooldown);
            }

            return stringBuilder.ToString();
        }
    }
}
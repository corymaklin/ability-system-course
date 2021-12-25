using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(GameplayPersistentEffectDefinition))]
    public class GameplayPersistentEffectEditor : GameplayEffectEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateDurationFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }

        private void RegisterCallbacks(VisualElement root)
        {
            GameplayPersistentEffectDefinition definition = target as GameplayPersistentEffectDefinition;

            PropertyField durationField = root.Q<PropertyField>("duration-formula");
            PropertyField isInfiniteField = root.Q<PropertyField>("is-infinite");

            durationField.style.display = definition.isInfinite ? DisplayStyle.None : DisplayStyle.Flex;
            isInfiniteField.RegisterValueChangeCallback(evt =>
            {
                durationField.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            });
        }

        private VisualElement CreateDurationFieldsGUI()
        {
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_IsInfinite")) { name = "is-infinite"});
            root.Add(new PropertyField(serializedObject.FindProperty("m_DurationFormula")) { name = "duration-formula"});

            return root;
        }
        
        protected override VisualElement CreateSpecialEffectFieldsGUI()
        {
            VisualElement root = base.CreateSpecialEffectFieldsGUI();
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_SpecialPersistentEffectDefinition")));
            
            return root;
        }
        
        private VisualElement CreateTagFieldsGUI()
        {
            VisualElement root = new VisualElement();
            root.Add(new PropertyField(serializedObject.FindProperty("m_GrantedTags")));
            return root;
        }
    }
}
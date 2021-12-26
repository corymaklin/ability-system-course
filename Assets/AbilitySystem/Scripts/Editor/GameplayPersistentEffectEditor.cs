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
            
            StyleSheet styleSheet =
                AssetDatabase.LoadAssetAtPath<StyleSheet>(
                    "Assets/AbilitySystem/Scripts/Editor/GameplayEffectEditor.uss");
            root.styleSheets.Add(styleSheet);
            
            root.Add(CreateCoreFieldsGUI());
            root.Add(CreateApplicationFieldsGUI());
            root.Add(CreateDurationFieldsGUI());
            root.Add(CreatePeriodFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }

        protected void RegisterCallbacks(VisualElement root)
        {
            GameplayPersistentEffectDefinition definition = target as GameplayPersistentEffectDefinition;

            PropertyField durationField = root.Q<PropertyField>("duration-formula");
            PropertyField isInfiniteField = root.Q<PropertyField>("is-infinite");

            durationField.style.display = definition.isInfinite ? DisplayStyle.None : DisplayStyle.Flex;
            isInfiniteField.RegisterValueChangeCallback(evt =>
            {
                durationField.style.display = evt.changedProperty.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            });

            VisualElement periodFields = root.Q("period");
            PropertyField isPeriodicField = root.Q<PropertyField>("is-periodic");
            periodFields.style.display = definition.isPeriodic ? DisplayStyle.Flex : DisplayStyle.None;
            
            isPeriodicField.RegisterValueChangeCallback(evt =>
            {
                periodFields.style.display = evt.changedProperty.boolValue ? DisplayStyle.Flex : DisplayStyle.None;
            });
        }

        protected VisualElement CreateDurationFieldsGUI()
        {
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_IsInfinite")) { name = "is-infinite"});
            root.Add(new PropertyField(serializedObject.FindProperty("m_DurationFormula")) { name = "duration-formula"});

            return root;
        }
        
        protected VisualElement CreatePeriodFieldsGUI()
        {
            VisualElement root = new VisualElement();

            VisualElement periodFields = new VisualElement() { name = "period" };
            periodFields.Add(new PropertyField(serializedObject.FindProperty("m_Period")));
            periodFields.Add(new PropertyField(serializedObject.FindProperty("m_ExecutePeriodicEffectOnApplication")));
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_IsPeriodic")) { name = "is-periodic" });
            root.Add(periodFields);
            
            return root;
        }
        
        protected override VisualElement CreateSpecialEffectFieldsGUI()
        {
            VisualElement root = base.CreateSpecialEffectFieldsGUI();
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_SpecialPersistentEffectDefinition")));
            
            return root;
        }
        
        protected override VisualElement CreateTagFieldsGUI()
        {
            VisualElement root = base.CreateTagFieldsGUI();
            root.Add(new PropertyField(serializedObject.FindProperty("m_GrantedTags")));
            return root;
        }
    }
}
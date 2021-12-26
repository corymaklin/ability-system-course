using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(GameplayStackableEffectDefinition))]
    public class GameplayStackableEffectEditor : GameplayPersistentEffectEditor
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
            root.Add(CreateStackingFieldsGUI());
            root.Add(CreateGameplayEffectFieldsGUI());
            root.Add(CreateSpecialEffectFieldsGUI());
            root.Add(CreateTagFieldsGUI());

            RegisterCallbacks(root);
            
            return root;
        }

        protected VisualElement CreateStackingFieldsGUI()
        {
            VisualElement root = new VisualElement();
            
            root.Add(new PropertyField(serializedObject.FindProperty("m_DenyOverflowApplication")));
            root.Add(new PropertyField(serializedObject.FindProperty("m_ClearStackOnOverflow")));
            root.Add(new PropertyField(serializedObject.FindProperty("m_StackLimitCount")));
            root.Add(new PropertyField(serializedObject.FindProperty("m_StackDurationRefreshPolicy")));
            root.Add(new PropertyField(serializedObject.FindProperty("m_StackPeriodResetPolicy")));
            root.Add(new PropertyField(serializedObject.FindProperty("m_StackExpirationPolicy")));
            
            return root;
        }

        protected VisualElement CreateGameplayEffectFieldsGUI()
        {
            VisualElement root = new VisualElement();

            ListView overflowGameplayEffects = new ListView
            {
                bindingPath = "m_OverflowEffects",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Overflow Gameplay Effects"
            };
            overflowGameplayEffects.Bind(serializedObject);
            root.Add(overflowGameplayEffects);
            return root;
        }
    }
}
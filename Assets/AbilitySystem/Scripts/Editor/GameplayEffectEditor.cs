using System;
using System.Linq;
using AbilitySystem.Scripts.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AbilitySystem
{
    [CustomEditor(typeof(GameplayEffectDefinition))]
    public class GameplayEffectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            
            root.Add(CreateApplicationFieldsGUI());
            
            return root;
        }

        protected VisualElement CreateApplicationFieldsGUI()
        {
            VisualElement root = new VisualElement();
            ListView modifiers = new ListView
            {
                bindingPath = "m_ModifierDefinitions",
                virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
                reorderable = true,
                showFoldoutHeader = true,
                showAddRemoveFooter = true,
                headerTitle = "Modifiers"
            };
            modifiers.Bind(serializedObject);
            Button addButton = modifiers.Q<Button>("unity-list-view__add-button");
            addButton.clicked += AddButtonOnClicked;
            root.Add(modifiers);
            return root;
        }

        private void AddButtonOnClicked()
        {
            Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(AbstractGameplayEffectStatModifierDefinition).IsAssignableFrom(type) &&
                               type.IsClass && !type.IsAbstract).ToArray();
            if (types.Length > 1)
            {
                GenericMenu menu = new GenericMenu();
                foreach (Type type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, delegate
                    {
                        CreateItem(type);
                    });
                }
                menu.ShowAsContext();
            }
            else
            {
                CreateItem(types[0]);
            }
        }

        private void CreateItem(Type type)
        {
            AbstractGameplayEffectStatModifierDefinition item = ScriptableObject.CreateInstance(type) as AbstractGameplayEffectStatModifierDefinition;
            item.name = "Modifier";
            AssetDatabase.AddObjectToAsset(item, target);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SerializedProperty modifiers = serializedObject.FindProperty("m_ModifierDefinitions");
            modifiers.GetArrayElementAtIndex(modifiers.arraySize - 1).objectReferenceValue = item;
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
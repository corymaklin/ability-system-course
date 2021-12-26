using System.Collections;
using AbilitySystem.UI;
using LevelSystem;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace AbilitySystem.Tests
{
    public class AbilitiesUITests
    {
        private PlayerAbilityController m_PlayerPrefab;
        private PlayerAbilityController m_PlayerAbilityController;
        private AbilitiesUI m_AbilitiesUIPrefab;
        private AbilitiesUI m_AbilitiesUI;
        private LevelController m_LevelController;
        private UIDocument m_UIDocument;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_PlayerPrefab =
                AssetDatabase.LoadAssetAtPath<PlayerAbilityController>(
                    "Assets/AbilitySystem/Tests/Prefabs/Player.prefab");
            m_AbilitiesUIPrefab =
                AssetDatabase.LoadAssetAtPath<AbilitiesUI>("Assets/AbilitySystem/Tests/Prefabs/AbilitiesUI.prefab");
        }

        [SetUp]
        public void BeforeEachTestSetup()
        {
            m_PlayerAbilityController = GameObject.Instantiate(m_PlayerPrefab);
            m_AbilitiesUI = GameObject.Instantiate(m_AbilitiesUIPrefab);
            SerializedObject so = new SerializedObject(m_AbilitiesUI);
            so.FindProperty("m_Controller").objectReferenceValue = m_PlayerAbilityController;
            so.ApplyModifiedProperties();
            m_LevelController = m_PlayerAbilityController.GetComponent<LevelController>();
            m_LevelController.currentExperience += 100;
            m_UIDocument = m_AbilitiesUI.GetComponent<UIDocument>();
        }

        [UnityTest]
        public IEnumerator AbilitiesUI_WhenIncrementButtonClicked_IncrementsAbilityLevel()
        {
            yield return null;
            Assert.AreEqual(0, m_PlayerAbilityController.abilities["Test"].level);
            VisualElement testElement = m_UIDocument.rootVisualElement.Q("Test");
            Button incrementButton = testElement.Q<Button>("ability__increment-button");
            using (var e = new NavigationSubmitEvent { target = incrementButton })
            {
                incrementButton.SendEvent(e);
            }
            Assert.AreEqual(1, m_PlayerAbilityController.abilities["Test"].level);
        }
        
        [UnityTest]
        public IEnumerator AbilitiesUI_WhenIncrementButtonClicked_DecrementsAbilityPoints()
        {
            yield return null;
            Assert.AreEqual(3, m_PlayerAbilityController.abilityPoints);
            VisualElement testElement = m_UIDocument.rootVisualElement.Q("Test");
            Button incrementButton = testElement.Q<Button>("ability__increment-button");
            using (var e = new NavigationSubmitEvent { target = incrementButton })
            {
                incrementButton.SendEvent(e);
            }
            Assert.AreEqual(2, m_PlayerAbilityController.abilityPoints);
        }
        
        [UnityTest]
        public IEnumerator AbilitiesUI_WhenNoAbilityPoints_DisablesIncrementButtons()
        {
            yield return null;
            VisualElement testElement = m_UIDocument.rootVisualElement.Q("Test");
            Button incrementButton = testElement.Q<Button>("ability__increment-button");
            for (int i = 0; i < 3; i++)
            {
                using (var e = new NavigationSubmitEvent { target = incrementButton })
                {
                    incrementButton.SendEvent(e);
                }
            }
            Assert.AreEqual(0, m_PlayerAbilityController.abilityPoints);
            Assert.IsFalse(incrementButton.enabledSelf);
        }
        
        [UnityTest]
        public IEnumerator AbilitiesUI_WhenAbilityLevelChanged_UpdatesLevelText()
        {
            yield return null;
            VisualElement testElement = m_UIDocument.rootVisualElement.Q("Test");
            Button incrementButton = testElement.Q<Button>("ability__increment-button");
            Label testLevel = testElement.Q<Label>("ability__level");
            using (var e = new NavigationSubmitEvent { target = incrementButton })
            {
                incrementButton.SendEvent(e);
            }
            Assert.AreEqual("1", testLevel.text);
        }
        
        [UnityTest]
        public IEnumerator AbilitiesUI_WhenMaxLevelReached_DisablesIncrementButton()
        {
            yield return null;
            m_LevelController.currentExperience += 5000;
            VisualElement testElement = m_UIDocument.rootVisualElement.Q("Test");
            Button incrementButton = testElement.Q<Button>("ability__increment-button");
            Assert.AreEqual(0, m_PlayerAbilityController.abilities["Test"].level);
            for (int i = 0; i < 20; i++)
            {
                using (var e = new NavigationSubmitEvent { target = incrementButton })
                {
                    incrementButton.SendEvent(e);
                }
            }
            Assert.AreEqual(20, m_PlayerAbilityController.abilities["Test"].level);
            Assert.IsFalse(incrementButton.enabledSelf);
        }
        
        [UnityTest]
        public IEnumerator AbilitiesUI_WhenLevelUp_UpdatesAbilityPointsText()
        {
            yield return null;
            Label abilityPointsValue = m_UIDocument.rootVisualElement.Q<Label>("abilities__ability-points-value");
            Assert.AreEqual("3", abilityPointsValue.text);
            m_LevelController.currentExperience += 100;
            Assert.AreEqual("6", abilityPointsValue.text);
        }
    }
}
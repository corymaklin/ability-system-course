using System.Collections;
using LevelSystem;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace StatSystem.Tests
{
    public class StatsUITests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatSystem/Tests/Scenes/Test.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenIncrementButtonClicked_IncrementsStatBaseValue()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            Assert.AreEqual(1, playerStatController.stats["Strength"].value);
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button incrementButton = strengthElement.Q<Button>("increment-button");
            using (var e = new NavigationSubmitEvent { target = incrementButton })
            {
                incrementButton.SendEvent(e);
            }
            Assert.AreEqual(2, playerStatController.stats["Strength"].value);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenIncrementButtonClicked_DecrementsStatPoints()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            Assert.AreEqual(5, playerStatController.statPoints);
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button incrementButton = strengthElement.Q<Button>("increment-button");
            using (var e = new NavigationSubmitEvent { target = incrementButton })
            {
                incrementButton.SendEvent(e);
            }
            Assert.AreEqual(4, playerStatController.statPoints);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenNoStatPoints_DisablesIncrementButtons()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            Assert.AreEqual(5, playerStatController.statPoints);
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement strengthElement = uiDocument.rootVisualElement.Q("strength");
            Button incrementButton = strengthElement.Q<Button>("increment-button");
            for (int i = 0; i < 5; i++)
            {
                using (var e = new NavigationSubmitEvent { target = incrementButton })
                {
                    incrementButton.SendEvent(e);
                }
            }
            Assert.AreEqual(0, playerStatController.statPoints);
            Assert.AreEqual(false, incrementButton.enabledSelf);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenStatValueChanged_UpdatesText()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement physicalAttackElement = uiDocument.rootVisualElement.Q("physical-attack");
            Label physicalAttackValue = physicalAttackElement.Q<Label>("value");
            Assert.AreEqual("3", physicalAttackValue.text);
            playerStatController.stats["PhysicalAttack"].AddModifier(new StatModifier
            {
                magnitude = 5,
                type = ModifierOperationType.Additive
            });
            Assert.AreEqual("8", physicalAttackValue.text);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenStatCapReached_DisablesIncrementButton()
        {
            yield return null;
            // Assumes, in the database, the Charisma stat has a cap of 1 and its base value is 1
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement charismaElement = uiDocument.rootVisualElement.Q("charisma");
            Button incrementButton = charismaElement.Q<Button>("increment-button");
            Assert.AreEqual(false,incrementButton.enabledSelf);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenLevelUp_UpdatesText()
        {
            yield return null;
            LevelController levelController = GameObject.FindObjectOfType<LevelController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement levelElement = uiDocument.rootVisualElement.Q("level");
            Label levelValue = levelElement.Q<Label>("value");
            Assert.AreEqual("1", levelValue.text);
            levelController.currentExperience += 100;
            Assert.AreEqual("2", levelValue.text);
        }
        
        [UnityTest]
        public IEnumerator StatUI_WhenGainExperience_UpdatesText()
        {
            yield return null;
            LevelController levelController = GameObject.FindObjectOfType<LevelController>();
            UIDocument uiDocument = GameObject.FindObjectOfType<UIDocument>();
            VisualElement experienceElement = uiDocument.rootVisualElement.Q("experience");
            Label experienceValue = experienceElement.Q<Label>("value");
            Assert.AreEqual("0 / 83", experienceValue.text);
            levelController.currentExperience += 5;
            Assert.AreEqual("5 / 83", experienceValue.text);
        }
    }
}
using System.Collections;
using LevelSystem;
using NUnit.Framework;
using StatSystem.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace StatSystem.Tests
{
    public class HeadsUpDisplayTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/StatSystem/Tests/Scenes/Test.unity", new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator HeadsUpDisplayUI_WhenLevelUp_UpdatesText()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            ILevelable levelable = playerStatController.GetComponent<ILevelable>();
            HeadsUpDisplayUI headsUpDisplayUI = GameObject.FindObjectOfType<HeadsUpDisplayUI>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            Label level = uiDocument.rootVisualElement.Q<Label>("level");
            Assert.AreEqual("1", level.text);
            levelable.currentExperience += 100;
            Assert.AreEqual("2", level.text);
        }

        [UnityTest]
        public IEnumerator HeadsUpDisplayUI_WhenGainExperience_UpdatesExperienceBar()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            ILevelable levelable = playerStatController.GetComponent<ILevelable>();
            HeadsUpDisplayUI headsUpDisplayUI = GameObject.FindObjectOfType<HeadsUpDisplayUI>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            ProgressBar experienceBar = uiDocument.rootVisualElement.Q<ProgressBar>("experience");
            Assert.AreEqual(0, experienceBar.value);
            levelable.currentExperience += 5;
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(6, experienceBar.value, 0.5f);
        }

        [UnityTest]
        public IEnumerator HeadsUpDisplayUI_WhenLoseMana_UpdatesManaBar()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            HeadsUpDisplayUI headsUpDisplayUI = GameObject.FindObjectOfType<HeadsUpDisplayUI>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            ProgressBar manaBar = uiDocument.rootVisualElement.Q<ProgressBar>("mana");
            Assert.AreEqual(100, manaBar.value);
            Attribute mana = playerStatController.stats["Mana"] as Attribute;
            mana.ApplyModifier(new StatModifier
            {
                magnitude = -10,
                type = ModifierOperationType.Additive
            });
            UnityEngine.Assertions.Assert.AreEqual(90, manaBar.value);
        }
        
        [UnityTest]
        public IEnumerator HeadsUpDisplayUI_WhenLoseHealth_UpdatesHealthBar()
        {
            yield return null;
            PlayerStatController playerStatController = GameObject.FindObjectOfType<PlayerStatController>();
            HeadsUpDisplayUI headsUpDisplayUI = GameObject.FindObjectOfType<HeadsUpDisplayUI>();
            UIDocument uiDocument = headsUpDisplayUI.GetComponent<UIDocument>();
            ProgressBar healthBar = uiDocument.rootVisualElement.Q<ProgressBar>("health");
            Assert.AreEqual(100, healthBar.value);
            Attribute health = playerStatController.stats["Health"] as Attribute;
            health.ApplyModifier(new StatModifier
            {
                magnitude = -10,
                type = ModifierOperationType.Additive
            });
            UnityEngine.Assertions.Assert.AreEqual(90, healthBar.value);
        }
    }
}
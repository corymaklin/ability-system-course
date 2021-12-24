using System.Collections;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace LevelSystem.Tests
{
    public class LevelControllerTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EditorSceneManager.LoadSceneInPlayMode("Assets/LevelSystem/Tests/Scenes/Test.unity",
                new LoadSceneParameters(LoadSceneMode.Single));
        }

        [UnityTest]
        public IEnumerator LevelController_WhenLevelUp_UpdateRequiredExperience()
        {
            yield return null;
            LevelController levelController = GameObject.FindObjectOfType<LevelController>();
            Assert.AreEqual(1, levelController.level);
            levelController.currentExperience += 83;
            Assert.AreEqual(2, levelController.level);
            Assert.AreEqual(0, levelController.currentExperience);
            Assert.AreEqual(92, levelController.requiredExperience);
        }
    }
}
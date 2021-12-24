using System.Collections;
using AbilitySystem.Scripts.Runtime;
using NUnit.Framework;
using StatSystem;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;

namespace AbilitySystem.Tests.Scripts.Runtime
{
    public class GameplayEffectControllerTests
    {
        private GameObject m_PlayerPrefab;
        private GameObject m_EnemyPrefab;
        private GameObject m_Player;
        private GameObject m_Enemy;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            m_PlayerPrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AbilitySystem/Tests/Prefabs/Player.prefab");
            m_EnemyPrefab =
                AssetDatabase.LoadAssetAtPath<GameObject>("Assets/AbilitySystem/Tests/Prefabs/Enemy.prefab");
        }

        [SetUp]
        public void BeforeEachTestSetup()
        {
            m_Player = GameObject.Instantiate(m_PlayerPrefab);
            m_Enemy = GameObject.Instantiate(m_EnemyPrefab);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenEffectApplied_ModifyAttribute()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();
            GameplayEffectDefinition damageEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/Test_GameplayEffect_HealthModifier.asset");
            GameplayEffect damageEffect = new GameplayEffect(damageEffectDefinition, null, m_Enemy);
            Health health = statController.stats["Health"] as Health;
            Assert.AreEqual(100, health.currentValue);
            effectController.ApplyGameplayEffectToSelf(damageEffect);
            Assert.AreEqual(90, health.currentValue);
        }
    }
}
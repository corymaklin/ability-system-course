using System.Collections;
using AbilitySystem.Scripts.Runtime;
using Core;
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

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectApplied_AddStatModifier()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();
            GameplayPersistentEffectDefinition testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenPersistentEffectApplied_AddStatModifier/GameplayPersistentEffect.asset");
            GameplayPersistentEffect testEffect = new GameplayPersistentEffect(testEffectDefinition, null, m_Player);
            Stat intelligence = statController.stats["Intelligence"];
            Assert.AreEqual(1, intelligence.value);
            effectController.ApplyGameplayEffectToSelf(testEffect);
            Assert.AreEqual(4, intelligence.value);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveStatModifier()
        {
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();
            GameplayPersistentEffectDefinition testEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenPersistentEffectExpires_RemoveStatModifier/GameplayPersistentEffect.asset");
            GameplayPersistentEffect testEffect = new GameplayPersistentEffect(testEffectDefinition, null, m_Player);
            Stat intelligence = statController.stats["Intelligence"];
            effectController.ApplyGameplayEffectToSelf(testEffect);
            Assert.AreEqual(4, intelligence.value);
            yield return new WaitForSeconds(3f);
            Assert.AreEqual(1, intelligence.value);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenStart_AppliesStartingEffects()
        {
            yield return null;
            StatController statController = m_Player.GetComponent<StatController>();
            Stat dexterity = statController.stats["Dexterity"];
            Assert.AreEqual(4, dexterity.value);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenEffectApplied_AddGrantedTags()
        {
            yield return null;
            TagController tagController = m_Player.GetComponent<TagController>();
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayPersistentEffectDefinition persistentEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenEffectApplied_GrantTags/GameplayPersistentEffect.asset");
            GameplayPersistentEffect persistentEffect =
                new GameplayPersistentEffect(persistentEffectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            Assert.Contains("test", tagController.tags);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPersistentEffectExpires_RemoveGrantedTags()
        {
            yield return null;
            TagController tagController = m_Player.GetComponent<TagController>();
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayPersistentEffectDefinition persistentEffectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenEffectApplied_GrantTags/GameplayPersistentEffect.asset");
            GameplayPersistentEffect persistentEffect =
                new GameplayPersistentEffect(persistentEffectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(persistentEffect);
            Assert.Contains("test", tagController.tags);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(0, tagController.tags.Count);
        }
    }
}
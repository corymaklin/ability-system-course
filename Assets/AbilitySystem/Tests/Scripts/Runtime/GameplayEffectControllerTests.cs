using System.Collections;
using System.Linq;
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

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenPeriodReached_ExecuteGameplayEffect()
        {
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();
            GameplayPersistentEffectDefinition effectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenPeriodReached_ExecuteGameplayEffect/GameplayPersistentEffect.asset");
            Health health = statController.stats["Health"] as Health;
            GameplayPersistentEffect effect = new GameplayPersistentEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(effect);
            Assert.AreEqual(100, health.currentValue);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(95, health.currentValue);
        }
        
        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplied_ExecutePeriodicGameplayEffect()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();
            GameplayPersistentEffectDefinition effectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayPersistentEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenApplied_ExecutePeriodicGameplayEffect/GameplayPersistentEffect.asset");
            Health health = statController.stats["Health"] as Health;
            GameplayPersistentEffect effect = new GameplayPersistentEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(effect);
            Assert.AreEqual(95, health.currentValue);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_AppliesOverflowEffects()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            StatController statController = m_Player.GetComponent<StatController>();

            GameplayStackableEffectDefinition effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/AbilitySystem/Tests/ScriptableObjects/WhenOverflow_AppliesOverflowEffects/GameplayStackableEffect.asset");
            Health health = statController.stats["Health"] as Health;
            Assert.AreEqual(100, health.currentValue);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            Assert.AreEqual(95, health.currentValue);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_ClearsStack()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/AbilitySystem/Tests/ScriptableObjects/WhenOverflow_ClearsStack/GameplayStackableEffect.asset");
            GameplayStackableEffect stackableEffect = new GameplayStackableEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            GameplayStackableEffect secondStackableEffect =
                effectController.activeEffects.FirstOrDefault(activeEffect =>
                    activeEffect.definition == effectDefinition) as GameplayStackableEffect;
            Assert.AreNotEqual(stackableEffect, secondStackableEffect);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenOverflow_DoNotApplyEffect()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/AbilitySystem/Tests/ScriptableObjects/WhenOverflow_DoNotApplyEffect/GameplayStackableEffect.asset");
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            Assert.IsFalse(effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player)));
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_ResetDuration()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition = AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                "Assets/AbilitySystem/Tests/ScriptableObjects/WhenApplyStack_ResetDuration/GameplayStackableEffect.asset");
            GameplayStackableEffect stackableEffect = new GameplayStackableEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            yield return new WaitForSeconds(1f);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingDuration, 9f, 0.1f);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingDuration, 10f, 0.1f);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_ResetPeriod()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenApplyStack_ResetPeriod/GameplayStackableEffect.asset");
            GameplayStackableEffect stackableEffect = new GameplayStackableEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            yield return new WaitForSeconds(1f);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingPeriod, 2f, 0.1f);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingPeriod, 3f, 0.1f);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenApplyStack_IncreaseStackCount()
        {
            yield return null;
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenApplyStack_IncreaseStackCount/GameplayStackableEffect.asset");
            GameplayStackableEffect stackableEffect = new GameplayStackableEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            Assert.AreEqual(2, stackableEffect.stackCount);
        }

        [UnityTest]
        public IEnumerator GameplayEffectController_WhenDurationReached_RemoveStackAndRefresh()
        {
            GameplayEffectController effectController = m_Player.GetComponent<GameplayEffectController>();
            GameplayStackableEffectDefinition effectDefinition =
                AssetDatabase.LoadAssetAtPath<GameplayStackableEffectDefinition>(
                    "Assets/AbilitySystem/Tests/ScriptableObjects/WhenDurationReached_RemoveStackAndRefresh/GameplayStackableEffect.asset");
            GameplayStackableEffect stackableEffect = new GameplayStackableEffect(effectDefinition, null, m_Player);
            effectController.ApplyGameplayEffectToSelf(stackableEffect);
            effectController.ApplyGameplayEffectToSelf(new GameplayStackableEffect(effectDefinition, null, m_Player));
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingDuration, 3f, 0.1f);
            Assert.AreEqual(2, stackableEffect.stackCount);
            yield return new WaitForSeconds(3f);
            UnityEngine.Assertions.Assert.AreApproximatelyEqual(stackableEffect.remainingDuration, 3f, 0.1f);
            Assert.AreEqual(1, stackableEffect.stackCount);
        }
    }
}
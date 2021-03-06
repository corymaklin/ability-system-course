using System.Collections;
using AbilitySystem.Scripts.Runtime;
using Core;
using LevelSystem;
using NUnit.Framework;
using StatSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace AbilitySystem.Tests.Scripts.Runtime
{
    public class AbilityControllerTests
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
        public IEnumerator AbilityController_WhenStart_ApplyPassiveAbilities()
        {
            yield return null;
            StatController statController = m_Player.GetComponent<StatController>();
            Stat wisdom = statController.stats["Wisdom"];
            Assert.AreEqual(4, wisdom.value);
        }

        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyEffects()
        {
            yield return null;
            AbilityController abilityController = m_Player.GetComponent<AbilityController>();
            StatController statController = m_Enemy.GetComponent<StatController>();
            Health health = statController.stats["Health"] as Health;
            Assert.AreEqual(100, health.currentValue);
            abilityController.TryActivateAbility("SingleTargetAbility", m_Enemy);
            Assert.AreEqual(95, health.currentValue);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenActivateAbility_ApplyCostEffect()
        {
            yield return null;
            AbilityController abilityController = m_Player.GetComponent<AbilityController>();
            StatController statController = m_Player.GetComponent<StatController>();
            Attribute mana = statController.stats["Mana"] as Attribute;
            Assert.AreEqual(100, mana.currentValue);
            abilityController.TryActivateAbility("AbilityWithCost", m_Enemy);
            Assert.AreEqual(50, mana.currentValue);
        }
        
        [UnityTest]
        public IEnumerator AbilityController_WhenCannotSatisfyAbilityCost_BlockAbilityActivation()
        {
            yield return null;
            AbilityController abilityController = m_Player.GetComponent<AbilityController>();
            StatController statController = m_Player.GetComponent<StatController>();
            Attribute mana = statController.stats["Mana"] as Attribute;
            Assert.AreEqual(100, mana.currentValue);
            Assert.IsTrue(abilityController.TryActivateAbility("AbilityWithCost", m_Enemy));
            Assert.IsTrue(abilityController.TryActivateAbility("AbilityWithCost", m_Enemy));
            Assert.AreEqual(0, mana.currentValue);
            Assert.IsFalse(abilityController.TryActivateAbility("AbilityWithCost", m_Enemy));
        }
        [UnityTest]
        public IEnumerator AbilityController_WhenAbilityOnCooldown_BlockAbilityActivation()
        {
            TagController tagController = m_Player.GetComponent<TagController>();
            AbilityController abilityController = m_Player.GetComponent<AbilityController>();
            abilityController.TryActivateAbility("AbilityWithCooldown", m_Player);
            Assert.Contains("test.cooldown", tagController.tags);
            bool isActivated = abilityController.TryActivateAbility("AbilityWithCooldown", m_Player);
            Assert.IsFalse(isActivated);
            yield return new WaitForSeconds(2f);
            isActivated = abilityController.TryActivateAbility("AbilityWithCooldown", m_Player);
            Assert.IsTrue(isActivated);
        }

        [UnityTest]
        public IEnumerator AbilityController_WhenLevelUp_GainAbilityPoints()
        {
            yield return null;
            PlayerAbilityController playerAbilityController = m_Player.GetComponent<PlayerAbilityController>();
            LevelController levelController = m_Player.GetComponent<LevelController>();
            Assert.AreEqual(0, playerAbilityController.abilityPoints);
            levelController.currentExperience += 100;
            Assert.AreEqual(3, playerAbilityController.abilityPoints);
        }
    }
}
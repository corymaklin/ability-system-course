﻿using System.Collections;
using AbilitySystem.Scripts.Runtime;
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
    }
}
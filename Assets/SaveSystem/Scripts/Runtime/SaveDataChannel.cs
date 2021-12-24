using System;
using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "SaveDataChannel", menuName = "SaveSystem/Channels/SaveDataChannel", order = 0)]
    public class SaveDataChannel : ScriptableObject
    {
        public event Action save;

        public void Save()
        {
            save?.Invoke();
        }
    }
}
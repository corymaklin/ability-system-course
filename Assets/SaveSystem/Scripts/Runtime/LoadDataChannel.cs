using System;
using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    [CreateAssetMenu(fileName = "LoadDataChannel", menuName = "SaveSystem/Channels/LoadDataChannel", order = 0)]
    public class LoadDataChannel : ScriptableObject
    {
        public event Action load;

        public void Load()
        {
            load?.Invoke();
        }
    }
}
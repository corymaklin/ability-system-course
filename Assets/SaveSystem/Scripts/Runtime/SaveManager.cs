using UnityEngine;

namespace SaveSystem.Scripts.Runtime
{
    [DefaultExecutionOrder(1)]
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SaveData m_SaveData;

        private void Awake()
        {
            if (m_SaveData.previousSaveExists)
                m_SaveData.Load();
        }

        private void OnApplicationQuit()
        {
            m_SaveData.Save();
        }
    }
}
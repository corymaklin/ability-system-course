using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    public class VisualEffect : MonoBehaviour
    {
        private ParticleSystem m_ParticleSystem;
        private Coroutine m_Coroutine;
        public event Action<VisualEffect> finished;
        public bool isLooping => m_ParticleSystem.main.loop;

        private void Awake()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            m_ParticleSystem.Play();
            if (!m_ParticleSystem.main.loop) m_Coroutine = StartCoroutine(WaitForDuration());
        }

        public void Stop()
        {
            if (m_Coroutine != null) StopCoroutine(m_Coroutine);
            m_ParticleSystem.Stop();
            finished?.Invoke(this);
        }

        private IEnumerator WaitForDuration()
        {
            yield return new WaitForSeconds(m_ParticleSystem.main.duration);
            m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            yield return new WaitUntil(() => m_ParticleSystem.particleCount == 0);
            finished?.Invoke(this);
        }
    }
}
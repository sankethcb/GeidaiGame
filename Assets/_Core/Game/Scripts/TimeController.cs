using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Core
{
    [CreateAssetMenu(fileName = "TimeController", menuName = "Core/Game/Time Controller", order = 3)]
    public class TimeController : ScriptableObject
    {
        public float CurrentTimeScale => Time.timeScale;

        float m_timeScale;
        Tween m_timeModifer;

        bool isFrozen = false;

        public void FreezeTime()
        {
            StoreTimeScale();
            Time.timeScale = 0;
            isFrozen = true;
        }

        public Tween FreezeTime(float duration)
        {
            StoreTimeScale();

            Time.timeScale = 0;

            isFrozen = true;
            m_timeModifer = DOVirtual.DelayedCall(duration, ResumeFrozenTime);

            return m_timeModifer;
        }

        public Tween SlowTimeInstant(float duration, float multiplier)
        {
            StoreTimeScale();

            Time.timeScale = m_timeScale * multiplier;

            m_timeModifer = DOVirtual.DelayedCall(duration, ResumeTime);

            return m_timeModifer;
        }

        public Tween SlowTimeEased(float duration, float multiplier, Ease ease)
        {
            StoreTimeScale();

            m_timeModifer =
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, Time.timeScale * multiplier, duration)
            .SetEase(ease);


            return m_timeModifer;
        }

        public Tween ResetTimeEased(float duration, Ease ease)
        {
            StoreTimeScale();

            m_timeModifer =
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, m_timeScale, duration)
            .SetEase(ease);


            return m_timeModifer;
        }

        public void ResumeFrozenTime()
        {
            isFrozen = false;
            ResumeTime();
        }

        void StoreTimeScale()
        {
            if (m_timeModifer != null && !m_timeModifer.IsComplete())
                m_timeModifer.Complete();

            m_timeScale = Time.timeScale;
        }

        void ResumeTime()
        {
            Time.timeScale = m_timeScale;
        }

    }
}
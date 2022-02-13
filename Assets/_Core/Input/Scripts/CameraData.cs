using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Utilities
{
    [CreateAssetMenu(fileName = "CameraData", menuName = "Utilities/Reference Data/Camera", order = 1)]
    public class CameraData : ScriptableObject
    {
        [SerializeField] [ReadOnly] Camera m_current;
        [SerializeField] [ReadOnly] CinemachineBrain m_brain;
        public Camera Current
        {
            get
            {
                if (m_current == null)
                {
                    m_current = Camera.main;
                }

                return m_current;
            }
        }

        public CinemachineBrain Brain
        {
            get
            {
                return (m_brain == null ? (Camera.main.TryGetComponent<CinemachineBrain>(out m_brain) ? m_brain : null) : null);
            }
        }
    }
}
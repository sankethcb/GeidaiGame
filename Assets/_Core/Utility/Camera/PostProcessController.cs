using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

namespace Core
{
    [RequireComponent(typeof(Cinemachine.PostFX.CinemachineVolumeSettings))]
    public class PostProcessController : MonoBehaviour
    {
        [Utilities.ReadOnly] [SerializeField] VolumeProfile volumeProfile;
        [SerializeField] PostProcessSettings settings;

        void Awake()
        {
            volumeProfile = GetComponent<Cinemachine.PostFX.CinemachineVolumeSettings>().m_Profile;
            settings.VolumeComponents = volumeProfile.components;
        }
    }
}
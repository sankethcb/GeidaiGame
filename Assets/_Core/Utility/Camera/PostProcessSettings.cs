using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

namespace Core
{
    [CreateAssetMenu(fileName = "PostProcessSettings", menuName = "Core/Camera/Post Process Settings", order = 0)]
    public class PostProcessSettings : ScriptableObject
    {
        [Utilities.ReadOnly] public List<VolumeComponent> VolumeComponents;

        
    }
}

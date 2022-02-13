using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3Reference", menuName = "Utilities/Reference Data/Vector3", order = 0)]
public class Vector3Reference : ScriptableObject
{
    public Vector3 value = Vector3.zero;
}

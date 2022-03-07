using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagReseter : MonoBehaviour
{
    public List<GameEventFlag> GameFlags;

    void Awake()
    {
        foreach (GameEventFlag gameFlags in GameFlags)
        {
            gameFlags.Reset();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEventFlag", menuName = "Utilities/Reference Data/Game Event Flag", order = 1)]
public class GameEventFlag : ScriptableObject
{
    [SerializeField] bool raised = false;
    public bool Raised => raised;

    public event System.Action OnFlagReset;
    public event System.Action OnFlagRaised;

    public void SetValue(bool value)
    {
        if (!raised && value)
            Raise();
        else if(raised && !value)
            Reset();
    }

    [ContextMenu("Raise")]
    public void Raise()
    {
        Debug.Log(name + " raised");

        raised = true;
        OnFlagRaised?.Invoke();
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        raised = false;
        OnFlagReset?.Invoke();
    }
}

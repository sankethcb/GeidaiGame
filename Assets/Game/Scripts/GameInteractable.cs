using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractable : MonoBehaviour, IInteractable
{
    public bool LockPlayer => true;

    public Vector3 Position => throw new NotImplementedException();

    public event Action OnInteractionComplete;

    public void CancelInteraction()
    {
        throw new NotImplementedException();
    }

    public IEnumerator DoInteraction()
    {
        
        OnInteractionComplete?.Invoke();
        yield break;
    }
}

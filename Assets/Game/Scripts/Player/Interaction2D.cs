using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public interface IInteractable
{
    bool LockPlayer { get; }
    void DoInteraction();

    void CancelInteraction();

    event System.Action OnInteractionComplete;
}

public class Interaction2D : MonoBehaviour
{
    public CircleCollider2D InteractionCollider;
     IInteractable interactable;
    public IInteractable Interactable => interactable;

    public event System.Action<bool> OnInteractStart;
    public event System.Action OnInteractComplete;

    public bool interacting = false;

    public void InteractWith()
    {
        interacting = true;
        OnInteractStart?.Invoke(interactable.LockPlayer);
        interactable.OnInteractionComplete += ResolveInteraction;
        InteractionCollider.enabled = false;

        interactable.DoInteraction();
    }

    public void ResolveInteraction()
    {
        interactable.OnInteractionComplete -= ResolveInteraction;
        InteractionCollider.enabled = true;
        interactable = null;
        interacting = false;
        OnInteractComplete?.Invoke();
    }

    IInteractable temp;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IInteractable>(out temp))
        {
            Debug.Log(temp.ToString());
            if (temp == interactable)
                return;

            interactable = temp;
            //INTERACTION UI ENABLE
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (interacting) return;

        if (other.TryGetComponent<IInteractable>(out interactable))
        {
            if (temp != interactable)
                return;
            interactable = null;
            //INTERACTION UI DISABLE
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using UnityEngine.UI;

public interface IInteractable
{
    bool LockPlayer { get; }
    Vector3 Position { get; }
    IEnumerator DoInteraction();

    void CancelInteraction();

    event System.Action OnInteractionComplete;
}

public class Interaction2D : MonoBehaviour
{
    [Header("References")]
    public CircleCollider2D InteractionCollider;
    public GameObject InteractionUI;

    [Header("Variables")]
    [SerializeField] [ReadOnly] bool m_interacting = false;
    [SerializeReference] [ReadOnly] IInteractable m_interactable;

    public IInteractable Interactable => m_interactable;
    public bool Interacting => m_interacting;


    public event System.Action<bool> OnInteractStart;
    public event System.Action OnInteractComplete;

    public void InteractWith()
    {
        m_interacting = true;
        OnInteractStart?.Invoke(m_interactable.LockPlayer);
        m_interactable.OnInteractionComplete += ResolveInteraction;
        InteractionCollider.enabled = false;

        InteractionUI.SetActive(false);
        m_interactable.DoInteraction().Start();
    }

    public void ResolveInteraction()
    {
        m_interactable.OnInteractionComplete -= ResolveInteraction;
        InteractionCollider.enabled = true;
        m_interactable = null;
        m_interacting = false;
        OnInteractComplete?.Invoke();
    }

    IInteractable temp;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.TryGetComponent<IInteractable>(out temp))
        {
            Debug.Log(temp.ToString());
            if (temp == m_interactable)
                return;
            InteractionUI.SetActive(true);

            m_interactable = temp;
            TrackInteractionUI().Start();
            //INTERACTION UI ENABLE
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (m_interacting) return;

        if (other.transform.parent.TryGetComponent<IInteractable>(out m_interactable))
        {
            if (temp != m_interactable)
                return;
            m_interactable = null;
            InteractionUI.SetActive(false);
            //INTERACTION UI DISABLE
        }
    }

    IEnumerator TrackInteractionUI()
    {
        while (m_interactable != null)
        {
            InteractionUI.transform.position = Camera.main.WorldToScreenPoint(m_interactable.Position);
            yield return null;
        }

    }
}

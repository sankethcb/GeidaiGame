using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MusicNoteBlock : MonoBehaviour, IPointerDownHandler
{
    public SpriteRenderer SpriteRenderer;
    public event System.Action<MusicNoteBlock> OnTap;
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Tapped");
        OnTap?.Invoke(this);
    }

}

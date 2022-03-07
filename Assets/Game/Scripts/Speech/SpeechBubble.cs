using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public TMPro.TextMeshPro TextObject;
    public SpriteRenderer Sprite;


    Vector3 flipScale = new Vector3(-1, 1, 1);

    public void SetRight()
    {
        Sprite.transform.parent.localScale = flipScale;
    }

    public void SetLeft() 
    { 
        Sprite.transform.parent.localScale = Vector3.one;
    }

}

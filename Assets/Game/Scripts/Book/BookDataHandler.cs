using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDataHandler : MonoBehaviour
{
    public List<PageData> PageData;
}


[System.Serializable]
public class PageData
{
    public Sprite Portrait;
    public string Name;
    public string Language;
}

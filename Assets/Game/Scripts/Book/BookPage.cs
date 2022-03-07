using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BookPage : MonoBehaviour
{
    [Header("References")]
    public Image Portrait;
    public TextMeshProUGUI NameObject;
    public TextMeshProUGUI LangObject;


    public void LoadPageData(PageData pageData)
    {
        Portrait.sprite = pageData?.Portrait;
        NameObject.text = pageData?.Name;
        LangObject.text = pageData?.Language;
    }
}

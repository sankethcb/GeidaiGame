using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Animator2D : MonoBehaviour
{
    [Header("References")]
    public Animator Animator;


    //[Header("Variables")]
    //[ReadOnly] [SerializeField] List<int> codeSet;


    public void SetInt(string intName, int value)
    {
        Animator.SetInteger(intName, value);
    }

    public void SetFloat(string floatName, float value)
    {
        Animator.SetFloat(floatName, value);
    }
    public void SetTrigger(string triggerName)
    {
        Animator.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool value)
    {
        Animator.SetBool(boolName, value);
    }

    void OnValidate()
    {
       // codeSet.Clear();

        //foreach (string variable in variableSet)
            //codeSet.Add(Animator.StringToHash(variable));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlagEvent : MonoBehaviour
{
    public GameEventFlag Flag;
    public bool OneShot = true;
    public UnityEvent triggerEvent;


    void Start()
    {
        Flag.OnFlagRaised += Trigger;
    }

    public void Trigger()
    {
        if(OneShot)
            Flag.OnFlagRaised -= Trigger;
        triggerEvent.Invoke();
    }
}

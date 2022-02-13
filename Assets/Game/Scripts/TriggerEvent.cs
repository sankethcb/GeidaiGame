using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerEvent : MonoBehaviour
{
    public bool TriggerOnStart = false;
    public UnityEvent triggerEvent;
    void OnTriggerEnter2D(Collider2D other)
    {
        Trigger();
    }

    void OnTriggerEnter(Collider other)
    {
        Trigger();
    }

    void Start()
    {
        if (TriggerOnStart) Trigger();
    }

    public void Trigger()
    {
        triggerEvent.Invoke();
    }
}

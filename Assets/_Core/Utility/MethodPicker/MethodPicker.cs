using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [System.Serializable]
    public class MethodPicker
    {
        [SerializeField] protected string key;
        [SerializeField] protected Object target;
        [SerializeField] protected Object behaviour;

        public Object Behaviour => behaviour;

        public System.Action Method { get; private set; }
        public virtual System.Action BuildMethod()
        {
            if (target == null || behaviour == null || key == "")
                return null;

            foreach (var method in behaviour.GetType().GetMethods())
            {
                if (method.Name == key)
                {
                    Method = (System.Action)System.Delegate.CreateDelegate(typeof(System.Action), behaviour, method);
                    return Method;
                }
            }
            return null;
        }
    }

    [System.Serializable]
    public class MethodPicker<T> : MethodPicker where T : struct
    {
        public new System.Action<T> Method { get; private set; }

        public new System.Action<T> BuildMethod()
        {
            if (target == null || behaviour == null || key == "")
                return null;

            foreach (var method in behaviour.GetType().GetMethods())
            {
                if (method.Name == key)
                {
                    Method = (System.Action<T>)System.Delegate.CreateDelegate(typeof(System.Action<T>), behaviour, method);
                    return Method;
                }
            }

            return null;
        }
    }
}
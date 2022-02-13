using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.FSM
{

    [System.Serializable]
    class Condition
    {
        public event System.Action OnSatisfied;

        public virtual void Satisfy() => OnSatisfied?.Invoke();
    }

    public class Transition
    {
        State m_to = null;

        public event System.Action<State> OnTrigger;

        public System.Action<bool> Trigger;
        public System.Action Reset;

        public Transition(State to)
        {
            m_to = to;

            Trigger = (bool condition) =>
            {
                if (condition)
                    OnTrigger?.Invoke(m_to);
            };
            
            Reset = () => OnTrigger?.Invoke(null);
        }

    }
}
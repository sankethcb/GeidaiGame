using System.Collections;
using System.Collections.Generic;

using Utilities;



namespace Core.FSM
{
    [System.Serializable]
    public class State
    {
        private System.Action m_OnEnter;
        private System.Action m_OnExit;
        private System.Action m_OnLogicUpdate;
        private System.Action m_OnPhysicsUpdate;

        public string Name { get; protected set; }

        List<Transition> m_transitions = new List<Transition>(1);
        public List<Transition> Transitions => m_transitions;

        bool m_hasExitTime = false;

        public State(string name, bool hasExitTime = false)
        {
            Name = name;
            m_hasExitTime = hasExitTime;
        }

        public State(
            string name,
            bool hasExitTime = false,
            System.Action onEnter = null,
            System.Action onExit = null,
            System.Action onLogicUpdate = null,
            System.Action onPhysicsUpdate = null)
        {
            m_OnEnter = onEnter;
            m_OnExit = onExit;
            m_OnLogicUpdate = onLogicUpdate;
            m_OnPhysicsUpdate = onPhysicsUpdate;

            Name = name;
            m_hasExitTime = hasExitTime;
        }

        public void SetActions(System.Action onEnter = null,
            System.Action onExit = null,
            System.Action onLogicUpdate = null,
            System.Action onPhysicsUpdate = null)
        {
            m_OnEnter = onEnter;
            m_OnExit = onExit;
            m_OnLogicUpdate = onLogicUpdate;
            m_OnPhysicsUpdate = onPhysicsUpdate;
        }


        public Transition AddTransition(Transition transition)
        {
            if (!m_transitions.Contains(transition))
            {
                m_transitions.Add(transition);
                return transition;
            }

            return null;

        }
        public Transition RemoveTransition(Transition transition)
        {
            if (m_transitions.Contains(transition))
            {
                m_transitions.Remove(transition);
                return transition;
            }
            return null;
        }

        public virtual void Enter()
        {
            m_OnEnter?.Invoke();
        }

        public virtual void Exit()
        {
            m_OnExit?.Invoke();
        }

        public virtual void LogicUpdate()
        {
            m_OnLogicUpdate?.Invoke();
        }

        public virtual void PhysicsUpdate()
        {
            m_OnPhysicsUpdate?.Invoke();
        }

        public virtual void Release()
        {
            //for (int i = 0; i < m_transitions.Count; i++)
            //m_transitions[i].Release();
        }
    }
}

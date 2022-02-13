using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DG.Tweening;

namespace Core.FSM
{
    [System.Serializable]
    public class StateMachine : State
    {
        public State activeState { get; protected set; }
        public State nextState { get; protected set; }
        public event System.Action OnTransition;

        [UnityEngine.SerializeField] protected List<State> m_states = new List<State>(5);
        [UnityEngine.SerializeField] protected List<Transition> m_globalTransitions = new List<Transition>(5);

        System.Action<State> SetTranstion;

        public StateMachine(string stateMachineName) : base(stateMachineName, false)
        {
            SetTranstion = (State nextState) => this.nextState = nextState;
        }

        public virtual void SetStartSate(State startState) => activeState = startState;

        public virtual void AddState(State newState)
        {
            //if (!m_states.Contains(newState))
            m_states.Add(newState);
        }

        public virtual void RemoveState(State state)
        {
            if (m_states.Contains(state))
                m_states.Remove(state);
        }

        public virtual void IntegrateSubStateMachine(State state, StateMachine subStateMachine)
        {
            if (m_states.Contains(state))
                m_states[m_states.IndexOf(state)] = subStateMachine;
        }

        public override void Enter()
        {
            RegisterStateTransitions();
            activeState?.Enter();
            base.Enter();
        }

        public virtual void Enter(State startState)
        {
            SetStartSate(startState);
            Enter();
        }

        public override void Exit()
        {
            activeState?.Exit();
            base.Exit();
        }

        public override void LogicUpdate()
        {
            if (nextState != null)
            {
                TransitionTo(nextState);
            }

            activeState?.LogicUpdate();
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            activeState?.PhysicsUpdate();
            base.PhysicsUpdate();
        }

        public virtual void TransitionTo(State nextState)
        {
            activeState?.Exit();

            DeregisterStateTransitions();

            activeState = nextState;

            RegisterStateTransitions();
            this.nextState = null;

            activeState?.Enter();

            OnTransition?.Invoke();
        }

        public virtual Transition AddGlobalTransition(Transition transition)
        {
            if (m_globalTransitions.Contains(transition))
                return null;

            m_globalTransitions.Add(transition);
            transition.OnTrigger += SetTranstion;

            return transition;
        }

        public virtual Transition RemoveGlobalTransition(Transition transition)
        {
            if (!m_globalTransitions.Contains(transition))
                return null;

            m_globalTransitions.Remove(transition);
            transition.OnTrigger -= SetTranstion;

            return transition;
        }

        public virtual Transition AddTransition(State from, State to)
        {
            if (m_states.Contains(from))
            {
                from.AddTransition(new Transition(to));
                return from.Transitions[from.Transitions.Count - 1];
            }

            return null;
        }

        public virtual Transition AddTransition(State from, Transition transition)
        {
            if (m_states.Contains(from))
            {
                from.AddTransition(transition);
                return transition;
            }
            return null;
        }

        public virtual void RemoveTransition(State from, Transition transition)
        {
            if (m_states.Contains(from))
                from.RemoveTransition(transition);
        }

        public override void Release()
        {
            for (int i = 0; i < m_globalTransitions.Count; i++)
                m_globalTransitions[i].OnTrigger -= SetTranstion;

            base.Release();
        }

        void RegisterStateTransitions()
        {
            for (int i = 0; i < activeState.Transitions.Count; i++)
                activeState.Transitions[i].OnTrigger += SetTranstion;
        }

        void DeregisterStateTransitions()
        {
            for (int i = 0; i < activeState.Transitions.Count; i++)
                activeState.Transitions[i].OnTrigger -= SetTranstion;
        }
    }

    [System.Serializable]
    public class StateMachine<TEnum> : StateMachine where TEnum : System.Enum
    {
        int m_index;

        public StateMachine(string stateMachineName) : base(stateMachineName) { }

        public virtual void Enter(TEnum startState)
        {
            m_index = ToInt(startState);
            Enter(m_states[m_index]);
        }

        public virtual State GetState(TEnum state)
        {
            m_index = ToInt(state);
            return m_states[m_index];

        }

        public virtual void AddSubStateMachine(TEnum state, StateMachine subStateMachine)
        {
            m_index = ToInt(state);
            m_states[m_index] = subStateMachine;
        }

        public virtual Transition AddTransition(TEnum from, TEnum to)
        {
            m_index = ToInt(from);
            return m_states[m_index].AddTransition(new Transition(m_states[ToInt(to)]));
        }

        public virtual Transition RemoveTransition(TEnum from, Transition transition)
        {
            m_index = ToInt(from);
            return m_states[m_index].RemoveTransition(transition);
        }

        public virtual void TransitionTo(TEnum nextState)
        {
            m_index = ToInt(nextState);
            base.TransitionTo(m_states[m_index]);
        }

        protected int ToInt(TEnum enumValue) => System.Runtime.CompilerServices.Unsafe.As<TEnum, int>(ref enumValue);
    }
}

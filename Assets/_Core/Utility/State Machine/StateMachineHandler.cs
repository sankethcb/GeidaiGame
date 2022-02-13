using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DG.Tweening;

namespace Core.FSM
{
    public class StateMachineHandler : MonoBehaviour
    {
        [SerializeField] StateMachine m_stateMachine;

        [ReadOnly] [SerializeField] string m_currentState = string.Empty;

        public virtual StateMachine<TEnum> Initialize<TEnum>() where TEnum : System.Enum
        {
            System.Type stateType = typeof(TEnum);

            StateMachine<TEnum> stateMachine = new StateMachine<TEnum>(stateType.ToString());
            m_stateMachine = stateMachine;

            InitalizeStates(System.Enum.GetNames(stateType));

            stateMachine.OnTransition += SetActiveState;
            //SetActiveState();

            return stateMachine;
        }

        public virtual void Release()
        {
            if (m_stateMachine != null)
            {
                m_stateMachine.Exit();
                m_stateMachine.Release();

                m_stateMachine.OnTransition -= SetActiveState;
                m_stateMachine = null;
            }

            m_currentState = string.Empty;
        }

        protected virtual void SetActiveState() => m_currentState = m_stateMachine.activeState.Name;

        void OnDestroy()
        {
            m_stateMachine?.Release();
        }

        void InitalizeStates(string[] stateNames)
        {
            for (int i = 0; i < stateNames.Length; i++)
            {
                m_stateMachine.AddState(new State(stateNames[i], false));
            }
        }

        void Update()
        {
            m_stateMachine.LogicUpdate();
        }

        void FixedUpdate()
        {
            m_stateMachine.PhysicsUpdate();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using DG.Tweening;
using UnityEngine.Events;
public class ScreenTransition : MonoBehaviour
{
    [Header("Settings")]
    public Animator TransitionAnimator;
    public string TransitionTriggerName;

    public UnityEvent OnTransitionStart;
    public UnityEvent  OnTransitionEnd;
    public UnityEvent  OnFadeIn;
    public UnityEvent  OnFadeOut;

    Coroutine m_current;
    bool m_halfComplete;

    public Coroutine BeginTransitionHalf()
    {
        if (!m_halfComplete)
        {
            OnTransitionStart?.Invoke();
            m_current = FadeIn().Start(this);
        }
        else
            m_current = FadeOut().Start(this);
    

        return m_current;
    }

    public void StartTransition()
    {
        BeginTransitionHalf();
    }


    IEnumerator FadeIn()
    {
        TransitionAnimator.SetTrigger(TransitionTriggerName);
        yield return null;
        yield return new WaitForSeconds(TransitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        m_halfComplete = true;
        OnFadeIn?.Invoke();
    }

    IEnumerator FadeOut()
    {
        TransitionAnimator.SetTrigger(TransitionTriggerName);
        yield return null;
        yield return new WaitForSeconds(TransitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        m_halfComplete = false;
        OnFadeOut?.Invoke();
        TransitionComplete();
    }

    void TransitionComplete()
    {
        m_current = null;
        OnTransitionEnd?.Invoke();
    }
}

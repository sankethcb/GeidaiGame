using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public List<TaskPair> Tasks;
    public Task CurrentTask => Tasks[m_currentTaskIndex].Task;

    int m_currentTaskIndex;

    public event System.Action<Quest> OnQuestStart;
    public event System.Action<Quest> OnQuestComplete;
    public event System.Action<Task> OnNextTask;


    public void StartQuest()
    {
        m_currentTaskIndex = -1;
        OnQuestStart?.Invoke(this);
        NextTask();
    }

    public void NextTask()
    {
        if (m_currentTaskIndex != -1)
        {
            Tasks[m_currentTaskIndex].Event.OnFlagRaised -= NextTask;
            CurrentTask.CompleteTask();
        }

        m_currentTaskIndex++;
        if (m_currentTaskIndex == Tasks.Count)
        {
            Completed();
            return;
        }

        if (Tasks[m_currentTaskIndex].CheckCurrent && Tasks[m_currentTaskIndex].Event.Raised)
        {
            OnNextTask?.Invoke(CurrentTask);
            NextTask();
        }
        else
        {
            Tasks[m_currentTaskIndex].Event.OnFlagRaised += NextTask;
            CurrentTask.StartTask();
            OnNextTask?.Invoke(CurrentTask);
        }

        
    }

    public void Completed()
    {
        OnQuestComplete?.Invoke(this);
    }

}

[System.Serializable]
public class Task
{
    public event System.Action OnTaskStart;
    public event System.Action OnTaskComplete;

    public void StartTask()
    {
        Debug.Log("task started");
        OnTaskStart?.Invoke();
    }

    public void CompleteTask()
    {
        Debug.Log("task complete");
        OnTaskComplete?.Invoke();
    }
}

[System.Serializable]
public struct TaskPair
{
    public string Name;
    public Task Task;
    public GameEventFlag Event;
    public bool CheckCurrent;

}

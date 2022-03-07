using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestTracker : MonoBehaviour
{
    public List<QuestPair> QuestList;
    public Quest Current => QuestList[m_questIndex].QuestData.Quest;
    int m_questIndex = 0;

    void Start()
    {
        RegisterQuestEvents();
        Current.StartQuest();
    }

    void OnDestroy() 
    {
        DeregisterQuestEvents();
    }

    void RegisterQuestEvents()
    {
        for (int i = 0; i < Current.Tasks.Count; i++)
        {
            Current.Tasks[i].Task.OnTaskComplete += QuestList[m_questIndex].TaskEvents[i].Invoke;
        }
    }

    void DeregisterQuestEvents()
    {
        for (int i = 0; i < Current.Tasks.Count; i++)
        {
            Current.Tasks[i].Task.OnTaskComplete -= QuestList[m_questIndex].TaskEvents[i].Invoke;
        }
    }


    void OnValidate()
    {
        foreach (QuestPair questPair in QuestList)
        {
            if (questPair.TaskEvents.Count < questPair.QuestData.Quest.Tasks.Count)
            {
                int difference = questPair.QuestData.Quest.Tasks.Count - questPair.TaskEvents.Count;

                for (int i = 0; i < difference; i++)
                    questPair.TaskEvents.Add(new UnityEvent());
            }
            else if (questPair.TaskEvents.Count > questPair.QuestData.Quest.Tasks.Count)
            {
                int difference = questPair.QuestData.Quest.Tasks.Count - questPair.TaskEvents.Count;

                for (int i = 0; i < difference; i++)
                    questPair.TaskEvents.RemoveAt(questPair.TaskEvents.Count - 1);
            }
        }
    }

}

[System.Serializable]
public struct QuestPair
{
    public QuestData QuestData;
    public List<UnityEvent> TaskEvents;
}

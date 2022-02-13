using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CustomMessageHub", menuName = "Utilities/Message Hub", order = 1)]
public class MessageHub : ScriptableObject
{
    private Dictionary<Type, IMessageID> m_messageSet = new Dictionary<Type, IMessageID>();

    IMessageID m_customEvent;

    ///<summary>
    ///
    ///</summary>
    /// <typeparam name="MessageName">Custom Message Name</typeparam>
    public MessageName Message<MessageName>() where MessageName : IMessageID, new()
    {
        Type eventType = typeof(MessageName);
    
        if (m_messageSet.TryGetValue(eventType, out m_customEvent))
        {
            return (MessageName)m_customEvent;
        }
        
        return (MessageName)RegisterEvent(eventType);
    }

    private IMessageID RegisterEvent(Type eventType)
    {
    
        m_customEvent = Activator.CreateInstance(eventType) as IMessageID;
        m_messageSet.Add(eventType, m_customEvent);
            
        return m_customEvent;
    }

    /// <summary>
    /// Prints list of currently registered events to the Unity Console
    /// </summary>
    public string LogMessages()
    {
        string eventSetLog = "No Messages Currently Registered";

        if (m_messageSet.Count > 0)
        {
            eventSetLog = m_messageSet.Count + " Messages Currently Registered:\n";

            foreach (KeyValuePair<Type, IMessageID> pair in m_messageSet)
            {
                eventSetLog += "Owner: " + pair.Key.ToString().Replace("+", " // Message Name: ") + "\n";
            }
        }

        Debug.Log(eventSetLog);

        return eventSetLog;
    }

}

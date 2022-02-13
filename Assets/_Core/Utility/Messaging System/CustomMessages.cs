using System;

//public delegate void Response<T>(out T response);
public interface IMessageID
{
    string MessageID { get; }
}

public abstract class CustomMessageBase : IMessageID
{
    protected string _eventID;

    public string MessageID
    {
        get
        {
            if (string.IsNullOrEmpty(_eventID)) { _eventID = this.ToString(); }

            return _eventID;
        }
    }
}

//Event with no parameters
public abstract class GameMessage : CustomMessageBase
{
    Action _callback;

    public void AddListener(Action listener)
    {
        _callback += listener;
    }

    public void RemoveListener(Action listener)
    {
        _callback -= listener;
    }

    public void RemoveAllListeners()
    {
        _callback = null;
    }

    public void Send()
    {
        _callback?.Invoke();
    }
}

//Event with 1 generic parameter
public abstract class GameMessage<T> : CustomMessageBase
{
    Action<T> _callback;

    public void AddListener(Action<T> listener)
    {
        _callback += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        _callback -= listener;
    }

    public void RemoveAllListeners()
    {
        _callback = null;
    }

    public void Send(T payload)
    {
        _callback?.Invoke(payload);
    }
}




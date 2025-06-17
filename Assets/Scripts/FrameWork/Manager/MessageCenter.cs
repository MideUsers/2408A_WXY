using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum MessageID
{
    RECEPTIONSCROLL = 101,
    RECEPTIONBAG,
    SENDSCROLL,
}

public class Message
{
    public int Type;
    public object Data;

    public Message(int type, object data)
    {
        Type = type;
        Data = data;
    }
}

internal class MessageCenter:Singleton<MessageCenter>
{
    private Dictionary<int, Action<Message>> _listeners = new Dictionary<int, Action<Message>>();
    public void AddListener(int messageType, Action<Message> listener)
    {
        if (!_listeners.ContainsKey(messageType))
        {
            _listeners[messageType] = listener;
        }
        else
        {
            _listeners[messageType] += listener;
        }
    }

    // 取消订阅消息
    public void RemoveListener(int messageType, Action<Message> listener)
    {
        if (_listeners.ContainsKey(messageType))
        {
            _listeners[messageType] -= listener;
        }
    }

    // 发送消息
    public void SendMessage(int messageType,object obj)
    {
        if (_listeners.ContainsKey(messageType))
        {
            _listeners[messageType]?.Invoke(new Message(messageType,obj));
        }
    }
}

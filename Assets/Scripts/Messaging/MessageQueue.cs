using System.Collections;
using System.Collections.Generic;

public class MessageQueue {

    public delegate void MessageDelegate<T> (T msg) where T : IGameMessage;
    private delegate void MessageDelegate (IGameMessage e, string filter);
    
    private int limitQueueProcesing = 30;
    private Queue messageQueue = new Queue();
    private Dictionary<System.Type, MessageDelegate> delegates = new Dictionary<System.Type, MessageDelegate>();
    private Dictionary<System.Delegate, MessageDelegate> delegateLookup = new Dictionary<System.Delegate, MessageDelegate>();
    
    
    public MessageQueue () {
        UEventsManager.Instance.OnLateUpdate += Update;
    }

    public void Destroy () {
        UEventsManager.Instance.OnLateUpdate -= Update;
        delegates.Clear();
        delegateLookup.Clear();
        messageQueue.Clear();
    }

    private void Update () {
        int i = 0;
        while (messageQueue.Count > 0) {
            if (i >= limitQueueProcesing) return;
            i++;
            IGameMessage msg = messageQueue.Dequeue() as IGameMessage;
            FireMessage(msg);
        }
    }
    
    public void QueueEvent (IGameMessage message) {
        messageQueue.Enqueue(message);
    }

    public void AddListener<T> (MessageDelegate<T> del, string filter = "All") where T : IGameMessage {
        if (delegateLookup.ContainsKey(del)) return;
        MessageDelegate internalDelegate = (msg, fltr) => { if (fltr == filter) del((T) msg); };
        delegateLookup[del] = internalDelegate;
        
        MessageDelegate tempDel;
        if (delegates.TryGetValue(typeof(T), out tempDel)) {
            delegates[typeof(T)] = tempDel += internalDelegate; 
        } else {
            delegates[typeof(T)] = internalDelegate;
        }
    }
    
    public void RemoveListener<T> (MessageDelegate<T> del) where T : IGameMessage {
        MessageDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate)) {
            MessageDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel)){
                tempDel -= internalDelegate;
                if (tempDel == null){
                    delegates.Remove(typeof(T));
                } else {
                    delegates[typeof(T)] = tempDel;
                }
            }
            delegateLookup.Remove(del);
        }
    }
    
    private void FireMessage(IGameMessage msg) {
        MessageDelegate del;
        if (delegates.TryGetValue(msg.GetType(), out del)) {
            del.Invoke(msg, "All");
            del.Invoke(msg, msg.SquadName);
        }
    }

}

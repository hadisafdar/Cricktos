using UnityEngine;
using System;

public abstract class EventListenerMonoBehaviour : MonoBehaviour
{

    protected virtual void Awake()
    {
        SubscribeToEvents();
    }

    /// <summary>
    /// Use this method to subscribe to all events
    /// </summary>
    protected abstract void SubscribeToEvents();
   
    /// <summary>
    /// Use this method to unsubscribe to all events
    /// </summary>
    protected abstract void UnSubscribeEvents();

   
    protected virtual void OnDestroy()
    {
        UnSubscribeEvents();
    }

   
}

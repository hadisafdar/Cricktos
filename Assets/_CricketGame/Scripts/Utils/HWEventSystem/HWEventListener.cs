namespace HyyderWorks.EventSystem
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public class HWCustomUnityEvent : UnityEvent<Component, object[]> { }

    [System.Serializable]
    public class HWEventListenerData
    {
        public HWEvent EventToListenTo;
        public HWCustomUnityEvent Response;
    }

    /// <summary>
    /// Class to listen to a specific HWEvent
    /// </summary>
    public class HWEventListener : MonoBehaviour,IEventListener
    {
        [SerializeField]private HWEventListenerData eventData;

        public void OnEventRaised(HWEvent raisedEvent,Component sender,object[] payload)
        {
            if (raisedEvent.Equals(eventData.EventToListenTo))
            {
                eventData.Response?.Invoke(sender, payload);
            }
        }
        private void OnEnable()
        {
            AddListener();
        }
        private void OnDisable()
        {
            RemoveListener();
        }

        private void OnDestroy()
        {
            RemoveListener();
        }

        public void AddListener()
        {
            eventData.EventToListenTo.AddListener(this);
        }

        public void RemoveListener()
        {
            eventData.EventToListenTo.RemoveListener(this);
        }
    }

}
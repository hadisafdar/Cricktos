namespace HyyderWorks.EventSystem
{
    using UnityEngine;

    /// <summary>
    /// A class that can listen to multiple events
    /// </summary>

    public class HWMultiEventListener : MonoBehaviour, IEventListener
    {
        [SerializeField] private HWEventListenerData[] EventsData;


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
            foreach (HWEventListenerData data in EventsData)
            {
                data.EventToListenTo.AddListener(this);
            }
        }


        public void OnEventRaised(HWEvent raisedEvent,Component sender, object[] payload)
        {
            foreach (HWEventListenerData data in EventsData)
            {
                if (data.EventToListenTo.Equals(raisedEvent))
                {
                    data.Response?.Invoke(sender, payload);
                }
            }
        }

        public void RemoveListener()
        {
            foreach (HWEventListenerData data in EventsData)
            {
                data.EventToListenTo.RemoveListener(this);
            }
        }
    }

}
namespace HyyderWorks.EventSystem {

    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "New HW Event", menuName = "HyyderWorks/Event System/Events")]

    public class HWEvent : ScriptableObject
    {
        private List<IEventListener> eventListeners = new List<IEventListener>();

        public void Raise(Component sender, object[] payload)
        {
            for (int i = 0; i < eventListeners.Count; i++)
            {
                eventListeners[i].OnEventRaised(this,sender,payload);
            }
        }

        public void AddListener(IEventListener listener)
        {
            if (eventListeners.Contains(listener)) return;
            eventListeners.Add(listener);
        }

        public void RemoveListener(IEventListener listener)
        {
            if (!eventListeners.Contains(listener)) return;
            eventListeners.Remove(listener);
        }



    
    }


}

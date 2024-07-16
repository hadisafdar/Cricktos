namespace HyyderWorks.EventSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IEventListener
    {
        public void OnEventRaised(HWEvent raisedEvent,Component sender, object[] payload);
        public void AddListener();
        public void RemoveListener();
    }

}
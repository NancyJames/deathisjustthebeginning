using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HR.Utilities.Events
{
    [System.Serializable]
    public class Listener
    {
        public GameEvent Event;
        public UnityEvent<object> Response = new UnityEvent<object>();

        public void OnEventRaised(object test = null)
        { 
            Response.Invoke(test); 
        }
    }
}


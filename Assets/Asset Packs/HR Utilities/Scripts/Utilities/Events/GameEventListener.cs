using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HR.Utilities.Events
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField] Listener[] listeners;

        private void OnEnable()
        { 
            for(int i = 0;i<listeners.Length;i++)
            {
                listeners[i].Event.Subscribe(listeners[i]);
            }
            
        }

        private void OnDisable()
        {
            for (int i = 0; i < listeners.Length; i++)
            {
                listeners[i].Event.Unsubscribe(listeners[i]);
            }
        }

        
    }
}


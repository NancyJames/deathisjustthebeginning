using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Variables;
using HR.Utilities.Events;

namespace HR.Utilities.UI
{

    public abstract class UIMeter : MonoBehaviour
    {
        [Tooltip("Value between 0 and 1 for standard slider or image scale, or use in conjunction with max value to automatically calculate a value between 0 and 1")]
        [SerializeField] FloatVariable value;
        [Tooltip("Optional")] [SerializeField] FloatVariable maxValue;
        [Tooltip("Game Event that triggers the meter to update")]
        [SerializeField] GameEvent updateEvent;
        Listener listener = new Listener();

        private void Awake()
        {
            listener.Event = updateEvent;
            listener.Response.AddListener(ValueUpdated);
        }
        private void Start()
        {
            ValueUpdated();
        }

        public float GetValue()
        {
            if(maxValue!=null)
            {
                return value.Get() / maxValue.Get();
            }
            return value.Get();
        }

        protected virtual void ValueUpdated(object o=null)
        {
            
        }

        private void OnEnable()
        {
            listener.Event.Subscribe(listener);
        }

        private void OnDisable()
        {
            listener.Event.Unsubscribe(listener);
        }

    }
}


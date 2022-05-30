using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

namespace HR.Utilities.Variables
{
    public abstract class CustomVariable : ScriptableObject
    {
        [SerializeField] protected GameEvent valueUpdated = null;

        public void Updated()
        {
            valueUpdated?.Raise();
        }

        
    }
}

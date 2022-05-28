using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HR.Utilities.Events
{
	[CreateAssetMenu]
	public class GameEvent : ScriptableObject
	{
		private List<Listener> listeners =
			new List<Listener>();

		public void Raise(object test=null)
		{
			for (int i = listeners.Count - 1; i >= 0; i--)
				listeners[i].OnEventRaised(test);
		}

		public void Subscribe(Listener listener)
		{ listeners.Add(listener); }

		public void Unsubscribe(Listener listener)
		{ listeners.Remove(listener); }
	}
}


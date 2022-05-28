using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;


namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class GameObjectVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public GameObject storedValue;

		[NonSerialized]
		public GameObject runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public GameObject Get()
		{
			return runtimeValue;
		}

		public void Set(GameObject val)
		{
			runtimeValue = val;
			Updated();
		}


		public void OnBeforeSerialize()
		{

		}

	}
}

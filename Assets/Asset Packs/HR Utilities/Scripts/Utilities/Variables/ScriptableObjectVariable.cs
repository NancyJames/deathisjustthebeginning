using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR.Utilities.Events;

namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class ScriptableObjectVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public ScriptableObject storedValue;

		[NonSerialized]
		public ScriptableObject runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public ScriptableObject Get()
		{
			return runtimeValue;
		}

		public void Set(ScriptableObject val)
		{
			runtimeValue = val;
			Updated();
		}


		public void OnBeforeSerialize()
		{

		}

	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class StringVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public string storedValue;

		[NonSerialized]
		public string runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public string Get()
		{
			return runtimeValue;
		}

		public void Set(string val)
		{
			runtimeValue = val;
			Updated();
		}

		public void OnBeforeSerialize()
		{
		}
	}
}


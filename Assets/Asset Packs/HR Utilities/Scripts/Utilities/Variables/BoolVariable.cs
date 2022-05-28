using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class BoolVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public bool storedValue;

		[NonSerialized]
		public bool runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public bool Get()
		{
			return runtimeValue;
		}

		public void Set(bool val)
		{
			runtimeValue = val;
			Updated();
		}

		public void Toggle()
		{
			runtimeValue = !runtimeValue;
			Updated();
		}

		public void OnBeforeSerialize()
		{

		}
	}
}


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class IntVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public int storedValue;

		[NonSerialized]
		public int runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public int Get()
		{
			return runtimeValue;
		}

		public void Set(int val)
		{
			runtimeValue = val;
			Updated();
		}

		public void Increment(int by = 1)
		{
			runtimeValue += by;
			Updated();
		}

		public void Decrement(int by = 1)
		{
			runtimeValue -= by;
			Updated();
		}

		public void OnBeforeSerialize()
		{

		}
	}
}

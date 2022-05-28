using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HR.Utilities.Variables
{
	[CreateAssetMenu]
	public class FloatVariable : CustomVariable, ISerializationCallbackReceiver
	{
		public float storedValue ;

		[NonSerialized]
		private float runtimeValue;

		public void OnAfterDeserialize()
		{
			runtimeValue = storedValue;
			Updated();
		}

		public float Get()
		{
			return runtimeValue;
		}

		public void Set(float val)
		{
			runtimeValue = val;
			Updated();
		}

		public void Increment(float by = 1)
		{
			runtimeValue += by;
			Updated();
		}

		public void Decrement(float by = 1)
		{
			runtimeValue -= by;
			Updated();
		}

		public void OnBeforeSerialize()
		{

		}
	}
}

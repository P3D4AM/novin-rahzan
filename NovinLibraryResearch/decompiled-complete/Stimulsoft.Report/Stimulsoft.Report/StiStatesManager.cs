using System;
using System.Collections;

namespace Stimulsoft.Report;

public sealed class StiStatesManager
{
	private class IntStorage
	{
		public int value;

		public IntStorage(int value)
		{
			this.value = value;
		}
	}

	private class FloatStorage
	{
		public float value;

		public FloatStorage(float value)
		{
			this.value = value;
		}
	}

	private class DoubleStorage
	{
		public double value;

		public DoubleStorage(double value)
		{
			this.value = value;
		}
	}

	private class Int64Storage
	{
		public long value;

		public Int64Storage(long value)
		{
			this.value = value;
		}
	}

	private class DecimalStorage
	{
		public decimal value;

		public DecimalStorage(decimal value)
		{
			this.value = value;
		}
	}

	private class RangeStorage
	{
		public Type RangeType;

		public object From;

		public object To;

		public RangeStorage(Range range)
		{
			RangeType = range.GetType();
			From = range.FromObject;
			To = range.ToObject;
		}
	}

	private static object ValueBoolFalse = new object();

	private static object ValueBoolTrue = new object();

	private Hashtable states = new Hashtable();

	public void Push(string stateName, object obj, string property, object value)
	{
		Hashtable hashtable = states[stateName] as Hashtable;
		if (hashtable == null)
		{
			hashtable = new Hashtable();
			states.Add(stateName, hashtable);
		}
		Hashtable hashtable2 = hashtable[obj] as Hashtable;
		if (hashtable2 == null)
		{
			hashtable2 = new Hashtable();
			hashtable.Add(obj, hashtable2);
		}
		hashtable2[property] = value;
	}

	public void PushBool(string stateName, object obj, string property, bool value)
	{
		if (value)
		{
			Push(stateName, obj, property, ValueBoolTrue);
		}
		else
		{
			Push(stateName, obj, property, ValueBoolFalse);
		}
	}

	public void PushInt(string stateName, object obj, string property, int value)
	{
		Push(stateName, obj, property, new IntStorage(value));
	}

	public void PushInt64(string stateName, object obj, string property, long value)
	{
		Push(stateName, obj, property, new Int64Storage(value));
	}

	public void PushFloat(string stateName, object obj, string property, float value)
	{
		Push(stateName, obj, property, new FloatStorage(value));
	}

	public void PushDouble(string stateName, object obj, string property, double value)
	{
		Push(stateName, obj, property, new DoubleStorage(value));
	}

	public void PushDecimal(string stateName, object obj, string property, decimal value)
	{
		Push(stateName, obj, property, new DecimalStorage(value));
	}

	public void PushRange(string stateName, object obj, string property, Range value)
	{
		Push(stateName, obj, property, new RangeStorage(value));
	}

	public object Pop(string stateName, object obj, string property)
	{
		if (!(states[stateName] is Hashtable hashtable))
		{
			return null;
		}
		if (!(hashtable[obj] is Hashtable hashtable2))
		{
			return null;
		}
		return hashtable2[property];
	}

	public bool PopBool(string stateName, object obj, string property)
	{
		object obj2 = Pop(stateName, obj, property);
		if (obj2 == ValueBoolFalse)
		{
			return false;
		}
		return true;
	}

	public int PopInt(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is IntStorage intStorage))
		{
			return 0;
		}
		return intStorage.value;
	}

	public long PopInt64(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is Int64Storage int64Storage))
		{
			return 0L;
		}
		return int64Storage.value;
	}

	public double PopDouble(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is DoubleStorage doubleStorage))
		{
			return 0.0;
		}
		return doubleStorage.value;
	}

	public float PopFloat(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is FloatStorage floatStorage))
		{
			return 0f;
		}
		return floatStorage.value;
	}

	public decimal PopDecimal(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is DecimalStorage decimalStorage))
		{
			return 0m;
		}
		return decimalStorage.value;
	}

	public Range PopRange(string stateName, object obj, string property)
	{
		if (!(Pop(stateName, obj, property) is RangeStorage rangeStorage))
		{
			return null;
		}
		Range range = Activator.CreateInstance(rangeStorage.RangeType) as Range;
		range.FromObject = rangeStorage.From;
		range.ToObject = rangeStorage.To;
		return range;
	}

	public bool IsExist(string stateName, object obj)
	{
		if (states[stateName] == null)
		{
			return false;
		}
		Hashtable hashtable = states[stateName] as Hashtable;
		if (hashtable[obj] == null)
		{
			return false;
		}
		return true;
	}

	public void ClearState(string stateName)
	{
		states.Remove(stateName);
	}

	public void Clear()
	{
		states.Clear();
	}
}

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public readonly struct ValueBuffer
{
	public static readonly ValueBuffer Empty = default(ValueBuffer);

	private readonly object[] _values;

	internal static readonly MethodInfo GetValueMethod = typeof(ValueBuffer).GetRuntimeProperties().Single((PropertyInfo p) => p.GetIndexParameters().Length != 0).GetMethod;

	public object this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _values[index];
		}
		[param: JetBrains.Annotations.CanBeNull]
		set
		{
			_values[index] = value;
		}
	}

	public int Count => _values.Length;

	public bool IsEmpty => _values == null;

	public ValueBuffer([JetBrains.Annotations.NotNull] object[] values)
	{
		_values = values;
	}

	public override bool Equals(object obj)
	{
		if (obj != null && obj is ValueBuffer other)
		{
			return Equals(other);
		}
		return false;
	}

	private bool Equals(ValueBuffer other)
	{
		if (_values.Length != other._values.Length)
		{
			return false;
		}
		for (int i = 0; i < _values.Length; i++)
		{
			if (!object.Equals(_values[i], other._values[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override int GetHashCode()
	{
		HashCode hashCode = default(HashCode);
		object[] values = _values;
		foreach (object value in values)
		{
			hashCode.Add(value);
		}
		return hashCode.ToHashCode();
	}
}

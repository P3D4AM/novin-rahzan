using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Customization;

internal class OverwriteTransform<TValue> : ChangeTransform<TValue>, IEquatable<OverwriteTransform<TValue>>
{
	public TValue Value { get; }

	public OverwriteTransform(TValue value)
	{
		Value = value;
	}

	public override TValue Apply(TValue original)
	{
		return Value;
	}

	public bool Equals(OverwriteTransform<TValue> other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (Value is IEnumerable source)
		{
			return source.Cast<object>().SequenceEqual((other.Value as IEnumerable).Cast<object>());
		}
		return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((OverwriteTransform<TValue>)obj);
	}

	public override int GetHashCode()
	{
		return EqualityComparer<TValue>.Default.GetHashCode(Value);
	}
}

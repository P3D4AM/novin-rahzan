using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Expressions;

internal class Literal<T> : Expression, IComparable<Literal<T>>, IComparable, IEquatable<Literal<T>> where T : IComparable<T>
{
	public T Value { get; }

	public Literal(T value)
	{
		Value = value;
	}

	internal override object EvaluateCore(IEvaluationContext data)
	{
		return Value;
	}

	internal override IEnumerable<string> GetNames()
	{
		yield break;
	}

	public int CompareTo(Literal<T> other)
	{
		if (other == null)
		{
			return 1;
		}
		return Value.CompareTo(other.Value);
	}

	public int CompareTo(object obj)
	{
		if (obj == null)
		{
			return 1;
		}
		if (this == obj)
		{
			return 0;
		}
		if (!(obj is Literal<T> other))
		{
			throw new SqlAssessmentTypeMismatchException(typeof(Literal<T>), obj.GetType(), obj);
		}
		return CompareTo(other);
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as Literal<T>);
	}

	public bool Equals(Literal<T> other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return EqualityComparer<T>.Default.Equals(Value, other.Value);
	}

	public override int GetHashCode()
	{
		if (Value == null)
		{
			return 0;
		}
		return EqualityComparer<T>.Default.GetHashCode(Value);
	}
}

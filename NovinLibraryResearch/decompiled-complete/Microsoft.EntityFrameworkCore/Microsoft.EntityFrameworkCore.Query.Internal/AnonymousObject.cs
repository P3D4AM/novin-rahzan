using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Query.Internal;

public readonly struct AnonymousObject
{
	private readonly object[] _values;

	public static readonly ConstructorInfo AnonymousObjectCtor = typeof(AnonymousObject).GetTypeInfo().DeclaredConstructors.Single((ConstructorInfo c) => c.GetParameters().Length == 1);

	[JetBrains.Annotations.UsedImplicitly]
	public AnonymousObject([JetBrains.Annotations.NotNull] object[] values)
	{
		_values = values;
	}

	public static bool operator ==(AnonymousObject x, AnonymousObject y)
	{
		return x.Equals(y);
	}

	public static bool operator !=(AnonymousObject x, AnonymousObject y)
	{
		return !x.Equals(y);
	}

	public override bool Equals(object obj)
	{
		if (obj != null)
		{
			if (obj is AnonymousObject anonymousObject)
			{
				return _values.SequenceEqual(anonymousObject._values);
			}
			return false;
		}
		return false;
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

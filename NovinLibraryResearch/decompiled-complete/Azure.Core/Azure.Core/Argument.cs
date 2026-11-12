using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Azure.Core;

internal static class Argument
{
	public static void AssertNotNull<T>([AllowNull][NotNull] T value, string name) where T : class?
	{
		if (value == null)
		{
			throw new ArgumentNullException(name);
		}
	}

	public static void AssertNotNull<T>(T? value, string name) where T : struct
	{
		if (!value.HasValue)
		{
			throw new ArgumentNullException(name);
		}
	}

	public static void AssertNotNullOrEmpty<T>([AllowNull][NotNull] IEnumerable<T> value, string name)
	{
		if (value == null)
		{
			throw new ArgumentNullException(name);
		}
		if (value is ICollection<T> { Count: 0 })
		{
			throw new ArgumentException("Value cannot be an empty collection.", name);
		}
		if (value is ICollection { Count: 0 })
		{
			throw new ArgumentException("Value cannot be an empty collection.", name);
		}
		using IEnumerator<T> enumerator = value.GetEnumerator();
		if (!enumerator.MoveNext())
		{
			throw new ArgumentException("Value cannot be an empty collection.", name);
		}
	}

	public static void AssertNotNullOrEmpty([AllowNull][NotNull] string value, string name)
	{
		if (value == null)
		{
			throw new ArgumentNullException(name);
		}
		if (value.Length == 0)
		{
			throw new ArgumentException("Value cannot be an empty string.", name);
		}
	}

	public static void AssertNotNullOrWhiteSpace([AllowNull][NotNull] string value, string name)
	{
		if (value == null)
		{
			throw new ArgumentNullException(name);
		}
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentException("Value cannot be empty or contain only white-space characters.", name);
		}
	}

	public static void AssertNotDefault<T>(ref T value, string name) where T : struct, IEquatable<T>
	{
		if (value.Equals(default(T)))
		{
			throw new ArgumentException("Value cannot be empty.", name);
		}
	}

	public static void AssertInRange<T>(T value, T minimum, T maximum, string name) where T : notnull, IComparable<T>
	{
		if (minimum.CompareTo(value) > 0)
		{
			throw new ArgumentOutOfRangeException(name, "Value is less than the minimum allowed.");
		}
		if (maximum.CompareTo(value) < 0)
		{
			throw new ArgumentOutOfRangeException(name, "Value is greater than the maximum allowed.");
		}
	}
}

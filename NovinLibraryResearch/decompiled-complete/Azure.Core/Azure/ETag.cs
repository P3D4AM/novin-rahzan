using System;
using System.ComponentModel;

namespace Azure;

public readonly struct ETag : IEquatable<ETag>
{
	private const char QuoteCharacter = '"';

	private const string QuoteString = "\"";

	private const string WeakETagPrefix = "W/\"";

	private const string DefaultFormat = "G";

	private const string HeaderFormat = "H";

	private readonly string _value;

	public static readonly ETag All = new ETag("*");

	public ETag(string etag)
	{
		_value = etag;
	}

	public static bool operator ==(ETag left, ETag right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ETag left, ETag right)
	{
		return !left.Equals(right);
	}

	public bool Equals(ETag other)
	{
		return string.Equals(_value, other._value, StringComparison.Ordinal);
	}

	public bool Equals(string? other)
	{
		return string.Equals(_value, other, StringComparison.Ordinal);
	}

	public override bool Equals(object? obj)
	{
		if (obj is ETag other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _value?.GetHashCode() ?? 0;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override string ToString()
	{
		return ToString("G");
	}

	public string ToString(string format)
	{
		if (_value == null)
		{
			return "<null>";
		}
		bool flag = !IsValidQuotedFormat(_value);
		return format switch
		{
			"H" => flag ? ("\"" + _value + "\"") : _value, 
			"G" => _value, 
			_ => throw new ArgumentException("Invalid format string."), 
		};
	}

	internal static ETag Parse(string value)
	{
		if (value == All._value)
		{
			return All;
		}
		if (!IsValidQuotedFormat(value))
		{
			throw new ArgumentException("The value should be equal to * , be wrapped in quotes, or be wrapped in quotes prefixed by W/", "value");
		}
		if (value.StartsWith("W/\"", StringComparison.Ordinal))
		{
			return new ETag(value);
		}
		return new ETag(value.Trim(new char[1] { '"' }));
	}

	private static bool IsValidQuotedFormat(string value)
	{
		if ((!value.StartsWith("\"", StringComparison.Ordinal) && !value.StartsWith("W/\"", StringComparison.Ordinal)) || !value.EndsWith("\"", StringComparison.Ordinal))
		{
			return value == All._value;
		}
		return true;
	}
}

using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using Azure.Core;

namespace Azure;

public readonly struct HttpRange : IEquatable<HttpRange>
{
	private const string Unit = "bytes";

	public long Offset { get; }

	public long? Length { get; }

	public HttpRange(long offset = 0L, long? length = null)
	{
		if (offset < 0)
		{
			throw new ArgumentOutOfRangeException("offset");
		}
		if (length.HasValue && length <= 0)
		{
			throw new ArgumentOutOfRangeException("length");
		}
		Offset = offset;
		Length = length;
	}

	public override string ToString()
	{
		string text = "";
		if (Length.HasValue && Length != 0)
		{
			text = (Offset + Length.Value - 1).ToString(CultureInfo.InvariantCulture);
		}
		return FormattableString.Invariant(FormattableStringFactory.Create("{0}={1}-{2}", "bytes", Offset, text));
	}

	public static bool operator ==(HttpRange left, HttpRange right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(HttpRange left, HttpRange right)
	{
		return !(left == right);
	}

	public bool Equals(HttpRange other)
	{
		if (Offset == other.Offset)
		{
			return Length == other.Length;
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override bool Equals(object? obj)
	{
		if (obj is HttpRange other)
		{
			return Equals(other);
		}
		return false;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public override int GetHashCode()
	{
		return HashCodeBuilder.Combine(Offset, Length);
	}
}

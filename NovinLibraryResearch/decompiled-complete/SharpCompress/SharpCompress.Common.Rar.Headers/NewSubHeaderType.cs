using System;

namespace SharpCompress.Common.Rar.Headers;

internal sealed class NewSubHeaderType : IEquatable<NewSubHeaderType>
{
	internal static readonly NewSubHeaderType SUBHEAD_TYPE_CMT = new NewSubHeaderType('C', 'M', 'T');

	internal static readonly NewSubHeaderType SUBHEAD_TYPE_RR = new NewSubHeaderType('R', 'R');

	private readonly byte[] _bytes;

	private NewSubHeaderType(params char[] chars)
	{
		_bytes = new byte[chars.Length];
		for (int i = 0; i < chars.Length; i++)
		{
			_bytes[i] = (byte)chars[i];
		}
	}

	internal bool Equals(byte[] bytes)
	{
		if (_bytes.Length != bytes.Length)
		{
			return false;
		}
		return MemoryExtensions.AsSpan(_bytes).SequenceEqual(bytes);
	}

	public bool Equals(NewSubHeaderType? other)
	{
		if (other != null)
		{
			return Equals(other._bytes);
		}
		return false;
	}
}

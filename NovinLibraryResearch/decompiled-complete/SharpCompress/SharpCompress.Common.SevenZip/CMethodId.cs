namespace SharpCompress.Common.SevenZip;

internal readonly struct CMethodId
{
	public const ulong K_COPY_ID = 0uL;

	public const ulong K_LZMA_ID = 196865uL;

	public const ulong K_LZMA2_ID = 33uL;

	public const ulong K_AES_ID = 116459265uL;

	public static readonly CMethodId K_COPY = new CMethodId(0uL);

	public static readonly CMethodId K_LZMA = new CMethodId(196865uL);

	public static readonly CMethodId K_LZMA2 = new CMethodId(33uL);

	public static readonly CMethodId K_AES = new CMethodId(116459265uL);

	public readonly ulong _id;

	public CMethodId(ulong id)
	{
		_id = id;
	}

	public override int GetHashCode()
	{
		ulong id = _id;
		return id.GetHashCode();
	}

	public override bool Equals(object? obj)
	{
		if (obj is CMethodId other)
		{
			return Equals(other);
		}
		return false;
	}

	public bool Equals(CMethodId other)
	{
		return _id == other._id;
	}

	public static bool operator ==(CMethodId left, CMethodId right)
	{
		return left._id == right._id;
	}

	public static bool operator !=(CMethodId left, CMethodId right)
	{
		return left._id != right._id;
	}

	public int GetLength()
	{
		int num = 0;
		for (ulong num2 = _id; num2 != 0L; num2 >>= 8)
		{
			num++;
		}
		return num;
	}
}

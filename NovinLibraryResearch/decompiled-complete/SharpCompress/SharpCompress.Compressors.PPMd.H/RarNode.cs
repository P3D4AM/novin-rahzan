using System;
using System.Buffers.Binary;
using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class RarNode : Pointer
{
	private int _next;

	public const int SIZE = 4;

	public RarNode(byte[] memory)
		: base(memory)
	{
	}

	internal int GetNext()
	{
		if (base.Memory != null)
		{
			_next = BinaryPrimitives.ReadInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address));
		}
		return _next;
	}

	internal void SetNext(RarNode next)
	{
		SetNext(next.Address);
	}

	internal void SetNext(int next)
	{
		_next = next;
		if (base.Memory != null)
		{
			BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address), next);
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("State[");
		stringBuilder.Append("\n  Address=");
		stringBuilder.Append(Address);
		stringBuilder.Append("\n  size=");
		stringBuilder.Append(4);
		stringBuilder.Append("\n  next=");
		stringBuilder.Append(GetNext());
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}
}

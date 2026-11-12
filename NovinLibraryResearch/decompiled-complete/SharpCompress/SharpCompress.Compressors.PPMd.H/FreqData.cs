using System;
using System.Buffers.Binary;
using System.Text;

namespace SharpCompress.Compressors.PPMd.H;

internal class FreqData : Pointer
{
	internal const int SIZE = 6;

	internal int SummFreq
	{
		get
		{
			return BinaryPrimitives.ReadInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address)) & 0xFFFF;
		}
		set
		{
			BinaryPrimitives.WriteInt16LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address), (short)value);
		}
	}

	internal FreqData(byte[] memory)
		: base(memory)
	{
	}

	internal FreqData Initialize(byte[] mem)
	{
		return Initialize<FreqData>(mem);
	}

	internal void IncrementSummFreq(int dSummFreq)
	{
		SummFreq += (short)dSummFreq;
	}

	internal int GetStats()
	{
		return BinaryPrimitives.ReadInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 2));
	}

	internal virtual void SetStats(State state)
	{
		SetStats(state.Address);
	}

	internal void SetStats(int state)
	{
		BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan(base.Memory, Address + 2), state);
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("FreqData[");
		stringBuilder.Append("\n  Address=");
		stringBuilder.Append(Address);
		stringBuilder.Append("\n  size=");
		stringBuilder.Append(6);
		stringBuilder.Append("\n  summFreq=");
		stringBuilder.Append(SummFreq);
		stringBuilder.Append("\n  stats=");
		stringBuilder.Append(GetStats());
		stringBuilder.Append("\n]");
		return stringBuilder.ToString();
	}
}

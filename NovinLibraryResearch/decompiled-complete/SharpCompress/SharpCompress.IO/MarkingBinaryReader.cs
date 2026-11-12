using System;
using System.Buffers.Binary;
using System.IO;

namespace SharpCompress.IO;

internal class MarkingBinaryReader : BinaryReader
{
	public virtual long CurrentReadByteCount { get; protected set; }

	public MarkingBinaryReader(Stream stream)
		: base(stream)
	{
	}

	public virtual void Mark()
	{
		CurrentReadByteCount = 0L;
	}

	public override int Read()
	{
		throw new NotSupportedException();
	}

	public override int Read(byte[] buffer, int index, int count)
	{
		throw new NotSupportedException();
	}

	public override int Read(char[] buffer, int index, int count)
	{
		throw new NotSupportedException();
	}

	public override bool ReadBoolean()
	{
		return ReadByte() != 0;
	}

	public override byte ReadByte()
	{
		CurrentReadByteCount++;
		return base.ReadByte();
	}

	public override byte[] ReadBytes(int count)
	{
		CurrentReadByteCount += count;
		byte[] array = base.ReadBytes(count);
		if (array.Length != count)
		{
			throw new EndOfStreamException($"Could not read the requested amount of bytes.  End of stream reached. Requested: {count} Read: {array.Length}");
		}
		return array;
	}

	public override char ReadChar()
	{
		throw new NotSupportedException();
	}

	public override char[] ReadChars(int count)
	{
		throw new NotSupportedException();
	}

	public override double ReadDouble()
	{
		throw new NotSupportedException();
	}

	public override short ReadInt16()
	{
		return BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
	}

	public override int ReadInt32()
	{
		return BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
	}

	public override long ReadInt64()
	{
		return BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(8));
	}

	public override sbyte ReadSByte()
	{
		return (sbyte)ReadByte();
	}

	public override float ReadSingle()
	{
		throw new NotSupportedException();
	}

	public override string ReadString()
	{
		throw new NotSupportedException();
	}

	public override ushort ReadUInt16()
	{
		return BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
	}

	public override uint ReadUInt32()
	{
		return BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
	}

	public override ulong ReadUInt64()
	{
		return BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(8));
	}

	public ulong ReadRarVInt(int maxBytes = 10)
	{
		return DoReadRarVInt((maxBytes - 1) * 7);
	}

	private ulong DoReadRarVInt(int maxShift)
	{
		int num = 0;
		ulong num2 = 0uL;
		do
		{
			byte b = ReadByte();
			uint num3 = b & 0x7Fu;
			long num4 = num3;
			ulong num5 = (ulong)(num4 << num);
			if (num4 != (long)(num5 >> num))
			{
				break;
			}
			num2 |= num5;
			if (b == num3)
			{
				return num2;
			}
			num += 7;
		}
		while (num <= maxShift);
		throw new FormatException("malformed vint");
	}

	public uint ReadRarVIntUInt32(int maxBytes = 5)
	{
		return DoReadRarVIntUInt32((maxBytes - 1) * 7);
	}

	public ushort ReadRarVIntUInt16(int maxBytes = 3)
	{
		return checked((ushort)DoReadRarVIntUInt32((maxBytes - 1) * 7));
	}

	public byte ReadRarVIntByte(int maxBytes = 2)
	{
		return checked((byte)DoReadRarVIntUInt32((maxBytes - 1) * 7));
	}

	private uint DoReadRarVIntUInt32(int maxShift)
	{
		int num = 0;
		uint num2 = 0u;
		do
		{
			byte b = ReadByte();
			uint num3 = b & 0x7Fu;
			uint num4 = num3 << num;
			if (num3 != num4 >> num)
			{
				break;
			}
			num2 |= num4;
			if (b == num3)
			{
				return num2;
			}
			num += 7;
		}
		while (num <= maxShift);
		throw new FormatException("malformed vint");
	}
}

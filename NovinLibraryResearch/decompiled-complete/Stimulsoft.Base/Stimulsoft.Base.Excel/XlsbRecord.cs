using System;
using System.IO;

namespace Stimulsoft.Base.Excel;

internal class XlsbRecord
{
	protected byte[] _bytes;

	protected int _offset;

	public BIFF12 ID => GetID(_bytes, _offset);

	internal byte[] Bytes => _bytes;

	internal int Offset => _offset - 4;

	public ushort RecordSize => BitConverter.ToUInt16(_bytes, _offset - 2);

	public int Size => 4 + RecordSize;

	private static BIFF12 GetID(byte[] bytes, int offset)
	{
		BIFF12 bIFF = (BIFF12)BitConverter.ToUInt16(bytes, offset - 4);
		BIFF12 result = BIFF12.ROW;
		uint num = 0u;
		if (offset < bytes.Length)
		{
			byte b = bytes[offset++];
			num = b;
			if ((b & 0x80) > 0 && offset < bytes.Length)
			{
				byte b2 = bytes[offset++];
				num = (uint)(b2 << 8) | num;
				if ((b2 & 0x80) == 0)
				{
					return (BIFF12)b2;
				}
				if (offset >= bytes.Length)
				{
					return BIFF12.ROW;
				}
				byte b3 = bytes[offset++];
				num = (uint)(b3 << 16) | num;
				if ((b3 & 0x80) == 0)
				{
					return (BIFF12)b3;
				}
				if (offset >= bytes.Length)
				{
					return BIFF12.ROW;
				}
				byte b4 = bytes[offset++];
				num = (uint)(b4 << 24) | num;
				return (BIFF12)b4;
			}
		}
		return result;
	}

	public uint GetLength()
	{
		uint num = 0u;
		if (_offset >= _bytes.Length)
		{
			return 0u;
		}
		byte b = _bytes[_offset++];
		num = b & 0x7Fu;
		if ((b & 0x80) == 0)
		{
			return num;
		}
		if (_offset >= _bytes.Length)
		{
			return 0u;
		}
		byte b2 = _bytes[_offset++];
		num = (uint)((b2 & 0x7F) << 7) | num;
		if ((b2 & 0x80) == 0)
		{
			return num;
		}
		if (_offset >= _bytes.Length)
		{
			return 0u;
		}
		byte b3 = _bytes[_offset++];
		num = (uint)((b3 & 0x7F) << 14) | num;
		if ((b3 & 0x80) == 0)
		{
			return num;
		}
		if (_offset >= _bytes.Length)
		{
			return 0u;
		}
		byte b4 = _bytes[_offset++];
		return (uint)((b4 & 0x7F) << 21) | num;
	}

	protected XlsbRecord(byte[] bytes)
		: this(bytes, 0u)
	{
	}

	protected XlsbRecord(byte[] bytes, uint offset)
	{
		if (bytes.Length - offset < 4)
		{
			throw new ArgumentException("Error: Buffer size is less than minimum BIFF record size.");
		}
		_bytes = bytes;
		_offset = (int)(4 + offset);
	}

	public static XlsbRecord GetRecord(byte[] bytes, uint offset)
	{
		return new XlsbRecord(bytes, offset);
	}

	public int ReadUInt32(int offset)
	{
		int num = (int)BitConverter.ToUInt32(_bytes, _offset + offset);
		return (_bytes[_offset + offset + 3] << 24) + (_bytes[_offset + offset + 2] << 16) + (_bytes[_offset + offset + 1] << 8) + _bytes[_offset + offset];
	}

	public ushort ReadUInt16(int offset)
	{
		ushort num = BitConverter.ToUInt16(_bytes, _offset + offset);
		ushort num2 = (ushort)(_bytes[_offset + offset + 1] << 8);
		return (ushort)(num2 + _bytes[_offset + offset]);
	}

	public byte ReadByte(int offset)
	{
		byte @byte = Buffer.GetByte(_bytes, _offset + offset);
		return _bytes[_offset + offset];
	}

	public string ReadString(int offset, int len)
	{
		throw new NotImplementedException();
	}

	public double ReadFloat(int offset)
	{
		double num = 0.0;
		int num2 = ReadUInt32(_offset + offset);
		if (((uint)num2 & 2u) != 0)
		{
			num = num2 >> 2;
		}
		else
		{
			using MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(0);
			binaryWriter.Write(num2 & -4);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			num = binaryReader.ReadDouble();
			binaryReader.Close();
			binaryWriter.Close();
		}
		if (((uint)num2 & (true ? 1u : 0u)) != 0)
		{
			num /= 100.0;
		}
		float num3 = BitConverter.ToSingle(_bytes, _offset + offset);
		return num;
	}

	public double ReadDouble(int offset)
	{
		double result = 0.0;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			for (uint num = 0u; num < 8; num++)
			{
				binaryWriter.Write(_bytes[offset + _offset + num]);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			result = binaryReader.ReadDouble();
			binaryReader.Close();
			binaryWriter.Close();
		}
		double num2 = BitConverter.ToDouble(_bytes, _offset + offset);
		return result;
	}
}

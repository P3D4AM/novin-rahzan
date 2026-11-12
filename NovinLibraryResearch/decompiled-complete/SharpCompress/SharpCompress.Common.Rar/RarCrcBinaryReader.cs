using System.IO;
using SharpCompress.Compressors.Rar;
using SharpCompress.IO;

namespace SharpCompress.Common.Rar;

internal class RarCrcBinaryReader : MarkingBinaryReader
{
	private uint _currentCrc;

	public RarCrcBinaryReader(Stream stream)
		: base(stream)
	{
	}

	public uint GetCrc32()
	{
		return ~_currentCrc;
	}

	public void ResetCrc()
	{
		_currentCrc = uint.MaxValue;
	}

	protected void UpdateCrc(byte b)
	{
		_currentCrc = RarCRC.CheckCrc(_currentCrc, b);
	}

	protected byte[] ReadBytesNoCrc(int count)
	{
		return base.ReadBytes(count);
	}

	public override byte ReadByte()
	{
		byte b = base.ReadByte();
		_currentCrc = RarCRC.CheckCrc(_currentCrc, b);
		return b;
	}

	public override byte[] ReadBytes(int count)
	{
		byte[] array = base.ReadBytes(count);
		_currentCrc = RarCRC.CheckCrc(_currentCrc, array, 0, array.Length);
		return array;
	}
}

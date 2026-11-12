using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsByteString : IXlsString
{
	private readonly byte[] _bytes;

	private readonly uint _offset;

	public ushort CharacterCount => BitConverter.ToUInt16(_bytes, (int)_offset);

	public XlsByteString(byte[] bytes, uint offset)
	{
		_bytes = bytes;
		_offset = offset;
	}

	public string GetValue(Encoding encoding)
	{
		byte[] array = ReadArray(2, CharacterCount * (Helpers.IsSingleByteEncoding(encoding) ? 1 : 2));
		return encoding.GetString(array, 0, array.Length);
	}

	public byte[] ReadArray(int offset, int size)
	{
		byte[] array = new byte[size];
		Buffer.BlockCopy(_bytes, (int)(_offset + offset), array, 0, size);
		return array;
	}
}

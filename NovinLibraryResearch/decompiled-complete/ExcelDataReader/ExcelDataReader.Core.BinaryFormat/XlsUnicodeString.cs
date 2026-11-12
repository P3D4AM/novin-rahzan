using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsUnicodeString : IXlsString
{
	private readonly byte[] _bytes;

	private readonly uint _offset;

	public ushort CharacterCount => BitConverter.ToUInt16(_bytes, (int)_offset);

	public bool IsMultiByte => (_bytes[_offset + 2] & 1) != 0;

	public XlsUnicodeString(byte[] bytes, uint offset)
	{
		_bytes = bytes;
		_offset = offset;
	}

	public string GetValue(Encoding encoding)
	{
		if (CharacterCount == 0)
		{
			return string.Empty;
		}
		if (IsMultiByte)
		{
			return Encoding.Unicode.GetString(_bytes, (int)(_offset + 3), CharacterCount * 2);
		}
		byte[] array = new byte[CharacterCount * 2];
		for (int i = 0; i < CharacterCount; i++)
		{
			array[i * 2] = _bytes[_offset + 3 + i];
		}
		return Encoding.Unicode.GetString(array);
	}
}

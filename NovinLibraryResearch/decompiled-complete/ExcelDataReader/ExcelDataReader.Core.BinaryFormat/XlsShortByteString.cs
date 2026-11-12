using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsShortByteString : IXlsString
{
	private readonly byte[] _bytes;

	private readonly uint _offset;

	public ushort CharacterCount => _bytes[_offset];

	public XlsShortByteString(byte[] bytes, uint offset)
	{
		_bytes = bytes;
		_offset = offset;
	}

	public string GetValue(Encoding encoding)
	{
		if (!Helpers.IsSingleByteEncoding(encoding))
		{
			return encoding.GetString(_bytes, (int)(_offset + 1), CharacterCount * 2);
		}
		return encoding.GetString(_bytes, (int)(_offset + 1), CharacterCount);
	}
}

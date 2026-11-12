using System.Collections.Generic;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffSST : XlsBiffRecord
{
	private readonly List<IXlsString> _strings;

	public uint Count => ReadUInt32(0);

	public uint UniqueCount => ReadUInt32(4);

	internal XlsBiffSST(byte[] bytes, uint offset)
		: base(bytes, offset)
	{
		_strings = new List<IXlsString>();
	}

	public void ReadStrings(XlsBiffStream biffStream)
	{
		XlsSSTReader xlsSSTReader = new XlsSSTReader(this, biffStream);
		for (int i = 0; i < UniqueCount; i++)
		{
			IXlsString item = xlsSSTReader.ReadString();
			_strings.Add(item);
		}
	}

	public string GetString(uint sstIndex, Encoding encoding)
	{
		if (sstIndex < _strings.Count)
		{
			return _strings[(int)sstIndex].GetValue(encoding);
		}
		return string.Empty;
	}
}

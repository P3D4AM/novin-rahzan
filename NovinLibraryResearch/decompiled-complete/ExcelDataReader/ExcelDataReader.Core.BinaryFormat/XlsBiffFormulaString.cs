using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffFormulaString : XlsBiffRecord
{
	private readonly IXlsString _xlsString;

	internal XlsBiffFormulaString(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		switch (biffVersion)
		{
		case 2:
			_xlsString = new XlsShortByteString(bytes, offset + 4);
			return;
		case 3:
		case 4:
		case 5:
			_xlsString = new XlsByteString(bytes, offset + 4);
			return;
		}
		if (biffVersion == 8)
		{
			_xlsString = new XlsUnicodeString(bytes, offset + 4);
			return;
		}
		throw new ArgumentException("Unexpected BIFF version " + biffVersion, "biffVersion");
	}

	public string GetValue(Encoding encoding)
	{
		return _xlsString.GetValue(encoding);
	}
}

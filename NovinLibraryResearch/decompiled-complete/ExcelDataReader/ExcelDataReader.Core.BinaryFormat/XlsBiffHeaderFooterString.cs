using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal sealed class XlsBiffHeaderFooterString : XlsBiffRecord
{
	private readonly IXlsString _xlsString;

	internal XlsBiffHeaderFooterString(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		if (biffVersion < 8)
		{
			_xlsString = new XlsShortByteString(bytes, offset + 4);
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

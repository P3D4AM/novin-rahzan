using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffLabelCell : XlsBiffBlankCell
{
	private readonly IXlsString _xlsString;

	internal XlsBiffLabelCell(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset, biffVersion)
	{
		if (base.Id == BIFFRECORDTYPE.LABEL_OLD)
		{
			_xlsString = new XlsShortByteString(bytes, offset + 4 + 7);
			return;
		}
		if (biffVersion >= 2 && biffVersion <= 5)
		{
			_xlsString = new XlsByteString(bytes, offset + 4 + 6);
			return;
		}
		if (biffVersion == 8)
		{
			_xlsString = new XlsUnicodeString(bytes, offset + 4 + 6);
			return;
		}
		throw new ArgumentException("Unexpected BIFF version " + biffVersion, "biffVersion");
	}

	public string GetValue(Encoding encoding)
	{
		return _xlsString.GetValue(encoding);
	}
}

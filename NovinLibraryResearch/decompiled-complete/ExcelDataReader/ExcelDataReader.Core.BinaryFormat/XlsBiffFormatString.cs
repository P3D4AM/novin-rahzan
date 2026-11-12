using System;
using System.Text;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsBiffFormatString : XlsBiffRecord
{
	private readonly IXlsString _xlsString;

	public ushort Index
	{
		get
		{
			BIFFRECORDTYPE id = base.Id;
			if (id == BIFFRECORDTYPE.FORMAT_V23)
			{
				throw new NotSupportedException("Index is not available for BIFF2 and BIFF3 FORMAT records.");
			}
			return ReadUInt16(0);
		}
	}

	internal XlsBiffFormatString(byte[] bytes, uint offset, int biffVersion)
		: base(bytes, offset)
	{
		if (base.Id == BIFFRECORDTYPE.FORMAT_V23)
		{
			_xlsString = new XlsShortByteString(bytes, offset + 4);
			return;
		}
		if (biffVersion >= 2 && biffVersion <= 5)
		{
			_xlsString = new XlsShortByteString(bytes, offset + 4 + 2);
			return;
		}
		if (biffVersion == 8)
		{
			_xlsString = new XlsUnicodeString(bytes, offset + 4 + 2);
			return;
		}
		throw new ArgumentException("Unexpected BIFF version " + biffVersion, "biffVersion");
	}

	public string GetValue(Encoding encoding)
	{
		return _xlsString.GetValue(encoding);
	}
}
